using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastEngSite.Models
{
    public class ModelUploadViewModel
    {
        public int ModelTypeUserId { get; set; }
        public int UploadedBy    { get; set; }
        public string ModelType { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string ModelName { get; set; }
        public string ModelUrl { get; set; }
        public string Description { get; set; }
    }
}