using System.Web.Mvc;

namespace DotWeb.Controllers
{
    public class _SysAdmController : Controller
    {
        public RedirectResult Index()
        {
            //後台登錄
            return Redirect("~/Sys_Base/SystemLogin/Index");
        }
    }
}
