using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace SomeStore.Infrustructure.Helpers
{
    public static class ImageHelper
    {
        private static readonly string ImageFolder;

        static ImageHelper()
        {
            ImageFolder = HostingEnvironment.MapPath("~/Images");
        }

        public static string SaveImage(HttpPostedFileBase file)
        {
            if (file != null && Regex.IsMatch(file.ContentType.ToLower(), "image/*"))
            {
                if (!string.IsNullOrWhiteSpace(ImageFolder))
                {
                    var fileExt = Path.GetExtension(file.FileName);
                    var guid = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    var fileId = guid + fileExt;

                    var date = DateTime.Now.ToString("ddMMyyyy");
                    var path = Path.Combine(ImageFolder, date);
                    Directory.CreateDirectory(path);
                    file.SaveAs(Path.Combine(path, fileId));

                    return $"~/Images/{date}/{fileId}";
                }

                throw new DirectoryNotFoundException("\"Images\" directory is missing.");
            }

            return null;
        }
    }
}