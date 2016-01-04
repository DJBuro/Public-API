using System.IO;
using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Menus.Services.Media;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System;
using MyAndromeda.Logging;

namespace MyAndromeda.Web.Areas.Menu.Controllers
{
    public class MenuThumbsController : Controller
    {
        private readonly WorkContextWrapper workContextWrapper;

        private readonly IMediaLibraryServiceProvider mediaLibraryServiceProvider;
        private readonly IMyAndromedaLogger logger;

        public MenuThumbsController(WorkContextWrapper workContextWrapper, 
            IMediaLibraryServiceProvider mediaLibraryServiceProvider,
            IMyAndromedaLogger logger) 
        {
            this.logger = logger;
            this.workContextWrapper = workContextWrapper;
            this.mediaLibraryServiceProvider = mediaLibraryServiceProvider;
        }

        public ActionResult Upload(List<HttpPostedFileBase> files, string folderPath, int? mediaServerId)
        {
            var statuses = new List<Menus.Data.ThumbnailFileResult>();
            
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                //ensure IE doesn't confuse things 
                var filename = Path.GetFileName(file.FileName);
                //let the media library (or many) service(s) deal with the file(s)

                MemoryStream origin = new MemoryStream();
                file.InputStream.CopyTo(origin);
                origin.Seek(0, SeekOrigin.Begin);

                folderPath = string.Format("menus/{0}/{1}", this.workContextWrapper.Current.CurrentSite.Site.ExternalSiteId, folderPath);

                var filesReposonses = mediaLibraryServiceProvider
                    .ImportMedia(origin, folderPath, filename, workContextWrapper.Current.CurrentSite.Site.AndromediaSiteId)
                    .OrderByDescending(e=> e.Height).ToList();

                //statuses = filesReposonses;
                statuses.AddRange(filesReposonses);
            }

            // Return response
            return Json(statuses.OrderByDescending(e=> e.Height));
        }

        public ActionResult Remove(string fileName, string folderPath, int? mediaServerId) 
        {
            var statuses = new List<object>();
 
            //mediaLibraryService.RemoveMedia(folderPath, fileName);

            statuses.Add(new { FileName = fileName, Removed = true });

            return Json(statuses);
        }
    }
}