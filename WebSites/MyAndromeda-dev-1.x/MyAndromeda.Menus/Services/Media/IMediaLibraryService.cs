using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Menus.Data;
using MyAndromeda.Storage.Models;

namespace MyAndromeda.Menus.Services.Media
{
    public interface IMediaLibraryService : IDependency
    {
        void CreateDirectory(string path, string name);

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
        /// Lists the specified folder path.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        IEnumerable<FileModel> List(string folderPath, int andromedaSiteId);

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

        /// <summary>
        /// Imports the image.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="newExtension">The new extension.</param>
        /// <returns></returns>
        ThumbnailFileResult ImportImage(MemoryStream post, string folderPath, string fileName, string newExtension);

        bool DeleteFile(string filePath);
        
    }
}
