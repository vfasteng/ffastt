using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace FastEngSite.App_Start
{
    public class PluginsBundle
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                "~/Scripts/PureJs/underscore.js",
                "~/Scripts/PureJs/angular.js",
                "~/Scripts/PureJs/hashset.js",
                "~/Scripts/PureJs/hashtable.js",
                "~/Scripts/PureJs/SimpleAjaxUploader.js"
                ));

            bundles.Add(new StyleBundle("~/Content/stylePlugins").Include(
                "~/Content/Ribbon/angular-csp.css"
                ));
        }
    }
}