using System.Web;
using System.Web.Optimization;

namespace AndroAdmin.AppStart
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundler is turned off as css img links break ie they are relative to the odd new position of the css file(s)! 
            
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            //bundles.Add(new StyleBundle("~/styles/myAndromeda/css").Include("~/Content/myAndromeda/site.css"));


            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                        "~/Content/kendo/kendo.common.min.css",
                        "~/Content/kendo/kendo.dataviz.min.css",
                        "~/Content/kendo/kendo.silver.min.css",
                        "~/Content/kendo/kendo.dataviz.silver.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                        "~/Scripts/kendo/jquery.min.js",
                        "~/Scripts/kendo/kendo.all.min.js",
                        "~/Scripts/kendo/kendo.aspnetmvc.min.js",
                        "~/Scripts/kendo/cultures/kendo.culture.en-GB.min.js",
                        "~/Scripts/kendo.modernizr.custom.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}