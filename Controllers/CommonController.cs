using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using CADFEA.AzureStorage;
using Microsoft.WindowsAzure;

namespace FastEngSite.Controllers
{
    public class CommonController : Controller
    {
        public static object _gLocker = new object();

        public ActionResult JsonSuccess
        {
            get
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetJsonFail(Exception ex)
        {
            if (ex is UserException)
            {
                return Json(new { success = false, message = ex.Message, stackTrace = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string innerMsg = String.Empty;
                if (ex.InnerException != null)
                {
                    innerMsg = "Inner Exception:" + ex.InnerException.Message + " " + ex.InnerException.StackTrace;
                }
                return Json(new { success = false, message = ex.Message, stackTrace = ex.StackTrace + innerMsg }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetJsonFail(string  message)
        {
            return Json(new { success = false, message = message, stackTrace = "" }, JsonRequestBehavior.AllowGet);
        }

        public SessionData GetSessionData()
        {
            lock (_gLocker)
            {
                SessionData session = this.Session["Session"] as SessionData;
                if (session == null)
                {
                    session = CreateNewSession();
                }
                return session;
            }
        }

        public SessionData CreateNewSession()
        {
            lock (_gLocker)
            {
                if (User == null || !User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    throw new Exception("Access denied");
                }
                string idStr = User.Identity.Name;
                if (String.IsNullOrEmpty(User.Identity.Name))
                {
                    FormsAuthentication.SignOut();
                    throw new Exception("Access denied");
                }
                int userId = Int32.Parse(idStr);


                var session = new SessionData(userId);
                Session["Session"] = session;
                return session;
            }
        }
        
        public string GetInpusStreamAsString()
        {
            string strData = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            if (String.IsNullOrEmpty(strData))
            {
                throw new Exception("Input data is empty");
            }
            return strData;
        }

        public T GetInpusStreamAs<T>() where T : class
        {
            string strData = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            if (String.IsNullOrEmpty(strData))
            {
                throw new Exception("Input data is empty");
            }
            return new JavaScriptSerializer().Deserialize<T>(strData);
        }

        public Storage GetStorage(int userId)
        {
            var sessionKey = string.Format("AzureStorage_{0}", userId);
            if (Session[sessionKey] == null)
            {
                var storage =
                    new CADFEA.AzureStorage.Storage(CloudConfigurationManager.GetSetting("StorageConnectionString"),
                        userId);

                Session[sessionKey] = storage;
            }

            return Session[sessionKey] as Storage;
        }
    }
}