using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastEngSite.Controllers
{

    public class AdminController : CommonController
    {
        const int LogoWidth = 200;

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var model = new MainPageData(GetSessionData());

                return View("~/Views/Admin.cshtml", model);
            }
            else
            {
                return View("~/Views/Admin.cshtml");
            }
        }

        public void GetLogo()
        {
            try
            {
                SessionData session = GetSessionData();
                var image = session.AzureStorage.GetFile(CADFEA.AzureStorage.Storage.SettingsFolder + "/" + "logo.png");
                if (image != null)
                {
                    Response.ClearContent();
                    Response.ContentType = "image/png";
                    Response.OutputStream.Write(image, 0, image.Length);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public ActionResult CheckLogo()
        {
            var result = false;
            try
            {
                SessionData session = GetSessionData();
                result = session.AzureStorage.FileExists(CADFEA.AzureStorage.Storage.SettingsFolder + "/" + "logo.png");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadLogo()
        {
            try
            {
                SessionData session = GetSessionData();

                using (Stream receiveStream = Request.InputStream)
                {
                    var buf = new byte[receiveStream.Length];
                    int readCnt = receiveStream.Read(buf, 0, (int)receiveStream.Length);
                    if (readCnt != receiveStream.Length)
                    {
                        throw new Exception("Can't read upload file");
                    }
                    Bitmap bmp;
                    using (var ms = new MemoryStream(buf))
                    {
                        bmp = new Bitmap(ms);
                        if (bmp.Size.Width > LogoWidth)
                        {
                            var koef = bmp.Size.Width / LogoWidth;
                            var height = (int)(bmp.Size.Height / koef);
                            bmp = new Bitmap(bmp, new Size(LogoWidth, height));
                            using (var saveMemoryStream = new MemoryStream())
                            {
                                bmp.Save(saveMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                buf = saveMemoryStream.ToArray();
                            }
                        }
                    };
                    session.AzureStorage.CreateFile(CADFEA.AzureStorage.Storage.SettingsFolder, "logo.png", buf);
                }

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }
    }
}
