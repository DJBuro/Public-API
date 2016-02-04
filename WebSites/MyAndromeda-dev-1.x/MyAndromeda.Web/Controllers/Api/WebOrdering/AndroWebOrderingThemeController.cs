using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Services.WebOrdering.Services;
using MyAndromeda.Storage.Azure;
using AndroAdminDataAccess.Domain.WebOrderingSetup;

namespace MyAndromeda.Web.Controllers.Api.WebOrdering
{
    public class AndroWebOrderingThemeController : ApiController 
    {
        private readonly IAndroWebOrderingWebSiteService androWebOrderingWebSiteService;
        private readonly IMyAndromedaSiteMediaServerDataService siteMediaSevice;
        private readonly IBlobStorageService storageService;

        public AndroWebOrderingThemeController(
            IAndroWebOrderingWebSiteService androWebOrderingWebSiteService,
            IMyAndromedaSiteMediaServerDataService siteMediaSevice,
            IBlobStorageService storageService)
        { 
            this.storageService = storageService;
            this.siteMediaSevice = siteMediaSevice;
            this.androWebOrderingWebSiteService = androWebOrderingWebSiteService;
        }

        [HttpGet]
        [Route("api/AndroWebOrderingTheme/{AndromedaSiteId}/List")]
        public IEnumerable<ThemeSettings> Get() 
        {
            return GetAndroWebOrderingThemesList();
        }

        [HttpGet]
        [Route("api/AndroWebOrderingTheme/{AndromedaSiteId}/Get/{Id}")]
        public ThemeSettings Get(int id) 
        {
            var model = this.androWebOrderingWebSiteService
                .ListThemes(e => e.Id == id)
                .Select(s => new ThemeSettings
                        {
                            Description = s.Description,
                            ThemeName = s.ThemeName,
                            ThemePath = s.ThemePath,
                            Height = s.Height,
                            Width = s.Width,
                            //InternalName = s.InternalName,
                            Id = s.Id
                        })
                .FirstOrDefault();

            return model;
        }

        private IList<ThemeSettings> GetAndroWebOrderingThemesList()
        {
            //string remoteLocationPath = RemoteLocationPath();
            IList<ThemeSettings> result = this.androWebOrderingWebSiteService
                .ListThemes()
                .Select(s => new ThemeSettings
                {
                    Description = s.Description,
                    ThemeName = s.ThemeName,
                    ThemePath = s.ThemePath, 
                    Height = s.Height, 
                    Width = s.Width, 
                    Id = s.Id
                }).ToList();

            return result;
        }

        private string RemoteLocationPath()
        {
            var host = this.siteMediaSevice.GetMediaServerWithDefault(-1).Address;
            var remoteLocation = this.storageService.RemoteLocation(host);
            return remoteLocation;
        }
    }
}