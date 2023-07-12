using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace FastEngSite.App_Start
{
    public class BootstrapBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/PureJs/bootstrap.js",
                "~/Scripts/PureJs/bootstrap-select.js",
                "~/Scripts/PureJs/bootstrap-treeview.js"
                ));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Content/body.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-mvc-validation.css",
                "~/Content/bootstrap-select.css",
                "~/Content/bootstrap-treeview.css"
                ));
        }
    }
}