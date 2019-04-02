using System.Web;
using System.Web.Optimization;

namespace Website
{
    public class BundleConfig
    {
        /// <summary>
        /// Correctly handles Url's in virtual directories.
        /// </summary>
        public class CssRewriteUrlTransformWrapper : IItemTransform
        {
            public string Process(string includedVirtualPath, string input)
            {
                return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
            }
        }


        /// <summary>
        /// Registers the bundles.
        /// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Bundle all scripts and styles in to each one file.
            // This will improve performance on client side.
            bundles.Add(new ScriptBundle("~/Scripts/scripts").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/free-jqGrid/jquery.jqgrid.src.js",
                "~/Scripts/free-jqGrid/i18n/grid.locale-en.js",
                "~/Scripts/jquery.prettyPhoto.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/script.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/styles").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/prettyPhoto.css",
                        "~/Content/ui.jqgrid.css",
                        "~/Content/site.css")
            // This changes any urls for assets from within the css file to absolute urls so the bundling doesn't mess up the relative path. 
            // Make sure to delete font-awesome.css otherwise the optimizer will not generate a minified version with the correct path.
            .Include("~/Content/fontawesome/font-awesome.css", new CssRewriteUrlTransformWrapper()));
        }
    }
}