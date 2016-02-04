using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Menus.Data;
using MyAndromeda.Services.Media.Models;

namespace MyAndromeda.Services.Media
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
        IEnumerable<ThumbnailFileResult> ImportLogo(MemoryStream post, string folderPath, string fileName, List<LogoConfiguration> sizeList);

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
}