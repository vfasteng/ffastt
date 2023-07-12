using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastEngSite.Controllers
{
    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        {
        }
    }
}