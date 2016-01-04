using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.AndroWebOrdering.Models;
using MyAndromeda.Services.WebOrdering.Services;
using MyAndromeda.Menus.Services.Media;
using System.IO;
using MyAndromeda.Storage.Azure;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using System.Drawing;
using System.Web.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using AndroAdminDataAccess.Domain.WebOrderingSetup;

namespace MyAndromeda.Web.Areas.AndroWebOrdering.Controllers
{
    public class AndroWebOrderingController : Controller
    {
        private readonly IAndroWebOrderingWebSiteService androWebOrderingWebSiteService;
        private readonly IMediaLibraryServiceProvider mediaLibraryServiceProvider;
        private readonly IBlobStorageService storageService;
        private readonly IMyAndromedaSiteMediaServerDataService siteMediaSevice;
        private readonly ICurrentSite currentSite;

        public AndroWebOrderingController(IMediaLibraryServiceProvider mediaLibraryServiceProvider, IBlobStorageService storageService, IMyAndromedaSiteMediaServerDataService siteMediaSevice, IAndroWebOrderingWebSiteService androWebOrderingWebSiteService, ICurrentSite currentSite)
        {
            this.androWebOrderingWebSiteService = androWebOrderingWebSiteService;
            this.mediaLibraryServiceProvider = mediaLibraryServiceProvider;
            this.storageService = storageService;
            this.siteMediaSevice = siteMediaSevice;
            this.currentSite = currentSite;
        }

        // GET: AndroWebOrdering/AndroWebOrdering
        [HttpGet]
        public ActionResult Index(int id)
        {
            //var rnd = new Random();
            //string randomNum = "?" + rnd.Next(10000);
            //var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == id);
            //AndroWebOrderingWebSiteModel model = new AndroWebOrderingWebSiteModel { Id = website.Id };
            //model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
            //string remotePath = RemoteLocationPath();

            //if (!string.IsNullOrEmpty(website.PreviewSettings))
            //{
            //    model.WebsiteConfigurations = androWebOrderingWebSiteService.DeSerializeConfigurations(website.PreviewSettings);
            //    if (model.WebsiteConfigurations != null)
            //    {
            //        if (!string.IsNullOrEmpty(model.WebsiteConfigurations.SiteDetails.WebsiteLogoPath))
            //        {
            //            model.WebsiteConfigurations.SiteDetails.WebsiteLogoPath = remotePath + model.WebsiteConfigurations.SiteDetails.WebsiteLogoPath.Remove(0, 1);
            //        }
            //        if (!string.IsNullOrEmpty(model.WebsiteConfigurations.SiteDetails.MobileLogoPath))
            //        {
            //            model.WebsiteConfigurations.SiteDetails.MobileLogoPath = remotePath + model.WebsiteConfigurations.SiteDetails.MobileLogoPath.Remove(0, 1);
            //        }
            //        //if (model.WebsiteConfigurations.Home.Carousel1.Items != null)
            //        //{
            //        //    model.WebsiteConfigurations.Home.Carousel1.Items.ToList().ForEach(f => f.ImagePath = remotePath + f.ImagePath + "?" + randomNum);
            //        //}
            //    }
            //}
            //else
            //{
            //    model.WebsiteConfigurations = new WebSiteConfigurations();
            //    model.WebsiteConfigurations.GeneralSettings.IsList = true;
            //    model.WebsiteConfigurations.GeneralSettings.IsEnterPostCode = false;
            //}
            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == id);
            var model  = new AndroWebOrderingWebSiteModel 
            { 
                Id = website.Id, 
                LiveDomainName = website.LiveDomainName,
                PreviewWebSite = website.PreviewDomainName 
            }; 

            return View(model);
        }

        public ActionResult Publish(int id)
        {
            var store = this.currentSite.Store;
            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == id);

            WebSiteConfigurations configuration = WebSiteConfigurations.DeserializeJson(website.PreviewSettings);
            WebSiteConfigurations existingLiveConfigurations = WebSiteConfigurations.DeserializeJson(website.LiveSettings);
            if (existingLiveConfigurations.SiteDetails.DomainName != configuration.SiteDetails.DomainName)
                configuration.SiteDetails.OldDomainName = existingLiveConfigurations.SiteDetails.DomainName;

            // update liveSettings column in db with preview-settings
            website.LiveSettings = website.PreviewSettings;
            this.androWebOrderingWebSiteService.Update(website);

