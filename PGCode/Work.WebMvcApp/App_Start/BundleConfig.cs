using System.Web;
using System.Web.Optimization;

namespace DotWeb.AppStart
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region JScript
            bundles.Add(new ScriptBundle("~/commJScript")
            .Include(
            "~/_Code/jqScript/jquery-1.10.2.js",
            "~/_Code/jqScript/angular-1.0.7/angular.js",
            "~/_Code/jqScript/angular-1.0.7/i18n/angular-locale_zh-tw.js",
            "~/_Code/jqScript/jquery-ui-1.10.3.custom/development-bundle/ui/jquery-ui.custom.js",
            "~/_Code/jqScript/jquery-ui-1.10.3.custom/development-bundle/ui/i18n/jquery.ui.datepicker-zh-TW.js",
            "~/_Code/jqScript/jquery-validation-1.11.0/dist/jquery.validate.js",
            "~/_Code/jqScript/jquery-validation-1.11.0/localization/messages_zh_TW.js",
            "~/_Code/jqScript/jquery.unobtrusive-ajax.js",
            "~/_Code/jqScript/jquery.query-2.1.7.js",
            "~/_Code/jqScript/commfunc.js"));

            bundles.Add(new ScriptBundle("~/jqGrid")
            .Include(
            "~/_Code/jqScript/ui.jquery.jqGrid-4.5.2/js/i18n/grid.locale-tw.js",
            "~/_Code/jqScript/ui.jquery.jqGrid-4.5.2/js/jquery.jqGrid.src.js"));

            bundles.Add(new ScriptBundle("~/fileuploader")
            .Include(
            "~/_Code/jqScript/Fileuploader/jquery.fineuploader-3.0.js"));

            bundles.Add(new ScriptBundle("~/fancybox")
            .Include(
            "~/_Code/jqScript/fancyapps-fancyBox/source/jquery.fancybox.js",
            "~/Scripts/fancyBox/fancyboxset.js"));
            #endregion

            #region CSS

            bundles.Add(new StyleBundle("~/_Code/jqScript/jquery-ui-1.10.3.custom/css/redmond/custom").Include(
            "~/_Code/jqScript/jquery-ui-1.10.3.custom/css/redmond/jquery-ui-1.10.3.custom.css"));

            bundles.Add(new StyleBundle("~/_Code/CSS/commCss").Include(
            "~/_Code/CSS/formStyle.css",
            "~/_Code/CSS/commStyle.css"));

            //jqGrid的css未引用任何圖片，故可任意設定。
            bundles.Add(new StyleBundle("~/css/jqGrid").Include(
            "~/_Code/jqScript/ui.jquery.jqGrid-4.5.2/css/ui.jqgrid.css"));

            bundles.Add(new StyleBundle("~/_Code/jqScript/Fileuploader/css").Include(
            "~/_Code/jqScript/Fileuploader/fineuploader.css"));

            bundles.Add(new StyleBundle("~/_Code/jqScript/fancyapps-fancyBox/source/css").Include(
            "~/_Code/jqScript/fancyapps-fancyBox/source/jquery.fancybox.css"));
            #endregion
        }
    }
}