using System.Web.Mvc;

namespace DotWeb.Areas.Sys_Active
{
    public class Sys_RootDataAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sys_Active";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sys_Active_default",
                "Sys_Active/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
