using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIShare.Models;
using APIShare.Models.Helpers;
using System.Security.Cryptography;

namespace APIShare.Controllers
{
    public class AuthenticationController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Takes in a username and password
        /// </summary>
        /// <returns>Json object with whether or not the login succeeded</returns>
        [HttpPost]
        [ActionName("Login")]
        public JsonResult LoginSubmit(string username, string password)
        {
            using(APIToolEntities context = new APIToolEntities())
            {
                User user = context.Users.Where(s => s.Username == username).FirstOrDefault();
                if(user != null)
                {
                    string hashedPass = SecurityHelper.GetHash(password, user.Salt);

                    user = context.Users.Where(u => u.Username == username && u.Password == hashedPass).FirstOrDefault();
                    if(user != null)
                    {
                        //User exists... start the session
                        Session["UserID"] = user.UserID;
                        Session["Username"] = user.Username;
                        return Json(new { Success = true });
                    }
                    else
                    {
                        //password is incorrect
                        return Json(new { Success = false, ErrorMessage = "Login failed" });
                    }
                }
                else
                {
                    //username doesnt exist
                    return Json(new { Success = false, ErrorMessage = "Login failed" });
                }
                
            }

            return Json("");
        }

        [HttpPost]
        [ActionName("Register")]
        public JsonResult RegistrationSubmit(string username, string password, string email)
        {
            using(APIToolEntities context = new APIToolEntities())
            {
                if (username != "" && username != null && password != "" && password != null && email != ""  && email != null)
                {
                    var usersWithEmailAndPass = context.Users.Where(s => username == s.Username || email == s.Email);
                    if (usersWithEmailAndPass.Count() == 0)
                    {
                        //User doesnt already exist, so create the user
                        Models.User user = new User();
                        user.Username = username;

                        string salt = SecurityHelper.GetSalt();
                        user.Salt = salt;
                        user.Password = SecurityHelper.GetHash(password, salt);
                        user.Email = email;

                        context.Users.Add(user);
                        context.SaveChanges();

                        //Start the user's session
                        Session["UserID"] = user.UserID;
                        Session["Username"] = user.Username;

                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, ErrorMessage = "Username or password already exists" });
                    }
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Must supply username, password, and email" });
                }
            }
        }

        [HttpDelete]
        public JsonResult Logout()
        {
            try
            {
                Session.Clear();
                return Json(new {Success = true});
            }
            catch(Exception ex)
            {
                return Json(new { Success = false, ErrorMessage = ex });
            }
        }

    }
}