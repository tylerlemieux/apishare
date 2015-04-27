using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIShare.ViewModels;
using APIShare.Models;

namespace APIShare.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// populate a UserVM object with relevant user data and return it to the view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (APIToolEntities context = new APIToolEntities())
            {
                UserVM viewModel = null;
                if (Session["UserID"] != null)
                {
                    int userId = (int)Session["UserID"];
                    viewModel =
                        (from users in context.Users
                         where users.UserID == userId
                         select new UserVM
                         {
                             Bio = users.Bio,
                             UserID = users.UserID,
                             Username = users.Name ?? users.Username ?? users.Email
                         }).First();

                    viewModel.Bookmarks =
                        (from ub in context.UserBookmarks
                         join b in context.Bookmarks on ub.BookmarkID equals b.BookmarkID
                         //join bt in context.BookmarkTags on b.BookmarkID equals bt.BookmarkID
                         //join t in context.Tags on bt.TagID equals t.TagID
                         where ub.UserID == viewModel.UserID
                         select new BookmarkVM
                         {
                             BookmarkID = b.BookmarkID,
                             Description = b.Description,
                             Name = b.Name,
                             Tags =
                                (from t in context.Tags
                                 join bt in context.BookmarkTags on t.TagID equals bt.TagID
                                 where bt.BookmarkID == b.BookmarkID
                                 select t).ToList()
                         }).ToList();

                    viewModel.Skills =
                        (from us in context.UserSkills
                         join t in context.Tags on us.TagID equals t.TagID
                         where us.UserID == viewModel.UserID
                         select t).ToList();

                    viewModel.Followers =
                        (from f in context.Followers
                         join follower in context.Users on f.FollowerID equals follower.UserID
                         where f.UserBeingFollowedID == viewModel.UserID
                         select new FollowerVM
                         {
                             UserID = follower.UserID,
                             Username = follower.Username
                         }).ToList();

                    viewModel.Following =
                        (from f in context.Followers
                         join follower in context.Users on f.UserBeingFollowedID equals follower.UserID
                         where f.FollowerID == viewModel.UserID
                         select new FollowerVM
                         {
                             UserID = follower.UserID,
                             Username = follower.Username
                         }).ToList();
                }
                return View(viewModel);   
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult LandingPage()
        {
            return View();
        }
    }
}