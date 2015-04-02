using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIShare.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["test"] = "Hello this is some random message to see if sessions work";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SessionTest()
        {
            ViewBag.Message = Session["test"];

            return View();
        }
   
    }
}