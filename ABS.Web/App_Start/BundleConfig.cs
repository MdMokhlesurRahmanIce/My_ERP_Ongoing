using System.Web.Optimization;

namespace ABS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/JavaScript").Include(
                "~/Themes/assets/js/libs/jquery-1.10.2.min.js",
                "~/Themes/plugins/jquery-ui/jquery-ui-1.10.2.custom.min.js",
                "~/Themes/bootstrap/js/bootstrap.min.js",
                "~/Themes/assets/js/libs/lodash.compat.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/Smartphone").Include(
                "~/Themes/plugins/touchpunch/jquery.ui.touch-punch.min.js",
                "~/Themes/plugins/event.swipe/jquery.event.move.js",
                "~/Themes/plugins/event.swipe/jquery.event.swipe.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/General").Include(
                "~/Themes/assets/js/libs/breakpoints.js",
                "~/Themes/plugins/respond/respond.min.js",
                "~/Themes/plugins/cookie/jquery.cookie.min.js",
                "~/Themes/plugins/slimscroll/jquery.slimscroll.min.js",
                "~/Themes/plugins/slimscroll/jquery.slimscroll.horizontal.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/Page").Include(
                "~/Themes/plugins/sparkline/jquery.sparkline.min.js",
                "~/Themes/plugins/flot/jquery.flot.min.js",
                "~/Themes/plugins/flot/jquery.flot.tooltip.min.js",
                "~/Themes/plugins/flot/jquery.flot.resize.min.js",
                "~/Themes/plugins/flot/jquery.flot.time.min.js",
                "~/Themes/plugins/flot/jquery.flot.growraf.min.js",
                "~/Themes/plugins/easy-pie-chart/jquery.easy-pie-chart.min.js",
                "~/Themes/plugins/daterangepicker/moment.min.js",
                "~/Themes/plugins/daterangepicker/daterangepicker.js",
                "~/Themes/plugins/blockui/jquery.blockUI.min.js",
                "~/Themes/plugins/fullcalendar/fullcalendar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/picker").Include(
                "~/Themes/plugins/pickadate/picker.js",
                "~/Themes/plugins/pickadate/picker.date.js",
                "~/Themes/plugins/pickadate/picker.time.js",
                "~/Themes/plugins/bootstrap-colorpicker/bootstrap-colorpicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/Noty").Include(
                "~/Themes/plugins/noty/jquery.noty.js",
                "~/Themes/plugins/noty/layouts/top.js",
                "~/Themes/plugins/noty/themes/default.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/Forms").Include(
                "~/Themes/plugins/uniform/jquery.uniform.min.js",
                //"~/Themes/plugins/select2/select2.min.js",
                "~/Themes/plugins/select2/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/App").Include(
                "~/Themes/assets/js/app.js",
                "~/Themes/assets/js/plugins.js",
                "~/Themes/assets/js/plugins.form-components.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins/Demo").Include(
                "~/Themes/assets/js/custom.js",
                "~/Themes/assets/js/demo/ui_general.js"));

            bundles.Add(new ScriptBundle("~/bundles/Angular").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-route.min.js",
                "~/Scripts/ui-grid.min.js",
                "~/Scripts/angular-touch.min.js",
                "~/Scripts/angular-animate.min.js"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include(
                "~/Themes/bootstrap/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/bundles/custom/css").Include(
                "~/Themes/assets/css/CustomStyles.css"));

            bundles.Add(new StyleBundle("~/bundles/ui-grid/css").Include(
                "~/Content/ui-grid.min.css"));

            bundles.Add(new StyleBundle("~/bundles/assets/css").Include(
                "~/Themes/assets/css/main.css",
                "~/Themes/assets/css/plugins.css",
                "~/Themes/assets/css/responsive.css",
                "~/Themes/assets/css/icons.css",
                "~/Themes/assets/css/fontawesome/font-awesome.min.css"));


            //Only For Accounting! Other Don't Need to..
            bundles.Add(new ScriptBundle("~/bundles/plugins/AccForms").Include(
                "~/Themes/plugins/uniform/jquery.uniform.min.js",
               "~/Scripts/Select21/select2.min.js"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap/Acss").Include(
             "~/Themes/bootstrap/css/bootstrap.min.css",
             "~/Scripts/Select21/select21.min.css",
             "~/Scripts/Select21/select21-bootstrap.css"
             ));
            bundles.Add(new ScriptBundle("~/bundles/Script/CustomScript").Include(
           "~/Themes/plugins/nprogress/nprogress.js",
           "~/Scripts/uy.script.js"
           ));
            bundles.Add(new ScriptBundle("~/bundles/plugins/Datatable").Include(
                "~/Themes/plugins/datatables/jquery.dataTables.min.js"
                , "~/Themes/plugins/datatables/tabletools/TableTools.min.js"
                , "~/Themes/plugins/datatables/colvis/ColVis.min.js",
                "~/Themes/plugins/datatables/DT_bootstrap.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/plugins/Validation").Include(
                  "~/Scripts/jquery.unobtrusive-ajax.min.js",
                  "~/Scripts/jquery.validate.min.js",
                  "~/Scripts/jquery.validate.unobtrusive.min.js",
                  "~/Scripts/jquery.validate.unobtrusive.bootstrap.min.js"
                ));


        }
    }
}
