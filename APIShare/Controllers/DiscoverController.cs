using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIShare.Models.Engines;
using APIShare.Models;
using APIShare.ViewModels;

namespace APIShare.Controllers
{
    public class DiscoverController : Controller
    {
        // GET: Discover
        public ActionResult Index()
        {
            int userId = (int)Session["UserID"];
            APIToolEntities context = new APIToolEntities();
            RecommendationInput input = new RecommendationInput();
            input.LibraryIDs =
                (from l in context.Bookmarks
                 select l.BookmarkID).ToArray();

            input.Users =
                (from u in context.Users
                 join f in context.Followers on u.UserID equals f.UserBeingFollowedID
                 where f.FollowerID == userId
                 select new UserRecommendation
                 {
                     UserID = u.UserID,
                 }).ToArray();

            //Populate the dictionary for the user's liked libraries
            foreach(var u in input.Users)
            {
                u.LikedLibraries = new Dictionary<int, bool>();
                var likedLibraries =
                    (from b in context.Bookmarks
                     join ub in context.UserBookmarks on b.BookmarkID equals ub.BookmarkID
                     where ub.UserID == u.UserID
                     select b.BookmarkID).ToArray();

                foreach(var l in input.LibraryIDs)
                {
                    u.LikedLibraries.Add(l, likedLibraries.Contains(l));
                }

            }

            input.CurrentUser = new UserRecommendation();
            input.CurrentUser.UserID = userId;
            var currentUserLibraries =
                (from b in context.Bookmarks
                 join ub in context.UserBookmarks on b.BookmarkID equals ub.BookmarkID
                 where ub.UserID == userId
                 select b.BookmarkID).ToArray();
            input.CurrentUser.LikedLibraries = new Dictionary<int, bool>();
            foreach(var l in input.LibraryIDs)
            {
                input.CurrentUser.LikedLibraries.Add(l, currentUserLibraries.Contains(l));
            }

            //Populate the input
            RecomendationEngine recommendationEngine = new RecomendationEngine(input);
            int[] sortedLibraryIds = recommendationEngine.RunRecommendationEngine();
            BookmarkVM[] viewModel = 
                (from id in sortedLibraryIds
                join b in context.Bookmarks on id equals b.BookmarkID
                select new BookmarkVM
                {
                    BookmarkID = id,
                    Description = b.Description,
                    Name = b.Name,
                    Tags = 
                        (from t in context.Tags
                        join bt in context.BookmarkTags on t.TagID equals bt.TagID
                        where bt.BookmarkID == b.BookmarkID
                        select t).ToList()
                }).ToArray();

            return View(viewModel);
        }
    }
}