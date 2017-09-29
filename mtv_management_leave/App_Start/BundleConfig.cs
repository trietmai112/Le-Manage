using System.Web;
using System.Web.Optimization;

namespace mtv_management_leave
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/materialstyle").Include(
                    "~/Content/vendors/bower_components/animate.css/animate.min.css",
                    "~/Content/vendors/bower_components/sweetalert2/dist/sweetalert2.min.css",
                    "~/Content/vendors/bower_components/material-design-iconic-font/dist/css/material-design-iconic-font.min.css",
                    "~/Content/vendors/bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.min.css",
                    "~/Content/vendors/bower_components/bootstrap-select/dist/css/bootstrap-select.css",
                    "~/Content/vendors/bower_components/nouislider/distribute/nouislider.min.css",
                    "~/Content/vendors/bower_components/eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.min.css",
                    "~/Content/vendors/bower_components/dropzone/dist/min/dropzone.min.css",
                    "~/Content/vendors/farbtastic/farbtastic.css",
                    "~/Content/vendors/bower_components/chosen/chosen.css",
                    "~/Content/vendors/summernote/dist/summernote.css",
                    "~/Content/vendors/bower_components/datatables.net-dt/css/jquery.dataTables.min.css",
                    "~/Content/Site.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/materialscript").Include(
                    "~/Content/vendors/bower_components/jquery/dist/jquery.min.js",
                    "~/Content/vendors/bower_components/bootstrap/dist/js/bootstrap.min.js",
                    "~/Content/vendors/bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js",
                    "~/Content/vendors/bower_components/Waves/dist/waves.min.js",
                    "~/Content/vendors/bootstrap-growl/bootstrap-growl.min.js",
                    "~/Content/vendors/bower_components/sweetalert2/dist/sweetalert2.min.js",
                    "~/Content/vendors/bower_components/datatables.net/js/jquery.dataTables.min.js",
                    "~/Content/vendors/bower_components/jquery-placeholder/jquery.placeholder.min.js",
                    "~/Content/vendors/bower_components/moment/min/moment.min.js",
                    "~/Content/vendors/bower_components/bootstrap-select/dist/js/bootstrap-select.js",
                    "~/Content/vendors/bower_components/nouislider/distribute/nouislider.min.js",
                    "~/Content/vendors/bower_components/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
                    "~/Content/vendors/bower_components/typeahead.js/dist/typeahead.bundle.min.js",
                    "~/Content/vendors/bower_components/dropzone/dist/min/dropzone.min.js",
                    "~/Content/vendors/summernote/dist/summernote-updated.min.js",
                    "~/Content/vendors/bower_components/jquery-placeholder/jquery.placeholder.min.js",
                    "~/Content/vendors/bower_components/chosen/chosen.jquery.js",
                    "~/Content/vendors/bower_components/jquery-mask-plugin/dist/jquery.mask.min.js",
                    "~/Content/vendors/fileinput/fileinput.min.js",
                    "~/Content/vendors/farbtastic/farbtastic.min.js",
                    "~/Content/js/app.min.js"
                ));

            //BundleTable.EnableOptimizations = true;
        }
    }
}
