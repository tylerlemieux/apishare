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
        public ActionResult User()
        {
            return View();
        }

        /// <summary>
        /// Follows a user
        /// </summary>
        /// <returns>json result with success</returns>
        [HttpPost]
        public JsonResult Follow(int userToFollowId)
        {
            throw new NotImplementedException();
            return Json(false);
        }

        /// <summary>
        /// Unfollows a user
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult Unfollow(int userToUnfollowId)
        {
            throw new NotImplementedException();
            return Json(false);
        }

        /// <summary>
        /// Allows a user to edit their account information and profile
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public JsonResult EditInformation()
        {
            throw new NotImplementedException();
            return Json(false);
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
                         //group ub by new  
                         //{
                         //   ub.BookmarkID,
                         //   b.Name,
                         //   b.Description
                         //} into groupedBookmarks
                         select new BookmarkVM
                         {
                             BookmarkID = b.BookmarkID,
                             Description = b.Description,
                             Name = b.Name,
                             //Tags = groupedBookmarks
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