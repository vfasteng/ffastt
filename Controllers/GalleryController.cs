using CADFEA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace FastEngSite.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class GalleryController : CommonController
    {
        public ActionResult GetItems()
        {
            try
            {
                if (!Request.IsAuthenticated)
                {
                    return null;
                }
                var session = GetSessionData();
                lock (session.Locker)
                {
                    List<string> filenames = session.AzureStorage.GetAllFiles(CADFEA.AzureStorage.Storage.GalleryFolder);
                    List<GalleryItemJson> outItems = new List<GalleryItemJson>();

                    using (var context = new DatabaseContext())
                    {
                        var user = context.GetUserById(session.UserId);
                        var gallaries = user.Gallery.ToList();

                        foreach (string filename in filenames)
                        {
                            string galleryFolder = "gallery/";
                            if (filename.ToLower().StartsWith(galleryFolder))
                            {
                                string outFilename = filename.Remove(0, galleryFolder.Length);

                                Gallery gallery = gallaries.FirstOrDefault(x => x.Path == filename);
                                if (gallery == null)
                                {
                                    // Add to database new Gallery Item
                                    gallery = new Gallery();
                                    gallery.Path = filename;
                                    gallery.User = user;
                                    gallery.IsEnabled = false;

                                    gallery = context.AddGallery(gallery);
                                }

                                outItems.Add(new GalleryItemJson(gallery));
                            }
                        }
                    }

                    return Json(outItems, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

        [HttpPost]
        public ActionResult SaveItemsStates()
        {
            try
            {
                var session = GetSessionData();

                lock (session.Locker)
                {
                    var items = GetInpusStreamAs<List<GalleryItemJson>>();
                    if (items != null && items.Count > 0)
                    {
                        // Save states to database
                        using (var context = new DatabaseContext())
                        {
                            var user = context.GetUserById(session.UserId);
                            foreach (var item in items)
                            {
                                Gallery galeryItem = user.Gallery.FirstOrDefault(x => x.Id == item.Id);
                                if (galeryItem == null)
                                {
                                    continue;
                                }
                                galeryItem.IsEnabled = item.IsEnabled;
                            }
                            context.Save();
                        }
                    }
                    return JsonSuccess;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

        public ActionResult GetImage()
        {
            try
            {
                string idStr = Request.Params["id"];
                if (String.IsNullOrEmpty(idStr))
                {
                    throw new ArgumentNullException("Argument 'id' is empty");
                }
                int id = int.Parse(idStr);

                byte[] bytes;
                string path;
                var session = GetSessionData();
                using (var database = new DatabaseContext())
                {
                    var user = database.GetUserById(session.UserId);
                    var galleryItem = user.Gallery.FirstOrDefault(x => x.Id == id);
                    if (galleryItem == null)
                    {
                        return null;
                    }
                    bytes = session.AzureStorage.GetFile(galleryItem.Path);
                    path = galleryItem.Path;
                }

                string contentType = MimeMapping.GetMimeMapping(path);
                return File(bytes, contentType);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

    }
}
