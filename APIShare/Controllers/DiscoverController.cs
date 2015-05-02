using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIShare.Models.Engines;

namespace APIShare.Controllers
{
    public class DiscoverController : Controller
    {
        // GET: Discover
        public ActionResult Index()
        {
            RecommendationInput input = new RecommendationInput();
            //Populate the input
            RecomendationEngine recommendationEngine = new RecomendationEngine(input);
            int[] sortedInts = recommendationEngine.RunRecommendationEngine();
            return View();
        }
    }
}