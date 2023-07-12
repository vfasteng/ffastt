using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FastEngSite.Controllers
{
    public class LoginController : CommonController
    {
        [HttpPost]
        public ActionResult Login()
        {
            try
            {
                string email = Request.Params["name"];
                if (String.IsNullOrEmpty(email))
                {
                    throw new ArgumentNullException("Argument 'name' is empty");
                }

                string password = Request.Params["password"];
                if (String.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException("Argument 'password' is empty");
                }

                int userId;
                using (var database = new CADFEA.Database.DatabaseContext())
                {
                    if (!database.CheckUser(email, password))
                    {
                        throw new UserException("Username or Password not valid");
                    }
                    userId = database.GetUserByEmail(email).Id;
                }

                FormsAuthentication.SetAuthCookie(userId.ToString(), true);

                
                // Kill current session
                Session["Session"] = null;
                
                return JsonSuccess;
            }
            catch (Exception ex)
            {
                return GetJsonFail(ex);
            }
        }

        [HttpPost]
        public ActionResult CheckUserLicense()
        {
            try
            {
                string name = Request.Params["name"];
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("Argument 'name' is empty");
                }

                string password = Request.Params["password"];
                if (String.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException("Argument 'password' is empty");
                }

                using (var database = new CADFEA.Database.DatabaseContext())
                {
                    if (!database.CheckUser(name, password))
                    {
                        throw new UserException("Username or Password not valid");
                    }

                    var user = database.GetUserByEmail(name);
                    if (!user.License.Any(x => x.LeftMinutes > 0))
                    {
                        throw new Exception("Licenses invalid");
                    }
                }

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail("Fail");
            }
        }

        public ActionResult CreateAccount()
        {
            if (Request.IsAuthenticated)
            {
                var model = new MainPageData(GetSessionData());
                return View("~/Views/SignUpPage.cshtml", model);
            }
            else
            {
                return View("~/Views/SignUpPage.cshtml");
            }
        }

        [HttpPost]
        public ActionResult Register()
        {
            try
            {
                string email = Request.Params["email"];
                if (String.IsNullOrEmpty(email))
                {
                    throw new ArgumentNullException("Argument 'email' is empty");
                }

                string password = Request.Params["password"];
                if (String.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException("Argument 'password' is empty");
                }

                int userId;
                using (var database = new CADFEA.Database.DatabaseContext())
                {
                    var user = database.CreateUser(email, password);
                    userId = user.Id;

                    string hoursStr = ConfigurationManager.AppSettings["Product1yearHours"];
                    int hours = int.Parse(hoursStr);
                    string txn_id = "";
                    database.CreateLicense(user, hours, txn_id);

                    Logger.Info("License payment confirm: " + user.Email + " Hours:" + hours + " txn_id:" + txn_id);
                }
              
                

                FormsAuthentication.SetAuthCookie(userId.ToString(), true);

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                return GetJsonFail(ex);
            }
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            try
            {
                FormsAuthentication.SignOut();

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                return GetJsonFail(ex);
            }
        }
        private string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encryptedPassword = Convert.ToBase64String(b);
            return encryptedPassword;
        }

    }// LoginController

}
