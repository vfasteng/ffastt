using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FastEngSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ThreeDModel_Image",
                url: "ThreeDModel/GetImage/{id}/{userId}",
                defaults: new { controller = "ThreeDModel", action = "GetImage", id = UrlParameter.Optional, userId = UrlParameter.Optional }
            );

            var route = new Route(
                    "{controller}/{action}",
                    new RouteValueDictionary(
                        new { controller = "Home", action = "Index", id = UrlParameter.Optional }),
                        new DashedRouteHandler());
            routes.Add(route);
        }
    }

    public class DashedRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.RouteData.Values["controller"] = requestContext.RouteData.Values["controller"].ToString().Replace("-", "_");
            requestContext.RouteData.Values["action"] = requestContext.RouteData.Values["action"].ToString().Replace("-", "_");
            return base.GetHttpHandler(requestContext);
        }
    }
}