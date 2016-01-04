using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Monads;
using ImageResizer;
using MyAndromeda.Core;
using MyAndromeda.Menus.Context.Thumbnails;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromeda.Menus.Services.Media
{
    public interface IMediaResizeService : IDependency
    {
        IEnumerable<ResizeSizeTaskContext> ResizeImage(int andromedaSiteId, MemoryStream stream);
        IEnumerable<ResizeSizeTaskContext> ResizeLogoImage(MemoryStream stream, List<MyAndromeda.Menus.Services.Media.AzureMediaLibraryService.LogoConfigurations> sizeList);
    }

    public class MediaResizeService : IMediaResizeService 
    {
        private readonly IMyAndromedaSiteMenuDataService siteMenuDataAccess;
        private readonly IMyAndromedaMenuItemThumbnailDataService menuItemDataAccess;
        private readonly IMyAndromedaMediaProfilesDataService mediaProfileDataService;

        public MediaResizeService(IMyAndromedaMenuItemThumbnailDataService menuDataAccess, IMyAndromedaMediaProfilesDataService mediaProfileDataService, IMyAndromedaSiteMenuDataService siteMenuDataAccess) 
        {
            this.siteMenuDataAccess = siteMenuDataAccess;
            this.mediaProfileDataService = mediaProfileDataService;
            this.menuItemDataAccess = menuDataAccess;
        }

        public IEnumerable<ResizeSizeTaskContext> ResizeLogoImage(MemoryStream stream, List<MyAndromeda.Menus.Services.Media.AzureMediaLibraryService.LogoConfigurations> sizeList)
        {
            Func<MemoryStream> copyStream = () =>
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                stream.Seek(0, SeekOrigin.Begin);
                ms.Seek(0, SeekOrigin.Begin);

                return ms;
            };

            foreach (var resizeSettings in this.GetLogoResizeSettings(sizeList))
            {
                var activityStrem = copyStream();
                yield return this.DoTheResizeWork(activityStrem, resizeSettings);
            }
            yield break;
        }

        public IEnumerable<ResizeSizeTaskContext> ResizeImage(int andromediaSiteId, MemoryStream stream)
        {
            var menu = this.siteMenuDataAccess.GetMenu(andromediaSiteId);
            Func<MemoryStream> copyStream = () =>
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                stream.Seek(0, SeekOrigin.Begin);
                ms.Seek(0, SeekOrigin.Begin);

                return ms;
            };

            foreach (var resizeSettings in this.GetResizeSettings(menu))
            {
                var activityStrem = copyStream();

                yield return this.DoTheResizeWork(activityStrem, resizeSettings);
            }

            yield break;
        }

        private ICollection<SiteMenuMediaProfileSize> DefaultFallbackSizes() 
        {
            //to-do get out of the database 
            return new List<SiteMenuMediaProfileSize>()
            {
                new SiteMenuMediaProfileSize()
                {
                    Name = "Max",
                    Height = 320,
                    Width = 320
                },
                new SiteMenuMediaProfileSize()
                {
                    Name = "Med",
                    Height = 160,
                    Width = 160
                },
                new SiteMenuMediaProfileSize()
                {
                    Name = "Low",
                    Height = 80,
                    Width = 80
                }
            };
        }

        private FitMode GetFitMode(string fitMode) 
        {
            switch (fitMode)
            {
                case "max":
                    return FitMode.Max; 
                case "pad":
                    return FitMode.Pad; 
                case "crop":
                    return FitMode.Crop; 
                case "stretch":
                    return FitMode.Stretch;
            }

            return FitMode.Crop;
        }

        private ContentAlignment GetAlignement(string alignment) 
        {
            switch (alignment.ToLower())
            {
                case "topleft":
                    return ContentAlignment.TopLeft; 
                case "topcenter":
                    return ContentAlignment.TopCenter; 
                case "topright":
                    return ContentAlignment.TopRight; 
                case "middleleft":
                    return ContentAlignment.MiddleLeft; 
                case "middlecenter":
                    return ContentAlignment.MiddleCenter; 
                case "middleright":
                    return ContentAlignment.MiddleRight; 
                case "bottomleft":
                    return ContentAlignment.BottomLeft; 
                case "bottomcenter":
                    return ContentAlignment.BottomCenter; 
                case "bottomright":
                    return ContentAlignment.BottomRight; 
            }

            return ContentAlignment.MiddleCenter;
        }

        private ResizeSettings[] GetLogoResizeSettings(List<MyAndromeda.Menus.Services.Media.AzureMediaLibraryService.LogoConfigurations> sizeList)
        {
            List<ResizeSettings> retList = new List<ResizeSettings>();
            foreach (AzureMediaLibraryService.LogoConfigurations size in sizeList)
            {
                ResizeSettings LogoResizeSettings = new ResizeSettings(size.Width, size.Height, FitMode.Pad, "png")
                {
                    Anchor = size.Alignment,
                    PaddingColor = ColorTranslator.FromHtml("#FFFFFF"),
                    Scale = ScaleMode.Both
                };
                retList.Add(LogoResizeSettings);
            }
            return retList.ToArray();
        }


        private ResizeSettings[] GetResizeSettings(SiteMenu menu) 
        {
            var profile = menu.SiteMenuMediaProfile ?? new SiteMenuMediaProfile()
            {
                Mode = FitMode.Pad.ToString().ToLower(),
                Alignment = ContentAlignment.MiddleCenter.ToString().ToLower(),
                PaddingColour = "#FFFFFF",
                SiteMenuMediaProfileSizes = this.mediaProfileDataService.Query().Where(e => e.IsDefault == true).ToArray().Select(e => new SiteMenuMediaProfileSize()
                {
                    Name = e.Name,
                    Height = e.Height,
                    Width = e.Width
                }).ToList()
                //this.DefaultFallbackSizes()
            };

            //grab from profile / default ones above
            IEnumerable<SiteMenuMediaProfileSize> resizeSettingProfiles =
                profile.SiteMenuMediaProfileSizes.CheckWithDefault(e => e != null && e.Count > 0, this.DefaultFallbackSizes());
            
            //create all profiles to resize with
            var fitMode =  this.GetFitMode(profile.Mode);

            IEnumerable<ResizeSettings> resizeSettings = resizeSettingProfiles.Select(e => new ResizeSettings(e.Width, e.Height, fitMode, "png")
            {
                Anchor = this.GetAlignement(profile.Alignment),
                PaddingColor = ColorTranslator.FromHtml(profile.PaddingColour),
                Scale = ScaleMode.Both
            });

            return resizeSettings.ToArray();
        }

        private ResizeSizeTaskContext DoTheResizeWork(MemoryStream inputStream, ResizeSettings resizeSettings) 
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
            return new ResizeSizeTaskContext()
            {
                Result = result,
                Width = b.Width, //resizeSettings.Width,
                Height = b.Height, //resizeSettings.Height,
                Name = string.Format("{0}x{1}", resizeSettings.Width, resizeSettings.Height)
            };
        }
    }
}
