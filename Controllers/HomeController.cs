using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FastEngSite.Controllers
{
    public class HomeController : CommonController
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var model = new MainPageData(GetSessionData());

                return View("~/Views/Home.cshtml", model);
            }
            else
            {
                return View("~/Views/Home.cshtml");
            }
        }
    }
}
