using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyAndromeda.Menus.Data;
using MyAndromeda.Services.Media.Models;

namespace MyAndromeda.Services.Media
{
    public class MediaLibraryServiceProvider : IMediaLibraryServiceProvider
    {
        private readonly IEnumerable<IMediaLibraryService> mediaLibraryServices;

        public MediaLibraryServiceProvider(IEnumerable<IMediaLibraryService> mediaLibraryServices)
        {
            this.mediaLibraryServices = mediaLibraryServices;
        }

        public IEnumerable<ThumbnailFileResult> ImportMedia(MemoryStream post, string folderPath, string fileName, int andromedaSiteId)
        {
            IEnumerable<ThumbnailFileResult> output = this.mediaLibraryServices.SelectMany(e => e.ImportMedia(post, folderPath, fileName, andromedaSiteId));

            return output;
        }

        public IEnumerable<ThumbnailFileResult> ImportLogo(MemoryStream post, string folderPath, string fileName, List<LogoConfiguration> sizeList)
        {
            IEnumerable<ThumbnailFileResult> output = this.mediaLibraryServices.SelectMany(e => e.ImportMedia(post, folderPath, fileName, sizeList));
            return output;
        }

        public ThumbnailFileResult ImportImage(MemoryStream post, string folderPath, string fileName, string newExtension) 
        {
            ThumbnailFileResult output = this.mediaLibraryServices.Select(e => e.ImportImage(post, folderPath, fileName, newExtension)).FirstOrDefault();
            return output;
        }

        public bool DeleteFile(string filePath)
        {
            bool issuccess = this.mediaLibraryServices.Select(e => e.DeleteFile(filePath)).FirstOrDefault();
            return issuccess;
        }
    }
}