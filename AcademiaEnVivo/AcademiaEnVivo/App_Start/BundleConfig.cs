using System.Web;
using System.Web.Optimization;

namespace AcademiaEnVivo
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-3.3.1.js",
                        "~/Scripts/pixeladmin.min.js",
                        "~/Scripts/bootstrap.min.js",                        
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/timepicker.min.js",
                        "~/Scripts/pace.min.js",
                        "~/Scripts/app.js",
                        "~/Scripts/jquery.validate*"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery.timepicker.min.css",
                      "~/Content/pixeladmin.min.css",
                      "~/Content/widgets.min.css",
                      "~/Content/clean.min.css",
                      "~/Content/ionicons.min.css"
                      ));


            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


        }
    }
}
