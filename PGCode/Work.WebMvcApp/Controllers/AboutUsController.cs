using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcCore.Business.Logic;

namespace DotWeb.WebApp.Controllers
{
    public class AboutUsController : WebFrontController
    {
        //
        // GET: /AboutUs/
        public AboutUsController()
        {
            ViewBag.BodyClass = "AboutUs";
        }

        public ActionResult Index()
        {
            ViewBag.IsFirstPage = true;

            a_Message ac_Message = new a_Message() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };
            var r = ac_Message.SearchMaster(new q_Message() { s_isopen = true, MaxRecord = 5 }, 0).SearchData;
            ViewBag.NewsLists = r;

            return View();
        }

    }
}
