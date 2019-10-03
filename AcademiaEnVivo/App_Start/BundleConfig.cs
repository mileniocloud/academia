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
                        "~/Scripts/jquery-3.4.1.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/pixeladmin.min.js",
                        "~/Scripts/pace.min.js",
                        "~/Scripts/sweetalert.min.js",
                        "~/Scripts/mensajes.js",
                        "~/Scripts/Comun.js",
                        "~/Scripts/jquery.blockUI.js"
                        //"~/Scripts/mensajes.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/pixeladmin.min.css",
                      "~/Content/widgets.min.css",
                      "~/Content/clean.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/custom.css"
                      //"~/Content/jquery-ui.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/chatjs").Include(
                        "~/Scripts/jquery-3.4.1.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/pixeladmin.min.js",
                        "~/Scripts/pace.min.js",
                        "~/Scripts/sweetalert.min.js",
                        "~/Scripts/mensajes.js",                        
                        "~/Scripts/jquery.signalR-2.4.1.min.js",                        
                        "~/Scripts/emojionearea.js",
                        "~/signalr/hubs"
                        //"~/Scripts/Chat.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/chatcss").Include(
                     "~/Content/emojionearea.min.css",
                     "~/Content/chat.css"
                     ));
        }
    }
}

