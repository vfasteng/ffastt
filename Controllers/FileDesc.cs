using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastEngSite.Controllers
{
    public class FileDesc
    {
        public string Name { get; set; }

        public string FilePath { get; set; }

        public FileDesc() { }

        public FileDesc(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }
    }
}