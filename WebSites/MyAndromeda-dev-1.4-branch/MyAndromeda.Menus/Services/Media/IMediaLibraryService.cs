using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Menus.Data;

namespace MyAndromeda.Menus.Services.Media
{
    public interface IMediaLibraryService : IDependency
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        
        /// <summary>
        /// Removes the media.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        void RemoveMedia(string folderPath, string fileName);

        /// <summary>
        /// Imports the media.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        IEnumerable<ThumbnailFileResult> ImportMedia(MemoryStream post, string folderPath, string fileName, int andromedaSiteId);

        /// <summary>
        /// Import Logo
        /// </summary>
        /// <param name="post"></param>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        IEnumerable<ThumbnailFileResult> ImportLogo(MemoryStream post, string folderPath, string fileName, List<MyAndromeda.Menus.Services.Media.AzureMediaLibraryService.LogoConfigurations> sizeList);

        ThumbnailFileResult ImportImage(MemoryStream post, string folderPath, string fileName, string newExtension);

        bool DeleteFile(string filePath);
        
    }
}
