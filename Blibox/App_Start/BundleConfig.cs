using System.Web;
using System.Web.Optimization;

namespace Blibox
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                     "~/plugins/morris/morris.min.js",
                     "~/Content/dist/js/app.min.js",            //AdminLTE for demo purposes
                     "~/Content/dist/js/demo.js",               //  AdminLTE dashboard demo (This is only for demo purposes) 
                     "~/Content/dist/js/app.min.js",            //AdminLTE App
                     "~/plugins/fastclick/fastclick.min.js",    //FastClick
                     "~/plugins/slimScroll/jquery.slimscroll.min.js",
                     "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                     "~/plugins/datepicker/bootstrap-datepicker.js",
                     "~/plugins/daterangepicker/daterangepicker.js",
                     "~/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                     "~/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~/plugins/sparkline/jquery.sparkline.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include(
                      "~/Content/bootstrap/bootstrap.css",
                      "~/Content/bootstrap/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/plugins/css").Include(
                      "~/plugins/morris/morris.css", //Morris chart
                      "~/plugins/jvectormap/jquery-jvectormap-1.2.2.css", //jvectormap
                      "~/plugins/datepicker/datepicker3.css", //Date Picker
                      "~/plugins/daterangepicker/daterangepicker-bs3.css", //Daterange picker
                      "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css", //bootstrap wysihtml5 - text editor
                      "~/plugins/iCheck/flat/blue.css") //iCheck
                      );
        }
    }
}
