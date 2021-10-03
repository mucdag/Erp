using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Core.Utilities
{
    public class ImageHelper
    {
        public static byte[] ResizeImage(Stream stream, ImageInfo imageInfo)
        {
            if (imageInfo?.Height == null || imageInfo.Width == null)
                throw new Exception("Geçersiz boyut");

            if (stream == null || stream.Length == 0)
                throw new Exception("Geçersiz kaynak");

            byte[] imgBytes;

            using (var memoryStream = new MemoryStream())
            {
                var img = Image.FromStream(stream);
                img = new Bitmap(img, new Size(imageInfo.Width.Value, imageInfo.Height.Value));
                img.Save(memoryStream, ImageFormat.Png);
                imgBytes = memoryStream.ToArray();
            }

            return imgBytes;
        }

        public static bool IsImage(string fileName)
        {
            string[] validExtensions = {"jpg", "jpeg", "bmp", "gif", "png"};
            var extension = fileName.Split('.').LastOrDefault();
            return extension != null && validExtensions.Contains(extension.ToLower());
        }
    }

    public class ImageInfo
    {
        public int? Height { get; set; }
        public int? Width { get; set; }
    }
}