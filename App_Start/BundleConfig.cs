using System.Web;
using System.Web.Optimization;

namespace Euro
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Render Css for Global use
            BundleTable.EnableOptimizations = false;
            bundles.Add(new StyleBundle("~/Assets/css/vendor").Include(
                      "~/Assets/css/vendor/bootstrap.min.css",
                      "~/Assets/css/vendor/animate.css",
                      "~/Assets/css/vendor/font-awesome.min.css",
                      "~/Assets/js/vendor/animsition/css/animsition.min.css",
                      "~/Assets/js/vendor/chosen/chosen.css",
                      "~/Assets/js/vendor/morris/morris.css",
                      "~/Assets/js/vendor/owl-carousel/owl.carousel.css",
                      "~/Assets/js/vendor/owl-carousel/owl.theme.css",
                      "~/Assets/js/vendor/touchspin/jquery.bootstrap-touchspin.min.css" ,
                      "~/Assets/js/vendor/datetimepicker/css/bootstrap-datetimepicker.min.css",
                      "~/Assets/js/vendor/toastr/toastr.min.css",
                      "~/Assets/js/vendor/summernote/summernote.css",
                      "~/Assets/js/vendor/bootstrap-tags-input/bootstrap-tagsinput.css",
                      "~/Assets/js/vendor/file-upload/css/jquery.fileupload.css",
                      "~/Assets/js/vendor/file-upload/css/jquery.fileupload-ui.css",
                      "~/Assets/js/vendor/fullcalendar/fullcalendar.css"
                      ));

            bundles.Add(new ScriptBundle("~/Assets/css").Include(
                      "~/Assets/css/main.css"
                      ));

            bundles.Add(new ScriptBundle("~/Assets/modernizr").Include(
                        "~/Assets/js/vendor/modernizr/modernizr-2.8.3-respond-1.4.2.min.js"
                     ));


            // Render JS for Global Use
                
                  bundles.Add(new ScriptBundle("~/Assets/js/vedor").Include(
                        "~/Assets/js/vendor/jquery/jquery-1.11.2.min.js",
                        "~/Assets/js/vendor/bootstrap/bootstrap.min.js",
                        "~/Assets/js/vendor/jRespond/jRespond.min.js",
                        "~/Assets/js/vendor/sparkline/jquery.sparkline.min.js",
                        "~/Assets/js/vendor/slimscroll/jquery.slimscroll.min.js",
                        "~/Assets/js/vendor/animsition/js/jquery.animsition.min.js",
                        "~/Assets/js/vendor/morris/morris.min.js", 
                        "~/Assets/js/vendor/morris/morris.js", 
                        "~/Assets/js/vendor/screenfull/screenfull.min.js",
                        "~/Assets/js/vendor/parsley/parsley.min.js",
                        "~/Assets/js/vendor/daterangepicker/moment.min.js",
                        "~/Assets/js/vendor/datetimepicker/js/bootstrap-datetimepicker.min.js",
                        "~/Assets/js/vendor/chosen/chosen.jquery.js",
                        "~/Assets/js/vendor/toastr/toastr.min.js",
                        "~/Assets/js/vendor/owl-carousel/owl.carousel.min.js",
                         "~/Assets/js/vendor/summernote/summernote.min.js",
                         "~/Assets/js/vendor/bootstrap-tags-input/bootstrap-tagsinput.min.js",
                         "~/Assets/js/vendor/file-upload/js/vendor/jquery.ui.widget.js",
                         "~//blueimp.github.io/JavaScript-Templates/js/tmpl.min.js",
                         "~//blueimp.github.io/JavaScript-Load-Image/js/load-image.all.min.js",
                         "~/Assets/js/vendor/file-upload/js/jquery.iframe-transport.js",
                         "~/Assets/js/vendor/file-upload/js/jquery.fileupload.js",
                         "~/Assets/js/vendor/file-upload/js/jquery.fileupload-process.js",
                         "~/Assets/js/vendor/file-upload/js/jquery.fileupload-image.js",
                         "~/Assets/js/vendor/file-upload/js/jquery.fileupload-validate.js",
                         "~/Assets/js/vendor/file-upload/js/jquery.fileupload-ui.js",
                         "~/Assets/js/vendor/fullcalendar/lib/moment.min.js",
                         "~/Assets/js/vendor/fullcalendar/lib/jquery-ui.custom.min.js",
                         "~/Assets/js/vendor/fullcalendar/fullcalendar.min.js",
                         "~/Assets/js/vendor/filestyle/bootstrap-filestyle.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/Assets/js/main").Include(
                        "~/Assets/js/main.js"
                     ));
            bundles.Add(new ScriptBundle("~/Assets/js/vendor/datatablesjs").Include(
                        "~/Assets/js/vendor/datatables/js/jquery.dataTables.min.js",
                        "~/Assets/js/vendor/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js",
                        "~/Assets/js/vendor/datatables/extensions/Responsive/js/dataTables.responsive.min.js",
                        "~/Assets/js/vendor/datatables/extensions/ColVis/js/dataTables.colVis.min.js",
                        "~/Assets/js/vendor/datatables/extensions/TableTools/js/dataTables.tableTools.min.js",
                        "~/Assets/js/vendor/datatables/extensions/dataTables.bootstrap.js"
                     ));
            bundles.Add(new StyleBundle("~/Assets/js/vendor/datatables").Include(
                      "~/Assets/js/vendor/datatables/css/jquery.dataTables.min.css",
                      "~/Assets/js/vendor/datatables/datatables.bootstrap.min.css",
                      "~/Assets/js/vendor/datatables/extensions/ColReorder/css/dataTables.colReorder.min.css",
                      "~/Assets/js/vendor/datatables/extensions/Responsive/css/dataTables.responsive.css",
                      "~/Assets/js/vendor/datatables/extensions/ColVis/css/dataTables.colVis.min.css",
                      "~/Assets/js/vendor/datatables/extensions/TableTools/css/dataTables.tableTools.min.css"
                      ));
        }
    }
}
