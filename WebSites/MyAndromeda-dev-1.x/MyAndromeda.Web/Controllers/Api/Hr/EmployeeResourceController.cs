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
                string location = string.Format(format: "hr/employee/{0}/profile-pic.png", arg0: employeeId);
                Stream memoryStream = await blobStorage.DownloadBlob(location);

                if (memoryStream == null)
                {
                    string filePath = HostingEnvironment.MapPath(virtualPath: "~/content/profile-picture.jpg");
                    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    result.Content = new StreamContent(fileStream);
                    result.Content.Headers.ContentLength = fileStream.Length;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "image/jpeg");
                }
                else
                {
                    memoryStream.Position = 0;
                    result.Content = new StreamContent(memoryStream);
                    result.Content.Headers.ContentLength = memoryStream.Length;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "image/png");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(message: "Could not get the image");
                this.logger.Error(ex);
                throw;
            }

            return result;
        }


        [HttpPost]
        [Route("update-profile-pic")]
        public async Task<HttpResponseMessage> UploadProfilePic([FromUri] Guid employeeId) 
        {
            string newFileName = "profile-pic";

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            HttpContent file = provider.Contents.First();
            Stream stream = await file.ReadAsStreamAsync();

            string destinationPath = string.Format(format: "hr/employee/{0}/", arg0: employeeId);

            //go grab that file extension that i don't care about, but is used to transform it to png.
            string fileName = newFileName + Path.GetExtension(file.Headers.ContentDisposition.FileName.Replace(oldValue: "\"", newValue: String.Empty));

            ThumbnailFileResult result = UploadImages(stream, destinationPath, fileName);

            return Request.CreateResponse<ThumbnailFileResult>(HttpStatusCode.Created, result);
        }


        //hr/{0}/employees/{1}/resources/{2}/update-document/{3}
        [HttpPost]
        [Route("update-document/{documentId}")]
        public async Task<HttpResponseMessage> UpdateDocument([FromUri] Guid employeeId, [FromUri]string documentId)
        {
            var result = new List<ThumbnailFileResult>();
            //var newFileName = documentId;

            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents) 
            {
                Stream stream = await file.ReadAsStreamAsync();

                string destinationPath = string.Format(format: "hr/employee/{0}/documents/{1}", arg0: employeeId, arg1: documentId);

                //go grab that file extension that i don't care about, but is used to transform it to png.
                string fileName = file.Headers.ContentDisposition.FileName.Replace(oldValue: "\"", newValue: string.Empty);

                ThumbnailFileResult r = UploadFile(stream, destinationPath, fileName);
                result.Add(r);
            }

            //var file = provider.Contents.First();
            
            return Request.CreateResponse<IEnumerable<ThumbnailFileResult>>(HttpStatusCode.Created, result);
        }

        [HttpGet]
        [Route("document/{documentId}/download/{fileName}")]
        public async Task<HttpResponseMessage> DownloadDocument([FromUri] Guid employeeId, [FromUri]Guid documentId, string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            
            //result.Headers.Add(new MediaTypeHeaderValue("application/octet-stream"));
            try
            {
                string location = string.Format(format: "hr/employee/{0}/documents/{1}/{2}", arg0: employeeId, arg1: documentId, arg2: fileName);
                Stream memoryStream = await blobStorage.DownloadBlob(location);
                memoryStream.Position = 0;
                result.Content = new StreamContent(memoryStream);
                //result.Content.Headers.ContentLength = memoryStream.Length;
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "application/octet-stream");
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }


        //hr/{0}/employees/{1}/resources/{2}/document/{3}
        [HttpGet]
        [Route("document/{documentId}/{fileName}")]
        public async Task<HttpResponseMessage> GetDocumentPicture([FromUri] Guid employeeId, [FromUri]string documentId, [FromUri] string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            string[] imageExtensions = new []{"gif", "png", "jpg", "jpeg", "bmp" };
            try
            {
                string location = string.Format(format: "hr/employee/{0}/documents/{1}/{2}", arg0: employeeId, arg1: documentId, arg2: fileName);
                
                string fileExtension = Path.GetExtension(fileName).Replace(oldValue: ".", newValue: "");//) { }

                bool isImage = imageExtensions.Any(e => e.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase));

                Stream memoryStream = null;

                if (isImage) 
                {
                    memoryStream = await blobStorage.DownloadBlob(location);
                }

                if (memoryStream == null)
                {
                    string filePath = HostingEnvironment.MapPath(virtualPath: "~/content/no_image_available.png");
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    result.Content = new StreamContent(fileStream);
                    result.Content.Headers.ContentLength = fileStream.Length;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "image/png");
                }
                else
                {
                    memoryStream.Position = 0;
                    result.Content = new StreamContent(memoryStream);
                    result.Content.Headers.ContentLength = memoryStream.Length;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "image/png");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(message: "Could not get the image");
                this.logger.Error(ex);
                throw;
            }

            return result;
        }

        private ThumbnailFileResult UploadFile(Stream stream, string folderPath, string fileName) 
        {
            var origin = new MemoryStream();

            stream.CopyTo(origin);
            origin.Seek(0, SeekOrigin.Begin);

            string ext = Path.GetExtension(fileName);

            ThumbnailFileResult filesReposonse = mediaLibraryServiceProvider.ImportImage(origin, folderPath, fileName, ext);

            return filesReposonse;
        }

        private ThumbnailFileResult UploadImages(Stream stream, string folderPath, string fileName)
        {
            string newExtension = ".png";
            var origin = new MemoryStream();

            stream.CopyTo(origin);
            origin.Seek(0, SeekOrigin.Begin);

            ThumbnailFileResult filesReposonse = mediaLibraryServiceProvider.ImportImage(origin, folderPath, fileName, newExtension);

            return filesReposonse;
        }

       

    }
}