using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using MyAndromeda.Configuration;
using System.Threading.Tasks;
using MyAndromeda.Storage.Models;
using System.Collections.Generic;
using MyAndromeda.Logging;

namespace MyAndromeda.Storage.Azure
{
    public class BlobStorageService : IBlobStorageService 
    {
        private readonly StorageCredentials credentials; 
        private readonly CloudStorageAccount storageAccount;

        private readonly string containerName;
        private readonly string accountName;
        private readonly IMyAndromedaLogger logger;

        public BlobStorageService(IMyAndromedaLogger logger) 
        {
            this.logger = logger;
            this.containerName = AzureBlobStorage.ContainerName;//WebConfigurationManager.AppSettings["AzureMenuCloudContainer"].ToString(); 
            this.accountName = AzureBlobStorage.AccountName;
            
            this.credentials = new StorageCredentials(accountName.ToLower(), AzureBlobStorage.Password);
            this.storageAccount = new CloudStorageAccount(credentials, useHttps: true); 
        }

        public void RenameBlobs(string from, string to)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        

        public CloudBlobContainer GetOrCreateContainer(string name) 
        {
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(name);
            if (container.Exists()) {
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

        public void WriteFile(string location, string fileName, Stream stream) 
        {

            this.WriteFile(this.containerName, location, fileName, stream);
        }

        public void CreateDirectory(string location, string folderName)
        {
            location = location.ToLower();
            folderName = folderName.ToLower();

            var container = this.GetOrCreateContainer(containerName);
            var blobFolder = container.GetDirectoryReference(location);

            var folders = blobFolder.ListBlobs
                (
                    useFlatBlobListing: false, 
                    blobListingDetails: BlobListingDetails.Metadata
                );

            if (folders.Any(e => e.Uri.AbsoluteUri.EndsWith(folderName)))
            {
                return;
            }
            else 
            {
                //azure doesn't deal in folders :P ... so chuck a empty file. 
                var blobBlock = blobFolder.GetBlockBlobReference(folderName + "/" + "folder.dat");
                blobBlock.UploadText("");
            }

            return; 
        }

        public async Task<Stream> DownloadBlob(string location)
        {
            location = location.ToLowerInvariant();

            var container = this.GetOrCreateContainer(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(location);

            if (!blockBlob.Exists()) { return null; }

            MemoryStream ms = new MemoryStream();
             
            await blockBlob.DownloadToStreamAsync(ms);

            return ms;
        }

        public List<FileModel> List(string location)
        {
            location = location.ToLower();
            var container = this.GetOrCreateContainer(containerName);
            var blobFolder = container.ListBlobs(
                prefix:location, 
                useFlatBlobListing:false, 
                blobListingDetails: BlobListingDetails.Metadata
            );



            var files = blobFolder
                    .Where(e => e is CloudBlockBlob)
                    .Where(e=> !e.Uri.AbsoluteUri.EndsWith(".dat"))
                        .Select(e => e as CloudBlockBlob);

            var folders = blobFolder
                    .Where(e => e is CloudBlobDirectory)
                        .Select(e => e as CloudBlobDirectory);

            return 
                files.Select(e => new FileModel(Path.GetFileName(e.Name), FileType.File, 0))
                    .Union(folders.Select(e=> new FileModel(e.Prefix.Replace(location, "").Replace("/", ""), FileType.Directory, 0)))
                    .ToList();
        }

        public bool DeleteFile(string location)
        {
            bool fileDeletedSuccessfully = false;
            if (!string.IsNullOrEmpty(location))
            {
                location = location.ToLower();
                try
                {
                    var container = this.GetOrCreateContainer(this.containerName);
                    string fileUrl = location;// +"/" + fileName;
                    var blob = container.GetBlockBlobReference(location);

                    bool exists = blob.Exists();
                    if (exists) {
                        blob.Delete(deleteSnapshotsOption: DeleteSnapshotsOption.IncludeSnapshots);
                    }
                    
                    fileDeletedSuccessfully = true;
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex);
                    fileDeletedSuccessfully = false;
                }                
            }
            return fileDeletedSuccessfully;
        }

        public string RemoteLocation(string host) 
        {
            host = string.IsNullOrWhiteSpace(host) ? "https://cdn.myandromedaweb.co.uk/" : host;

            return host.EndsWith("/") ? 
                string.Format("{0}{1}/", host, containerName) : 
                string.Format("{0}/{1}/", host, containerName);
        }

        private void WriteFile(string containerName, string location, string fileName, Stream stream) 
        {
            location = location.ToLower();
            fileName = fileName.ToLower();

            var container = this.GetOrCreateContainer(containerName);
            var blobFolder = container.GetDirectoryReference(location);
            var blobBlock = blobFolder.GetBlockBlobReference(fileName);
            
            var maxage = 86400;
            blobBlock.Properties.ContentType = "image/png";
            blobBlock.Properties.CacheControl = String.Format("public, max-age={0}", maxage);
            //blobBlock.Properties.LastModified = DateTime.UtcNow;

            stream.Position = 0;
            blobBlock.UploadFromStream(stream);
        }

        private string contentPath;
       

        public string ContentPath(string hostAddress, string contentPathFormat, string externalSiteId)
        {
            if (!string.IsNullOrWhiteSpace(this.contentPath))
            {
                return this.contentPath;
            }

            var host = this.RemoteLocation(hostAddress);

            this.contentPath = string.Format(
                contentPathFormat,
                host,
                externalSiteId.ToLower()
            );

            //while (this.contentPath.Contains(@"//")) 
            //{
            //    this.contentPath = this.contentPath.Replace(@"//", "/");
            //}

            if (this.contentPath.EndsWith(@"/"))
                return this.contentPath;

            this.contentPath = this.contentPath + @"/";

            return this.contentPath;
        }

        public void RenameAsync(string containerName, string startsWith, string oldString, string newString) 
        {
            var container = this.GetOrCreateContainer(containerName);

            var blobs = container.ListBlobs(startsWith, true, BlobListingDetails.None);

            foreach (var blob in blobs) 
            {
                
            }

        }
    }

    public static class BlobExtensions
    {
        public static void Rename(this CloudBlobContainer container, string oldName, string newName)
        {
            var source = container.GetBlobReferenceFromServer(oldName);
            var target = container.GetBlockBlobReference(newName);
            target.StartCopyFromBlob(source.Uri);
            source.Delete();
        }

        public static async Task RenameAsync(this CloudBlobContainer container, string oldName, string newName)
        {
            var source = await container.GetBlobReferenceFromServerAsync(oldName);
            var target = container.GetBlockBlobReference(newName);
            
            await target.StartCopyFromBlobAsync(source.Uri);
            await source.DeleteAsync();
        }
    }
}
