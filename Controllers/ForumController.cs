using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastEngSite.Controllers
{
    public class ForumController : CommonController
    {
        //
        // GET: /Forum/
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var model = new MainPageData(GetSessionData());

                return View("~/Views/Forum.cshtml", model);
            }
            else
            {
                return View("~/Views/Forum.cshtml");
            }
        }
    }
}
