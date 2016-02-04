using System.IO;
using System.Threading.Tasks;
using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Menus.Data;
using MyAndromeda.Services.WebOrdering.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Drawing;
using System.Web.Configuration;
using Newtonsoft.Json;
using MyAndromeda.Services.Media;
using MyAndromeda.Services.Media.Models;

namespace MyAndromeda.Web.Controllers.Api.WebOrdering
{
    public class AndroWebOrderingController : ApiController
    {
        private readonly ICurrentSite currentSite;
        private readonly IAndroWebOrderingWebSiteService androWebOrderingWebSiteService;
        private readonly IMediaLibraryServiceProvider mediaLibraryServiceProvider;
        private readonly IPreviewAndPublishWebOrdering previewPublishService;

        private readonly INotifier notifier;
        private readonly IWorkContext context;

        public AndroWebOrderingController(ICurrentSite currentSite,
            IAndroWebOrderingWebSiteService androWebOrderingWebSiteService,
            IMediaLibraryServiceProvider mediaLibraryServiceProvider,
            IPreviewAndPublishWebOrdering previewPublishService,
            INotifier notifier,
            IWorkContext context)
        {
            this.context = context;
            this.notifier = notifier;
            this.previewPublishService = previewPublishService;
            this.mediaLibraryServiceProvider = mediaLibraryServiceProvider;
            this.currentSite = currentSite;
            this.androWebOrderingWebSiteService = androWebOrderingWebSiteService;
        }

        [HttpGet]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Stores/Read")]
        public IEnumerable<MyAndromeda.Web.ViewModels.StoreViewModel> ReadStores(int webOrderingWebsiteId) 
        {
            var webSiteConfig = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e=> e.Id == webOrderingWebsiteId);
            var associatedStores = webSiteConfig.ACSApplication.ACSApplicationSites.Select(e => new {
                AndromedaSiteId= e.Store.AndromedaSiteId,
                Name = e.Store.Name
            }).ToArray();

            return associatedStores.Select(e=> new MyAndromeda.Web.ViewModels.StoreViewModel{
                AndromedaSiteId = e.AndromedaSiteId,
                Name = e.Name
            });
        }


