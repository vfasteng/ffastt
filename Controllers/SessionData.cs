using CADFEA.Database;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastEngSite.Controllers
{
    public class SessionData
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        //public AzureStorage AzureStorage { get; private set; }
        public CADFEA.AzureStorage.Storage AzureStorage { get; private set; }

        public object Locker { get; private set; }

        public SessionData(int userId)
        {
            UserId = userId;
          //  AzureStorage = new AzureStorage(userId);
            AzureStorage = new CADFEA.AzureStorage.Storage(CloudConfigurationManager.GetSetting("StorageConnectionString"), userId);
            Locker = new object();

            using (var context = new DatabaseContext())
            {
                var user = context.GetUserById(userId);
                if (user == null)
                {
                    throw new Exception("User Not Found");
                }
                this.UserName = user.Email;
                this.PasswordHash = user.Password;
            }
        }
    }
}