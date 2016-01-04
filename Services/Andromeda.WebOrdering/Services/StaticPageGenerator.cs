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
    public class StaticPageGenerator : IStaticPageGenerator
    {
        private static string XmlEncode(string rawString)
        {
            string xmlEncoded = HttpUtility.UrlPathEncode(rawString);

            xmlEncoded = xmlEncoded.Replace("&", "&amp;");
            xmlEncoded = xmlEncoded.Replace("<", "&lt;");
            xmlEncoded = xmlEncoded.Replace(">", "&gt;");

            return xmlEncoded;
        }

        public Model.Site[] GetSiteList(string domainName)
        {
            Stream responseStream = null;
            string siteListJSON = StaticPageGenerator.GetSiteJSON(domainName, out responseStream);

            Site[] siteList = null;

            if (!string.IsNullOrEmpty(siteListJSON))
            {
                siteList = JsonConvert.DeserializeObject<Site[]>(siteListJSON);
            }

            return siteList;
        }

        public static void GenerateStoreSnapshot(WebSiteServicesData webSiteServicesData, IStaticPageGenerator staticPageGenerator)
        {
            string domainName = webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName;
            string webSiteFolder = WebSiteServices.GetWebsiteFolder(domainName);

            Site[] siteList = staticPageGenerator.GetSiteList(domainName);

            // A list of urls that need to have static pages generated
            List<string> urls = new List<string>();

            if (siteList != null)
            {
                // Generate urls for each store in the chain
                foreach (Site site in siteList)
                {
                    Logger.Log.Info("GenerateStoreSnapshot: Generating static files for " + site.name);

                    // Create a sub folder for the static page for this store
                    string siteDirectory = webSiteFolder + @"\" + site.name;
                    StaticPageGenerator.CleanAndCreateDirectory(siteDirectory);

                    // Home page
                    urls.Add(StaticPageGenerator.XmlEncode(site.name + "/home"));

                    // Store details page
                    urls.Add(StaticPageGenerator.XmlEncode(site.name + "/storedetails"));

                    // menu
                    StoreMenu storeMenu = staticPageGenerator.GetSiteMenu(domainName, webSiteServicesData, site.siteId);
                    if (storeMenu != null && storeMenu.MenuData != null && storeMenu.MenuData.Display != null && storeMenu.MenuData.Display.Count > 0)
                    {
                        // Menu page
                        urls.Add(StaticPageGenerator.XmlEncode(site.name + "/menu"));

                        // Menu sections pages
                        foreach (RawCategory category in storeMenu.MenuData.Display)
                        {
                            if (category.Name.ToString().ToLower().Equals("dealsonly")) continue;

                            urls.Add(StaticPageGenerator.XmlEncode(site.name + "/menu/" + category.Name));
                        }

                        if (storeMenu.MenuData.Deals != null && storeMenu.MenuData.Deals.Count > 0)
                        {
                            // Deals page
                            urls.Add(StaticPageGenerator.XmlEncode(site.name + "/menu/deals"));
                        }
                    }
                }

                // Generate static pages
                StaticPageGenerator.GetSnapshot(domainName, webSiteFolder, urls);
            }

            // Generate site-map file.
            StaticPageGenerator.UpdateSiteMapFile(domainName, urls);
        }

        private static bool UpdateSiteMapFile(string domainName, List<string> urls)
        {
            bool success = true;

            string sitemapFilename = Path.Combine(WebSiteServices.GetWebsiteFolder(domainName), "sitemap.xml");

            StringBuilder siteMapUrls = new StringBuilder();
            foreach (string phantomURL in urls)
            {
                string url = StaticPageGenerator.GetSiteMapURL(domainName, phantomURL);

                siteMapUrls.AppendLine(url);
            }

            // Write out the sitemap file
            string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
            string siteMapFileString = string.Format(@"<?xml version=""1.0"" encoding=""UTF-8""?><urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">{0}</urlset>", siteMapUrls.ToString());
            System.IO.File.WriteAllText(sitemapFilename, siteMapFileString);

            return success;
        }

        private static string GetSiteMapURL(string domainName, string url)
        {
            url = "http://" + domainName + "/" + url;

            if (!url.EndsWith("/")) url += "/";

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
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
            }
            finally
            {
                // Do we need to log the call data?
                bool log = true;
                if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                {
                    Logger.Log.Error("LogMenuCalls setting missing or invalid");
                    log = true;
                }

                Logger.Log.Debug(
                    "GET SiteList" +
                    " SourceIP:" +
                    " Status: " + httpStatus.ToString() +
                    " Response:" + (log ? responseJson : "Logging disabled"));
            }
            return responseJson;
        }

        private static void GetSnapshot(string domainName, string siteDirectory, List<string> urlParts)
        {
            // "C:\Andromeda Development Source\Rameses Web Components\Services\Andromeda.WebOrdering\packages\PhantomJS.2.0.0\tools\phantomjs\phantomjs.exe" c:\test\index.js c:\test\ http://localhost/websites/weborderingDEV/debug home_+_Dev%20Store%202%20-%20Rameses/StoreDetails/

            // Args
            string rootFolder = WebSiteServices.GetWebsiteFolder(domainName);
            string phantomAssemblyPath = System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", string.Empty) + @"\packages\PhantomJS.2.0.0\tools\phantomjs\phantomjs.exe";
            string indexJsFilename = ConfigurationManager.AppSettings["templatesFolder"] + "index.js";
            string baseUrl = "http://" + domainName;

            string urls = "";
            foreach (string urlPart in urlParts)
            {
                urls = urls + (urls.Length > 0 ? "___" : "") + urlPart;
            }

            string args = " " + indexJsFilename + " \"" + rootFolder + "\" " + baseUrl + " " + urls;

            Logger.Log.Info("GetSnapshot: Starting PhantomJS \"" + phantomAssemblyPath + "\" with args " + args);

            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = phantomAssemblyPath,
                Arguments = args
            };
            process.StartInfo = startInfo;

            process.Start();

            if (!process.WaitForExit(9999000))
            {
                process.Kill();
            }

            string output = process.StandardOutput.ReadToEnd();

            Console.WriteLine(process.ExitCode.ToString() + " " + output);

            // TODO check for errors
        }

        private static void GetSnapshotForRootIndex(string urlpart, string domainName)
        {
            //string unsafeText1 = @"Unsafe JavaScript attempt to access frame with URL about:blank from frame with URL file:///C:/inetpub/wwwroot/WebOrdering/Chains/" + StaticPageGenerator.XmlEncode(domainName) + "/index.js. Domains, protocols and ports must match.";
            //string unsafeText2 = @"Unsafe JavaScript attempt to access frame with URL about:blank from frame with URL file:///C:/inetpub/wwwroot/sites/" + StaticPageGenerator.XmlEncode(domainName) + "/index.js. Domains, protocols and ports must match.";

            //string rootFolder = WebSiteServices.GetWebsiteFolder(domainName);
            //string phantomAssemblyPath = System.IO.Path.GetDirectoryName(
            //        System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", string.Empty) + @"\packages\PhantomJS.2.0.0\tools\phantomjs\phantomjs.exe";

            //File.WriteAllText(rootFolder + "\\" + "index.js", GrabberJS);

            //string args = string.Format("\"{0}\\{1}\" {2}", rootFolder, "index.js", string.Format("http://{0}/{1}", domainName, urlpart));

            //var process = new System.Diagnostics.Process();
            //var startInfo = new System.Diagnostics.ProcessStartInfo
            //{
            //    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            //    UseShellExecute = false,
            //    RedirectStandardOutput = true,
            //    FileName = phantomAssemblyPath,
            //    Arguments = args
            //};
            //process.StartInfo = startInfo;
            //process.Start();

            //string output = process.StandardOutput.ReadToEnd();
            //output = output.Replace(unsafeText1, string.Empty).Replace(unsafeText2, string.Empty);

            //if (!string.IsNullOrEmpty(output) && output != "--TIMEOUT--")
            //{
            //    try
            //    {
            //        if (File.Exists(rootFolder + "\\" + "Index_Renamed.html"))
            //        {
            //            File.Delete(rootFolder + "\\" + "Index_Renamed.html");
            //        }
            //        File.Move(rootFolder + "\\" + "Index.html", rootFolder + "\\" + "Index_Renamed.html");
            //        File.WriteAllText(rootFolder + "\\" + "Index.html", output);
            //    }
            //    catch (Exception exception)
            //    {
            //        Logger.Log.Error("File Error - ROOT Index.html generation", exception);
            //    }
            //}
            //else
            //{
            //    Logger.Log.Error("TIME OUT in grabbing root index.html file");
            //}
        }

        private static string GetPhantomURL(string domainName, string storeName, string urlPart)
        {
            return string.Format(@"http://{0}?page={1}/{2}", domainName, storeName, urlPart);
        }

        private static string GetSitemapPhantomURL(string domainName, string storeName, string urlPart)
        {
            return string.Format(@"http://{0}/{1}/{2}", domainName, storeName, urlPart);
        }

        public Model.StoreMenu GetSiteMenu(string domainName, WebSiteServicesData webSiteServicesData, string siteId)
        {
            StoreMenu storeMenu = new StoreMenu();
            MenuRoot siteMenu = null;
            try
            {
                if (webSiteServicesData.WebSiteConfiguration != null
                    && !String.IsNullOrEmpty(webSiteServicesData.WebSiteConfiguration.ACSApplicationId)
                    && !String.IsNullOrEmpty(siteId))
                {
                    Stream responseStream = null;
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
            catch (Exception ex)
            {
                storeMenu = null;
                Logger.Log.Error("WebSiteServices.GetSiteMenu: Unhandled exception", ex);
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
                Logger.Log.Error("Unhandled exception", exception);
                responseJson = "Exception";
                response = Helper.InternalError(exception);
            }
            finally
            {
                // Do we need to log the call data?
                bool log = true;
                if (!bool.TryParse(ConfigurationManager.AppSettings["LogSiteCalls"], out log))
                {
                    Logger.Log.Error("LogMenuCalls setting missing or invalid");
                    log = true;
                }

                Logger.Log.Debug(
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

        private static bool GenerateIndexFileRanking(IStaticPageGenerator staticPageGenerator, string domainName, WebSiteServicesData webSiteServicesData)
        {
            try
            {
                string newWebsiteFolder = WebSiteServices.GetWebsiteFolder(domainName);
                string indexFilename = Path.Combine(newWebsiteFolder, "index.html");
                string html = System.IO.File.ReadAllText(indexFilename);

                // Inject Ranking Text
                if (html.Contains("<!-- RANKING CONTENT -->"))
                {
                    html = html.Replace("<!-- RANKING CONTENT -->", GenerateRankingString(staticPageGenerator.GetSiteMenu(domainName, webSiteServicesData, null)));
                    System.IO.File.WriteAllText(indexFilename, html);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Debug("Error: GenerateIndexFileRanking", ex);
            }
            return true;
        }
    }
}