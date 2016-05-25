using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class ManualController : WebFrontController
    {
        //
        // GET: /Manual/

        public ActionResult add()
        {
            return View();
        }
        public ActionResult edit()
        {
            return View();
        }
        public ActionResult del()
        {
            return View();
        }

    }
}
