using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIShare.Models.Search;

namespace APIShare.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        /// <summary>
        /// will call model to search for users and apis
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult Index(string searchString)
        {
            var searchResults = SearchModel.Search(searchString);
            return View(searchResults);
        }

        /// <summary>
        /// Takes in a search string and searches through api's and users (via a model)
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Object with found users and apis</returns>
        [HttpGet]
        public JsonResult Search(string searchString)
        {
            var searchResults = SearchModel.Search(searchString);
            return Json(searchResults);
        }
    }
}