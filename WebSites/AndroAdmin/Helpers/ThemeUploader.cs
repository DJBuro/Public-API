using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AndroAdmin.Helpers
{
    public static class ThemeUploader
    {
        public static ThemeFileResult ImportMedia(MemoryStream post, string folderPath, string fileName, int width, int height)
        {
            ImageResizer resizer = new ImageResizer();
            return ProviderResult(resizer.ResizeImage(post, width, height), folderPath, fileName);
        }

        private static ThemeFileResult ProviderResult(Image profile, string folderPath, string fileName)
        {
            var previousExtension = Path.GetExtension(fileName);
            var newExtension = ".png";
            var name = Path.GetFileName(fileName).Replace(previousExtension, string.Empty);
            
            var newFileName = string.Format("{0}{1}", name, profile.Name);
            var fullFilePath = string.Format("{0}{1}", newFileName, newExtension);

            AzureStorage azureStorage = new AzureStorage();
            azureStorage.WriteFile(folderPath, fullFilePath, profile.Result);

            var complexName = newFileName + newExtension;

            var host = WebConfigurationManager.AppSettings["AndroAdmin.Theme.Host"].ToString();
            var remoteLocation = azureStorage.RemoteLocation(host) + folderPath;
            var remoteFullPath = remoteLocation.EndsWith("/") ? string.Format("{0}{1}", remoteLocation, complexName) :
                                 string.Format("{0}/{1}", remoteLocation, complexName);

            return new ThemeFileResult(complexName.ToLower(), remoteFullPath.ToLower(), profile.Height.ToString(), profile.Width.ToString());
        }
    }

    public class ThemeFileResult
    {
        public ThemeFileResult(string fileName, string url, string height, string width)
        {
            this.FileName = fileName;
            this.Url = url;
            this.Height = height;
            this.Width = width;
        }

        public string FileName { get; set; }
        public string Url { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
    }

    public class AzureStorage
    {
        public string containerName { get; set; }
        public string accountName { get; set; }
        public string password { get; set; }

        public AzureStorage()
        {
            this.containerName = WebConfigurationManager.AppSettings["AndroAdmin.Theme.AzureThemeCloudContainer"].ToString();
            this.accountName = WebConfigurationManager.AppSettings["AndroAdmin.Theme.AzureAccountName"].ToString();
            this.password = WebConfigurationManager.AppSettings["AndroAdmin.Theme.AzureAccountPassword"].ToString();
        }

        public void WriteFile(string location, string fileName, Stream stream)
        {
            location = location.ToLower();
            fileName = fileName.ToLower();

            var container = this.GetOrCreateContainer();
            var blobFolder = container.GetDirectoryReference(location);
            var blobBlock = blobFolder.GetBlockBlobReference(fileName);

            var maxage = 86400;
            blobBlock.Properties.ContentType = "image/png";
            blobBlock.Properties.CacheControl = String.Format("public, max-age={0}", maxage);
            //blobBlock.Properties.LastModified = DateTime.UtcNow;

            stream.Position = 0;
            blobBlock.UploadFromStream(stream);
        }

        public string RemoteLocation(string host)
        {
            host = string.IsNullOrWhiteSpace(host) ? "https://cdn.myandromedaweb.co.uk/" : host;

            return host.EndsWith("/") ?
                string.Format("{0}{1}/", host, containerName) :
                string.Format("{0}/{1}/", host, containerName);
        }

        private CloudBlobContainer GetOrCreateContainer()
        {
            StorageCredentials credentials = new StorageCredentials(accountName.ToLower(), password);
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true); 
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containerName);
            if (container.Exists())
            {
                return container;
            }

            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess =
                    BlobContainerPublicAccessType.Blob
            });

            return container;
        }
    }
}