using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Services.Media;
using MyAndromeda.Storage.Models;

namespace MyAndromeda.Web.Controllers.Api.Files
{
    public class ImageApiController : ApiController 
    {
        private readonly IMediaLibraryService mediaLibraryService;
        private readonly ICurrentSite currentSite;

        public ImageApiController(IMediaLibraryService mediaLibraryService, ICurrentSite currentSite)
        { 
            this.currentSite = currentSite;
            this.mediaLibraryService = mediaLibraryService;
        }

        private string GeneratePath(string path) 
        {
            path = path ?? "";
            if (path.StartsWith(@"/"))
            {
                path = path.Substring(1, path.Length - 2);
            }
            
            var start = string.Format("stores/{0}/{1}", this.currentSite.ExternalSiteId, path);

            return start;
        }

        private string GeneratePath(string path, string file) 
        {
            if (path == null) 
            {
                return this.GeneratePath(file);
            }
            
            path = this.GeneratePath(path);

            if (path.EndsWith("/")) 
            {
                return string.Format("{0}{1}", path, file);
            }

            return string.Format("{0}/{1}", path, file);
        }

        [HttpPost]
        [Route("api/{andromedaSiteId}/files/ImageBrowser/Read")]
        public IEnumerable<FileModel> Read([FromUri] int andromedaSiteId, 
            [FromBody]KendoFileModel model) 
        {
            string path = model.Path;
            var start = this.GeneratePath(path);
            var files = this.mediaLibraryService.List(start, andromedaSiteId);

            return files;
        }


        [HttpPost]
        [Route("api/{andromedaSiteId}/files/ImageBrowser/Destroy")]
        public async Task Destroy([FromUri] int andromedaSiteId, 
            [FromBody] KendoFileModel model) 
        {
            var p = this.GeneratePath(model.Path, model.Name);
            this.mediaLibraryService.DeleteFile(p);
        }

        [HttpPost]
        [Route("api/{andromedaSiteId}/files/ImageBrowser/Create")]
        public async Task Create([FromUri]
                                 int andromedaSiteId, KendoFileModel model) 
        { 
            //only want to create directories. 
            if (model.Type != "d")
            {
                return;
            }

            try
            {
                var p = this.GeneratePath(model.Path);
                this.mediaLibraryService.CreateDirectory(p, model.Name);
            }
            catch (Exception ex)
            {
                
                throw;
            }
            
        }

        [HttpPost]
        [Route("api/{andromedaSiteId}/files/ImageBrowser/Upload")]
        public async Task<FileModel> Upload([FromUri]
                                                  int andromedaSiteId) 
        {
            List<FileModel> results = new List<FileModel>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new InvalidOperationException();
            }

            var provider =
               // new MultipartFormDataStreamProvider();
                new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            //var formPart = provider.Contents.Where(e => !e.Headers.Any(h => h.Value.Equals("file",)));
            string path = null; //detailFrom["path"]
            
            foreach (var item in provider.Contents) 
            {
                if (item.Headers.ContentDisposition
                    .Name.Replace("\"", String.Empty)
                    .Equals("path", StringComparison.InvariantCultureIgnoreCase))
                {
                    path = await item.ReadAsStringAsync();
                    //path = item.Headers.ContentDisposition.Name;
                    break;
                }
            }
            
            path = this.GeneratePath(path ?? "/");

            var files = provider.Contents.Skip(1);

            foreach (var file in files) 
            {
                var name = file.Headers.ContentDisposition.FileName.Replace("\"", String.Empty);
                var extension = Path.GetExtension(file.Headers.ContentDisposition.FileName.Replace("\"", String.Empty));
                
                using (var stream = await file.ReadAsStreamAsync()) 
                {
                    using (MemoryStream origin = new MemoryStream())
                    {
                        stream.CopyTo(origin);
                        origin.Seek(0, SeekOrigin.Begin);

                        var r = this.mediaLibraryService.ImportImage(origin, path, name, extension);

                        var result = new FileModel(r.FileName, FileType.File, origin.Length);
                        results.Add(result);
                    }
                }
            }

            return results.FirstOrDefault();
        }

        /// <summary>
        /// Gets the valid file extensions by which served files will be filtered.
        /// </summary>
        public string Filter
        {
            get
            {
                return "*.txt, *.doc, *.docx, *.xls, *.xlsx, *.ppt, *.pptx, *.zip, *.rar, *.jpg, *.jpeg, *.gif, *.png";
            }
        }
    }
}