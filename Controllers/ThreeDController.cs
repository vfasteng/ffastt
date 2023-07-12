using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CADFEA.Database;
using FastEngSite.Models;
using NLog.Internal;

namespace FastEngSite.Controllers
{
    public class ThreeDModelController : CommonController
    {
        //
        // GET: /3D/

        public ActionResult Index()
        {
            ThreedModelViewModels model;

            model = Request.IsAuthenticated ? new ThreedModelViewModels(GetSessionData()) : new ThreedModelViewModels(null);

            model.SmallImageViewModels = GetImages(System.Configuration.ConfigurationManager.AppSettings["modelssmall"]);
            model.MedimumImageViewModels = GetImages(System.Configuration.ConfigurationManager.AppSettings["modelmedium"]);
            model.LargeImageViewModels = GetImages(System.Configuration.ConfigurationManager.AppSettings["modellarge"]);
            model.ExtraLargeImageViewModels = GetImages(System.Configuration.ConfigurationManager.AppSettings["smallmodels"]);


            return View(model);
        }

        [HttpGet]
        public ActionResult GetImage(int id, int userId)
        {
            try
            {
                var galarryFolder = String.Format("{0}/{1}/", CADFEA.AzureStorage.Storage.StorageFolder, "gallery");

                byte[] bytes;
                string path;
                var storage = GetStorage(userId);
                
                using (var database = new DatabaseContext())
                {
                    var user = database.GetUserById(userId);
                    var galleryItem = user.Gallery.FirstOrDefault(x => x.Id == id);
                    if (galleryItem == null)
                    {
                        return null;
                    }
                    bytes = storage.GetFile(galleryItem.Path);
                    path = galleryItem.Path;
                }

                string contentType = MimeMapping.GetMimeMapping(path);
                return File(bytes, contentType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult UpLoadThreeDModel(ModelUploadViewModel model)
        {
            try
            {

                byte[] bytes;
                string path;
                var storage = GetStorage(model.ModelTypeUserId);
                using (var database = new DatabaseContext())
                {
                  //  storage.CreateFile(storage);
                    //var user = database.GetUserById(model.ModelTypeUserId);
                    //var galleryItem = user.Gallery.FirstOrDefault(x => x.Id == id);
                    //if (galleryItem == null)
                    //{
                    //    return null;
                    //}
                    //bytes = storage.GetFile(galleryItem.Path);
                    //path = galleryItem.Path;
                }

                //string contentType = MimeMapping.GetMimeMapping(path);
                //return File(bytes, contentType);
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        private List<ImageViewModel> GetImages(string email)
        {
            try
            {
                var images = new List<ImageViewModel>();

                using (var context = new DatabaseContext())
                {

                    var models = context.GetThreeDModels(email);

                    foreach (var model in models)
                    {
                        var gallery = model.GallerySet;
                        images.Add(new ImageViewModel
                        {
                            ImageId = gallery.Id,
                            ExternalUrl = gallery.Path,
                            UserId = gallery.User_Id,
                            Title = model.ModelName
                        });
                    }

                    return images;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new List<ImageViewModel>();
            }
        }
    }
}
