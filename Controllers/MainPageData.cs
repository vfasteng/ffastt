using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastEngSite.Controllers
{
    public class MainPageData
    {
        public string PaypalUrl { get; set; }

        public string PayPalBusiness { get; set; }

        public string UserName { get; set; }

        public string UserNamehash { get; set; }

        public string PasswordHash { get; set; }

        public MainPageData(SessionData session)
        {
            if (session != null)
            {
                this.UserName = session.UserName;
                this.UserNamehash = EnryptString(session.UserName);
                this.PasswordHash = session.PasswordHash;
            }

            this.PaypalUrl = System.Configuration.ConfigurationManager.AppSettings["PayPalSubmitUrl"];
            this.PayPalBusiness = System.Configuration.ConfigurationManager.AppSettings["PayPalBusiness"];
        }

        private string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encryptedPassword = Convert.ToBase64String(b);
            return encryptedPassword;
        }
    }
}