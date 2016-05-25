using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

using ProcCore.Business.Logic;

namespace DotWeb._Code.Ashx
{
    /// <summary>
    ///AjaxGetCountry 的摘要描述
    /// </summary>
    public class AjaxGetCountry : BaseIHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            vmJsonResult r_json_data = null;
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 4096;
            String city = context.Request.Form["city"];
            LogicAddress LBase = new LogicAddress();

            try
            {
                LBase.Connection = getSQLConnection();

                r_json_data = new vmJsonResult() { result = true, data = LBase.GetCounty(city) };
                context.Response.Write(js.Serialize(r_json_data));
            }
            catch (Exception ex)
            {
                r_json_data = new vmJsonResult() { result = false, message = ex.Message + ":" + ex.StackTrace };
                context.Response.Write(js.Serialize(r_json_data));
            }
        }
    }
}