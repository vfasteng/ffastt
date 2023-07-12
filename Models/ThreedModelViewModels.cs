using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FastEngSite.Controllers;

namespace FastEngSite.Models
{
    public class ThreedModelViewModels: MainPageData
    {
        public IList<ImageViewModel> SmallImageViewModels { get; set; }
        public IList<ImageViewModel> MedimumImageViewModels { get; set; }
        public IList<ImageViewModel> LargeImageViewModels { get; set; }
        public IList<ImageViewModel> ExtraLargeImageViewModels { get; set; }
        
        public ThreedModelViewModels(SessionData session) : base(session)
        {
        }
    }
}