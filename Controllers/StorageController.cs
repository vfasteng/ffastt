using CADFEA.AzureStorage;
using CADFEA.Database;
using FastEngSite.Controllers.JsonItems;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastEngSite.Controllers
{
    [Authorize]
    public class StorageController : CommonController
    {
        public ActionResult Index()
        {
            var model = new MainPageData(GetSessionData());
            return View("~/Views/StorageManager.cshtml", model);
        }

        public ActionResult GetFiles()
        {
            try
            {
                string folder = Request.Params["folder"] ?? String.Empty;

                var session = GetSessionData();
                if (String.IsNullOrEmpty(folder))
                {
                    folder = String.Format("{0}/", CADFEA.AzureStorage.Storage.StorageFolder);
                }
                else
                {
                    folder = String.Format("{0}/{1}/", CADFEA.AzureStorage.Storage.StorageFolder, folder);
                }
                List<string> files = session.AzureStorage.GetAllFiles(folder);
                List<FileDesc> outFileDesc = new List<FileDesc>();
                foreach (string file in files)
                {
                    string ext = Path.GetExtension(file).ToLower();
                    var extensions = new[] { ".xfrd", ".protobuf", ".txt", ".frd", ".inp", ".smesh", ".node", ".face", ".ele", ".edge", ".off", ".idtf", ".u3d", ".sys" };
                    
                    if (extensions.Contains(ext))
                    {
                        // Skip system files
                        continue;
                    }
                    if (file.Contains("/bctool/") ||
                        file.Contains("/tool/") ||
                        file.Contains("/meshing/"))
                    {
                        continue;
                    }

                    var parts = file.Split('/').ToList();
                    parts.RemoveAt(parts.Count - 1);
                    var dir = String.Join("/", parts) + "/";
                    dir = HttpUtility.UrlDecode(dir);
                    if (dir != folder)
                    {
                        continue;
                    }
                    var fileDecode = HttpUtility.UrlDecode(file);
                    outFileDesc.Add(new FileDesc(fileDecode, fileDecode));
                }
                return Json(outFileDesc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

        public ActionResult GetInformation()
        {
            try
            {
                var session = GetSessionData();
                ulong usedSpace = session.AzureStorage.GetSpaceUsed();
                // Convert bytes to GB
                double usedSpaceGB = Math.Round(Convert.ToDouble(usedSpace) / (1024.0 * 1024.0 * 1204.0), 3);

                int availSpaceGB = 2;
                using (var context = new DatabaseContext())
                {
                    var user = context.GetUserById(session.UserId);
                    if (user == null)
                    {
                        throw new Exception("User not found");
                    }
                    if (user.Storage.Count > 0)
                    {
                        int limit = 2;// limit in GB
                        foreach (var storage in user.Storage)
                        {
                            if (DateTime.UtcNow > storage.ExpiryDate)
                            {
                                // Skip expired
                                continue;
                            }
                            limit = Math.Max(limit, storage.Limit);
                        }

                        availSpaceGB = limit;
                    }
                }

                return Json(new 
                {
                    UsedSpace = usedSpaceGB,
                    AvailSpace = availSpaceGB
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

        public ActionResult Download()
        {
            string fileId = Request.Params["fileId"];
            if (String.IsNullOrEmpty(fileId))
            {
                throw new ArgumentNullException("Argument 'fileId' is empty");
            }

            SessionData session = GetSessionData();


            byte[] filedata = session.AzureStorage.GetFile(fileId);
            string contentType = MimeMapping.GetMimeMapping(Path.GetFileName(fileId));

            Response.AddHeader(
                "Content-Disposition",
                String.Format("attachment; filename=\"{0}\"", Path.GetFileName(fileId)));

            return File(filedata, contentType);
        }

        public ActionResult Delete()
        {
            try
            {
                string fileId = Request.Params["fileId"];
                if (String.IsNullOrEmpty(fileId))
                {
                    throw new ArgumentNullException("Argument 'fileId' is empty");
                }

                SessionData session = GetSessionData();

                session.AzureStorage.DeleteFile(fileId);

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            try
            {
                SessionData session = GetSessionData();

                string fName = Request.Params["uploadfile"];
                if (String.IsNullOrEmpty(fName) ||
                    Request.InputStream == null ||
                    Request.InputStream.Length == 0)
                {
                    throw new Exception("Upload filename not set");
                }

                string folder = Request.Params["folder"] ?? String.Empty;

                using (Stream receiveStream = Request.InputStream)
                {
                    var buf = new byte[receiveStream.Length];
                    int readCnt = receiveStream.Read(buf, 0, (int)receiveStream.Length);
                    if (readCnt != receiveStream.Length)
                    {
                        throw new Exception("Can't read upload file");
                    }

                    if (String.IsNullOrEmpty(folder))
                    {
                        session.AzureStorage.CreateFile(CADFEA.AzureStorage.Storage.StorageFolder + "/", fName, buf);
                    }
                    else
                    {
                        session.AzureStorage.CreateFile(CADFEA.AzureStorage.Storage.StorageFolder + "/" + folder + "/", fName, buf);
                    }
                }

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

        public ActionResult GetFolders()
        {
            try
            {
                var session = GetSessionData();

                List<AzureFolder> folders = session.AzureStorage.GetStoragesFolders();
                // Convert azure storage to bootstrap-treeview format
                List<TreeJsonItem> jsonFolders = new List<TreeJsonItem>();
                foreach (AzureFolder folder in folders)
                {
                    jsonFolders.Add(new TreeJsonItem(folder));
                }

                return Json(jsonFolders, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

        [HttpPost]
        public ActionResult CreateFolder()
        {
            try
            {
                string name = Request.Params["name"];
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("Argument 'name' is empty");
                }
                string parentFolder = Request.Params["parentFolder"];
                if (!String.IsNullOrWhiteSpace(parentFolder))
                {
                    parentFolder = parentFolder + "/";
                }
                var session = GetSessionData();

                // simply create your blob called "folder/_sys.sys", and it will work. No need to create a directory.
                if (!String.IsNullOrWhiteSpace(parentFolder))
                {
                    parentFolder = String.Format("{0}/{1}/", CADFEA.AzureStorage.Storage.StorageFolder, parentFolder);
                }
                else
                {
                    parentFolder = String.Format("{0}/", CADFEA.AzureStorage.Storage.StorageFolder);
                }
                session.AzureStorage.CreateFolder(name, parentFolder);

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }

        [HttpPost]
        public ActionResult DeleteFolder()
        {
            try
            {
                string path = Request.Params["path"];
                if (String.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException("Argument 'path' is empty");
                }

                var session = GetSessionData();

                if (String.IsNullOrEmpty(path))
                {
                    path = String.Format("{0}/", CADFEA.AzureStorage.Storage.StorageFolder);
                }
                else
                {
                    path = String.Format("{0}/{1}/", CADFEA.AzureStorage.Storage.StorageFolder, path);
                }

                session.AzureStorage.DeleteFolder(path);

                return JsonSuccess;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return GetJsonFail(ex);
            }
        }
    }
}
