using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AndroAdmin.Helpers;
using AndroAdminDataAccess.Domain;
using CloudSync;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace AndroAdmin.Controllers
{
    public class AndroWebOrderingController : BaseController
    {
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult List([DataSourceRequest]DataSourceRequest request) 
        {
            var query = this.AndroWebOrderingWebSiteDAO.Query().Select(e => new { 
                e.Id,
                e.Name,
                e.LiveDomainName,
                e.PreviewDomainName,
                StoresCount = e.ACSApplication.ACSApplicationSites.Count(),
                Status = e.Enabled ? "Enabled" : "Disabled",
            //    SubscriptionName = e.AndroWebOrderingSubscriptionType.Subscription,
                EnvironmentName = e.ACSApplication.Environment.Name
            }).OrderBy(e=> e.Name);

            var result = query.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void PopulateCultureDropDowns(AndroWebOrderingWebsite website)
        {
            var cultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures);

            var uiCultures = cultures.Select(e => new CultureChoiceViewModel
            {
                Name = System.Globalization.CultureInfo.CreateSpecificCulture(e.Name).Name,
                EnglishName = e.EnglishName
            }).OrderBy(e => e.Name);

            var timeZones = TimeZoneInfo.GetSystemTimeZones()
                .Select(e => new TimeZoneViewModel()
                {
                    DisplayName = e.DisplayName,
                    StandardName = e.StandardName,
                    Id = e.Id,
                })
                .OrderBy(e => e.DisplayName);

            website.CultureChoices = uiCultures;
            website.TimezoneChoices = timeZones;
        }

        private void UpdatesStoreCultures(AndroWebOrderingWebsite website)
        {
            if (website.MappedSiteIds != null && website.MappedSiteIds.Count > 0)
            {
                foreach (int siteId in website.MappedSiteIds)
                {
                    Store currentStore = this.StoreDAO.GetById(siteId);

                    if (currentStore != null)
                    {
                        currentStore.TimeZoneInfoId = website.SelectedTimeZoneId ?? "GMT Standard Time";
                        currentStore.UiCulture = website.SelectedCultureType ?? "en-GB";

                        //we shall let other things worry about this :)
                        //TimeZoneInfo ti = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id == currentStore.TimeZoneInfoId).SingleOrDefault();
                        //string offset = "0";
                        //if (ti.BaseUtcOffset.Hours != 0)
                        //    offset = ti.BaseUtcOffset.Hours.ToString("+#;-#;0"); 
                        //if(ti.BaseUtcOffset.Minutes != 0)
                        //    offset = offset + ":" + ti.BaseUtcOffset.Minutes.ToString("D" + 2);
                        //currentStore.TimeZone = "GMT" + offset;

                        this.StoreDAO.Update(currentStore);
                    }
                }
            }
        }

        public ActionResult ExistingAcsApplication()
        {
            var applications = this.ACSApplicationDAO.GetAll();

            return View(applications);
        }

        [HttpGet]
        public ActionResult Edit(int? id, int? acsApplicationId)
        {
            AndroWebOrderingWebsite webSite = this.AndroWebOrderingWebSiteDAO.GetAndroWebOrderingWebsiteById(id.GetValueOrDefault());
            webSite.UpdatedMappedStoreIds = webSite.MappedSiteIds != null ? string.Join(",", webSite.MappedSiteIds) : string.Empty;

            if (id == null) 
            { 
                webSite.Enabled = true;
            }

            PopulateCultureDropDowns(webSite);

            if (webSite.MappedSiteIds != null && webSite.MappedSiteIds.Count > 0)
            {
                Store currentStore = this.StoreDAO.GetById(webSite.MappedSiteIds[0]);

                webSite.SelectedCultureType = currentStore.UiCulture ?? "en-GB";
                webSite.SelectedTimeZoneId = currentStore.TimeZoneInfoId ?? "GMT Standard Time";
            }

            if (acsApplicationId.HasValue)
            {
                var acsApplication = this.ACSApplicationDAO.GetById(acsApplicationId.GetValueOrDefault());
                webSite.ACSApplication = acsApplication;
                webSite.ACSApplicationId = acsApplication.Id;
                webSite.Name = acsApplication.Name;

                var storeIds = this.StoreDAO.GetByACSApplicationId(acsApplication.Id).Select(e => e.Id).ToList();
                webSite.UpdatedMappedStoreIds = storeIds.Count > 0 ? string.Join(",", storeIds) : string.Empty;
            }

            return View(webSite);
        }

        private void Validate(AndroWebOrderingWebsite webSite) 
        {
            IList<AndroWebOrderingWebsite> webSitesList = this.AndroWebOrderingWebSiteDAO.GetAll().ToList();
            
            //List<string> previewDomainNameList = webSitesList.Select(x => x.PreviewDomainName.ToLower()).ToList();
            //List<string> liveDomainNameList = webSitesList.Select(x => x.LiveDomainName.ToLower()).ToList();
            if (webSite.PreviewDomainName != null)
                webSite.PreviewDomainName = webSite.PreviewDomainName.Trim();

            if (webSite.LiveDomainName != null)
                webSite.LiveDomainName = webSite.LiveDomainName.Trim();

            if (string.IsNullOrWhiteSpace(webSite.PreviewDomainName))
            {
                TempData["errorMessage"] = "Preview Domain Name is required";
                this.ModelState.AddModelError("PreviewDomainName", "Preview Domain Name is required");
            }

            if (string.IsNullOrWhiteSpace(webSite.LiveDomainName))
            {
                TempData["errorMessage"] = "Live Domain Name is required";
                this.ModelState.AddModelError("LiveDomainName", "Live Domain Name is required");
            }

            if (webSite.LiveDomainName.ToLower().Equals(webSite.PreviewDomainName.ToLower()))
            {
                TempData["errorMessage"] = "'Live Domain' and 'Preview Domain' should be different";
                this.ModelState.AddModelError("LivePreviewDomain", "Live Domain' and 'Preview Domain' should be different");
                return;
            }

            if (!IsValidWebSiteName(webSite.Name))
            {
                TempData["errorMessage"] = "Invalid Website Name, Allowed characters [A-Z, a-z, 0-9, &, -, _, space]";
                this.ModelState.AddModelError("WebsiteName", "Invalid Website Name");
                return;
            }


            if (!IsValidDomainName(webSite.PreviewDomainName) || webSite.PreviewDomainName.Trim().Contains("/"))
            {
                TempData["errorMessage"] = "Invalid Preview Domain";

                this.ModelState.AddModelError("PreviewDomain", "Invalid Preview Domain");
                return;
            }

            if (!IsValidDomainName(webSite.LiveDomainName) || webSite.LiveDomainName.Trim().Contains("/"))
            {
                TempData["errorMessage"] = "Invalid Live Domain";

                this.ModelState.AddModelError("LiveDomain", "Invalid Live Domain");
                return;
            }

            if (string.IsNullOrEmpty(webSite.UpdatedMappedStoreIds)) 
            {
                TempData["errorMessage"] = "Select at least one store";
                this.ModelState.AddModelError("UpdatedMappedStoreIds", "Select at least one store");
            }

            if (webSitesList.Any(x => x.LiveDomainName.Equals(webSite.PreviewDomainName, StringComparison.InvariantCultureIgnoreCase) && x.Id != webSite.Id)
                || webSitesList.Any(x => x.PreviewDomainName.Equals(webSite.PreviewDomainName, StringComparison.InvariantCultureIgnoreCase) && x.Id != webSite.Id))
            {
                TempData["errorMessage"] = "Preview Domain already exists";

                this.ModelState.AddModelError("PreviewDomainName", "The preview domain already exists");
            }

            if (webSitesList.Any(x => x.LiveDomainName.Equals(webSite.LiveDomainName, StringComparison.InvariantCultureIgnoreCase) && x.Id != webSite.Id)
                || webSitesList.Any(x => x.PreviewDomainName.Equals(webSite.LiveDomainName, StringComparison.InvariantCultureIgnoreCase) && x.Id != webSite.Id))
            {
                TempData["errorMessage"] = "Live Domain already exists";

                this.ModelState.AddModelError("LiveDomainName", "The live domain already exists");
            }
        }

        [HttpPost]
        public ActionResult Edit(AndroWebOrderingWebsite webSite)
        {
            List<string> errorList = new List<string>();
            List<string> createSiteErrors = new List<string>();
            List<string> updateSiteErrors = new List<string>();

            bool success = false;

            if (webSite == null)
            {
                this.ModelState.AddModelError("", "Cant process the data!");
                return View(webSite);
            }

            if (webSite.ACSApplicationId == 0)
            {
                webSite.ACSApplication = new ACSApplication { ExternalApplicationId = Guid.NewGuid().ToString(), ExternalApplicationName = webSite.Name, Name = webSite.Name };
            }
            else
            {
                webSite.ACSApplication = this.ACSApplicationDAO.GetById(webSite.ACSApplicationId);
            }

            this.Validate(webSite);

            if (!this.ModelState.IsValid) 
            {
                PopulateCultureDropDowns(webSite);

                webSite.SubscriptionsList = this.AndroWebOrderingSubscriptionDAO.GetAll();
                webSite.Chains = this.ChainDAO.GetAll();
                webSite.AllStores = this.StoreDAO.GetAll();
                webSite.EnvironmentsList = this.EnvironmentDAO.GetAll();

                return View(webSite);
            }

            webSite.MappedSiteIds = new List<int>();
            foreach (string id in webSite.UpdatedMappedStoreIds.Split(',').ToArray())
            {
                if (!webSite.MappedSiteIds.Contains(Convert.ToInt32(id)))
                {
                    webSite.MappedSiteIds.Add(Convert.ToInt32(id));
                }
            }

            if (webSite.Id > 0)
            {
                // Edit 
                webSite.ACSApplication = new ACSApplication { ExternalApplicationName = webSite.Name, Name = webSite.Name };
                AndroWebOrderingWebsite webSiteFromDB = this.AndroWebOrderingWebSiteDAO.GetAndroWebOrderingWebsiteById(webSite.Id);

                if (!ModelState.IsValid) { return this.View(webSite); }

                webSite.ThemeId = webSiteFromDB.ThemeId;

                bool isWebsiteDisabled = ((webSiteFromDB.Enabled == true) && webSite.Enabled == false);
                bool isWebsiteEnabled = ((webSiteFromDB.Enabled == false) && webSite.Enabled == true);

                List<string> webSiteUpdateErrors = this.AndroWebOrderingWebSiteDAO.Update(webSite);

                if (webSiteUpdateErrors.Count == 0)
                {
                    success = true;
                    AndroAdmin.Helpers.ErrorHelper.LogInfo("AndroAdmin.Controllers.AndroWebOrderingController.UpdateWebsite",
                        string.Format("{0}: Website updated", webSite.Name));
                }
                else
                {
                    foreach (string error in webSiteUpdateErrors)
                    {
                        AndroAdmin.Helpers.ErrorHelper.LogError("AndroAdmin.Controllers.AndroWebOrderingController.UpdateWebsite", new Exception(error));
                    }
                }

                // update Website settings and time zones
                if (success)
                {
                    UpdatesStoreCultures(webSite);
                    updateSiteErrors = GetWebSiteSettingsOnUpdate(webSite, isWebsiteDisabled, isWebsiteEnabled);
                    List<string> webSiteUpdateTimezoneErrors = this.AndroWebOrderingWebSiteDAO.Update(webSite);
                    if (webSiteUpdateTimezoneErrors.Count > 0)
                    {
                        foreach (string error in webSiteUpdateTimezoneErrors)
                        {
                            AndroAdmin.Helpers.ErrorHelper.LogError("AndroAdmin.Controllers.AndroWebOrderingController.UpdateWebsiteTZ", new Exception(error));
                        }
                    }
                    else
                    {
                        AndroAdmin.Helpers.ErrorHelper.LogInfo("AndroAdmin.Controllers.AndroWebOrderingController.UpdateWebsite", string.Format("{0}: Website time zones updated", webSite.Name));
                    }
                }
            }
            else
            {
                // Add New Web-site -> prepare default JSON settings string
                webSite.ThemeId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultThemeId"]);
                
                List<string> webSiteAddErrors = this.AndroWebOrderingWebSiteDAO.Add(webSite);

                if (webSiteAddErrors.Count == 0)
                {
                    AndroAdmin.Helpers.ErrorHelper.LogInfo("AndroAdmin.Controllers.AndroWebOrderingController.AddWebsite", string.Format("{0}: Website Added", webSite.Name));
                    AndroWebOrderingWebsite addedSite =
                        this.AndroWebOrderingWebSiteDAO.GetAll()
                        .Where(x => x.Name.Equals(webSite.Name, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault();

                    if (addedSite != null)
                    {
                        createSiteErrors.AddRange(GetNewWebSiteDefaultSettings(addedSite));
                        if (createSiteErrors.Count == 0)
                        {
                            // update Website settings and time zones
                            List<string> webSiteUpdateErrors = this.AndroWebOrderingWebSiteDAO.Update(addedSite);
                            if (webSiteUpdateErrors.Count == 0)
                            {
                                success = true;
                                AndroAdmin.Helpers.ErrorHelper.LogInfo("AndroAdmin.Controllers.AndroWebOrderingController.AddWebsite", string.Format("{0}: Website time zones updated", webSite.Name));
                            }
                            else
                            {
                                foreach (string error in webSiteUpdateErrors)
                                {
                                    AndroAdmin.Helpers.ErrorHelper.LogError("AndroAdmin.Controllers.AndroWebOrderingController.UpdateWebsiteTZ", new Exception(error));
                                }
                            }
                            UpdatesStoreCultures(webSite);
                            // call service
                            createSiteErrors.AddRange(OnWebSiteCreated(addedSite));
                        }
                    }
                }
                else
                {
                    foreach (string error in webSiteAddErrors)
                    {
                        AndroAdmin.Helpers.ErrorHelper.LogError("AndroAdmin.Controllers.AndroWebOrderingController.AddWebsite", new Exception(error));
                    }
                    createSiteErrors.AddRange(webSiteAddErrors);
                }
            }

            // Were there any errors?
            errorList.AddRange(createSiteErrors);
            errorList.AddRange(updateSiteErrors);

            if (!success)
            {
                errorList.Add("Error while saving website " + webSite.Name);
            }

            if (errorList.Count == 0)
            {
                // Synchronize with ACS Cloud
                this.SyncWithCloud(webSite, errorList);
            }

            if (errorList.Count == 0)
            {
                // Sync with signpost server
                this.SyncWithSignpost(webSite, errorList);
            }

            // Fin...
            if (errorList.Count == 0)
            {
                TempData["message"] = "Website " + webSite.Name + " updated successfully";
            }
            else
            {
                TempData["errorMessage"] = string.Join(System.Environment.NewLine, errorList);
            }

            return RedirectToAction("Index");
        }

        private void SyncWithCloud(AndroWebOrderingWebsite webSite, List<string> errorList)
        {
            // Do the cloud sync
            string syncErrorMsg = SyncHelper.ServerSync();

            // Did the sync work?
            if (syncErrorMsg.Length > 0)
            {
                errorList.Add("Failed to synchronise with one or more ACS cloud server" + syncErrorMsg);
            }
        }

        private void SyncWithSignpost(AndroWebOrderingWebsite webSite, List<string> errorList)
        {
            // Do the signpost server sync
            string syncErrorMsg = SignpostServerSync.ServerSync();

            // Did the sync work?
            if (!String.IsNullOrEmpty(syncErrorMsg))
            {
                errorList.Add("Failed to synchronise with one or more signpost server" + syncErrorMsg);
            }
        }

        private bool IsValidDomainName(string domainName)
        {
            //return Regex.IsMatch(domainName, @"^([a-zA-Z0-9_-]*(?:\.[a-zA-Z0-9_-]*)+):?([0-9]+)?/?");
            return Regex.IsMatch(domainName, @"^[\w\-\._]+\.[a-zA-Z]{2,3}$");
        }

        private bool IsValidWebSiteName(string websiteName)
        {
            return true;
            ///it annoys marketing | never add again :) 
            //return Regex.IsMatch(websiteName, @"^[a-zA-Z0-9&\-_\s]+$");
        }

        private List<string> OnWebSiteCreated(AndroWebOrderingWebsite webSite)
        {
            AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations previewConfigObj = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.DeserializeJson(webSite.PreviewSettings);
            AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations liveConfigObj = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.DeserializeJson(webSite.LiveSettings);

            liveConfigObj.SiteDetails.DomainName = liveConfigObj.LiveDomainName;
            liveConfigObj.SiteDetails.OldDomainName = string.Empty;

            previewConfigObj.SiteDetails.DomainName = previewConfigObj.PreviewDomainName;
            previewConfigObj.SiteDetails.OldDomainName = string.Empty;

            List<string> liveErrors = CallService(liveConfigObj);
            List<string> previewErrors = CallService(previewConfigObj);
            List<string> errorList = new List<string>();

            errorList.AddRange(liveErrors);
            errorList.AddRange(previewErrors);

            return errorList;
        }

        private List<string> CallService(AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations config)
        {
            string serviceUrl = ConfigurationManager.AppSettings["AndroAdmin.WebOrderingWebSite.CreateService"];

            List<string> errorList = new List<string>();
            WebRequest request = WebRequest.Create(serviceUrl);
            UTF8Encoding encoding = new UTF8Encoding();

            request.Method = "POST";
            request.ContentType = "application/json";

            var data = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(config);
            var postData = encoding.GetBytes(data);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }

            try
            {
                var response = request.GetResponse();
                //do we need to check the response or status id?
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        errorList.Add(reader.ReadToEnd());
                        AndroAdmin.Helpers.ErrorHelper.LogError("AndroAdmin.Controllers.AndroWebOrderingController.CallService", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                // log error
                AndroAdmin.Helpers.ErrorHelper.LogError("AndroAdmin.Controllers.AndroWebOrderingController.CallService", ex);
            }

            return errorList;
        }

        private List<string> GetNewWebSiteDefaultSettings(AndroWebOrderingWebsite webSite)
        {
            List<string> errors = new List<string>();

            AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations configObj = new AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations();
            configObj.WebSiteId = webSite.Id;
            configObj.WebSiteName = webSite.Name;
            //configObj.ACSApplicationId = webSite.ACSApplicationId;
            configObj.ACSApplicationId = webSite.ExternalApplicationId;
            configObj.IsWebSiteEnabled = webSite.Enabled;
            configObj.DisabledReason = webSite.DisabledReason;
            configObj.ChainId = webSite.ChainId;
            configObj.LiveDomainName = webSite.LiveDomainName;
            configObj.PreviewDomainName = webSite.PreviewDomainName;
            configObj.SubScriptionTypeId = webSite.SubscriptionTypeId;

            configObj.SiteDetails.DomainName = webSite.LiveDomainName;
            configObj.SiteDetails.WebsiteLogoPath = " ";
            configObj.SocialNetworkSettings.FacebookSettings = null;
            configObj.SocialNetworkSettings.InstagramSettings = null;
            configObj.SocialNetworkSettings.TwitterSettings = null;
            configObj.SocialNetworkSettings.PinterestSettings = null;

            if (webSite.Enabled)
                configObj.ThemeSettings.Id = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultThemeId"]);
            else
                configObj.ThemeSettings.Id = Convert.ToInt32(ConfigurationManager.AppSettings["WebsiteDisabledThemeId"]);

            ThemeAndroWebOrdering theme = this.AndroWebOrderingThemeDAO.GetAndroWebOrderingThemeById(configObj.ThemeSettings.Id);
            if (theme != null)
            {
                webSite.ThemeId = configObj.ThemeSettings.Id;
                configObj.ThemeSettings.Description = theme.Description;
                configObj.ThemeSettings.ThemePath = theme.Source;
                configObj.ThemeSettings.FileName = theme.FileName;
                configObj.ThemeSettings.Height = theme.Height;
                configObj.ThemeSettings.Width = theme.Width;
            }
            else
            {
                //errors.Add("Theme not found; Id - " + configObj.ThemeSettings.Id);
                // JSON to be updated in db; Theme not found error is handled by web-service 
            }
            webSite.PreviewSettings = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(configObj);
            webSite.LiveSettings = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(configObj);
            return errors;
        }

        private List<string> GetWebSiteSettingsOnUpdate(AndroWebOrderingWebsite webSite, bool isWebsiteDisabled, bool isWebsiteEnabled)
        {
            AndroWebOrderingWebsite webSiteFromDB = this.AndroWebOrderingWebSiteDAO.GetAndroWebOrderingWebsiteById(webSite.Id);
            AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations previewConfigObj;
            AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations liveConfigObj;

            if (string.IsNullOrEmpty(webSiteFromDB.PreviewSettings))
            {
                previewConfigObj = new AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations();
                previewConfigObj.WebSiteId = webSite.Id;
                previewConfigObj.WebSiteName = webSite.Name;
                previewConfigObj.ACSApplicationId = webSiteFromDB.ExternalApplicationId;
                previewConfigObj.IsWebSiteEnabled = webSite.Enabled;
                previewConfigObj.DisabledReason = webSite.DisabledReason;
                previewConfigObj.ChainId = webSite.ChainId;
                previewConfigObj.LiveDomainName = webSite.LiveDomainName;
                previewConfigObj.PreviewDomainName = webSite.PreviewDomainName;
                previewConfigObj.SubScriptionTypeId = webSite.SubscriptionTypeId;
                previewConfigObj.SiteDetails.WebsiteLogoPath = " ";
                if (webSite.Enabled)
                    previewConfigObj.ThemeSettings.Id = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultThemeId"]);
                else
                    previewConfigObj.ThemeSettings.Id = Convert.ToInt32(ConfigurationManager.AppSettings["WebsiteDisabledThemeId"]);
            }
            else
            {
                previewConfigObj = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.DeserializeJson(webSiteFromDB.PreviewSettings);
            }

            if (string.IsNullOrEmpty(webSiteFromDB.LiveSettings))
            {
                liveConfigObj = new AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations();
                liveConfigObj.WebSiteId = webSite.Id;
                liveConfigObj.WebSiteName = webSite.Name;
                liveConfigObj.ACSApplicationId = webSiteFromDB.ExternalApplicationId;
                liveConfigObj.IsWebSiteEnabled = webSite.Enabled;
                liveConfigObj.DisabledReason = webSite.DisabledReason;
                liveConfigObj.ChainId = webSite.ChainId;
                liveConfigObj.LiveDomainName = webSite.LiveDomainName;
                liveConfigObj.PreviewDomainName = webSite.PreviewDomainName;
                liveConfigObj.SubScriptionTypeId = webSite.SubscriptionTypeId;
                liveConfigObj.SiteDetails.WebsiteLogoPath = " ";
                if (webSite.Enabled)
                    liveConfigObj.ThemeSettings.Id = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultThemeId"]);
                else
                    liveConfigObj.ThemeSettings.Id = Convert.ToInt32(ConfigurationManager.AppSettings["WebsiteDisabledThemeId"]);
            }
            else
            {
                liveConfigObj = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.DeserializeJson(webSiteFromDB.LiveSettings);
            }

            // Preview update
            previewConfigObj.WebSiteName = webSite.Name;
            previewConfigObj.IsWebSiteEnabled = webSite.Enabled;
            previewConfigObj.DisabledReason = webSite.DisabledReason;
            previewConfigObj.ChainId = webSite.ChainId;
            previewConfigObj.SubScriptionTypeId = webSite.SubscriptionTypeId;

            // Live update
            liveConfigObj.WebSiteName = webSite.Name;
            liveConfigObj.IsWebSiteEnabled = webSite.Enabled;
            liveConfigObj.DisabledReason = webSite.DisabledReason;
            liveConfigObj.ChainId = webSite.ChainId;
            liveConfigObj.SubScriptionTypeId = webSite.SubscriptionTypeId;

            bool isLiveDomainModified = false;
            bool isPreviewDomainModified = false;

            // If live-domain-name is changed
            if (!webSite.LiveDomainName.Equals(liveConfigObj.SiteDetails.DomainName, StringComparison.InvariantCultureIgnoreCase))
            {
                // live-domain settings
                isLiveDomainModified = true;
                liveConfigObj.OldLiveDomainName = liveConfigObj.LiveDomainName;
                liveConfigObj.LiveDomainName = webSite.LiveDomainName;

                liveConfigObj.SiteDetails.OldDomainName = liveConfigObj.SiteDetails.DomainName;
                liveConfigObj.SiteDetails.DomainName = webSite.LiveDomainName;
            }

            if (!webSite.PreviewDomainName.Equals(previewConfigObj.PreviewDomainName, StringComparison.InvariantCultureIgnoreCase))
            {
                // preview-domain settings
                isPreviewDomainModified = true;
                previewConfigObj.OldPreviewDomainName = previewConfigObj.PreviewDomainName;
                previewConfigObj.PreviewDomainName = webSite.PreviewDomainName;
                //previewConfigObj.SiteDetails.OldDomainName = previewConfigObj.SiteDetails.DomainName;
                //previewConfigObj.SiteDetails.DomainName = webSite.LiveDomainName;
            }

            webSite.LiveSettings = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(liveConfigObj);
            webSite.PreviewSettings = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(previewConfigObj);

            List<string> errors = new List<string>();
            try
            {
                List<string> errorsLiveDomainChanged = new List<string>();
                List<string> errorsPreviewDomainChanged = new List<string>();
                List<string> errorsLiveDisabled = new List<string>();
                List<string> errorsLiveEnabled = new List<string>();

                if (isLiveDomainModified)
                {
                    errorsLiveDomainChanged = CallService(liveConfigObj);
                    bool isSuccess = (errorsLiveDomainChanged.Count == 0);
                    if (isSuccess)
                    {
                        liveConfigObj.SiteDetails.OldDomainName = string.Empty;
                        webSite.LiveSettings = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(liveConfigObj);
                    }
                    previewConfigObj.OldLiveDomainName = previewConfigObj.LiveDomainName;
                    previewConfigObj.LiveDomainName = webSite.LiveDomainName;

                    previewConfigObj.SiteDetails.OldDomainName = string.Empty;// previewConfigObj.SiteDetails.DomainName;
                    previewConfigObj.SiteDetails.DomainName = webSite.LiveDomainName;
                    webSite.PreviewSettings = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(previewConfigObj);
                }

                if (isPreviewDomainModified)
                {
                    // TODO: if the website is in enable state, call the service to create the new folder structure with the new-domain name for preview-site
                    previewConfigObj.SiteDetails.OldDomainName = previewConfigObj.OldPreviewDomainName;
                    previewConfigObj.SiteDetails.DomainName = webSite.PreviewDomainName;
                    errorsPreviewDomainChanged = CallService(previewConfigObj);
                }

                // disable web-site based on disable theme id
                if (isWebsiteDisabled)
                {
                    int themeId = Convert.ToInt32(ConfigurationManager.AppSettings["WebsiteDisabledThemeId"]);
                    ThemeAndroWebOrdering theme = this.AndroWebOrderingThemeDAO.GetAndroWebOrderingThemeById(themeId);
                    if (theme != null)
                    {
                        liveConfigObj.ThemeSettings.Id = themeId;
                        liveConfigObj.ThemeSettings.ThemePath = theme.Source;
                        liveConfigObj.ThemeSettings.ThemeName = theme.FileName;
                        liveConfigObj.ThemeSettings.Height = theme.Height;
                        liveConfigObj.ThemeSettings.Width = theme.Width;
                        errorsLiveDisabled = CallService(liveConfigObj);
                        webSite.LiveSettings = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(liveConfigObj);
                    }
                    else
                    {
                        //errorsLiveDisabled.Add("Theme not found; Id - " + themeId);
                        // JSON to be updated in db; Theme not found error is handled by web-service 
                    }
                }
                if (isWebsiteEnabled)
                {
                    int themeId = webSiteFromDB.ThemeId ?? 0;
                    int disabledThemeId = Convert.ToInt32(ConfigurationManager.AppSettings["WebsiteDisabledThemeId"]);
                    int enabledThemeId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultThemeId"]);
                    ThemeAndroWebOrdering theme = this.AndroWebOrderingThemeDAO.GetAndroWebOrderingThemeById(themeId);
                    if (theme != null)
                    {
                        if (theme.Id == disabledThemeId)
                        {
                            theme = this.AndroWebOrderingThemeDAO.GetAndroWebOrderingThemeById(enabledThemeId);
                        }
                        if (theme != null)
                        {
                            liveConfigObj.ThemeSettings.Id = theme.Id;
                            liveConfigObj.ThemeSettings.Description = theme.Description;
                            liveConfigObj.ThemeSettings.ThemePath = theme.Source;
                            liveConfigObj.ThemeSettings.ThemeName = theme.FileName;
                            liveConfigObj.ThemeSettings.Height = theme.Height;
                            liveConfigObj.ThemeSettings.Width = theme.Width;
                            errorsLiveEnabled = CallService(liveConfigObj);
                            webSite.LiveSettings = AndroAdminDataAccess.Domain.WebOrderingSetup.WebSiteConfigurations.SerializeJson(liveConfigObj);
                        }
                    }
                    else
                    {
                        //errorsLiveEnabled.Add("Theme not found; Id - " + themeId);
                        // JSON to be updated in db; Theme not found error is handled by web-service 
                    }
                }
                errors.AddRange(errorsLiveDomainChanged);
                errors.AddRange(errorsPreviewDomainChanged);
                errors.AddRange(errorsLiveDisabled);
                errors.AddRange(errorsLiveEnabled);
                return errors;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                AndroAdmin.Helpers.ErrorHelper.LogError("AndroAdmin.Controllers.AndroWebOrderingController.GetWebSiteSettingsOnUpdate", ex);
                return errors;
            }
        }
    }
}
