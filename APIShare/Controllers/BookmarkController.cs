using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIShare.Models;
using APIShare.Models.Helpers;

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


        public PartialViewResult AddBookmark()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetTags()
        {
            APIToolEntities context = new APIToolEntities();
            var tags =
                (from t in context.Tags
                 select t.Tag1).ToArray();
            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates a new bookmark
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateBookmark(string name, string description, string link, string[] tags)
        {
            if(name != null && description != null && link != null)
            {
                Bookmark bookmark = new Bookmark();
                bookmark.Name = name;
                bookmark.Description = description;
                bookmark.Website = link;
                bookmark.AddedDate = DateTime.Now;

                APIToolEntities context = new APIToolEntities();
                context.Bookmarks.Add(bookmark);
                context.SaveChanges();

                int bookmarkId = bookmark.BookmarkID;
                foreach(var tag in tags)
                {
                    BookmarkTag bookmarkTag = new BookmarkTag();
                    bookmarkTag.TagID = TagHelper.CheckTag(tag);
                    bookmarkTag.BookmarkID = bookmarkId;

                    context.BookmarkTags.Add(bookmarkTag);
                }

                context.SaveChanges();

                return Json(new { Success = true });

            }
            else
            {
                return Json(new {Success = false, ErrorMessage = "Must provide name, description, and link"});
            }
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