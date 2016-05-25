using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcCore.Business.Logic;

namespace DotWeb.WebApp.Controllers
{
    public class NewsController : WebFrontController
    {
        //
        // GET: /News/
        public NewsController()
        {
            ViewBag.BodyClass = "News";
        }

        public ActionResult News_list()
        {
            a_Message ac_Message = new a_Message() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };
            var r = ac_Message.SearchMaster(new q_Message() { s_isopen = true }, 0).SearchData;
            return View(r);
        }
        public ActionResult News(int id)
        {
            a_Message ac_Message = new a_Message() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };
            var r = ac_Message.GetDataMaster(id, 0).SearchData;
            return View(r);
        }

    }
}
