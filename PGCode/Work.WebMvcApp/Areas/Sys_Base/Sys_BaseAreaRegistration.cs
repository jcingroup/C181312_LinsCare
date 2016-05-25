using System.Web.Mvc;

namespace DotWeb.Areas.Sys_Base
{
    public class Sys_BaseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sys_Base";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sys_Base_default",
                "Sys_Base/{controller}/{action}/{id}",
                new { action = "ListGrid", id = UrlParameter.Optional },
                new string[] { "DotWeb.Areas.Sys_Base.Controllers" }
            ).DataTokens["UseNamespaceFallback" ] = false;
        }
    }
}
