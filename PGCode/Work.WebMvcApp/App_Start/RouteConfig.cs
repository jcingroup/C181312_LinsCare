using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotWeb.AppStart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Manager",
            url: "_SysAdm",
            defaults: new { controller = "_SysAdm", action = "Index" }
            ).DataTokens["UseNamespaceFallback"] = false;

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "AboutUs", action = "Index", id = 0 }
            ).DataTokens["UseNamespaceFallback"] = false;
        }
    }
}