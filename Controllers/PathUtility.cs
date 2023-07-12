using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FastEngSite.Controllers
{
    class PathUtility
    {
        public static String AssemblyFileVersion
        {
            get
            {
                String productVersion = String.Empty;
                System.Reflection.Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                object[] customAttributes = assembly.GetCustomAttributes(
                    typeof(System.Reflection.AssemblyFileVersionAttribute), false);
                if ((customAttributes.Length > 0))
                {
                    productVersion = ((AssemblyFileVersionAttribute)customAttributes[0]).Version;
                }
                if (String.IsNullOrEmpty(productVersion))
                {
                    productVersion = String.Empty;
                }
                return productVersion;
            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                String codeBase = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                String path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        public static string App_Data
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/App_Data/");
            }
        }

        public static string GetValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Parameter is null or empty");
            }
            string outName = "";
            string notValidSymbols = "\\<>/?\":;*|,=' ";
            foreach (char ch in name)
            {
                if (notValidSymbols.Contains(ch))
                {
                    continue;
                }
                outName += ch;
            }
            if (string.IsNullOrEmpty(outName))
            {
                return "_";
            }
            return outName.Trim();
        }
    }
}