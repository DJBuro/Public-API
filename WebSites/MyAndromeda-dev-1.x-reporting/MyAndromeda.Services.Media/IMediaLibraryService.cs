using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Menus.Data;
using MyAndromeda.Services.Media.Models;
using MyAndromeda.Storage.Models;

namespace MyAndromeda.Services.Media
{
    public interface IMediaLibraryService : IDependency
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        void CreateDirectory(string path, string name);

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
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
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
        /// Imports the media.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sizeList">The size list.</param>
        /// <returns></returns>
        IEnumerable<ThumbnailFileResult> ImportMedia(MemoryStream post, string folderPath, string fileName,
            List<LogoConfiguration> sizeList);

        /// <summary>
        /// Imports the media.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        ThumbnailFileResult ImportMedia(MemoryStream post, string folderPath, string fileName, LogoConfiguration configuration);

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
