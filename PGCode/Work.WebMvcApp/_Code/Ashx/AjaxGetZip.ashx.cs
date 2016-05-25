using System;
using System.Web;
using System.Web.Script.Serialization;

using ProcCore.Business.Logic;

namespace DotWeb._Code.Ashx
{
    /// <summary>
    ///AjaxGetZip 的摘要描述
    /// </summary>
    public class AjaxGetZip : BaseIHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            String city = context.Request.Form["city"];
            String county = context.Request.Form["county"];

            vmJsonResult r_json_data = null;
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 4096;

            try
            {
                LogicAddress LBase = new LogicAddress();
                LBase.Connection = getSQLConnection();

                r_json_data = new vmJsonResult() { result = true, data = LBase.GetZip(city, county) };
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