            // call service (Live domain Name is changed)
            bool isSuccess = CallService(configuration);
            if (isSuccess)
            {
                // clear old-domain name on success.
                configuration.SiteDetails.OldDomainName = string.Empty;
                website.PreviewSettings = JsonConvert.SerializeObject(configuration);
                this.androWebOrderingWebSiteService.Update(website);
            }
            return RedirectToAction("Index", new { id = id });
        }

        private bool CallService(WebSiteConfigurations config)
        {
            bool isSuccess = false;
            string serviceURL = ConfigurationManager.AppSettings["MyAndromeda.WebOrderingWebSite.CreateService"];
            WebRequest request = WebRequest.Create(serviceURL);
            request.Method = "POST";
            request.ContentType = "application/json";
            var data = WebSiteConfigurations.SerializeJson(config);

            UTF8Encoding encoding = new UTF8Encoding();
            var postData = encoding.GetBytes(data);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }

            try
            {
                var response = request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                // log error
            }
            return isSuccess;
        }

        //[HttpPost]
        //public ActionResult Index(int id, AndroWebOrderingWebSiteModel model)
        //{
        //    AndroWebOrderingWebsite website = new AndroWebOrderingWebsite();
        //    WebSiteConfigurations websiteConfigs = model.WebsiteConfigurations;
        //    if (websiteConfigs != null)
        //    {
        //        websiteConfigs.GeneralSettings.IsEnterPostCode = websiteConfigs.GeneralSettings.IsList ? false : true;
        //        websiteConfigs.SocialNetworkSettings.FacebookSettings.IsFollow = websiteConfigs.SocialNetworkSettings.FacebookSettings.IsShare ? false : true;
        //        websiteConfigs.SocialNetworkSettings.TwitterSettings.IsFollow = websiteConfigs.SocialNetworkSettings.TwitterSettings.IsShare ? false : true;
        //        websiteConfigs.CustomerAccountSettings.IsEnableOtherFacebook = websiteConfigs.CustomerAccountSettings.IsEnableFacebookAndromeda ? false : true;
        //        string settings = androWebOrderingWebSiteService.SerializeConfigurations(websiteConfigs);
        //        //website = webOrderingWebSiteDataService.Get(e => e.Id == id);
        //        website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == id);
        //        if (website != null)
        //        {
        //            website.PreviewSettings = settings;
        //            //webOrderingWebSiteDataService.Update(website);
        //            this.androWebOrderingWebSiteService.Update(website);
        //        }
        //    }

        //    model.WebsiteConfigurations = websiteConfigs;
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    return View(model);
        //}

        public ActionResult List()
        {
            //IList<AndroWebOrderingWebSiteModel> resultsList = usersDataService.GetAndroWebOrderingSitesForUser(workContext.CurrentUser.User.Id).Select(s => new AndroWebOrderingWebSiteModel { Id = s.Id, Name = s.Name }).ToList();
            IList<AndroWebOrderingWebSiteModel> resultsList = this.currentSite.AndroWebOrderingSites.Select(s => new AndroWebOrderingWebSiteModel { Id = s.Id, Name = s.Name, Enabled = s.Enabled }).ToList();
            return View(resultsList);
        } 

        //[HttpPost]
        //public ActionResult EditCarousel1(int Id,string itemId,HttpPostedFileBase carousel1ImageLogo, CarouselItem item)
        //{
        //    //Is delete
        //    if (!string.IsNullOrEmpty(itemId) && item.Id == new Guid())
        //    {
        //        DeleteCarousel1(Id, itemId);
        //    }
        //    //create/Edit
        //    else
        //    {
        //        string folderPath = string.Empty;
        //        string fileName = string.Empty;

        //        if (item != null && item.Id == new Guid())
        //        {
        //            item.Id = Guid.NewGuid();
        //        }
        //        if (carousel1ImageLogo != null)
        //        {
        //            folderPath = string.Format("websites/{0}/carousel1", Id);
        //            fileName = "carousel1_" + item.Id.ToString() + "_" + Path.GetExtension(carousel1ImageLogo.FileName);
        //            string path = UploadCarouselImages(carousel1ImageLogo, folderPath, Id, fileName, "carousel1");
        //            //item.ImageUrl = "websites/" + (path.Split(new string[] { "websites/" }, StringSplitOptions.RemoveEmptyEntries)[1]);
        //        }

        //        var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == Id);
        //        if (website != null)
        //        {
        //            if (!string.IsNullOrEmpty(website.PreviewSettings))
        //            {
        //                WebSiteConfigurations websiteConfigs = androWebOrderingWebSiteService.DeSerializeConfigurations(website.PreviewSettings);
        //                websiteConfigs = UpdateCarousel1(websiteConfigs, item);
        //                website.PreviewSettings = androWebOrderingWebSiteService.SerializeConfigurations(websiteConfigs);
        //            }
        //            else
        //            {
        //                // WebSiteConfigurations config = new WebSiteConfigurations { Home = new WebsiteHome { Carousel1 = new Carousel { Items = new List<CarouselItem>() } } };
        //                //config.Home.Carousel1.Items.Add(item);
        //                //website.Settings = androWebOrderingWebSiteService.SerializeConfigurations(config);
        //            }
        //            this.androWebOrderingWebSiteService.Update(website);
        //        }
        //    }
        //    return RedirectToAction("Index", new { id = Id });

        //}

        //private WebSiteConfigurations UpdateCarousel1(WebSiteConfigurations config, CarouselItem item)
        //{
        //    bool hasCarouselList = false;
        //    if (config != null){
        //        //if (config.Home.Carousel1.Items != null)
        //        //{
        //        //    var carouselItem = config.Home.Carousel1.Items.Where(w => w.ItemId.Equals(item.ItemId)).FirstOrDefault();
        //        //    if (carouselItem != null)
        //        //    {
        //        //        hasCarouselList = true;
        //        //        string imgPath = string.Empty;
        //        //        if (!string.IsNullOrEmpty(item.ImagePath))
        //        //        {
        //        //            imgPath = item.ImagePath;
        //        //        }
        //        //        else
        //        //        {
        //        //            imgPath = carouselItem.ImagePath;
        //        //        }
        //        //        item.ImagePath = imgPath;
        //        //        config.Home.Carousel1.Items.Where(w => w.ItemId == item.ItemId).ToList().ForEach(f => { f.ImagePath = item.ImagePath; f.IsEnabled = item.IsEnabled; f.IsOverlayText = item.IsOverlayText; f.Type = item.Type; f.IsEnabled = item.IsEnabled; f.HTML = item.HTML; });
        //        //    }
        //        //}
        //    }
        //    else{
        //        config = new WebSiteConfigurations();
        //        //config.Home.Carousel1.Items = new List<CarouselItem>();
        //    }
        //    if (!hasCarouselList)
        //    {
        //        //config.Home.Carousel1.Items.Add(item);
        //    }            
            
        //    return config;
        //}
                
        //[HttpPost]
        //public ActionResult DeleteCarousel1(int Id, string itemId)
        //{
        //    var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == Id);

        //    if (website != null && (!string.IsNullOrEmpty(website.PreviewSettings)))
        //    {
        //        WebSiteConfigurations config = androWebOrderingWebSiteService.DeSerializeConfigurations(website.PreviewSettings);
        //        //if (config.Home.Carousel1.Items != null) {
        //        //    var item = config.Home.Carousel1.Items.Where(e => e.ItemId.Equals(new Guid(itemId))).FirstOrDefault();
        //        //    if (item != null) {
        //        //        config.Home.Carousel1.Items.Remove(item);
        //        //        website.Settings = androWebOrderingWebSiteService.SerializeConfigurations(config);
        //        //        this.androWebOrderingWebSiteService.Update(website);
        //        //    }
        //        //}
        //    }
        //    return RedirectToAction("Index", new { id = Id });
        //}
        //[HttpPost]
        //public ActionResult Carousel2Settings(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    WebSiteConfigurations resultObj = UpdateConfigurations(Id, model.WebsiteConfigurations, "carousel2settings");
        //    model.WebsiteConfigurations = resultObj;
        //    return View("Index", model);
        //}

        //[HttpPost]
        //public ActionResult CarouselContainer1(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    WebSiteConfigurations resultObj = UpdateConfigurations(Id, model.WebsiteConfigurations, "carouselcontainer1");
        //    model.WebsiteConfigurations = resultObj;
        //    return RedirectToAction("Index", new { id = Id });
        //}

        //[HttpPost]
        //public ActionResult CarouselNavigator1(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    WebSiteConfigurations resultObj = UpdateConfigurations(Id, model.WebsiteConfigurations, "carouselnavigator1");
        //    model.WebsiteConfigurations = resultObj;
        //    return RedirectToAction("Index", new { id = Id });
        //}

        //[HttpPost]
        //public ActionResult CarouselContainer2(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    WebSiteConfigurations resultObj = UpdateConfigurations(Id, model.WebsiteConfigurations, "carouselcontainer2");
        //    model.WebsiteConfigurations = resultObj;
        //    return RedirectToAction("Index", new { id = Id });
        //}

        //[HttpPost]
        //public ActionResult CarouselNavigator2(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    WebSiteConfigurations resultObj = UpdateConfigurations(Id, model.WebsiteConfigurations, "carouselnavigator2");
        //    model.WebsiteConfigurations = resultObj;
        //    return RedirectToAction("Index", new { id = Id });
        //}

        //[HttpPost]
        //public ActionResult Footer(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    WebSiteConfigurations resultObj = UpdateConfigurations(Id, model.WebsiteConfigurations, "footer");
        //    model.WebsiteConfigurations = resultObj;
        //    return RedirectToAction("Index", new { id = Id });
        //}

        //[HttpPost]
        //public ActionResult Welcome(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    WebSiteConfigurations resultObj = UpdateConfigurations(Id, model.WebsiteConfigurations, "welcome");
        //    model.WebsiteConfigurations = resultObj;
        //    return RedirectToAction("Index", new { id = Id });
        //}

        

        //[HttpPost]
        //public ActionResult SiteDetails(int Id, HttpPostedFileBase webSiteLogo, HttpPostedFileBase mobileLogo, AndroWebOrderingWebSiteModel model)
        //{
        //    WebSiteConfigurations webConfig = model.WebsiteConfigurations;
        //    if (webConfig != null)
        //    {
        //        if (webSiteLogo != null)
        //        {
        //            webConfig.SiteDetails.WebSiteLogo = "websitelogo" + Path.GetExtension(webSiteLogo.FileName);
        //            webConfig.SiteDetails.WebsiteLogoPath = UploadImages(webSiteLogo, "websites", Id, webConfig.SiteDetails.WebSiteLogo, true);
        //        }
        //        if (mobileLogo != null)
        //        {
        //            webConfig.SiteDetails.MobileLogo = "mobilelogo" + Path.GetExtension(mobileLogo.FileName);
        //            webConfig.SiteDetails.MobileLogoPath = UploadImages(mobileLogo, "websites", Id, webConfig.SiteDetails.MobileLogo, false);
        //        }
        //    }
        //    WebSiteConfigurations resultObj = UpdateConfigurations(Id, webConfig, "sitedetails");
        //    if (!string.IsNullOrEmpty(resultObj.SiteDetails.WebsiteLogoPath))
        //    {
        //        resultObj.SiteDetails.WebsiteLogoPath = (resultObj.SiteDetails.WebsiteLogoPath.Contains("http") ? "" : RemoteLocationPath()) + resultObj.SiteDetails.WebsiteLogoPath.Remove(0, 1);
        //    }
        //    if (!string.IsNullOrEmpty(resultObj.SiteDetails.MobileLogoPath))
        //    {
        //        resultObj.SiteDetails.MobileLogoPath = (resultObj.SiteDetails.MobileLogoPath.Contains("http") ? "" : RemoteLocationPath()) + resultObj.SiteDetails.MobileLogoPath.Remove(0, 1);
        //    }

        //    model.WebsiteConfigurations = resultObj;
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    return View("Index", model);
        //}

        ////[HttpPost]
        ////public Task<ActionResult> UploadWebSiteLogo(HttpPostedFile file)
        ////{
        ////    webConfig.SiteDetails.WebSiteLogo = "websitelogo" + Path.GetExtension(webSiteLogo.FileName);
        ////    webConfig.SiteDetails.WebsiteLogoPath = UploadImages(webSiteLogo, "websites", Id, webConfig.SiteDetails.WebSiteLogo, true);
        ////}

        //public string UploadImages(HttpPostedFileBase logo, string folderPath, int Id, string fileName, bool isWebSiteLogo)
        //{
        //    List<AzureMediaLibraryService.LogoConfigurations> sizeList = new List<AzureMediaLibraryService.LogoConfigurations>();
        //    if (isWebSiteLogo)
        //        sizeList = getWebSiteLogoSettings();
        //    else
        //        sizeList = getMobileSiteLogoSettings();

        //    var statuses = new List<Menus.Data.ThumbnailFileResult>();

        //    MemoryStream origin = new MemoryStream();
        //    logo.InputStream.CopyTo(origin);
        //    origin.Seek(0, SeekOrigin.Begin);

        //    folderPath = string.Format(folderPath + "/" + Id);

        //    //var filesReposonses = mediaLibraryServiceProvider
        //    //    .ImportMedia(origin, folderPath, fileName, -1)
        //    //    .OrderByDescending(e => e.Height).ToList();

        //    var filesReposonses = mediaLibraryServiceProvider
        //        .ImportLogo(origin, folderPath, fileName, sizeList)
        //        .OrderByDescending(e => e.Height).ToList();

        //    statuses.AddRange(filesReposonses);
        //    return GetLogosFilePath(folderPath, fileName.Replace(Path.GetExtension(fileName), string.Empty), isWebSiteLogo ? "WebSiteLogo" : "MobileLogo");
        //}

        

        //private List<AzureMediaLibraryService.LogoConfigurations> getWebSiteLogoSettings()
        //{
        //    List<AzureMediaLibraryService.LogoConfigurations> sizeList = new List<AzureMediaLibraryService.LogoConfigurations>();
        //    //string webSiteLogoSettings = "450x150xMiddleLeft;320x320xMiddleCenter;130x130xMiddleCenter";
        //    string webSiteLogoSettings = WebConfigurationManager.AppSettings["MyAndromeda.WebsiteLogo.Settings"].ToString();
        //    webSiteLogoSettings = webSiteLogoSettings.ToLower();
        //    if (!string.IsNullOrEmpty(webSiteLogoSettings))
        //    {
        //        List<string> settings = webSiteLogoSettings.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //        foreach (string setting in settings)
        //        {
        //            sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(
        //                Convert.ToInt32(setting.Split(new char[] { 'x' })[0]),
        //                Convert.ToInt32(setting.Split(new char[] { 'x' })[1]),
        //                ParseEnum<ContentAlignment>(setting.Split(new char[] { 'x' })[2])));
        //        }
        //        return sizeList;
        //    }
        //    sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(450, 150, ContentAlignment.MiddleLeft));
        //    sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(320, 320, ContentAlignment.MiddleCenter));
        //    sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(130, 130, ContentAlignment.MiddleCenter));
        //    return sizeList;
        //}

        //private List<AzureMediaLibraryService.LogoConfigurations> getMobileSiteLogoSettings()
        //{
        //    List<AzureMediaLibraryService.LogoConfigurations> sizeList = new List<AzureMediaLibraryService.LogoConfigurations>();
        //    //string MobileLogoSettings = "450x150xMiddleLeft;320x320xMiddleCenter;130x130xMiddleCenter";
        //    string MobileLogoSettings = WebConfigurationManager.AppSettings["MyAndromeda.MobileLogo.Settings"].ToString();
        //    MobileLogoSettings = MobileLogoSettings.ToLower();
        //    if (!string.IsNullOrEmpty(MobileLogoSettings))
        //    {
        //        List<string> settings = MobileLogoSettings.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //        foreach (string setting in settings)
        //        {
        //            sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(
        //                Convert.ToInt32(setting.Split(new char[] { 'x' })[0]),
        //                Convert.ToInt32(setting.Split(new char[] { 'x' })[1]),
        //                ParseEnum<ContentAlignment>(setting.Split(new char[] { 'x' })[2])));
        //        }
        //        return sizeList;
        //    }
        //    sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(450, 150, ContentAlignment.MiddleLeft));
        //    sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(320, 320, ContentAlignment.MiddleCenter));
        //    sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(130, 130, ContentAlignment.MiddleCenter));
        //    return sizeList;
        //}

        //private T ParseEnum<T>(string value)
        //{
        //    return (T)Enum.Parse(typeof(T), value, true);
        //}

        //[HttpPost]
        //public ActionResult GeneralSettings(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    if (model.WebsiteConfigurations != null)
        //    {
        //        model.WebsiteConfigurations.GeneralSettings.IsEnterPostCode = model.WebsiteConfigurations.GeneralSettings.IsList ? false : true;
        //    }

        //    model.WebsiteConfigurations = UpdateConfigurations(Id, model.WebsiteConfigurations, "general");
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    return View("Index", model);
        //    //return RedirectToAction("Index", new { id = Id });
        //}

        //[HttpPost]
        //public ActionResult SocialNetworkSettings(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    if (model.WebsiteConfigurations != null)
        //    {
        //        model.WebsiteConfigurations.GeneralSettings.IsEnterPostCode = model.WebsiteConfigurations.GeneralSettings.IsList ? false : true;
        //        model.WebsiteConfigurations.SocialNetworkSettings.FacebookSettings.IsFollow = model.WebsiteConfigurations.SocialNetworkSettings.FacebookSettings.IsShare ? false : true;
        //        model.WebsiteConfigurations.SocialNetworkSettings.TwitterSettings.IsFollow = model.WebsiteConfigurations.SocialNetworkSettings.TwitterSettings.IsShare ? false : true;
        //        //model.WebsiteConfigurations.CustomerAccountSettings.IsEnableOtherFacebook = model.WebsiteConfigurations.CustomerAccountSettings.IsEnableFacebookAndromeda ? false : true;
        //    }
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    model.WebsiteConfigurations = UpdateConfigurations(Id, model.WebsiteConfigurations, "socialnetwork");
        //    return View("Index", model);
        //}

        //[HttpPost]
        //public ActionResult CustomerAccountSettings(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    if (model.WebsiteConfigurations != null) {
        //        //model.WebsiteConfigurations.CustomerAccountSettings.IsEnableOtherFacebook = model.WebsiteConfigurations.CustomerAccountSettings.IsEnableFacebookAndromeda ? false : true;
        //    }
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    model.WebsiteConfigurations = UpdateConfigurations(Id, model.WebsiteConfigurations, "customeraccountsettings");
        //    return View("Index", model);
        //}

        //[HttpPost]
        //public ActionResult MenuPageSettings(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    model.WebsiteConfigurations = UpdateConfigurations(Id, model.WebsiteConfigurations, "menupage");
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    return View("Index", model);
        //}

        //[HttpPost]
        //public ActionResult CheckoutPageSettings(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    model.WebsiteConfigurations = UpdateConfigurations(Id, model.WebsiteConfigurations, "checkout");
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    return View("Index", model);
        //}

        //[HttpPost]
        //public ActionResult UpsellingSettings(int Id, AndroWebOrderingWebSiteModel model)
        //{
        //    model.WebsiteConfigurations = UpdateConfigurations(Id, model.WebsiteConfigurations, "upselling");
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    return View("Index", model);
        //}

        //[HttpPost]
        //public ActionResult SaveThemeSettings(AndroWebOrderingWebSiteModel model)
        //{
        //    model.WebsiteConfigurations = UpdateConfigurations(model.Id, model.WebsiteConfigurations, "themes");
        //    model.AndroWebOrderingThemes = GetAndroWebOrderingThemesList();
        //    return View("Index", model);
        //}

        //private WebSiteConfigurations UpdateConfigurations(int Id, WebSiteConfigurations webConfig, string model)
        //{
        //    string settings = string.Empty;
        //    WebSiteConfigurations resultObj = new WebSiteConfigurations();
        //    if (webConfig != null)
        //    {
        //        //var website = webOrderingWebSiteDataService.Get(e => e.Id == Id);
        //        var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == Id);

        //        if (website != null)
        //        {
        //            if (!string.IsNullOrEmpty(website.PreviewSettings))
        //            {
        //                resultObj = androWebOrderingWebSiteService.DeSerializeConfigurations(website.PreviewSettings);
        //            }

        //            if (model.ToLower().Equals("sitedetails"))
        //            {
        //                resultObj.SiteDetails.WebSiteLogo = (string.IsNullOrEmpty(webConfig.SiteDetails.WebSiteLogo) ? resultObj.SiteDetails.WebSiteLogo : webConfig.SiteDetails.WebSiteLogo);
        //                if (!string.IsNullOrEmpty(webConfig.SiteDetails.WebsiteLogoPath))
        //                {
        //                    var urlParts = (webConfig.SiteDetails.WebsiteLogoPath.Split(new string[] { "/websites/" }, StringSplitOptions.RemoveEmptyEntries));
        //                    if (urlParts.Count() > 1)
        //                    {
        //                        webConfig.SiteDetails.WebsiteLogoPath = "/websites/" + urlParts[1];
        //                    }

        //                }
        //                else
        //                {
        //                    webConfig.SiteDetails.WebsiteLogoPath = resultObj.SiteDetails.WebsiteLogoPath;
        //                }

        //                resultObj.SiteDetails.MobileLogo = (string.IsNullOrEmpty(webConfig.SiteDetails.MobileLogo) ? resultObj.SiteDetails.WebSiteLogo : webConfig.SiteDetails.MobileLogo);
        //                //resultObj.SiteDetails.MobileLogoPath = (string.IsNullOrEmpty(webConfig.SiteDetails.MobileLogoPath) ? resultObj.SiteDetails.WebSiteLogo : webConfig.SiteDetails.MobileLogoPath);
        //                if (!string.IsNullOrEmpty(webConfig.SiteDetails.MobileLogoPath))
        //                {
        //                    var urlParts = (webConfig.SiteDetails.MobileLogoPath.Split(new string[] { "/websites/" }, StringSplitOptions.RemoveEmptyEntries));
        //                    if (urlParts.Count() > 1)
        //                    {
        //                        webConfig.SiteDetails.MobileLogoPath = "/websites/" + urlParts[1];
        //                    }
        //                }
        //                else
        //                {
        //                    webConfig.SiteDetails.MobileLogoPath = resultObj.SiteDetails.MobileLogoPath;
        //                }

        //                resultObj.SiteDetails = webConfig.SiteDetails;
        //            }
        //            else if (model.ToLower().Equals("themes"))
        //            {
        //                resultObj.ThemeSettings = webConfig.ThemeSettings;
        //            }
        //            else if (model.ToLower().Equals("general"))
        //            {
        //                resultObj.HomePageSettings = webConfig.HomePageSettings;
        //                resultObj.GeneralSettings = webConfig.GeneralSettings;
        //            }
        //            else if (model.ToLower().Equals("socialnetworks"))
        //            {
        //                resultObj.SocialNetworkSettings = webConfig.SocialNetworkSettings;
        //                //resultObj.CustomerAccountSettings = webConfig.CustomerAccountSettings;
        //            }
        //            else if (model.ToLower().Equals("customeraccountsettings")) {
        //                resultObj.CustomerAccountSettings = webConfig.CustomerAccountSettings;                        
        //            }
        //            else if (model.ToLower().Equals("menupage"))
        //            {
        //                resultObj.MenuPageSettings = webConfig.MenuPageSettings;
        //            }
        //            else if (model.ToLower().Equals("checkout"))
        //            {
        //                resultObj.CheckoutSettings = webConfig.CheckoutSettings;
        //            }
        //            else if (model.ToLower().Equals("upselling"))
        //            {
        //                resultObj.UpSellingSettings = webConfig.UpSellingSettings;
        //            }
        //            else if (model.ToLower().Equals("welcome"))
        //            {
        //                resultObj.Home.Welcome = webConfig.Home.Welcome;
        //                resultObj.Home.Menu = webConfig.Home.Menu;
        //            }
        //            else if (model.ToLower().Equals("footer"))
        //            {
        //                string TermsOfUse = HttpUtility.HtmlDecode(webConfig.Footer.TermsOfUse);
        //                string PrivacyPolicy = HttpUtility.HtmlDecode(webConfig.Footer.PrivacyPolicy);
        //                string CookiePolicy = HttpUtility.HtmlDecode(webConfig.Footer.CookiePolicy);
        //                resultObj.Footer.TermsOfUse = TermsOfUse;
        //                resultObj.Footer.PrivacyPolicy = PrivacyPolicy;
        //                resultObj.Footer.CookiePolicy = CookiePolicy;
        //                //resultObj.Footer = webConfig.Footer;
        //            }

        //            //else if (model.ToLower().Equals("carouselcontainer1"))
        //            //{
        //            //    resultObj.Home.Carousel1.CarouselContainer = webConfig.Home.Carousel1.CarouselContainer;
        //            //}
        //            //else if (model.ToLower().Equals("carouselcontainer2"))
        //            //{
        //            //    resultObj.Home.Carousel2.CarouselContainer = webConfig.Home.Carousel2.CarouselContainer;
        //            //}
        //            //else if (model.ToLower().Equals("carouselnavigator1"))
        //            //{
        //            //    resultObj.Home.Carousel1.CarouselNavigator = webConfig.Home.Carousel1.CarouselNavigator;
        //            //}
        //            //else if (model.ToLower().Equals("carouselnavigator2"))
        //            //{
        //            //    resultObj.Home.Carousel2.CarouselNavigator = webConfig.Home.Carousel2.CarouselNavigator;
        //            //}
        //            resultObj.WebSiteId = Id;
        //            resultObj.WebSiteName = website.Name;
        //            resultObj.ACSApplicationId = website.ACSApplicationId;

        //            settings = androWebOrderingWebSiteService.SerializeConfigurations(resultObj);
        //            website.PreviewSettings = settings;
        //            //webOrderingWebSiteDataService.Update(website);
        //            this.androWebOrderingWebSiteService.Update(website);
        //        }
        //    }

        //    if (!model.ToLower().Equals("sitedetails"))
        //    {
        //        if (!string.IsNullOrEmpty(resultObj.SiteDetails.WebsiteLogoPath))
        //        {
        //            resultObj.SiteDetails.WebsiteLogoPath = (resultObj.SiteDetails.WebsiteLogoPath.Contains("http") ? "" : RemoteLocationPath()) + resultObj.SiteDetails.WebsiteLogoPath.Remove(0, 1);
        //        }
        //        if (!string.IsNullOrEmpty(resultObj.SiteDetails.MobileLogoPath))
        //        {
        //            resultObj.SiteDetails.MobileLogoPath = (resultObj.SiteDetails.MobileLogoPath.Contains("http") ? "" : RemoteLocationPath()) + resultObj.SiteDetails.MobileLogoPath.Remove(0, 1);
        //        }
        //    }
        //    return resultObj;
        //}

        ///// <summary>
        ///// Returns image path from remote server
        ///// </summary>
        ///// <param name="folderPath">folder path</param>
        ///// <param name="fileName">image name</param>
        ///// <param name="sizeCode">Small, medium or large</param>
        ///// <returns></returns>
        //private string RemoteLocationPath()
        //{
        //    var host = this.siteMediaSevice.GetMediaServerWithDefault(-1).Address;
        //    var remoteLocation = this.storageService.RemoteLocation(host);
        //    return remoteLocation;
        //}

        //private string GetLogosFilePath(string folderPath, string fileName, string logoType)
        //{
        //    string size = string.Empty;
        //    if (logoType.Equals("WebSiteLogo", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        int webSiteOptedSize = Convert.ToInt32(WebConfigurationManager.AppSettings["MyAndromeda.WebsiteLogo.OptedSize"]);
        //        List<AzureMediaLibraryService.LogoConfigurations> settings = getWebSiteLogoSettings();
        //        size = string.Format("{0}x{1}", settings[webSiteOptedSize].Width, settings[webSiteOptedSize].Height);
        //    }
        //    else if (logoType.Equals("MobileLogo", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        int mobileOptedSize = Convert.ToInt32(WebConfigurationManager.AppSettings["MyAndromeda.MobileLogo.OptedSize"]);
        //        List<AzureMediaLibraryService.LogoConfigurations> settings = getMobileSiteLogoSettings();
        //        size = string.Format("{0}x{1}", settings[mobileOptedSize].Width, settings[mobileOptedSize].Height);
        //    }
        //    else if (logoType.Equals("Carousel1", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        int carousel1OptedSize = Convert.ToInt32(WebConfigurationManager.AppSettings["MyAndromeda.Carousel1Logo.OptedSize"]);
        //        List<AzureMediaLibraryService.LogoConfigurations> settings = GetCarouselLogoSettings(1);
        //        size = string.Format("{0}x{1}", settings[carousel1OptedSize].Width, settings[carousel1OptedSize].Height);
        //    }
        //    else if (logoType.Equals("Carousel2", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        int carousel2OptedSize = Convert.ToInt32(WebConfigurationManager.AppSettings["MyAndromeda.Carousel2Logo.OptedSize"]);
        //        List<AzureMediaLibraryService.LogoConfigurations> settings = GetCarouselLogoSettings(2);
        //        size = string.Format("{0}x{1}", settings[carousel2OptedSize].Width, settings[carousel2OptedSize].Height);
        //    }

        //    var complexName = fileName + size + ".png";
        //    var host = this.siteMediaSevice.GetMediaServerWithDefault(-1).Address;
        //    var remoteLocation = this.storageService.RemoteLocation(host) + folderPath;
        //    var remoteFullPath = remoteLocation.EndsWith("/") ? string.Format("{0}{1}", remoteLocation, complexName) :
        //                         string.Format("{0}/{1}", remoteLocation, complexName);

        //    return remoteFullPath;
        //}

        //private IList<ThemeSettings> GetAndroWebOrderingThemesList()
        //{
        //    string remoteLocationPath = RemoteLocationPath();
        //    IList<ThemeSettings> result = this.androWebOrderingWebSiteService.List().Select(s =>
        //            new ThemeSettings { Description = s.Description, ThemeName = s.ThemeName, ThemePath = remoteLocationPath + s.ThemePath, Height = s.Height, Width = s.Width, Id = s.Id }).ToList();

        //    return result;
        //}

        //public string UploadCarouselImages(HttpPostedFileBase logo, string folderPath, int Id, string fileName, string imageCategory)
        //{
        //    List<AzureMediaLibraryService.LogoConfigurations> sizeList = new List<AzureMediaLibraryService.LogoConfigurations>();
        //    int imageTypeId = string.Equals("carousel1", imageCategory, StringComparison.CurrentCultureIgnoreCase) ? 1 : 2;
        //    sizeList = GetCarouselLogoSettings(imageTypeId);

        //    var statuses = new List<Menus.Data.ThumbnailFileResult>();

        //    MemoryStream origin = new MemoryStream();
        //    logo.InputStream.CopyTo(origin);
        //    origin.Seek(0, SeekOrigin.Begin);

        //    var filesReposonses = mediaLibraryServiceProvider
        //        .ImportLogo(origin, folderPath, fileName, sizeList)
        //        .OrderByDescending(e => e.Height).ToList();

        //    statuses.AddRange(filesReposonses);
        //    return GetLogosFilePath(folderPath, fileName.Replace(Path.GetExtension(fileName), string.Empty), imageCategory);
        //}

        //private List<AzureMediaLibraryService.LogoConfigurations> GetCarouselLogoSettings(int id)
        //{
        //    List<AzureMediaLibraryService.LogoConfigurations> sizeList = new List<AzureMediaLibraryService.LogoConfigurations>();
        //    //string webSiteLogoSettings = "450x150xMiddleLeft;320x320xMiddleCenter;130x130xMiddleCenter";

        //    //this will have to be popped out of the theme meta data eventually.
        //    string carouselLogoSettings = //id == 1 ?
        //        WebConfigurationManager.AppSettings["MyAndromeda.Carousel1.Settings"].ToString();
        //    //: WebConfigurationManager.AppSettings["MyAndromeda.Carousel2.Settings"].ToString();

        //    carouselLogoSettings = carouselLogoSettings.ToLower();

        //    if (!string.IsNullOrWhiteSpace(carouselLogoSettings))
        //    {
        //        List<string> settings = carouselLogoSettings.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //        foreach (string setting in settings)
        //        {
        //            sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(
        //                Convert.ToInt32(setting.Split(new char[] { 'x' })[0]),
        //                Convert.ToInt32(setting.Split(new char[] { 'x' })[1]),
        //                ParseEnum<ContentAlignment>(setting.Split(new char[] { 'x' })[2])));
        //        }

        //        return sizeList;
        //    }

        //    sizeList.Add(new AzureMediaLibraryService.LogoConfigurations(970, 270, ContentAlignment.MiddleLeft));
        //    return sizeList;
        //}
    }
}