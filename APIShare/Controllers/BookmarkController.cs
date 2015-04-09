using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            throw new NotImplementedException();
            return Json(false);
        }

        /// <summary>
        /// removes a bookmark from the user's profile
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult RemoveBookmark()
        {
            throw new NotImplementedException();
            return Json(false);
        }

        /// <summary>
        /// adds a bookmark to the user's profile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddBookmark()
        {
            throw new NotImplementedException();
            return Json(false);
        }
    }
}