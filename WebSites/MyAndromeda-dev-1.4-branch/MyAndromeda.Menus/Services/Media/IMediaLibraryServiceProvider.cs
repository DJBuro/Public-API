using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Menus.Data;

namespace MyAndromeda.Menus.Services.Media
{
    public interface IMediaLibraryServiceProvider : IDependency
    {
        /// <summary>
        /// Imports the media.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        IEnumerable<ThumbnailFileResult> ImportMedia(MemoryStream post, string folderPath, string fileName, int andromedaSiteId);

        /// <summary>
        /// Imports the logo.
        /// </summary>
        /// <param name="post">The post.</param> 
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sizeList">The size list.</param>
        /// <returns></returns>
        IEnumerable<ThumbnailFileResult> ImportLogo(MemoryStream post, string folderPath, string fileName, List<MyAndromeda.Menus.Services.Media.AzureMediaLibraryService.LogoConfigurations> sizeList);

        /// <summary>
        /// Imports the image.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="newExtension">The new extension.</param>
        /// <returns></returns>
        ThumbnailFileResult ImportImage(MemoryStream post, string folderPath, string fileName, string newExtension);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        bool DeleteFile(string filePath);
    }

    public class MediaLibraryServiceProvider : IMediaLibraryServiceProvider
    {
        private readonly IEnumerable<IMediaLibraryService> mediaLibraryServices;

        public MediaLibraryServiceProvider(IEnumerable<IMediaLibraryService> mediaLibraryServices)
        {
            this.mediaLibraryServices = mediaLibraryServices;
        }

        public IEnumerable<ThumbnailFileResult> ImportMedia(MemoryStream post, string folderPath, string fileName, int andromedaSiteId)
        {
            var output = this.mediaLibraryServices.SelectMany(e => e.ImportMedia(post, folderPath, fileName, andromedaSiteId));

            return output;
        }

        public IEnumerable<ThumbnailFileResult> ImportLogo(MemoryStream post, string folderPath, string fileName, List<MyAndromeda.Menus.Services.Media.AzureMediaLibraryService.LogoConfigurations> sizeList)
        {
            var output = this.mediaLibraryServices.SelectMany(e => e.ImportLogo(post, folderPath, fileName, sizeList));
            return output;
        }
        public ThumbnailFileResult ImportImage(MemoryStream post, string folderPath, string fileName,string newExtension) 
        {
            var output = this.mediaLibraryServices.Select(e => e.ImportImage(post, folderPath, fileName, newExtension)).FirstOrDefault();
            return output;
        }

        public bool DeleteFile(string filePath)
        {
            bool issuccess = this.mediaLibraryServices.Select(e => e.DeleteFile(filePath)).FirstOrDefault();
            return issuccess;
        }
             
       
    }
}