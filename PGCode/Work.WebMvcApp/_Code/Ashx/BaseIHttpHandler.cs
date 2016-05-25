using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;

using ProcCore.DatabaseCore.TableFieldModule;
using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.DatabaseCore.DatabaseName;

namespace DotWeb._Code
{
    public class BaseIHttpHandler : IHttpHandler
    {
        public virtual void ProcessRequest(HttpContext context)
        {
            if (context.Session["Id"] == null) { }
        }
        protected CommConnection getSQLConnection()
        {
            BaseConnection BConn = new BaseConnection();
            String DataConnectionCode = System.Configuration.ConfigurationManager.AppSettings["DB00"];
            String[] DataConnectionInfo = DataConnectionCode.Split(',');
            BConn.Server = DataConnectionInfo[0];
            BConn.Account = DataConnectionInfo[1];
            BConn.Password = DataConnectionInfo[2];
            return BConn.GetConnection();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class vmJsonResult
    {
        public vmJsonResult() {
            message = "";
        }
        public Boolean result { get; set; }
        public String message { get; set; }
        public Object data { get; set; }
    }
}