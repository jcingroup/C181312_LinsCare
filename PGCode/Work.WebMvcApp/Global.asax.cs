using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using System.Web.Optimization;

namespace DotWeb.AppStart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        String VarCookie = "PiKen";
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("zh-CN")
            {
                ContextCondition = (Context => (Context.Request.Cookies[VarCookie + ".Lang"].Value == "zh-CN")) 
            });
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpCookie WebLang = Request.Cookies[VarCookie + ".Lang"];

            if (WebLang == null)
            {
                WebLang = new HttpCookie(VarCookie + ".Lang", System.Globalization.CultureInfo.CurrentCulture.Name);
                //WebLang = new HttpCookie(VarCookie + ".Lang", "zh-CN");
                Response.Cookies.Add(WebLang);
            }

            if (WebLang != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WebLang.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WebLang.Value);
            }
        }
    }
}