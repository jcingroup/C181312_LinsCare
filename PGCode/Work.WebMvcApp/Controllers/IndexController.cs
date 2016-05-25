using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcCore.Business.Logic;

namespace DotWeb.WebApp.Controllers
{
    public class IndexController : WebFrontController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
