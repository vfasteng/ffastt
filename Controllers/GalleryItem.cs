using CADFEA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastEngSite.Controllers
{
    public class GalleryItemJson
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsEnabled { get; set; }

        public GalleryItemJson()
        {
        }

        public GalleryItemJson(Gallery gallery)
        {
            this.Id = gallery.Id;
            this.Name = System.IO.Path.GetFileName(gallery.Path);
            this.Path = gallery.Path;
            this.IsEnabled = gallery.IsEnabled;
        }
    }
}