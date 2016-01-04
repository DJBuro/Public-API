using ImageResizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace AndroAdmin.Helpers
{
    public class ImageResizer
    {
        public Image ResizeImage(MemoryStream stream, int width, int height)
        {
            Func<MemoryStream> copyStream = () =>
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                stream.Seek(0, SeekOrigin.Begin);
                ms.Seek(0, SeekOrigin.Begin);

                return ms;
            };
            ResizeSettings ThemeResizeSettings = new ResizeSettings()
            {
                Width = width,
                Height = height,
                Mode = FitMode.Pad,
                Anchor = ContentAlignment.MiddleCenter,
                PaddingColor = ColorTranslator.FromHtml("#FFFFFF"),
                Scale = ScaleMode.Both
            };
            var activityStrem = copyStream();
            return this.DoTheResizeWork(activityStrem, ThemeResizeSettings) ;
        }

        private Image DoTheResizeWork(MemoryStream inputStream, ResizeSettings resizeSettings)
        {
            var job = new ImageJob()
            {
                Settings = resizeSettings
            };

            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            var result = new MemoryStream();
            resizeSettings.Format = "png";
            ImageBuilder.Current.Build(inputStream, result, resizeSettings);

            Bitmap b = new Bitmap(result);
            result.Seek(0, SeekOrigin.Begin);
            return new Image()
            {
                Result = result,
                Width = b.Width, //resizeSettings.Width,
                Height = b.Height, //resizeSettings.Height,
                Name = string.Format("{0}x{1}", resizeSettings.Width, resizeSettings.Height)
            };
        }
    }

    public class Image
    {
        public string Name { get; set; }
        public MemoryStream Result { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public void Dispose()
        {
            this.Result.Close();
            this.Result.Dispose();
        }
    }
}