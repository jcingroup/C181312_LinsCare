using System.Web.Mvc;

namespace DotWeb.Areas.Sys_Example
{
    public class Sys_ExampleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sys_Example";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sys_Example_default",
                "Sys_Example/{controller}/{action}/{id}",
                new { action = "ListGrid", id = UrlParameter.Optional },
                new string[] { "DotWeb.Areas.Sys_Example.Controllers" }
            ).DataTokens["UseNamespaceFallback" ] = false;
        }
    }
}
