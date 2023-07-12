using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastEngSite.Controllers
{
    public class AnimationController : CommonController
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var model = new MainPageData(GetSessionData());

                return View("~/Views/Animation.cshtml", model);
            }
            else
            {
                return View("~/Views/Animation.cshtml");
            }
        }
    }
}
