﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIShare.Controllers
{
    public class BookmarkController : Controller
    {
        // GET: Bookmark
        public ActionResult Index()
        {
            return View();
        }
    }
}