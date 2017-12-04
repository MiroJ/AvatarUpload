using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AvatarUploadMvc5.Controllers
{
    public class AvatarController : Controller
    {
        // Dimesnions of the cropped window - must match frontend definitions
        private const int _avatarWidth = 100;  // ToDo - Change the size of the stored avatar image
        private const int _avatarHeight = 100; // ToDo - Change the size of the stored avatar image
        // Width of initially uploaded image (scale is preserved so height is calculated).
        private const int _avatarScreenWidth = 400;  // ToDo - Change the value of the width of the image on the screen

        private const string _tempFolder = "/Temp";
        private const string _mapTempFolder = "~" + _tempFolder;
        private const string _avatarPath = "/Avatars";

        private readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpGet]
        public ActionResult _Upload()
        {
            return PartialView();
        }

        [ValidateAntiForgeryToken]
        public ActionResult _Upload(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null || !files.Any())
                return Json(new { success = false, errorMessage = "No file uploaded." });

            var file = files.FirstOrDefault();  // get ONE only
            if (file == null || !IsImage(file))
                return Json(new { success = false, errorMessage = "File is of wrong format." });

            if (file.ContentLength <= 0)
                return Json(new { success = false, errorMessage = "File cannot be zero length." });

            var webPath = GetTempSavedFilePath(file);

            return Json(new { success = true, fileName = webPath.Replace("\\", "/") }); // success
        }

        [HttpPost]
        public ActionResult Save(string t, string l, string h, string w, string fileName)
        {
            try
            {
                // Get file from temporary folder, ...
                var fn = Path.Combine(Server.MapPath(_mapTempFolder), Path.GetFileName(fileName));

                // ... get the image, ...
                var img = new WebImage(fn);

                // ... calculate its new dimensions, ...
                var height = Convert.ToInt32(h.Replace("-", "").Replace("px", ""));
                var width = Convert.ToInt32(w.Replace("-", "").Replace("px", ""));

                // ... scale it, ...
                img.Resize(width, height);

                // ... crop the part the user selected, ...
                var top = Convert.ToInt32(t.Replace("-", "").Replace("px", ""));
                var left = Convert.ToInt32(l.Replace("-", "").Replace("px", ""));
                var bottom = img.Height - top - _avatarHeight;
                var right = img.Width - left - _avatarWidth;

                // ... check for validity of calculations, ...
                if (bottom < 0 || right < 0)
                {
                    // If you reach this point, your avatar sizes in here and in the CSS file are different.
                    // Check _avatarHeight and _avatarWidth in this file
                    // and height and width for #preview-pane .preview-container in site.avatar.css
                    throw new ArgumentException("Definitions of dimensions of the cropping window do not match. Talk to the developer who customized the sample code :)");
                }

                img.Crop(top, left, bottom, right);

                // ... delete the temporary file,...
                System.IO.File.Delete(fn);

                // ... and save the new one.
                var newFileName = Path.Combine(_avatarPath, Path.GetFileName(fn));
                var newFileLocation = HttpContext.Server.MapPath(newFileName);
                if (Directory.Exists(Path.GetDirectoryName(newFileLocation)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newFileLocation));
                }

                img.Save(newFileLocation);
                return Json(new { success = true, avatarFileLocation = newFileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Unable to upload file.\nERRORINFO: " + ex.Message });
            }
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file == null) return false;
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private string GetTempSavedFilePath(HttpPostedFileBase file)
        {
            // Define destination
            var serverPath = HttpContext.Server.MapPath(_tempFolder);
            if (Directory.Exists(serverPath) == false)
            {
                Directory.CreateDirectory(serverPath);
            }

            // Generate unique file name
            var fileName = Path.GetFileName(file.FileName);
            fileName = SaveTemporaryAvatarFileImage(file, serverPath, fileName);

            // Clean up old files after every save
            CleanUpTempFolder(1);
            return Path.Combine(_tempFolder, fileName);
        }

        private static string SaveTemporaryAvatarFileImage(HttpPostedFileBase file, string serverPath, string fileName)
        {
            var img = new WebImage(file.InputStream);
            var ratio = img.Height / (double)img.Width;
            img.Resize(_avatarScreenWidth, (int)(_avatarScreenWidth * ratio));

            var fullFileName = Path.Combine(serverPath, fileName);
            if (System.IO.File.Exists(fullFileName))
            {
                System.IO.File.Delete(fullFileName);
            }

            img.Save(fullFileName);
            return Path.GetFileName(img.FileName);
        }

        private void CleanUpTempFolder(int hoursOld)
        {
            try
            {
                var currentUtcNow = DateTime.UtcNow;
                var serverPath = HttpContext.Server.MapPath("/Temp");
                if (!Directory.Exists(serverPath)) return;
                var fileEntries = Directory.GetFiles(serverPath);
                foreach (var fileEntry in fileEntries)
                {
                    var fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
                    var res = currentUtcNow - fileCreationTime;
                    if (res.TotalHours > hoursOld)
                    {
                        System.IO.File.Delete(fileEntry);
                    }
                }
            }
            catch
            {
                // Deliberately empty.
            }
        }
    }
}