        [HttpGet]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Read")]
        public WebSiteConfigurations Read(int webOrderingWebsiteId)
        {
            var store = this.currentSite.Store;
            //var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Chain.Stores.Any(s => s.Id == store.Id));
            var website = this.androWebOrderingWebSiteService
                .GetWebOrderingWebsite(e => e.Id == webOrderingWebsiteId && e.ACSApplication.ACSApplicationSites.Any(s => s.Store.Id == store.Id));
            
            var result = string.IsNullOrWhiteSpace(website.PreviewSettings)
                ? new WebSiteConfigurations()
                : androWebOrderingWebSiteService.DeSerializeConfigurations(website.PreviewSettings);

            /* I want all this removed from here --- */
            /* Sigh... im never going to get to remove this crap */
            //todo... move to js level. 
            
            result.LiveDomainName = website.LiveDomainName;
            result.PreviewDomainName = website.PreviewDomainName;

            if (result.GeneralSettings == null)
            {
                result.GeneralSettings = new GeneralSettings();
            }
            if (result.CustomerAccountSettings == null)
            {
                result.CustomerAccountSettings = new CustomerAccount()
                {
                    EnableAndromedaLogin = true,
                    IsEnable = true
                };
            }

            if (result.SocialNetworkSettings.FacebookSettings == null)
            {
                result.SocialNetworkSettings.FacebookSettings = new FavcebookSocialNetworkSettings();
            }
            if (result.SocialNetworkSettings.InstagramSettings == null)
            {
                result.SocialNetworkSettings.InstagramSettings = new SocialNetworkSiteSettings();
            }
            if (result.SocialNetworkSettings.PinterestSettings == null)
            {
                result.SocialNetworkSettings.PinterestSettings = new SocialNetworkSiteSettings();
            }
            if (result.SocialNetworkSettings.TwitterSettings == null)
            {
                result.SocialNetworkSettings.TwitterSettings = new SocialNetworkSiteSettings();
            }
            if (result.TripAdvisorSettings == null)
            {
                result.TripAdvisorSettings = new TripAdvisorSettings();
            }
            if (result.AnalyticsSettings == null)
            {
                result.AnalyticsSettings = new Analytics();
            }
            if (result.CustomThemeSettings == null)
            {
                result.CustomThemeSettings = new CustomThemeSettings { DesktopBackgroundImagePath = string.Empty, MobileBackgroundImagePath = string.Empty, ColourRange1 = string.Empty, ColourRange2 = string.Empty, ColourRange3 = string.Empty, ColourRange4 = string.Empty, ColourRange5 = string.Empty, ColourRange6 = string.Empty };
            }
            if (result.CustomEmailTemplate == null)
            {
                result.CustomEmailTemplate = new CustomEmailTemplate { HeaderColour = string.Empty, FooterColour = string.Empty };
            }
            if (result.FacebookCrawlerSettings == null)
            {
                result.FacebookCrawlerSettings = new FacebookCrawlerSettings { Title = string.Empty, Description = string.Empty, FacebookProfileLogoPath = string.Empty, SiteName = string.Empty };
            }
            if (result.SEOSettings == null)
            {
                result.SEOSettings = new SEOSettings { Title = string.Empty, Description = string.Empty, IsEnableDescription = false, Keywords = string.Empty };
            }
            if (result.JivoChatSettings == null) 
            {
                result.JivoChatSettings = new JivoChatSettings() { IsJivoChatEnabled = false, Script = string.Empty };
            }
            if (result.Pages == null) 
            {
                result.Pages = new CmsPages();
            }

            /* --- I want all this removed from here */

            //var settings = androWebOrderingWebSiteService.get
            return result;
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Update")]
        public async Task<HttpResponseMessage> Update([FromBody]WebSiteConfigurations configuration, [FromUri]int webOrderingWebsiteId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(error => error.ErrorMessage);

                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }

            var store = this.currentSite.Store;
            var website = this.androWebOrderingWebSiteService
                .GetWebOrderingWebsite(e => e.Id == webOrderingWebsiteId && e.ACSApplication.ACSApplicationSites.Any(s => s.Store.Id == store.Id));
            
            website.ThemeId = configuration.ThemeSettings.Id;

            configuration.MasterStoreId = store.ExternalId;
            configuration.ACSApplicationId = website.ACSApplication.ExternalApplicationId;

            try
            {
                website.PreviewSettings = JsonConvert.SerializeObject(configuration);

                this.androWebOrderingWebSiteService.Update(website);
                this.notifier.Notify("The changes have been saved.");
            }
            catch (Exception)
            {
                this.notifier.Error("There were errors while saving. Please try again.");
            }

            var response = Request.CreateResponse(HttpStatusCode.Created, new[] { configuration });

            return response;
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Publish")]
        public async Task<HttpResponseMessage> Publish(WebSiteConfigurations configuration, int webOrderingWebsiteId)
        {
            var store = this.currentSite.Store;
            var website = this.androWebOrderingWebSiteService
                .GetWebOrderingWebsite(e => e.Id == webOrderingWebsiteId && e.ACSApplication.ACSApplicationSites.Any(s => s.Store.Id == store.Id));

            if (!website.Enabled)
            {
                this.notifier.Error("Website is disabled - Please contact Administrator");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new[] { configuration });
            }

            HttpResponseMessage updateResponse = await Update(configuration, webOrderingWebsiteId);
            if (!updateResponse.StatusCode.Equals(HttpStatusCode.Created))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new[] { configuration });
            }

            website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == webOrderingWebsiteId && e.ACSApplication.ACSApplicationSites.Any(s => s.Store.Id == store.Id));
            
            WebSiteConfigurations existingLiveConfigurations = WebSiteConfigurations.DeserializeJson(website.LiveSettings);
            if (existingLiveConfigurations.SiteDetails.DomainName != configuration.SiteDetails.DomainName) 
            { 
                configuration.SiteDetails.OldDomainName = existingLiveConfigurations.SiteDetails.DomainName;
            }

            // update liveSettings column in db with preview-settings
            website.LiveSettings = website.PreviewSettings;
            this.androWebOrderingWebSiteService.Update(website);

            // call service (Live domain Name is changed)
            HttpResponseMessage publishResponse = await this.previewPublishService.PublishWebOrderingSettingsAsync(configuration);
            bool isSuccess = publishResponse != null && publishResponse.IsSuccessStatusCode;

            if (isSuccess)
            {
                // clear old-domain name on success.
                configuration.SiteDetails.OldDomainName = string.Empty;
                website.PreviewSettings = JsonConvert.SerializeObject(configuration);

                this.androWebOrderingWebSiteService.Update(website);
                this.notifier.Notify("The live website has been updated!");
            }
            else
            {
                if (publishResponse != null && publishResponse.StatusCode.Equals(HttpStatusCode.Conflict))
                {
                    this.notifier.Error("Another publish is in progress - Please try later");
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, new Exception("Another publish is in progress - Please try later"));
                }

                this.notifier.Error("There was an error updating the live website. Please try again.");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new Exception("Something went wrong in CallService(configuration)"));
            }

            var response = Request.CreateResponse(HttpStatusCode.Created, new[] { configuration });

            return response;
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Preview")]
        public async Task<HttpResponseMessage> Preview(WebSiteConfigurations configuration, int webOrderingWebsiteId)
        {
            HttpResponseMessage updateResponse = await Update(configuration, webOrderingWebsiteId);
            if (!updateResponse.StatusCode.Equals(HttpStatusCode.Created))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new[] { configuration });
            }

            var store = this.currentSite.Store;
            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == webOrderingWebsiteId && e.ACSApplication.ACSApplicationSites.Any(s => s.Store.Id == store.Id));

            //WebSiteConfigurations existingLiveConfigurations = WebSiteConfigurations.DeserializeJson(website.LiveSettings);
            //if (!configuration.SiteDetails.DomainName.Equals(website.PreviewDomainName))
            {
                configuration.SiteDetails.DomainName = website.PreviewDomainName;
                configuration.SiteDetails.OldDomainName = null;
                configuration.ACSApplicationId = website.ACSApplication.ExternalApplicationId;
            }

            var publishResponse = await this.previewPublishService.PreviewWebOrderingSettingsAsync(configuration);
            bool isSuccess = publishResponse != null && publishResponse.IsSuccessStatusCode;
            if (isSuccess)
            {
                // clear old-domain name on success.
                configuration.SiteDetails.OldDomainName = string.Empty;
                website.PreviewSettings = JsonConvert.SerializeObject(configuration);

                this.notifier.Notify("The preview website has been published!");
            }
            else
            {
                if (publishResponse != null && publishResponse.StatusCode.Equals(HttpStatusCode.Conflict))
                {
                    this.notifier.Error("Another publish is in progress - Please try later");
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, new Exception("Another publish is in progress - Please try later"));
                }

                this.notifier.Error("There was an error updating the live website. Please try again.");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new Exception("Something went wrong in CallService(configuration)"));
            }

            var response = Request.CreateResponse(HttpStatusCode.Created, new[] { configuration });

            return response;
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/UploadFacebookLogo/{name}")]
        public async Task<HttpResponseMessage> UploadFacebookLogo(string name, int webOrderingWebsiteId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new InvalidOperationException();
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            string logoName = "facebookcrawlerlogo";
            
            var file = provider.Contents.First();
            var store = this.currentSite.Store;
            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e =>
                e.Id == webOrderingWebsiteId &&
                e.ACSApplication.ACSApplicationSites.Any(acsApplicationSites => acsApplicationSites.Store.Id == store.Id));

            var stream = await file.ReadAsStreamAsync();

            var folderPath = string.Format("websites/{0}/facebooksettings/{1}", website.Id, logoName);
            var fileName = logoName + Path.GetExtension(file.Headers.ContentDisposition.FileName.Replace("\"", String.Empty));

            string newExtension = ".png";

            var result = UploadImages(stream, folderPath, fileName, newExtension);

            return Request.CreateResponse<ThumbnailFileResult>(HttpStatusCode.Created, result);
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/UploadLogo/{name}")]
        public async Task<HttpResponseMessage> UploadLogo(string name, int webOrderingWebsiteId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new InvalidOperationException();
            }

            string logoName = string.Empty;
            bool isWebsiteLogo = name.Equals("website", StringComparison.CurrentCultureIgnoreCase) ? true : false;

            switch (name.ToLower())
            {
                case "website":
                    logoName = "websitelogo";
                    break;
                case "mobile":
                    logoName = "mobilelogo";
                    break;
                case "favicon":
                    logoName = "favicon";
                    break;
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.First();
            var store = this.currentSite.Store;

            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e =>
                e.Id == webOrderingWebsiteId &&
                e.ACSApplication.ACSApplicationSites.Any(acsApplicationSites => acsApplicationSites.Store.Id == store.Id));

            var stream = await file.ReadAsStreamAsync();

            var folderPath = string.Format("websites/{0}/sitedetails/{1}", website.Id, logoName);
            var fileName = logoName + Path.GetExtension(file.Headers.ContentDisposition.FileName.Replace("\"", String.Empty));

            if (logoName.Equals("favicon", StringComparison.CurrentCultureIgnoreCase))
            {
                var result = UploadImages(stream, folderPath, fileName, ".ico");
                return Request.CreateResponse<ThumbnailFileResult>(HttpStatusCode.Created, result);
            }
            else
            {
                var result = UploadSiteImages(stream, folderPath, fileName, isWebsiteLogo);
                return Request.CreateResponse<ThumbnailFileResult>(HttpStatusCode.Created, result);
            }
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/UploadCarouselImage/{carouselName}/{carouselItemId}")]
        public async Task<HttpResponseMessage> UploadCarouselImage(string carouselName, string carouselItemId, int webOrderingWebsiteId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new InvalidOperationException();
            }

            ThumbnailFileResult result = null;
            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.Contents.Count == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("No files were uploaded"));
            }

            var store = this.currentSite.Store;
            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e =>
                e.Id == webOrderingWebsiteId &&
                e.ACSApplication.ACSApplicationSites.Any(acsApplicationSites => acsApplicationSites.Store.Id == store.Id));

            var file = provider.Contents.First();
            var stream = await file.ReadAsStreamAsync();


            var folderPath = string.Format("websites/{0}/carousels/{1}/", website.Id, carouselName.ToLowerInvariant());
            var fileName = carouselItemId + "_" + Path.GetExtension(file.Headers.ContentDisposition.FileName.Replace("\"", String.Empty));

            result = UploadCarouselImages(stream, folderPath, fileName, carouselName);

            return Request.CreateResponse<ThumbnailFileResult>(HttpStatusCode.Created, result);
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/UploadBackgroundImage/{name}")]
        public async Task<HttpResponseMessage> UploadBackgroundImage(string name, int webOrderingWebsiteId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new InvalidOperationException();
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var isdesktopBackgroundLogo = name.Equals("desktop", StringComparison.CurrentCultureIgnoreCase) ? true : false;
            var logoName = isdesktopBackgroundLogo ? "desktopbackgroundimage" : "mobilebackgroundimage";

            var file = provider.Contents.First();
            var store = this.currentSite.Store;
            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e =>
                e.Id == webOrderingWebsiteId &&
                e.ACSApplication.ACSApplicationSites.Any(acsApplicationSites => acsApplicationSites.Store.Id == store.Id));

            var stream = await file.ReadAsStreamAsync();

            var folderPath = string.Format("websites/{0}/customthemesettings/{1}", website.Id, logoName);
            var fileName = logoName + Path.GetExtension(file.Headers.ContentDisposition.FileName.Replace("\"", String.Empty));
            var newExtension = ".png";
            
            var result = UploadImages(stream, folderPath, fileName, newExtension);

            return Request.CreateResponse<ThumbnailFileResult>(HttpStatusCode.Created, result);
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/DeleteBackgroundImage/{name}")]
        public async Task<HttpResponseMessage> DeleteBackgroundImage(string name, int webOrderingWebsiteId)
        {
            var store = this.currentSite.Store;
            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e =>
                e.Id == webOrderingWebsiteId &&
                e.ACSApplication.ACSApplicationSites.Any(acsApplicationSites => acsApplicationSites.Store.Id == store.Id));

            var isdesktopBackgroundLogo = name.Equals("desktop", StringComparison.CurrentCultureIgnoreCase) ? true : false;
            var logoName = isdesktopBackgroundLogo ? "desktopbackgroundimage" : "mobilebackgroundimage";


            var fileName = logoName + ".png";
            var folderPath = string.Format("websites/{0}/customthemesettings/{1}/{2}", website.Id, logoName, fileName);

            var isSuccess = mediaLibraryServiceProvider.DeleteFile(folderPath);
            await Request.Content.ReadAsMultipartAsync();
         
            return Request.CreateResponse<bool>(isSuccess ? HttpStatusCode.OK : HttpStatusCode.NotFound, isSuccess);
        }


        private ThumbnailFileResult UploadCarouselImages(Stream stream, string folderPath, string fileName, string carouslName)
        {
            var sizeList = new List<LogoConfiguration>();

            sizeList = GetCarouselLogoSettings();

            MemoryStream origin = new MemoryStream();
            stream.CopyTo(origin);
            origin.Seek(0, SeekOrigin.Begin);

            var filesReposonses = mediaLibraryServiceProvider
                .ImportLogo(origin, folderPath, fileName, sizeList)
                .OrderByDescending(e => e.Height).ToList();

            return filesReposonses.First();
        }

        private ThumbnailFileResult UploadSiteImages(Stream stream, string folderPath, string fileName, bool isWebsiteLogo)
        {
            List<LogoConfiguration> sizeList = new List<LogoConfiguration>();

            sizeList = GetSiteLogoSettings(isWebsiteLogo);

            MemoryStream origin = new MemoryStream();
            stream.CopyTo(origin);
            origin.Seek(0, SeekOrigin.Begin);

            var filesReposonses = mediaLibraryServiceProvider
                .ImportLogo(origin, folderPath, fileName, sizeList)
                .OrderByDescending(e => e.Height).ToList();

            return filesReposonses.First();
        }

        private ThumbnailFileResult UploadImages(Stream stream, string folderPath, string fileName, string newExtension)
        {
            MemoryStream origin = new MemoryStream();

            stream.CopyTo(origin);
            origin.Seek(0, SeekOrigin.Begin);

            var filesReposonses = mediaLibraryServiceProvider.ImportImage(origin, folderPath, fileName, newExtension);

            return filesReposonses;
        }

        private List<LogoConfiguration> GetCarouselLogoSettings()
        {
            List<LogoConfiguration> sizeList = new List<LogoConfiguration>();
            //string webSiteLogoSettings = "450x150xMiddleLeft;320x320xMiddleCenter;130x130xMiddleCenter";

            //this will have to be popped out of the theme meta data eventually.
            string carouselLogoSettings = //id == 1 ?
                WebConfigurationManager.AppSettings["MyAndromeda.Carousel1.Settings"].ToString();
            //: WebConfigurationManager.AppSettings["MyAndromeda.Carousel2.Settings"].ToString();

            carouselLogoSettings = carouselLogoSettings.ToLower();

            if (!string.IsNullOrWhiteSpace(carouselLogoSettings))
            {
                List<string> settings = carouselLogoSettings.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string setting in settings)
                {
                    sizeList.Add(new LogoConfiguration(
                        Convert.ToInt32(setting.Split(new char[] { 'x' })[0]),
                        Convert.ToInt32(setting.Split(new char[] { 'x' })[1]),
                        ParseEnum<ContentAlignment>(setting.Split(new char[] { 'x' })[2])));
                }

                return sizeList;
            }

            sizeList.Add(new LogoConfiguration(970, 270, ContentAlignment.MiddleLeft));
            return sizeList;
        }

        private List<LogoConfiguration> GetSiteLogoSettings(bool isWebSiteLogo)
        {
            List<LogoConfiguration> sizeList = new List<LogoConfiguration>();
            string logoSettings = string.Empty;

            if (isWebSiteLogo)
            {
                logoSettings = WebConfigurationManager.AppSettings["MyAndromeda.WebsiteLogo.Settings"].ToString();
            }
            else
            {
                logoSettings = WebConfigurationManager.AppSettings["MyAndromeda.MobileLogo.Settings"].ToString();
            }

            logoSettings = logoSettings.ToLower();

            if (!string.IsNullOrEmpty(logoSettings))
            {
                List<string> settings = logoSettings.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string setting in settings)
                {
                    sizeList.Add(new LogoConfiguration(
                        Convert.ToInt32(setting.Split(new char[] { 'x' })[0]),
                        Convert.ToInt32(setting.Split(new char[] { 'x' })[1]),
                        ParseEnum<ContentAlignment>(setting.Split(new char[] { 'x' })[2])));
                }
                return sizeList;
            }

            sizeList.Add(new LogoConfiguration(450, 150, ContentAlignment.MiddleLeft));
            sizeList.Add(new LogoConfiguration(320, 320, ContentAlignment.MiddleCenter));
            sizeList.Add(new LogoConfiguration(130, 130, ContentAlignment.MiddleCenter));

            return sizeList;
        }

        private T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
