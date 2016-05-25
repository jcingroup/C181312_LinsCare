using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class StoryController : WebFrontController
    {
        //
        // GET: /Story/
        public StoryController()
        {
            ViewBag.BodyClass = "Story";
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}