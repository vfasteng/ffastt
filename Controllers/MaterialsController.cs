using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastEngSite.Controllers
{
    public class MaterialsController : CommonController
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var model = new MainPageData(GetSessionData());

                return View("~/Views/Materials.cshtml", model);
            }
            else
            {
                return View("~/Views/Materials.cshtml");
            }
        }

    }
}
