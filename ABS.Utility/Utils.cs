using System;
using System.Drawing;
using System.IO;

namespace ABS.Utility
{
    public static class Utils
    {
        public static string SaveImage(string image, string path)
        {
            byte[] fileContents = Convert.FromBase64String(image.Substring(image.IndexOf(',') + 1));
            using (var ms = new MemoryStream(fileContents))
            {
                var img = Image.FromStream(ms);
                var filename = Guid.NewGuid() + ".png";
                var directory = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/img/{path}/");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
               // img.Save(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/img/{path}/") + filename);
               return @"~/Content/img/{path}/{filename}";
            }
        }
    }
}