using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastEngSite.Models
{
    public class ImageViewModel
    {
        public int ImageId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string ExternalUrl { get; set; }
    }
}