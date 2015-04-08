using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public JsonResult Follow()
        {
            throw new NotImplementedException();
            return Json(false);
        }

        /// <summary>
        /// Unfollows a user
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult Unfollow()
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
    }
}