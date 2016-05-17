using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.AndroWebOrdering.Models;
using MyAndromeda.Services.WebOrdering.Services;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using AndroAdminDataAccess.Domain.WebOrderingSetup;

namespace MyAndromeda.Web.Areas.AndroWebOrdering.Controllers
{
    public class AndroWebOrderingController : Controller
    {
        private readonly IAuthorizer authorizer;
        private readonly IAndroWebOrderingWebSiteService androWebOrderingWebSiteService;
        private readonly ICurrentSite currentSite;

        public AndroWebOrderingController(
            IAuthorizer authorizer,
            IAndroWebOrderingWebSiteService androWebOrderingWebSiteService, 
            ICurrentSite currentSite)
        {
            this.authorizer = authorizer;
            this.androWebOrderingWebSiteService = androWebOrderingWebSiteService;
            this.currentSite = currentSite;
        }

        public ActionResult List()
        {
            //IList<AndroWebOrderingWebSiteModel> resultsList = usersDataService.GetAndroWebOrderingSitesForUser(workContext.CurrentUser.User.Id).Select(s => new AndroWebOrderingWebSiteModel { Id = s.Id, Name = s.Name }).ToList();
            IList<AndroWebOrderingWebSiteModel> resultsList = this.currentSite.AndroWebOrderingSites
                .Select(s => new AndroWebOrderingWebSiteModel { 
                    Id = s.Id, 
                    Name = s.Name, 
                    Enabled = s.Enabled,
                    AcsApplicationId = s.ACSApplicationId,
                    Devices = this.currentSite.Store
                            .StoreDevices
                            .Where(e=> !e.Removed)
                            .Select(e=> e.Device.Name).ToArray()
                })
                .ToList();
            
            return View(resultsList);
        }

        [HttpGet]
        public ActionResult Delete(int id) 
        {
            if (!this.authorizer.Authorize(UserPermissions.DeleteWebsite)) 
            {
                return new HttpUnauthorizedResult();
            }

            var website = this.androWebOrderingWebSiteService.GetWebOrderingWebsite(e => e.Id == id);
            this.androWebOrderingWebSiteService.Delete(website);

            return RedirectToAction("List");
        }

        // GET: AndroWebOrdering/AndroWebOrdering
        [HttpGet]
        public ActionResult Index(int id)
        {
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
            string serviceUrl = ConfigurationManager.AppSettings["MyAndromeda.WebOrderingWebSite.CreateService"];
            WebRequest request = WebRequest.Create(serviceUrl);
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

    }
}