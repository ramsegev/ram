using System.Web;
using System.Web.Optimization;

namespace RamXML
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/fileinput.js",
                      "~/Scripts/jstree.js",
                      "~/Scripts/jquery.session.js",
                      "~/Scripts/jquery.bootpag.min.js",
                      "~/Scripts/typeahead.bundle.js",
                      "~/Scripts/select2.js",
                      "~/Scripts/jquery.jsPlumb-1.7.2.js",
                      "~/Scripts/jquery.biltong-0.2.js",
                      "~/Scripts/jquery.jsBezier-0.6.js",
                      "~/Scripts/jquery.katavorio-0.4.js",
                      "~/Scripts/jquery.mottle-0.4.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-select.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/fileinput.css",
                      "~/Content/typeaheadjs.css",
                      "~/Content/select2.css",
                      "~/Content/select2-bootstrap.css",
                      "~/Content/Themes/default/style.css",
                      "~/Content/site.css"));
        }
    }
}
