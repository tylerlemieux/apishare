using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIShare.Controllers
{
    public class AuthenticationController : Controller
    {
        /// <summary>
        /// Takes in a username and password
        /// </summary>
        /// <returns>Json object with whether or not the login succeeded</returns>
        [HttpPost]
        [ActionName("Login")]
        public JsonResult LoginSubmit(string username, string password)
        {
            return Json("");
        }

        [HttpPost]
        [ActionName("Registration")]
        public JsonResult RegistrationSubmit()
        {
            return Json("");
        }


    }
}