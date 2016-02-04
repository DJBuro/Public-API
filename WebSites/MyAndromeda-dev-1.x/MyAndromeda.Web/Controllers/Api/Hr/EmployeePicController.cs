using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MyAndromeda.Logging;
using MyAndromeda.Storage.Azure;
using System.IO;
using MyAndromeda.Menus.Data;
using System.Net;
using MyAndromeda.Services.Media;
using System.Web.Hosting;
using System.Net.Http.Headers;

namespace MyAndromeda.Web.Controllers.Api.Hr
{
    [RoutePrefix("hr/{chainId}/employees/{andromedaSiteId}/resources/{employeeId}")]
    public class EmployeeResourceController : ApiController
    {
        private readonly IBlobStorageService blobStorage;
        private readonly IMediaLibraryServiceProvider mediaLibraryServiceProvider;
        private readonly IMyAndromedaLogger logger;

        public EmployeeResourceController(IBlobStorageService blobStorage, IMyAndromedaLogger logger, IMediaLibraryServiceProvider mediaLibraryServiceProvider) 
        {
            this.mediaLibraryServiceProvider = mediaLibraryServiceProvider;
            this.logger = logger;
            this.blobStorage = blobStorage;
        }

        [HttpGet]
        [Route("profile-pic")]
        public async Task<HttpResponseMessage> GetProfilePic([FromUri] Guid employeeId)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                var location = string.Format("hr/employee/{0}/profile-pic.png", employeeId);
                Stream memoryStream = await blobStorage.DownloadBlob(location);

                if (memoryStream == null)
                {
                    String filePath = HostingEnvironment.MapPath("~/content/profile-picture.jpg");
                    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    result.Content = new StreamContent(fileStream);
                    result.Content.Headers.ContentLength = fileStream.Length;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                }
                else
                {
                    memoryStream.Position = 0;
                    result.Content = new StreamContent(memoryStream);
                    result.Content.Headers.ContentLength = memoryStream.Length;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Could not get the image");
                this.logger.Error(ex);
                throw;
            }

            return result;
        }


        [HttpPost]
        [Route("update-profile-pic")]
        public async Task<HttpResponseMessage> UploadProfilePic([FromUri] Guid employeeId) 
        {
            var newFileName = "profile-pic";

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.First();
            var stream = await file.ReadAsStreamAsync();

            var destinationPath = string.Format("hr/employee/{0}/", employeeId);

            //go grab that file extension that i don't care about, but is used to transform it to png.
            var fileName = newFileName + Path.GetExtension(file.Headers.ContentDisposition.FileName.Replace("\"", String.Empty));

            var result = UploadImages(stream, destinationPath, fileName);

            return Request.CreateResponse<ThumbnailFileResult>(HttpStatusCode.Created, result);
        }


        //hr/{0}/employees/{1}/resources/{2}/update-document/{3}
        [HttpPost]
        [Route("update-document/{documentId}")]
        public async Task<HttpResponseMessage> UpdateDocument([FromUri] Guid employeeId, [FromUri]string documentId)
        {
            List<ThumbnailFileResult> result = new List<ThumbnailFileResult>();
            //var newFileName = documentId;

            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents) 
            {
                var stream = await file.ReadAsStreamAsync();

                var destinationPath = string.Format("hr/employee/{0}/documents/{1}", employeeId, documentId);

                //go grab that file extension that i don't care about, but is used to transform it to png.
                var fileName = file.Headers.ContentDisposition.FileName.Replace("\"", String.Empty);

                var r = UploadFile(stream, destinationPath, fileName);
                result.Add(r);
            }

            //var file = provider.Contents.First();
            
            return Request.CreateResponse<IEnumerable<ThumbnailFileResult>>(HttpStatusCode.Created, result);
        }

        //[HttpGet]
        //[Route("document/{documentId}/download/{fileName}")]
        //public async Task<HttpResponseMessage> DownloadDocument([FromUri] Guid employeeId, string fileName)
        //{
            
        //}

        //hr/{0}/employees/{1}/resources/{2}/document/{3}
        [HttpGet]
        [Route("document/{documentId}/{fileName}")]
        public async Task<HttpResponseMessage> GetDocumentPicture([FromUri] Guid employeeId, [FromUri]string documentId, [FromUri] string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var imageExtensions = new []{"gif", "png", "jpg", "jpeg", "bmp" };
            try
            {
                var location = string.Format("hr/employee/{0}/documents/{1}/{2}", employeeId, documentId, fileName);
                
                var fileExtension = Path.GetExtension(fileName).Replace(".", "");//) { }
                var isImage = imageExtensions.Any(e => e.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase));

                Stream memoryStream = null;

                if (isImage) 
                {
                    memoryStream = await blobStorage.DownloadBlob(location);
                }

                if (memoryStream == null)
                {
                    String filePath = HostingEnvironment.MapPath("~/content/no_image_available.png");
                    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    result.Content = new StreamContent(fileStream);
                    result.Content.Headers.ContentLength = fileStream.Length;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                }
                else
                {
                    memoryStream.Position = 0;
                    result.Content = new StreamContent(memoryStream);
                    result.Content.Headers.ContentLength = memoryStream.Length;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Could not get the image");
                this.logger.Error(ex);
                throw;
            }

            return result;
        }

        private ThumbnailFileResult UploadFile(Stream stream, string folderPath, string fileName) 
        {
            MemoryStream origin = new MemoryStream();

            stream.CopyTo(origin);
            origin.Seek(0, SeekOrigin.Begin);

            var ext = Path.GetExtension(fileName);

            var filesReposonse = mediaLibraryServiceProvider.ImportImage(origin, folderPath, fileName, ext);

            return filesReposonse;
        }

        private ThumbnailFileResult UploadImages(Stream stream, string folderPath, string fileName)
        {
            var newExtension = ".png";
            MemoryStream origin = new MemoryStream();

            stream.CopyTo(origin);
            origin.Seek(0, SeekOrigin.Begin);

            var filesReposonse = mediaLibraryServiceProvider.ImportImage(origin, folderPath, fileName, newExtension);

            return filesReposonse;
        }

       

    }
}