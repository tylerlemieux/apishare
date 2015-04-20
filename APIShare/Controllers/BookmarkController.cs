using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIShare.Models;

namespace APIShare.Controllers
{
    public class BookmarkController : Controller
    {
        // GET: Bookmark
        /// <summary>
        /// Gets bookmark information
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Bookmark(string bookmarkName, int? bookmarkId)
        {
            return PartialView();
        }

        /// <summary>
        /// Creates a new bookmark
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateBookmark()
        {

            return Json(false);
        }

        /// <summary>
        /// removes a bookmark from the user's profile
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult RemoveBookmark(int bookmarkId)
        {
            if (Session["UserID"] != null)
            {
                using (APIToolEntities context = new APIToolEntities())
                {
                    var bookmarkToDelete =
                    (from ub in context.UserBookmarks
                     where bookmarkId == ub.BookmarkID && (int) Session["UserID"] == ub.UserID
                     select ub).FirstOrDefault();

                    if(bookmarkToDelete != null)
                    {
                        context.UserBookmarks.Remove(bookmarkToDelete);
                        context.SaveChanges();

                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, ErrorMessage = "Couldn't find user's bookmark" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, ErrorMessage = "No user logged in" });
            }
        }

        /// <summary>
        /// adds a bookmark to the user's profile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddBookmark(int bookmarkId)
        {
            if (Session["UserID"] != null)
            {
                int userId = (int)Session["UserID"];
                using (APIToolEntities context = new APIToolEntities())
                {
                    UserBookmark bookmarkToAdd = new UserBookmark();
                    bookmarkToAdd.BookmarkID = bookmarkId;
                    bookmarkToAdd.DateAdded = DateTime.Now;
                    bookmarkToAdd.Favorited = "N";
                    bookmarkToAdd.UserID = userId;

                    context.UserBookmarks.Add(bookmarkToAdd);
                    context.SaveChanges();

                    return Json(new { Success = true });
                }
            }
            else
            {
                return Json(new { Success = false, ErrorMessage = "No user logged in" });
            }
        }
    }
}