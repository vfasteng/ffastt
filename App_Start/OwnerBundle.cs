using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace FastEngSite.App_Start
{
    public class OwnerBundle
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js").Include(
                        "~/Scripts/Typescript/Events/CEvent.js",
                        "~/Scripts/Typescript/Events/Keyboard.js",
                        "~/Scripts/Typescript/UI/MessageBox.js",
                        "~/Scripts/Typescript/ErrorHandler.js",
                        "~/Scripts/Typescript/HtmlFactory.js",

                        "~/Scripts/Typescript/SimGallery/SimGalleryController.js",
                        "~/Scripts/Typescript/SimGallery/SimGallerySliderController.js",
                        "~/Scripts/Typescript/StorageController.js",
                        "~/Scripts/Typescript/AdminController.js",

                        "~/Scripts/Typescript/App.js",
                        "~/Scripts/Typescript/AppStart.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/jquery.horizontal.scroll.css",
                "~/Content/styles.css"
                ));
        }
    }
}