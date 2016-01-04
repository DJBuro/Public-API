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
                        Global.Log.Error("DeployWebsite Error", ex);
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
                Global.Log.Error("No payload");
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

            // Add ranking html to index file.
            //if (success)
            //{
            //    success = WebSiteServices.GenerateIndexFileRanking(newDomainName, webSiteServicesData);
            //}
            if (success)
            {
                //BackgroundServices.QueueStaticContent(webSiteServicesData);
                //while (BackgroundServices.GenerateStaticContentQueue.TryPeek(out queueContent))
                //foreach (var queueContent in BackgroundServices.GenerateStaticContentQueue.Values)

                WebSiteServicesDataWrapper queueContent = null;
                if (BackgroundServices.GenerateStaticContentQueue.TryGetValue(webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName, out queueContent))
                {
                    queueContent.deQueueTime = System.DateTime.Now.AddMinutes(WebSiteServices.GetStaticContentDequeueDelayInMinutes());
                    queueContent.webSiteServicesData = webSiteServicesData;
                }
                else
                {
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

                Global.Log.Error((webSiteServicesData.WebSiteConfiguration.SiteDetails.FaviconPath));

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

        private static string GetWebsiteFolder(string domainName)
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
                    Global.Log.Error("RetryCount is 10 unable to create directory:" + destinationFolder);
                }
                else
                {
                    if (retryCount > 0)
                    {
                        Global.Log.Info("Folder created - RetryCount:" + retryCount);
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
                        Global.Log.Error("unable to copy file after 10 attempts:" + destinationFilename);
            }
                    else if (retryCount > 0)
                    {
                        Global.Log.Info("File copied after RetryCount:" + retryCount + " for the file:" + destinationFilename);
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

        #region Ranking

        private static StoreMenu GetSiteMenu(string domainName, WebSiteServicesData webSiteServicesData, string siteId)
        {
            StoreMenu storeMenu = new StoreMenu();
            MenuRoot siteMenu = null;
            try
            {
            if (webSiteServicesData.WebSiteConfiguration != null
                && !String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.ACSApplicationId)
                && !String.IsNullOrEmpty(siteId))
                //&& !String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.MasterStoreId))
            {
                Stream responseStream = null;
                //string menuJson = GetMenuJSON(webSiteServicesData.WebSiteConfiguration.MasterStoreId, out responseStream);
                string menuJson = GetMenuJSON(domainName, siteId, out responseStream);
                if (!string.IsNullOrEmpty(menuJson))
                {
                    siteMenu = JsonConvert.DeserializeObject<MenuRoot>(menuJson);
                        if (siteMenu != null)
                        {
                    storeMenu.MenuData = JsonConvert.DeserializeObject<RawMenu>(siteMenu.Menu.MenuData);
                    foreach (RawMenuItem item in storeMenu.MenuData.Items)
                    {
                        item.ItemName = storeMenu.MenuData.ItemNames[item.Name ?? 0];
                    }
                            if (siteMenu.Menu.MenuDataThumbnails != null)
                            {
                    storeMenu.MenuDataThumbnails = JsonConvert.DeserializeObject<MenuThumbnails>(siteMenu.Menu.MenuDataThumbnails);
                    foreach (MenuThumbnailItem thumbnail in storeMenu.MenuDataThumbnails.MenuItemThumbnails)
                    {
                        thumbnail.Src = storeMenu.MenuDataThumbnails.Server.Endpoint + thumbnail.Src;
                    }
                }
            }
                    }
                }
            else
            {
                // Error
            }
            }
            catch(Exception ex)
            {
                storeMenu = null;
                Global.Log.Error("WebSiteServices.GetSiteMenu: Unhandled exception", ex);
            }
            return storeMenu;
        }

        private static string GetMenuJSON(string domainName, string siteId, out Stream response)
        {
            string responseJson = "";
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(domainName);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the menu from ACS
                    bool success = Andromeda.WebOrdering.Services.MenuServices.GetSiteMenuWithImages(domainConfiguration, siteId, out httpStatus, out responseJson);
                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
            }
            finally
            {
                // Do we need to log the call data?
                bool log = true;
                if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                {
                    Global.Log.Error("LogMenuCalls setting missing or invalid");
                    log = true;
                }

                Global.Log.Debug(
                    "GET Menu/" + siteId +
                    " SourceIP:" + 
                    " Status: " + httpStatus.ToString() +
                    " Response:" + (log ? responseJson : "Logging disabled"));
            }
            return responseJson;
        }

        private static string GenerateRankingString(StoreMenu storeMenu)
        {
            StringBuilder retString = new StringBuilder();

            var menuItemsForDisplay = from item in storeMenu.MenuData.Items
                                      group item by item.Name
                                          into groups
                                          select groups.OrderByDescending(p => p.DelPrice).First();
            foreach (RawMenuItem item in menuItemsForDisplay)
            {
                string imagePath = string.Empty;
                List<MenuThumbnailItem> matchImages = storeMenu.MenuDataThumbnails.MenuItemThumbnails.Where(x => x.ItemIds.Contains(item.Name ?? 0)).ToList();
                if (matchImages != null && matchImages.Count > 0)
                {
                    matchImages = matchImages.OrderBy(x => x.Src).ToList();
                    imagePath = matchImages[0].Src;
                }
                retString.Append(GetMenuTemplate()
                    .Replace("{item-name}", item.ItemName)
                    .Replace("{item-img}", imagePath)
                    .Replace("{item-price}", ((double)((item.DelPrice ?? 0) / (100))).ToString())
                    .Replace("{item-desc}", item.Desc)
                    );
            }
            return @"<div style=""display: none"">" + retString.ToString() + "</div>";
        }

        private static string GetMenuTemplate()
        {
            string template = @"
        <div itemscope itemtype=""http://schema.org/Product"" class=""itemRow"" >
            <div itemprop=""name"" class=""itemName"" data-bind=""html: name"">{item-name}</div>
            <div class=""itemImage"" style=""height: 130px, width: 130px"">
                <img width=""130px"" height=""130px"" 
                    src=""{item-img}"" 
                    alt=""{item-name}""/>
            </div>
            <div class=""itemDetailsWithImage"">
                <div class=""priceCategoryContainer"">                    
                    
                    <div class=""priceAddContainer"">
                        <div class=""itemPrice"">
                            <span itemprop=""priceCurrency"" content=""GBP"">£</span>
                            <span itemprop=""price"" content=""{item-price}"">{item-price}</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class=""itemDescription""><span itemprop=""description"">{item-desc}</span></div>";
            return template;
        }

        private static bool GenerateIndexFileRanking(string domainName, WebSiteServicesData webSiteServicesData)
        {
            try
            {
                string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
                string indexFilename = Path.Combine(newWebsiteFolder, "index.html");
                string html = System.IO.File.ReadAllText(indexFilename);

                // Inject Ranking Text
                if (html.Contains("<!-- RANKING CONTENT -->"))
                {
                    html = html.Replace("<!-- RANKING CONTENT -->", GenerateRankingString(GetSiteMenu(domainName, webSiteServicesData, null)));
                    System.IO.File.WriteAllText(indexFilename, html);
                }
            }
            catch (Exception ex)
            { 
                Global.Log.Debug("Error: GenerateIndexFileRanking", ex);
            }
            return true;
        }

        #endregion

        #region Snapshot - Phantom

        public static void GenerateStoreDirectory(WebSiteServicesData webSiteServicesData)
        {
            string domainName = webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName;
            string webSiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
            Stream responseStream = null;
            string siteListJSON = WebSiteServices.GetSiteJSON(domainName, out responseStream);
            Site[] siteList;
            List<string> phantomURLs = new List<string>();
            phantomURLs.Add(string.Format("http://{0}/Index.html", domainName));

            if (!string.IsNullOrEmpty(siteListJSON))
            {
                siteList = JsonConvert.DeserializeObject<Site[]>(siteListJSON);
                if (siteList != null && siteList.Count() > 0)
                {
                    string siteDirectory = string.Empty;
                    foreach (Site site in siteList)
                    {
                        // create new folders with store-name
                        siteDirectory = webSiteFolder + @"\" + site.name;
                        WebSiteServices.CleanAndCreateDirectory(siteDirectory);

                        // get snapshots into store folder.
                        WebSiteServices.GetSnapshot(domainName, site.name, siteDirectory, "home");
                        phantomURLs.Add(WebSiteServices.GetSitemapPhantomURL(domainName, site.name, "home/"));

                        WebSiteServices.GetSnapshot(domainName, site.name, siteDirectory, "storedetails");
                        phantomURLs.Add(WebSiteServices.GetSitemapPhantomURL(domainName, site.name, "storedetails/"));

                        StoreMenu storeMenu = WebSiteServices.GetSiteMenu(domainName, webSiteServicesData, site.siteId);
                        if (storeMenu != null && storeMenu.MenuData != null && storeMenu.MenuData.Display != null && storeMenu.MenuData.Display.Count > 0)
                        {
                            WebSiteServices.CleanAndCreateDirectory(siteDirectory + "\\" + "menu");
                            foreach (RawCategory category in storeMenu.MenuData.Display)
                            {
                                if (category.Name.ToString().ToLower().Equals("dealsonly"))
                                    continue;

                                WebSiteServices.GetSnapshot(domainName, site.name, siteDirectory, "menu/" + category.Name);
                                phantomURLs.Add(WebSiteServices.GetSitemapPhantomURL(domainName, site.name, "menu/" + category.Name + "/"));
                            }

                            if (storeMenu.MenuData.Deals != null && storeMenu.MenuData.Deals.Count > 0)
                            {
                                WebSiteServices.CleanAndCreateDirectory(siteDirectory + "\\" + "menu\\deals");
                                
                                WebSiteServices.GetSnapshot(domainName, site.name, siteDirectory, "menu/" + "deals");
                                phantomURLs.Add(WebSiteServices.GetSitemapPhantomURL(domainName, site.name, "menu/" + "deals/"));
                            }
                        }
                    }
                }
            }
            // update site-map file.
            UpdateSiteMapFile(domainName, phantomURLs);
  //          WebSiteServices.GetSnapshotForRootIndex("Index.html", domainName);
        }

        private static bool UpdateSiteMapFile(string domainName, List<string> phantomURLs)
        {
            bool success = true;

            string sitemapFilename = Path.Combine(WebSiteServices.GetWebsiteFolder(domainName), "sitemap.xml");
            string sitemap = System.IO.File.ReadAllText(sitemapFilename);

            StringBuilder siteMapUrls = new StringBuilder();
            foreach (string phantomURL in phantomURLs)
            {
                siteMapUrls.AppendLine(WebSiteServices.GetSiteMapURL(phantomURL));
            }
            // Write out the sitemap file
            string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
            string siteMapFileString = string.Format(@"<?xml version=""1.0"" encoding=""UTF-8""?><urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">{0}</urlset>", siteMapUrls.ToString());
            System.IO.File.WriteAllText(sitemapFilename, siteMapFileString);

            return success;
        }

        private static string GetSiteMapURL(string url)
        {
            return string.Format(@" 
            <url>
                 <loc>{0}</loc>
                 <lastmod>{1}</lastmod>
                 <changefreq>weekly</changefreq>
                 <priority>0.8</priority>
            </url>", url, DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));
        }

        private static void CleanAndCreateDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                int retryCount = 0;
                while (retryCount < 10)
                {
                    try
                    {
                        Directory.Delete(directoryPath, true);
                        break;
                    }
                    catch { }

                    // Error - probably file locked - wait and try again
                    Thread.Sleep(25);
                    retryCount++;
                }
            }
            Directory.CreateDirectory(directoryPath);
        }

        private static string GetSiteJSON(string domainName, out Stream response)
        {
            string responseJson = "";
            HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;

            try
            {
                // Get the application id to use from the host header
                DomainConfiguration domainConfiguration = Helper.GetDomainConfiguration(domainName);

                if (domainConfiguration == null)
                {
                    response = Helper.InternalErrorNoLog(null);
                }
                else
                {
                    // Get the menu from ACS
                    bool success = Andromeda.WebOrdering.Services.SiteListServices.GetSiteList(null, null, domainConfiguration, out httpStatus, out responseJson);
                    response = Helper.StringToStream(responseJson);
                }
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
            }
            finally
            {
                // Do we need to log the call data?
                bool log = true;
                if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                {
                    Global.Log.Error("LogMenuCalls setting missing or invalid");
                    log = true;
                }

                Global.Log.Debug(
                    "GET SiteList" + 
                    " SourceIP:" +
                    " Status: " + httpStatus.ToString() +
                    " Response:" + (log ? responseJson : "Logging disabled"));
            }
            return responseJson;
        }

        private static void GetSnapshot(string domainName, string storeName, string siteDirectory, string urlPart)
        {
            //var stopwatch = Stopwatch.StartNew();
            string storeFolder = WebSiteServices.GetWebsiteFolder(siteDirectory);
            string url = WebSiteServices.GetPhantomURL(domainName, storeName, urlPart);
            string snapShotPath = storeFolder + "\\" + urlPart;
            WebSiteServices.CleanAndCreateDirectory(snapShotPath);
            File.WriteAllText(snapShotPath + "\\" + "index.js", GrabberJS);

            string args = string.Format("\"{0}\\{1}\" {2}", snapShotPath, "index.js", url);
            string phantomAssemblyPath = System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", string.Empty) + @"\packages\PhantomJS.1.9.8\tools\phantomjs\phantomjs.exe";
            //"@"C:\bin\PhantomJS\phantomjs.exe"
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = phantomAssemblyPath, //@"C:\bin\PhantomJS\phantomjs.exe"
                Arguments = args
            };
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            //long msec = stopwatch.ElapsedMilliseconds;
            File.WriteAllText(snapShotPath + "\\" + "Index.html", output);
        }

        private static void GetSnapshotForRootIndex(string urlpart, string domainName)
        {
            string unsafeText1 = @"Unsafe JavaScript attempt to access frame with URL about:blank from frame with URL file:///C:/inetpub/wwwroot/WebOrdering/Chains/" + HttpUtility.UrlEncode(domainName) + "/index.js. Domains, protocols and ports must match.";
            string unsafeText2 = @"Unsafe JavaScript attempt to access frame with URL about:blank from frame with URL file:///C:/inetpub/wwwroot/sites/" + HttpUtility.UrlEncode(domainName) + "/index.js. Domains, protocols and ports must match.";
            string rootFolder = WebSiteServices.GetWebsiteFolder(domainName);
            File.WriteAllText(rootFolder + "\\" + "index.js", GrabberJS);

            string args = string.Format("\"{0}\\{1}\" {2}", rootFolder, "index.js", string.Format("http://{0}/{1}", domainName, urlpart));
            string phantomAssemblyPath = System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", string.Empty) + @"\packages\PhantomJS.1.9.8\tools\phantomjs\phantomjs.exe";
            //"@"C:\bin\PhantomJS\phantomjs.exe"
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = phantomAssemblyPath, //@"C:\bin\PhantomJS\phantomjs.exe"
                Arguments = args
            };
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            output = output.Replace(unsafeText1, string.Empty).Replace(unsafeText2, string.Empty);

            if (!string.IsNullOrEmpty(output) && output != "--TIMEOUT--")
            {
                try
                {
                    if (File.Exists(rootFolder + "\\" + "Index_Renamed.html"))
                    {
                        File.Delete(rootFolder + "\\" + "Index_Renamed.html");
                    }
                    File.Move(rootFolder + "\\" + "Index.html", rootFolder + "\\" + "Index_Renamed.html");
                    File.WriteAllText(rootFolder + "\\" + "Index.html", output);
                }
                catch (Exception exception)
                {
                    Global.Log.Error("File Error - ROOT Index.html generation", exception);
                }
            }
            else
            {
                Global.Log.Error("TIME OUT in grabbing root index.html file");
            }
        }

        private static string GetPhantomURL(string domainName, string storeName, string urlPart)
        {
            return string.Format(@"http://{0}?page={1}/{2}", domainName, storeName, urlPart);
        }

        private static string GetSitemapPhantomURL(string domainName, string storeName, string urlPart)
        {
            return string.Format(@"http://{0}/{1}/{2}", domainName, storeName, urlPart);
        }

        private static string GrabberJS
        {
//            get
//            {
//                return @"var page = require('webpage').create(),system = require('system');
//                 page.onLoadFinished = function() {
//                  console.log(page.content);
//                    phantom.exit();
////                };
//                page.open(system.args[1]);";
//            }
            get
            {
                return @"var page = require('webpage').create(),system = require('system');

                    function checkIfFinished()
                    {
                        try
                        {
                            //console.log("" in checkIfFinished"");
                            var finished = page.evaluate
                            (
                                function() 
                                {
                                return window.isFinished;            
                                }
                            );

                            if(finished == undefined || finished)
                            {
                    console.log(page.content);
                    phantom.exit();
                    }
                    else
                    {
                                    //console.log(""checkIfFinished calling setTimeout"");
                                    window.setTimeout(checkIfFinished,100);
                            }
                        }
                        catch(e)
                        {
                            //console.log(""Unhandled exception in checkIfFinished: "" + e.message);
                            phantom.exit();
                    }
                    }
                    
                    page.onLoadFinished = function() 
                    {
                            //console.log(""checkIfFinished calling setTimeout in LoadFinished"");
                            checkIfFinished();
                            window.setTimeout
                            (
                                function()
                                {
                                    console.log(""--TIMEOUT--"");
                                    phantom.exit();
                                }
                                ,10000
                             );
                    };

                    page.open(system.args[1]);";
            }
        }
        #endregion
    }
}