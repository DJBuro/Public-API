using System;
using System.Collections.Generic;
using System.IO;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Menus.Data;
using MyAndromeda.Services.Media.Models;
using MyAndromeda.Storage.Azure;
using MyAndromeda.Storage.Models;
using MyAndromeda.Data.DataAccess.Menu;

namespace MyAndromeda.Services.Media
{
    public class AzureMediaLibraryService : IAzureMediaLibraryService
    {
        private readonly IBlobStorageService storageService;
        private readonly IMediaResizeService resizeService;
        private readonly IMyAndromedaSiteMediaServerDataService siteMediaSevice;

        private readonly ICurrentSite currentSite;

        public AzureMediaLibraryService(
            IBlobStorageService storageService,
            IMediaResizeService resizeService,
            IMyAndromedaSiteMediaServerDataService siteMediaSevice,
            ICurrentSite currentSite)
        {
            this.resizeService = resizeService;
            this.siteMediaSevice = siteMediaSevice;
            this.currentSite = currentSite;
            this.storageService = storageService;
        }

        public string Name
        {
            get
            {
                return "Azure Media Library";
            }
        }

        public void RemoveMedia(string folderPath, string fileName)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public IEnumerable<FileModel> List(string folderPath, int andromedaSiteId)
        {
            return this.storageService.List(folderPath);
        }

        public IEnumerable<ThumbnailFileResult> ImportMedia(MemoryStream post, string folderPath, string fileName, int andromedaSiteId)
        {
            foreach (var resizeProfile in this.resizeService.ResizeImage(andromedaSiteId, post))
            {
                yield return this.ProviderResult(resizeProfile, folderPath, fileName);
            }

            yield break;
        }

        public IEnumerable<ThumbnailFileResult> ImportMedia(MemoryStream post, string folderPath, string fileName, List<LogoConfiguration> sizeList)
        {
            foreach (var resizeProfile in this.resizeService.ResizeLogoImage(post, sizeList))
            {
                yield return this.ProviderResult(resizeProfile, folderPath, fileName);
            }

            yield break;
        }

        public ThumbnailFileResult ImportMedia(MemoryStream post, string folderPath, string fileName, LogoConfiguration configuration)
        {
            var resizeProfile = this.resizeService.ResizeImage(post, configuration);
            var result = this.ProviderResult(resizeProfile, folderPath, fileName);

            return result;
            //this.resizeService.ResizeImage(post, configuration);
        }

        public ThumbnailFileResult ImportImage(MemoryStream post, string folderPath, string fileName, string newExtension)
        {
            ThumbnailFileResult result = this.ProviderResult(post, folderPath, fileName, newExtension);
            return result;
        }

        public void CreateDirectory(string path, string name)
        {
            var host = this.siteMediaSevice.GetMediaServerWithDefault(this.currentSite.AndromediaSiteId).Address;
            var remoteLocation = this.storageService.RemoteLocation(host) + path;

            this.storageService.CreateDirectory(path, name);
        }

        public bool DeleteFile(string filePath)
        {
            //var host = this.siteMediaSevice.GetMediaServerWithDefault(this.currentSite.AndromediaSiteId).Address;
            //var remoteLocation = this.storageService.RemoteLocation(host) + filePath;
            return this.storageService.DeleteFile(filePath);
        }

        private ThumbnailFileResult ProviderResult(MemoryStream stream, string folderPath, string fileName, string newExtension)
        {
            var name = Path.GetFileName(fileName).Replace(Path.GetExtension(fileName), string.Empty);
            //include profile name
            //var newFileName = string.Format("{0}{1}", name, profile.Name);
            var fullFileName = string.Format("{0}{1}", name, newExtension);

            this.storageService.WriteFile(folderPath, fullFileName, stream);

            //var complexName = newFileName + newExtension;

            var host = this.siteMediaSevice.GetMediaServerWithDefault(this.currentSite.AndromediaSiteId).Address;
            var remoteLocation = this.storageService.RemoteLocation(host) + folderPath;
            var remoteFullPath = remoteLocation.EndsWith("/") ? string.Format("{0}{1}", remoteLocation, fullFileName) :
                                 string.Format("{0}/{1}", remoteLocation, fullFileName);

            return new ThumbnailFileResult(fullFileName.ToLower(), remoteFullPath.ToLower(), string.Empty, string.Empty);
        }

        private ThumbnailFileResult ProviderResult(ResizeSizeTaskContext profile, string folderPath, string fileName)
        {
            var previousExtension = Path.GetExtension(fileName);
            var newExtension = ".png";
            var name = Path.GetFileName(fileName).Replace(previousExtension, string.Empty);
            //include profile name
            var newFileName = string.Format("{0}{1}", name, profile.Name);
            var fullFilePath = string.Format("{0}{1}", newFileName, newExtension);

            this.storageService.WriteFile(folderPath, fullFilePath, profile.Result);

            var complexName = newFileName + newExtension;

            var host = this.siteMediaSevice.GetMediaServerWithDefault(this.currentSite.AndromediaSiteId).Address;
            var remoteLocation = this.storageService.RemoteLocation(host) + folderPath;
            var remoteFullPath = remoteLocation.EndsWith("/") ? string.Format("{0}{1}", remoteLocation, complexName) :
                                 string.Format("{0}/{1}", remoteLocation, complexName);

            return new ThumbnailFileResult(complexName.ToLower(), remoteFullPath.ToLower(), profile.Height.ToString(), profile.Width.ToString());
        }

    }
}