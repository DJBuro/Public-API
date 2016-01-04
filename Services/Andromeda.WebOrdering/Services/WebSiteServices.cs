using Andromeda.WebOrdering.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace Andromeda.WebOrdering.Services
{
    public class WebSiteServicesData
    {
        public string Key { get; set; }
        public string RequestJson { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ResultJson { get; set; }
        public WebSiteConfiguration WebSiteConfiguration { get; set; }

        public WebSiteServicesData()
        {
            this.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            this.ResultJson = "";
        }
    }

    public class WebSiteServicesDataWrapper
    {
        public WebSiteServicesData webSiteServicesData;
        public DateTime deQueueTime;
        public string domainName;
    }

    public class WebSiteServices
    {
        public static Dictionary<string, object> publishingCurrently = new Dictionary<string, object>();
        public static bool AddUpdateWebSite(WebSiteServicesData webSiteServicesData, out string errorCode)
        {
            bool success = true;
            errorCode = string.Empty;
            // Make sure the caller provided the super secret key
            if (!WebSiteServices.CheckKey(webSiteServicesData)) success = false;

            // Extract the config data from the payload
            if (success)
            {
                success = WebSiteServices.ExtractJson(webSiteServicesData);
            }

            if (success)
            {
                object currentProcessingDomain = null;
                lock (publishingCurrently)
                {
                    if (publishingCurrently.TryGetValue(webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName, out currentProcessingDomain))
                    {
                        errorCode = ((int)HttpStatusCode.Conflict).ToString(); // Another publish is in progress
                        success = false;
                    }
                    else
                    {
                        currentProcessingDomain = new object();
                        publishingCurrently.Add(webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName, currentProcessingDomain);
                    }
                }

                // Deploy the website
                if (success)
                {
                    try
                    {
                        success = WebSiteServices.DeployWebsite
                        (
                            webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName,
                            webSiteServicesData.WebSiteConfiguration.SiteDetails.OldDomainName,
                            webSiteServicesData
                        );
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        Logger.Log.Error("DeployWebsite Error", ex);
                    }
                    finally
                    {
                        WebSiteServices.publishingCurrently.Remove(webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName);
                    }
                }
            }
            if (success)
            {
                webSiteServicesData.HttpStatusCode = HttpStatusCode.Created;
            }
            return success;
        }

        private static bool CheckKey(WebSiteServicesData webSiteServicesDat)
        {
            bool isKeyOk = (webSiteServicesDat.Key == ConfigurationManager.AppSettings["KEY"]);

            // Did the caller pass in the correct key?
            if (!isKeyOk)
            {
                // No access for you!
                webSiteServicesDat.HttpStatusCode = HttpStatusCode.Forbidden;
            }

            return isKeyOk;
        }

        private static bool ExtractJson(WebSiteServicesData webSiteServicesData)
        {
            bool success = true;

            // Is there a payload to process
            if (webSiteServicesData.RequestJson == null || webSiteServicesData.RequestJson.Length == 0)
            {
                // No payload
                webSiteServicesData.HttpStatusCode = HttpStatusCode.BadRequest;
                webSiteServicesData.ResultJson = "No payload";
                Logger.Log.Error("No payload");
                success = false;
            }

            // Payload should be the configuration JSON.  Deserialize it into an object
            WebSiteConfiguration webSiteConfiguration = null;
            if (success)
            {
                webSiteConfiguration = JsonConvert.DeserializeObject<WebSiteConfiguration>(webSiteServicesData.RequestJson);

                // Did we find any configuration data in the payload?
                if (webSiteConfiguration == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Payload does not contain any settings data");
                }
                else if (webSiteConfiguration.ACSApplicationId == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing ACSApplicationId");
                }
                // Site details
                else if (webSiteConfiguration.SiteDetails == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing SiteDetails section");
                }
                else if (String.IsNullOrEmpty(webSiteConfiguration.SiteDetails.DomainName))
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing SiteDetails.DomainName");
                }
                // Theme settings
                else if (webSiteConfiguration.ThemeSettings == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing ThemeSettings section");
                }
                else if (webSiteConfiguration.ThemeSettings.Id == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing ThemeSettings.Id");
                }   
                // General settings
                else if (webSiteConfiguration.GeneralSettings == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing GeneralSettings");
                }
                else if (webSiteConfiguration.GeneralSettings.MinimumDeliveryAmount == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing GeneralSettings.MinimumDeliveryAmount");
                }
                // Social network settings
                else if (webSiteConfiguration.SocialNetworkSettings == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing SocialNetworkSettings");
                }
                // Home
                else if (webSiteConfiguration.Home == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing Home");
                }
                else if (webSiteConfiguration.Home.Welcome == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing Home.Welcome");
                }
                else if (webSiteConfiguration.Home.Menu == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing Home.Menu");
                }
                else if (webSiteConfiguration.Home.Carousels == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing Home.Carousels");
                }
                // LegalNotices
                else if (webSiteConfiguration.LegalNotices == null)
                {
                    success = WebSiteServices.GenerateMissingDataError(webSiteServicesData, "Settings data is missing LegalNotices");
                }
                else
                {
                    // Got enough configuration data - all good
                    webSiteServicesData.WebSiteConfiguration = webSiteConfiguration;
                }
            }

            return success;
        }

        private static bool GenerateMissingDataError(WebSiteServicesData webSiteServicesData, string message)
        {
            webSiteServicesData.ResultJson = message;
            webSiteServicesData.HttpStatusCode = HttpStatusCode.BadRequest;

            return false; // Somthing bad happened
        }

        private static bool DeployWebsite(string newDomainName, string oldDomainName, WebSiteServicesData webSiteServicesData)
        {
            bool success = true;

            // Do we need to remove the old website?
            string themeFolder = WebSiteServices.GetThemeFolder(webSiteServicesData);

            if (!Directory.Exists(themeFolder))
            {
                success = false;
                webSiteServicesData.ResultJson = "Theme not found: " + webSiteServicesData.WebSiteConfiguration.ThemeSettings.Id.ToString();
                webSiteServicesData.HttpStatusCode = HttpStatusCode.BadRequest;
            }

            if (success)
            {
                success = WebSiteServices.CleanupDomainFolderAndMapping(newDomainName, oldDomainName);
            }

            // Deploy the website files using the theme template specified in the configuration
            if (success)
            {
                success = WebSiteServices.DeployWebsiteFiles(newDomainName, webSiteServicesData);
            }

            // generate custom.js based on the JSON settings
            if (success)
            {
                success = WebSiteServices.GenerateCustomSettingsFile(newDomainName, webSiteServicesData);
            }

            // generate custom.css based on the JSON settings
            if (success)
            {
                success = WebSiteServices.GenerateCustomCSSFile(newDomainName, webSiteServicesData);
            }

            // create / update mappings.xml file for the domain.
            if (success)
            {
                success = WebSiteServices.GenerateMapping(newDomainName, webSiteServicesData);
            }

            if (success)
            {
                WebSiteServicesDataWrapper queueContent = null;

                // Is the website already queued for static file genreation?
                if (BackgroundServices.GenerateStaticContentQueue.TryGetValue(webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName, out queueContent))
                {
                    // Push back the time when we will generate the static files
                    queueContent.deQueueTime = System.DateTime.Now.AddMinutes(WebSiteServices.GetStaticContentDequeueDelayInMinutes());
                    queueContent.webSiteServicesData = webSiteServicesData;
                }
                else
                {
                    // Add the website to the queue for static file generation
                    queueContent = new WebSiteServicesDataWrapper();
                    queueContent.webSiteServicesData = webSiteServicesData;
                    queueContent.deQueueTime = System.DateTime.Now.AddMinutes(WebSiteServices.GetStaticContentDequeueDelayInMinutes());
                    queueContent.domainName = newDomainName;
                
                    BackgroundServices.QueueStaticContent(queueContent);
                }
            }

            return success;
        }

        private static bool CleanupDomainFolderAndMapping(string newDomainName, string oldDomainName)
        {
            if (!String.IsNullOrEmpty(oldDomainName))
            {
                string oldWebsiteFolder = WebSiteServices.GetWebsiteFolder(oldDomainName);

                // Remove the old website
                if (Directory.Exists(oldWebsiteFolder))
                {
                    int retryCount = 0;
                    while (retryCount < 10)
                    {
                        try
                        {
                            Directory.Delete(oldWebsiteFolder, true);
                            break;
                        }
                        catch { }

                        // Error - probably file locked - wait and try again
                        Thread.Sleep(25);

                        retryCount++;
                    }
                }

                // Remove the old mapping file
                string oldMappingFilename = WebSiteServices.GetMappingFilename(oldDomainName);
                if (File.Exists(oldMappingFilename))
                {
                    int retryCount = 0;
                    while (retryCount < 10)
                    {
                        try
                        {
                            File.Delete(oldMappingFilename);
                            break;
                        }
                        catch { }

                        // Error - probably file locked - wait and try again
                        Thread.Sleep(25);

                        retryCount++;
                    }
                }

                // Unload any mapping for the old domain from memory
                lock (Helper.DomainConfiguration)
                {
                    if (Helper.DomainConfiguration.ContainsKey(oldDomainName.ToUpper()))
                    {
                        Helper.DomainConfiguration.Remove(oldDomainName.ToUpper());
                    }
                }
            }

            // Make sure that the new website folder does not exist
            string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(newDomainName);
            if (Directory.Exists(newWebsiteFolder))
            {
                int retryCount = 0;
                while (retryCount < 10)
                {
                    try
                    {
                        Directory.Delete(newWebsiteFolder, true);
                        break;
                    }
                    catch { }

                    // Error - probably file locked - wait and try again
                    Thread.Sleep(25);

                    retryCount++;
                }
            }

            return true;
        }

        private static bool DeployWebsiteFiles(string domainName, WebSiteServicesData webSiteServicesData)
        {
            bool success = true;

            string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);

            // Create a folder for the domain
            Directory.CreateDirectory(newWebsiteFolder);

            // Figure out where to get the theme files from
            string themeFolder = WebSiteServices.GetThemeFolder(webSiteServicesData);

            if (!Directory.Exists(themeFolder))
            {
                success = false;
                webSiteServicesData.ResultJson = "Theme not found: " + webSiteServicesData.WebSiteConfiguration.ThemeSettings.Id.ToString();
                webSiteServicesData.HttpStatusCode = HttpStatusCode.BadRequest;
            }

            if (success)
            {
                // Copy over the template website files
                WebSiteServices.RecursiveCopy(themeFolder, newWebsiteFolder);

                WebSiteServices.GenerateIndexFile(domainName, webSiteServicesData);

                WebSiteServices.GenerateSiteMapFile(domainName, webSiteServicesData);

                WebSiteServices.GenerateRobotsFile(domainName, webSiteServicesData);
            }

            return success;
        }

        private static bool GenerateIndexFile(string domainName, WebSiteServicesData webSiteServicesData)
        {
            if (webSiteServicesData.WebSiteConfiguration.Home != null)
            {
                string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
                string indexFilename = Path.Combine(newWebsiteFolder, "index.html");
                string html = System.IO.File.ReadAllText(indexFilename);

                Logger.Log.Error((webSiteServicesData.WebSiteConfiguration.SiteDetails.FaviconPath));

                // Favicon
                if (!String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.SiteDetails.FaviconPath))
                {
                    html = html.Replace("<!-- FAVICON -->", "<link rel=\"icon\" type=\"image/png\" href=\"" + webSiteServicesData.WebSiteConfiguration.SiteDetails.FaviconPath + "\" />");
                }
                else
                {
                    html = html.Replace("<!-- FAVICON -->", "");
                }

                // Inject page title
                if (webSiteServicesData.WebSiteConfiguration.SEOSettings != null &&
                    !String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.SEOSettings.Title))
                {
                    html = html.Replace("<!-- TITLE -->", HttpUtility.HtmlEncode(webSiteServicesData.WebSiteConfiguration.SEOSettings.Title));
                }
                else
                {
                    html = html.Replace("<!-- TITLE -->", HttpUtility.HtmlEncode(webSiteServicesData.WebSiteConfiguration.WebSiteName));
                }

                // Inject Google analytics id
                html = html.Replace("<<INJECTGOOGLEANALYTICSIDHERE>>", webSiteServicesData.WebSiteConfiguration.AnalyticsSettings.GoogleAnalyticsId);

                // Inject the Trip Advisor script
                if (webSiteServicesData.WebSiteConfiguration.TripAdvisorSettings != null && webSiteServicesData.WebSiteConfiguration.TripAdvisorSettings.IsEnable.HasValue &&
                    webSiteServicesData.WebSiteConfiguration.TripAdvisorSettings.IsEnable.Value)
                {
                html = html.Replace("<!-- INJECTTRIPADVISORHERE -->", webSiteServicesData.WebSiteConfiguration.TripAdvisorSettings.Script);
                }
                else
                {
                    html = html.Replace("<!-- INJECTTRIPADVISORHERE -->", "");
                }

                // Inject meta tags
                WebSiteServices.GenerateIndexFileMetaTags(webSiteServicesData, ref html);

                // Inject JivoChat
                WebSiteServices.GenerateJivoChatJavascript(webSiteServicesData, ref html);

                // Inject static html
                WebSiteServices.GenerateIndexFileStaticHtml(webSiteServicesData, ref html);

                // Write out the modified index file to disk
                System.IO.File.WriteAllText(indexFilename, html);
            }

            return true;
        }
         
        private static bool GenerateIndexFileMetaTags(WebSiteServicesData webSiteServicesData, ref string html)
        {
            StringBuilder metaTags = new StringBuilder();

            if (webSiteServicesData.WebSiteConfiguration.SEOSettings != null)
            {   
                // Keywords meta tag
                WebSiteServices.GenerateMetaTag(metaTags, "keywords", webSiteServicesData.WebSiteConfiguration.SEOSettings.Keywords, MetaTypeEnum.Name);

                // Description meta tag
                WebSiteServices.GenerateMetaTag(metaTags, "description", webSiteServicesData.WebSiteConfiguration.SEOSettings.Description, MetaTypeEnum.Name);
            }

            // Facebook crawler meta tags
            if (webSiteServicesData.WebSiteConfiguration.FacebookCrawlerSettings != null)
            {
                WebSiteServices.GenerateMetaTag(metaTags, "og:type", "website", MetaTypeEnum.Property);
                WebSiteServices.GenerateMetaTag(metaTags, "og:locale", "en_GB", MetaTypeEnum.Property);
                WebSiteServices.GenerateMetaTag(metaTags, "og:title", webSiteServicesData.WebSiteConfiguration.FacebookCrawlerSettings.Title, MetaTypeEnum.Property);
                WebSiteServices.GenerateMetaTag(metaTags, "og:site_name", webSiteServicesData.WebSiteConfiguration.FacebookCrawlerSettings.SiteName, MetaTypeEnum.Property);
                WebSiteServices.GenerateMetaTag(metaTags, "og:url", "http://" + webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName, MetaTypeEnum.Property);
                WebSiteServices.GenerateMetaTag(metaTags, "og:description", webSiteServicesData.WebSiteConfiguration.FacebookCrawlerSettings.Description, MetaTypeEnum.Property);
                WebSiteServices.GenerateMetaTag(metaTags, "og:image", webSiteServicesData.WebSiteConfiguration.FacebookCrawlerSettings.FacebookProfileLogoPath, MetaTypeEnum.Property);       
            }

            // Facebook meta tags
            if (webSiteServicesData.WebSiteConfiguration.CustomerAccountSettings != null)
            {
                WebSiteServices.GenerateMetaTag(metaTags, "fb:app_id", webSiteServicesData.WebSiteConfiguration.CustomerAccountSettings.FacebookAccountId, MetaTypeEnum.Property);
            }

            // Inject the meta tags into the index html
            html = html.Replace("<!-- METATAGS -->", metaTags.ToString());

            return true;
        }

        private static bool GenerateJivoChatJavascript(WebSiteServicesData webSiteServicesData, ref string html)
        {
            string jivoChatScript = "";

            if (webSiteServicesData.WebSiteConfiguration.JivoChatSettings != null &&
                webSiteServicesData.WebSiteConfiguration.JivoChatSettings.IsJivoChatEnabled.HasValue &&
                webSiteServicesData.WebSiteConfiguration.JivoChatSettings.IsJivoChatEnabled.Value &&
                webSiteServicesData.WebSiteConfiguration.JivoChatSettings.Script != null &&
                webSiteServicesData.WebSiteConfiguration.JivoChatSettings.Script.Length > 0)
            {
                jivoChatScript = webSiteServicesData.WebSiteConfiguration.JivoChatSettings.Script;
            }

            html = html.Replace("<!--INSERT_JIVOCHAT_HERE-->", jivoChatScript);

            return true;
        }

        private enum MetaTypeEnum { HttpEquiv, Name, Property }
        private static void GenerateMetaTag(StringBuilder metaTags, string name, string value, MetaTypeEnum metaType)
        {
            if (!String.IsNullOrEmpty(value))
            {
                metaTags.Append("    <meta ");
                metaTags.Append(metaType == MetaTypeEnum.Property ? "property" : metaType == MetaTypeEnum.HttpEquiv ? "http-equiv" : metaType == MetaTypeEnum.Name ? "name" : "");
                metaTags.Append("=\"");
                metaTags.Append(name);
                metaTags.Append("\" content=\"");
                metaTags.Append(HttpUtility.HtmlEncode(value));
                metaTags.Append("\" />\r\n");
            }
        }

        private static bool GenerateIndexFileStaticHtml(WebSiteServicesData webSiteServicesData, ref string html)
        {
            if (webSiteServicesData.WebSiteConfiguration.Home.Welcome != null)
            {
                if (webSiteServicesData.WebSiteConfiguration.Home.Welcome.TitleText != null)
                {
                    html = html.Replace("<!-- HOMESTOREDETAILSTITLE -->", webSiteServicesData.WebSiteConfiguration.Home.Welcome.TitleText);
                }
                if (webSiteServicesData.WebSiteConfiguration.Home.Welcome.DescriptionText != null)
                {
                    html = html.Replace("<!-- HOMESTOREDETAILS -->", webSiteServicesData.WebSiteConfiguration.Home.Welcome.DescriptionText);
                }
            }

            if (webSiteServicesData.WebSiteConfiguration.Home.Menu != null)
            {
                if (webSiteServicesData.WebSiteConfiguration.Home.Menu.TitleText != null)
                {
                    html = html.Replace("<!-- HOMEMENUTITLE -->", webSiteServicesData.WebSiteConfiguration.Home.Menu.TitleText);
                }
                if (webSiteServicesData.WebSiteConfiguration.Home.Menu.DescriptionText != null)
                {
                    html = html.Replace("<!-- HOMEMENU -->", webSiteServicesData.WebSiteConfiguration.Home.Menu.DescriptionText);
                }
            }

            return true;
        }

        private static bool GenerateSiteMapFile(string domainName, WebSiteServicesData webSiteServicesData)
        {
            bool success = true;

            string templateSitemapFilename = WebSiteServices.GetTemplateFilename("sitemap.xml");
            string sitemap = System.IO.File.ReadAllText(templateSitemapFilename);

            sitemap = sitemap.Replace("<!-- DOMAIN -->", domainName);
            sitemap = sitemap.Replace("<!-- LASTMOD -->", DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));


            // Write out the sitemap file
            string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
            string sitemapFilename = Path.Combine(newWebsiteFolder, "sitemap.xml");
            System.IO.File.WriteAllText(sitemapFilename, sitemap);

            return success;
        }

        private static bool GenerateRobotsFile(string domainName, WebSiteServicesData webSiteServicesData)
        {
            bool success = true;

            string templateSitemapFilename = WebSiteServices.GetTemplateFilename("robots.txt");
            string sitemap = System.IO.File.ReadAllText(templateSitemapFilename);

            sitemap = sitemap.Replace("<<DOMAIN>>", domainName);

            // Write out the sitemap file
            string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
            string sitemapFilename = Path.Combine(newWebsiteFolder, "robots.txt");
            System.IO.File.WriteAllText(sitemapFilename, sitemap);

            return success;
        }

        private static bool GenerateCustomSettingsFile(string domainName, WebSiteServicesData webSiteServicesData)
        {
            bool success = true;

            string templateCustomJSFilename = WebSiteServices.GetTemplateFilename("Custom.js");
            string customJSTemplate = System.IO.File.ReadAllText(templateCustomJSFilename);

            if (webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings == null)
            {
                webSiteServicesData.WebSiteConfiguration.SocialNetworkSettings = new SocialNetworkSettings();
            }

            // Extract javascript settings
            CustomSettings customSettings = new CustomSettings();
            string customJS = customSettings.GenerateCustomJS(webSiteServicesData, customJSTemplate);        

            string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
            string customFolder = Path.Combine(newWebsiteFolder, "Custom");
            string customJsFilename = Path.Combine(customFolder, "Custom.js");

            // Make sure the custom folder exists
            if (!Directory.Exists(customFolder))
            {
                Directory.CreateDirectory(customFolder);
            }

            // Write out the custom JS file
            System.IO.File.WriteAllText(customJsFilename, customJS);

            return success;
        }

        private static bool GenerateCustomCSSFile(string domainName, WebSiteServicesData webSiteServicesData)
        {
            bool success = true;

            string templateCustomCSSFilename = WebSiteServices.GetTemplateFilename("Custom.css");
            string customCSS = System.IO.File.ReadAllText(templateCustomCSSFilename);

            /* Button */
            customCSS = customCSS.Replace("<<BUTTONCOLOR>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.ColourRange1, " "));
            customCSS = customCSS.Replace("<<BUTTONTEXTCOLOR>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.ColourRange2, " "));

            /* Selected button */
            customCSS = customCSS.Replace("<<SELECTEDBUTTONCOLOR>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.ColourRange3, " "));
            customCSS = customCSS.Replace("<<SELECTEDBUTTONTEXTCOLOR>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.ColourRange4, " "));

            /* Main menu */
            customCSS = customCSS.Replace("<<MAINMENUBARCOLOUR>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.ColourRange5, " "));
            customCSS = customCSS.Replace("<<MAINMENUTEXTCOLOUR>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.ColourRange6, " "));

            /* Background image PC/Tablet */
            if (String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.DesktopBackgroundImagePath))
            {
                // Remove the css that shows the background image
                customCSS = WebSiteServices.RemoveBlock(customCSS, "/* STARTBACKGROUNDIMAGEBLOCK */", "/* ENDBACKGROUNDIMAGEBLOCK */");
            }
            else
            {
                // Remove the css that assumes no background image
                customCSS = WebSiteServices.RemoveBlock(customCSS, "/* STARTNOBACKGROUNDIMAGEBLOCK */", "/* ENDNOBACKGROUNDIMAGEBLOCK */");
                
                // Set the background image
                customCSS = customCSS.Replace("<<BACKGROUNDIMAGEURL>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.DesktopBackgroundImagePath, " "));
            }

            /* Background image Mobile */
            if (String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.MobileBackgroundImagePath))
            {
                
                customCSS = WebSiteServices.RemoveBlock(customCSS, "/* STARTMOBILEBACKGROUNDIMAGEBLOCK */", "/* ENDMOBILEBACKGROUNDIMAGEBLOCK */");
            }
            else
            {
                customCSS = customCSS.Replace("<<MOBILEBACKGROUNDIMAGEURL>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.MobileBackgroundImagePath, " "));
            }

       //     customCSS = customCSS.Replace("<<MAINMENUSELECTEDTEXTCOLOUR>>", CustomSettings.NullToText(webSiteServicesData.WebSiteConfiguration.CustomThemeSettings.ColourRange6, " "));

            string additionalCSS = "";

            // Do we need to hide the terms of use button?
            bool hasTermsOfUse = true;
            if (String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.LegalNotices.TermsOfUse))
            {
                // Hide the terms of use button
                hasTermsOfUse = false;
                additionalCSS += "#termsOfUse { display: none; }\r\n";
            }

            // Do we need to hide the privacy policy button?
            bool hasPrivacyPolicy = true;
            if (String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.LegalNotices.PrivacyPolicy))
            {
                // Hide the privacy policy button
                hasPrivacyPolicy = false;
                if (!hasTermsOfUse)
                {
                    additionalCSS += "#footerSplitter1 { display: none; }\r\n";
                }
                additionalCSS += "#privacyPolicy  { display: none; }\r\n";
            }

            // Do we need to hide the cookies button?
            if (String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.LegalNotices.CookiePolicy))
            {
                // Hide the cookies button
                if (!hasTermsOfUse && !hasPrivacyPolicy)
                {
                    additionalCSS += "#footerSplitter2 { display: none; }\r\n";
                }
                additionalCSS += "#cookiePolicy  { display: none; }\r\n";
            }

            customCSS = customCSS.Replace("<<ADDITIONAL>>", additionalCSS);

            string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
            string customFolder = Path.Combine(newWebsiteFolder, "Custom");
            string customCSSFilename = Path.Combine(customFolder, "Custom.css");

            // Write out the custom CSS file
            System.IO.File.WriteAllText(customCSSFilename, customCSS);

            return success;
        }

        private static string RemoveBlock(string fromText, string startBlock, string endBlock)
        {
            string resultText = fromText;
            
            // Remove the background css
            int startIndex = fromText.IndexOf(startBlock);
            if (startIndex > -1)
            {
                int endIndex = fromText.IndexOf(endBlock);
                if (endIndex > -1)
                {
                    int cutStartIndex = startIndex + startBlock.Length;
                    int cutLength = endIndex - startIndex;
                    
                    resultText = fromText.Substring(0, startIndex);
                    resultText += fromText.Substring(cutStartIndex + cutLength);
                }
            }

            return resultText;
        }

        private static bool GenerateMapping(string domainName, WebSiteServicesData webSiteServicesData)
        {
            bool success = true;

            string templateMappingFilename = WebSiteServices.GetTemplateFilename("Mapping.xml");

            string mappingXML = System.IO.File.ReadAllText(templateMappingFilename);

            mappingXML = mappingXML.Replace("{DOMAINNAME}", webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName);
            mappingXML = mappingXML.Replace("{APPLICATIONID}", webSiteServicesData.WebSiteConfiguration.ACSApplicationId);

            string mappingFilename = WebSiteServices.GetMappingFilename(domainName);

            // Write out the mapping XML file
            System.IO.File.WriteAllText(mappingFilename, mappingXML);

            // Load the mapping file into memory
            lock (Helper.DomainConfiguration)
            {
                Global.LoadMappingFile(mappingFilename);
            }

            return success;
        }

        private static int GetStaticContentDequeueDelayInMinutes()
        {
            try
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["StaticContentDequeueDelayInMinutes"]);
            }
            catch
            {
                return 60;
            }
        }

        private static string GetWebsitesFolder()
        {
            return ConfigurationManager.AppSettings["websitesFolder"];
        }

        public static string GetMappingFolder()
        {
            return ConfigurationManager.AppSettings["mappingsFolder"];
        }

        private static string GetThemesFolder()
        {
            return ConfigurationManager.AppSettings["themesFolder"];
        }

        private static string GetTemplatesFolder()
        {
            return ConfigurationManager.AppSettings["templatesFolder"];
        }

        public static string GetWebsiteFolder(string domainName)
        {
            string baseWebsitesFolder = WebSiteServices.GetWebsitesFolder();
            return Path.Combine(baseWebsitesFolder, domainName);
        }

        private static string GetMappingFilename(string domainName)
        {
            string baseMappingFolder = WebSiteServices.GetMappingFolder();
            return Path.Combine(baseMappingFolder, domainName + ".mapping.xml");
        }

        private static string GetThemeFolder(WebSiteServicesData webSiteServicesData)
        {
            string baseThemesFolder = WebSiteServices.GetThemesFolder();
            return Path.Combine(baseThemesFolder, webSiteServicesData.WebSiteConfiguration.ThemeSettings.Id.ToString());
        }

        private static string GetTemplateFilename(string templateName)
        {
            string baseTemplatesFolder = WebSiteServices.GetTemplatesFolder();
            return Path.Combine(baseTemplatesFolder, templateName);
        }

        private static void RecursiveCopy(string sourceFolder, string destinationFolder)
        {
            foreach (string sourceFilenameWithPath in Directory.GetFiles(sourceFolder))
            {
                string sourceFilenameWithoutPath = System.IO.Path.GetFileName(sourceFilenameWithPath);
                string destinationFilename = System.IO.Path.Combine(destinationFolder, sourceFilenameWithoutPath);

                int retryCount = 0;

                while (retryCount < 10)
                {
                    try
                    {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }
                        break;
                    }
                    catch { }

                    // Error - probably file locked - wait and try again
                    Thread.Sleep(25);

                    retryCount++;
                }
                if (retryCount == 10)
                {
                    Logger.Log.Error("RetryCount is 10 unable to create directory:" + destinationFolder);
                }
                else
                {
                    if (retryCount > 0)
                    {
                        Logger.Log.Info("Folder created - RetryCount:" + retryCount);
                    }

                    retryCount = 0;

                while (retryCount < 10)
                {
                    try
                    {
                        File.Copy(sourceFilenameWithPath, destinationFilename);
                        break;
                    }
                    catch { }

                    // Error - probably file locked - wait and try again
                    Thread.Sleep(25);

                    retryCount++;
                }
                    if (retryCount == 10)
                    {
                        Logger.Log.Error("unable to copy file after 10 attempts:" + destinationFilename);
            }
                    else if (retryCount > 0)
                    {
                        Logger.Log.Info("File copied after RetryCount:" + retryCount + " for the file:" + destinationFilename);
                    }
                }
            }

            foreach (string childFolder in Directory.GetDirectories(sourceFolder))
            {
                string childFolderWithoutPath = childFolder.Substring(childFolder.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);
                string childDestinationFolder = System.IO.Path.Combine(destinationFolder, childFolderWithoutPath);

                WebSiteServices.RecursiveCopy(childFolder, childDestinationFolder);
            }
        }
    }
}