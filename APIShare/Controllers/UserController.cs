using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIShare.ViewModels;
using APIShare.Models;

namespace APIShare.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        /// <summary>
        /// No user is specified, sends model to page containing the current logged in user
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Finds a certain user and sends their data to the page to be displayed
        /// </summary>
        /// <returns></returns>
        public ActionResult User(int userId)
        {
            using (APIToolEntities context = new APIToolEntities())
            {
                UserVM viewModel = null;

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

                return View(viewModel);
            }
        }

        /// <summary>
        /// Follows a user
        /// </summary>
        /// <returns>json result with success</returns>
        [HttpPost]
        public JsonResult Follow(int userToFollowId)
        {
            try
            {
                APIToolEntities context = new APIToolEntities();
                int userId = (int)Session["UserID"];
                Follower newFollow = new Follower();
                //TODO change this to N and make other user need to 
                    // allow based on security settings
                newFollow.AllowFollow = "Y";
                newFollow.FollowDate = DateTime.Now;
                newFollow.UserBeingFollowedID = userToFollowId;
                newFollow.FollowerID = userId;

                context.Followers.Add(newFollow);
                context.SaveChanges();

                return Json(new { Success = true });
            }
            catch(Exception)
            {
                return Json(new { Success = false });
            }
        }

        /// <summary>
        /// Unfollows a user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Unfollow(int userToUnfollowId)
        {
            try
            {
                using (APIToolEntities context = new APIToolEntities())
                {
                    int userId = (int)Session["UserID"];
                    var followToDelete = context.Followers.Single(f => f.UserBeingFollowedID == userToUnfollowId
                        && f.FollowerID == userId);

                    context.Followers.Remove(followToDelete);
                    context.SaveChanges();
                }
                return Json(new { Success = true });
            }
            catch(Exception)
            {
                return Json(new { Success = false });
            }
        }

        /// <summary>
        /// Allows a user to edit their account information and profile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditInformation(string type, string newValue)
        {
            APIToolEntities context = new APIToolEntities();
            int userId = (int)Session["UserID"];
            var user = context.Users.Single(u => u.UserID == userId);
            if(type == "bio")
            {
                user.Bio = newValue;
            }
            //ADD OTHER INFORMATION

            context.SaveChanges();

            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult EditTags(string[] tags)
        {

            return Json(true); 
        }

        public ActionResult Friends()
        {
            using(APIToolEntities context = new APIToolEntities())
            {
                int userId = (int)Session["UserID"];

                List<FriendsVM> friends =
                    (from u in context.Users
                     join f in context.Followers on u.UserID equals f.UserBeingFollowedID
                     where f.FollowerID == userId
                     select new FriendsVM
                     {
                         UserID = u.UserID,
                         Bio = u.Bio,
                         Username = u.Username ?? u.Email,
                         Tags = 
                            (from t in context.Tags
                             join ut in context.UserSkills on t.TagID equals ut.TagID
                             where ut.UserID == u.UserID
                             select t.Tag1).ToList()
                     }).ToList();

                return View(friends);
            }
        }

        public ActionResult EditProfile()
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
    }
}