using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OrderTracking.Dao;
using OrderTracking.Dao.Domain;
using OrderTrackingAdmin.Models;
using OrderTrackingAdmin.Mvc;
using OrderTrackingAdmin.Mvc.Filters;
using System.Configuration;
using System.IO;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web;

namespace OrderTrackingAdmin.Controllers
{
    public class ChainController : SiteController
    {
        public ITrackerDao TrackerDao;
        public IClientDao ClientDao;
        public IClientStoreDao ClientStoreDao;
        public IClientWebsiteCustomContentDao ClientWebsiteCustomContentDao;
        public ILogDao LogDao;
        public IOrderDao OrderDao;
        public IDriverDao DriverDao;
        public IOrderStatusDao OrderStatusDao;
        public ICoordinatesDao CoordinatesDao;
        public ICustomerDao CustomerDao;

        [RequiresAuthorisation]
        public ActionResult Index()
        {
            var data = new OrderTrackingAdminViewData.ClientViewData
            {
                Clients = ClientDao.FindAll() //.OrderBy(c => c.Client.Name).ToList()
            };

            return (View(StoreControllerViews.All, data));
        }

        [RequiresAuthorisation]
        public ActionResult All()
        {
            return Index();
        }

        // Add new chain
        [RequiresAuthorisation]
        public ActionResult Add()
        {
            var data = new OrderTrackingAdminViewData.ClientViewData();

            string webTemplatesFolder = ConfigurationManager.AppSettings["WebTemplatesFolder"];
            List<SelectListItem> listItems = new List<SelectListItem>();

            SelectListItem listItem = new SelectListItem();
            listItem.Text = "None";
            listItem.Value = "";
            listItems.Add(listItem);

            foreach (string fullDirectory in Directory.GetDirectories(webTemplatesFolder))
            {
                string directory = fullDirectory.Substring(fullDirectory.LastIndexOf("\\") + 1);

                listItem = new SelectListItem();
                listItem.Text = directory;
                listItem.Value = directory;

                listItems.Add(listItem);
            }

            data.WebsiteTemplates = (IEnumerable<SelectListItem>)listItems;

            return (View(data));
        }

        [RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(Client client)
        {
            client.WebKey = Guid.NewGuid().ToString("N");
            ClientDao.Create(client);

            return RedirectToAction("View");
        }

        [RequiresAuthorisation]
        public ActionResult Edit(int id)
        {
            Client client = ClientDao.FindById(id);

            // Build a list of templates
            string webTemplatesFolder = ConfigurationManager.AppSettings["WebTemplatesFolder"];
            List<SelectListItem> listItems = new List<SelectListItem>();

            SelectListItem listItem = new SelectListItem();
            listItem.Text = "None";
            listItem.Value = "";

            if (client.WebsiteTemplateName.Length == 0)
            {
                listItem.Selected = true;
            }

            listItems.Add(listItem);

            foreach (string fullDirectory in Directory.GetDirectories(webTemplatesFolder))
            {
                string directory = fullDirectory.Substring(fullDirectory.LastIndexOf("\\") + 1);

                listItem = new SelectListItem();
                listItem.Text = directory;
                listItem.Value = directory;

                if (directory == client.WebsiteTemplateName)
                {
                    listItem.Selected = true;
                }

                listItems.Add(listItem);
            }

            // Get the chains stores
            IList<ClientStore> clientStores = ClientStoreDao.FindByChainId(id);

            List<Store> stores = new List<Store>();
            foreach (ClientStore clientStore in clientStores)
            {
                stores.Add(StoreDao.FindById((int)clientStore.StoreId));
            }

            string url = "";
            string webSitesFolder = ConfigurationManager.AppSettings["WebSitesFolder"];
            string chainFolder = Path.Combine(webSitesFolder, client.Name);

            if (System.IO.File.Exists(Path.Combine(chainFolder, "index.htm")))
            {
                url = ConfigurationManager.AppSettings["BaseChainWebsiteUrl"] + client.Name;
            }

            var data = new OrderTrackingAdminViewData.ClientViewData
            {
                Client = client,
                Stores = stores,
                WebsiteTemplates = (IEnumerable<SelectListItem>)listItems,
                Url = url,
                HasTemplate = (client.WebsiteTemplateName.Length > 0)
            };

            return View(data);
        }

        [RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection form)
        {
            List<int> storeIds = new List<int>();
            string websiteTemplateName = "";

            // Get the stores that the user ticked
            foreach (string key in form.Keys)
            {
                // Is this a store checkbox control?
                if (key.StartsWith("store_"))
                {
                    string value = form.Get(key);

                    // Do we need to add this store to the chain?
                    if (value.StartsWith("true"))
                    {
                        string storeIdText = key.Substring(6);
                        int storeId = 0;

                        if (Int32.TryParse(storeIdText, out storeId))
                        {
                            // Remove the store from the chain
                            storeIds.Add(storeId);
                        }
                    }
                }
                else if (key == "WebsiteTemplateName")
                {
                    websiteTemplateName = form.Get(key);
                }
            }

            // Remove chain stores that are not ticked
            foreach (int storeId in storeIds)
            {
                ClientStoreDao.DeleteByStoreId(storeId);
            }

            // Did the user select a different web template?
            Client existingClient = ClientDao.FindById(id);

            if (existingClient.WebsiteTemplateName != websiteTemplateName)
            {
                // Change the template name
                existingClient.WebsiteTemplateName = websiteTemplateName;
                ClientDao.Update(existingClient);

                // Delete all custom content for this chain
                ClientWebsiteCustomContentDao.DeleteAllForChain(id);

                if (websiteTemplateName == "")
                {
                    // Remove all website files
                    string websiteFolder = Path.Combine(ConfigurationManager.AppSettings["WebSitesFolder"], existingClient.Name);

                    string impersonateUsername = ConfigurationManager.AppSettings["ImpersonateUsername"];
                    string impersonateDomain = ConfigurationManager.AppSettings["ImpersonateDomain"];
                    string impersonatePassword = ConfigurationManager.AppSettings["ImpersonatePassword"];

                    if (this.ImpersonateValidUser(impersonateUsername, impersonateDomain, impersonatePassword))
                    {
                        if (Directory.Exists(websiteFolder))
                        {
                            foreach (string fullSourceFilename in Directory.GetFiles(websiteFolder))
                            {
                                System.IO.File.Delete(fullSourceFilename);
                            }
                        }

                        this.UndoImpersonation();
                    }
                }
                else
                {
                    // Use default values for blank values

                    // Get the injection points from the template
                    Dictionary<string, List<InjectionPoint>> injectionPoints = new Dictionary<string, List<InjectionPoint>>();
                    this.GetInjectionPoints(id, injectionPoints);
                    
                    // Check each injection point to see if a custom value already exists for it
                    foreach (List<InjectionPoint> groupInjectionPoints in injectionPoints.Values)
                    {
                        foreach (InjectionPoint injectionPoint in groupInjectionPoints)
                        {
                            // The web key is a special injection point that can't be customized
                            if (injectionPoint.Type != "WebKey")
                            {                              
                                ClientWebsiteCustomContent newClientWebsiteCustomContent = new ClientWebsiteCustomContent();
                                newClientWebsiteCustomContent.ClientId = id;
                                newClientWebsiteCustomContent.Key = injectionPoint.Id;
                                newClientWebsiteCustomContent.Value = injectionPoint.DefaultValue;

                                ClientWebsiteCustomContentDao.Create(newClientWebsiteCustomContent);
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Edit");
        }

        [RequiresAuthorisation]
        public ActionResult Stores(int id)
        {
            IList<Store> allStores = StoreDao.FindAll();
            IList<Store> unassignedStores = new List<Store>();

            foreach (Store store in allStores)
            {
                IList<ClientStore> stores = ClientStoreDao.FindByStoreId(store.Id.Value);

                if (stores.Count == 0)
                {
                    unassignedStores.Add(store);
                }
            }

            var data = new OrderTrackingAdminViewData.ClientViewData
            {
                Client = ClientDao.FindById(id),
                Stores = unassignedStores
            };

            return View(data);
        }

        [RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Stores(int id, FormCollection form)
        {
            List<int> storeIds = new List<int>();

            // Get the stores that the user ticked
            foreach (string key in form.Keys)
            {
                // Is this a store checkbox control?
                if (key.StartsWith("store_"))
                {
                    string value = form.Get(key);

                    // Do we need to add this store to the chain?
                    if (value.StartsWith("true"))
                    {
                        // Extract the store id from the controlname
                        string storeIdText = key.Substring(6);
                        int storeId = 0;

                        if (Int32.TryParse(storeIdText, out storeId))
                        {
                            // Add the store to the chain
                            storeIds.Add(storeId);
                        }
                    }
                }
            }

            foreach (int storeId in storeIds)
            {
                ClientStore clientStore = new ClientStore(id, storeId);
                ClientStoreDao.Create(clientStore);
            }

            return RedirectToAction("Stores");
        }

        [RequiresAuthorisation]
        public ActionResult WebSite(int id)
        {
            // Get the client details from the database;
            Client client = ClientDao.FindById(id);

            // Get the injection points from the template
            Dictionary<string, List<InjectionPoint>> injectionPoints = new Dictionary<string, List<InjectionPoint>>();
            this.GetInjectionPoints(id, injectionPoints);

            // Get the existing injection values
            IList<ClientWebsiteCustomContent> clientWebsiteCustomContent = ClientWebsiteCustomContentDao.FindByChainId(id);

            // Check each injection point to see if a custom value exists for it
            foreach (List<InjectionPoint> groupInjectionPoints in injectionPoints.Values)
            {
                foreach (InjectionPoint injectionPoint in groupInjectionPoints)
                {
                    // The web key is a special injection point that can't be customized
                    if (injectionPoint.Type != "WebKey")
                    {
                        // See if there is a custom value for this injection point
                        foreach (ClientWebsiteCustomContent customContent in clientWebsiteCustomContent)
                        {
                            // Is this custom value for the injection point?
                            if (customContent.Key == injectionPoint.Id)
                            {
                                // There is a custom value for this injection point
                                injectionPoint.Value = customContent.Value;
                                break;
                            }
                        }
                    }
                }
            }

            string url = "";
            string webSitesFolder = ConfigurationManager.AppSettings["WebSitesFolder"];
            string chainFolder = Path.Combine(webSitesFolder, client.Name);

            if (System.IO.File.Exists(Path.Combine(chainFolder, "index.htm")))
            {
                url = ConfigurationManager.AppSettings["BaseChainWebsiteUrl"] + client.Name;
            }

            var data = new OrderTrackingAdminViewData.ClientViewData
            {
                Client = client,
                InjectionPoints = injectionPoints,
                Url = url
            };

            return View(data);
        }

        [RequiresAuthorisation]
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult WebSite(int id, OrderTrackingAdminViewData.ClientViewData clientViewData, FormCollection form)
        {
            // Get the client details from the database;
            Client client = ClientDao.FindById(id);

            Dictionary<string, string> injectValues = new Dictionary<string,string>();

            // Get the values that the user entered
            foreach (string key in form.Keys)
            {
                if (key.StartsWith("inj_"))
                {
                    string value = form[key].Replace("\"", "'");
                    injectValues.Add(key.Substring(4), value);
                }
            }

            // Inject the template with the values provided by the user
            this.ParseInjectionTemplate(id, injectValues);

            // Delete all custom content for this chain
            ClientWebsiteCustomContentDao.DeleteAllForChain(id);

            Dictionary<string, Dictionary<string, string>> borderChunks = new Dictionary<string, Dictionary<string, string>>();
            foreach (KeyValuePair<string, string> keyValuePair in injectValues)
            {
                if (keyValuePair.Key.EndsWith("_Colour") || 
                    keyValuePair.Key.EndsWith("_Top") || 
                    keyValuePair.Key.EndsWith("_Right") ||
                    keyValuePair.Key.EndsWith("_Bottom") ||
                    keyValuePair.Key.EndsWith("_Left"))
                {
                    string key = keyValuePair.Key.Substring(0, keyValuePair.Key.LastIndexOf("_"));

                    Dictionary<string, string> chunks = null;
                    if (!borderChunks.TryGetValue(key, out chunks))
                    {
                        chunks = new Dictionary<string, string>();
                        borderChunks.Add(key, chunks); 
                    }

                    chunks.Add(keyValuePair.Key, keyValuePair.Value);
                }
                else
                {
                    ClientWebsiteCustomContent clientWebsiteCustomContent = new ClientWebsiteCustomContent();
                    clientWebsiteCustomContent.ClientId = id;
                    clientWebsiteCustomContent.Key = keyValuePair.Key;
                    clientWebsiteCustomContent.Value = keyValuePair.Value;

                    ClientWebsiteCustomContentDao.Create(clientWebsiteCustomContent);
                }
            }

            foreach (KeyValuePair<string, Dictionary<string, string>> borderChunk in borderChunks)
            {
                // Put the border chunks back together
                ClientWebsiteCustomContent clientWebsiteCustomContent = new ClientWebsiteCustomContent();
                clientWebsiteCustomContent.ClientId = id;
                clientWebsiteCustomContent.Key = borderChunk.Key;
                clientWebsiteCustomContent.Value = "";

                string colour = "";
                if (borderChunk.Value.TryGetValue(borderChunk.Key + "_Colour", out colour))
                {
                    clientWebsiteCustomContent.Value += colour + " ";
                }

                string top = "";
                if (borderChunk.Value.TryGetValue(borderChunk.Key + "_Top", out top))
                {
                    clientWebsiteCustomContent.Value += top + " ";
                }

                string right = "";
                if (borderChunk.Value.TryGetValue(borderChunk.Key + "_Right", out right))
                {
                    clientWebsiteCustomContent.Value += right + " ";
                }

                string bottom = "";
                if (borderChunk.Value.TryGetValue(borderChunk.Key + "_Bottom", out bottom))
                {
                    clientWebsiteCustomContent.Value += bottom + " ";
                }

                string left = "";
                if (borderChunk.Value.TryGetValue(borderChunk.Key + "_Left", out left))
                {
                    clientWebsiteCustomContent.Value += left + " ";
                }

                ClientWebsiteCustomContentDao.Create(clientWebsiteCustomContent);
            }

            return RedirectToAction("WebSite");
        }

        private void GetInjectionPoints(int id, Dictionary<string, List<InjectionPoint>> injectionPoints)
        {
            Client client = ClientDao.FindById(id);

            string webTemplatesFolder = ConfigurationManager.AppSettings["WebTemplatesFolder"];

            if (client.WebsiteTemplateName.Length > 0)
            {
                string templateFilename = Path.Combine(webTemplatesFolder, Path.Combine(client.WebsiteTemplateName, "index.htm"));

                string templateHtml = "";
                using (StreamReader streamReader = new StreamReader(templateFilename))
                {
                    templateHtml = streamReader.ReadToEnd();
                }

                int startIndex = 0;
                int endIndex = 0;

                do
                {
                    startIndex = templateHtml.IndexOf("<injectionPoint ", endIndex);

                    if (startIndex > -1)
                    {
                        endIndex = templateHtml.IndexOf("/>", startIndex);

                        InjectionPoint injectionPoint = new InjectionPoint();
                        injectionPoint.Id = this.GetAttributeValue(ref templateHtml, startIndex + 16, endIndex, "id");
                        injectionPoint.Prompt = this.GetAttributeValue(ref templateHtml, startIndex + 16, endIndex, "prompt");
                        injectionPoint.Type = this.GetAttributeValue(ref templateHtml, startIndex + 16, endIndex, "type");
                        injectionPoint.Value = this.GetAttributeValue(ref templateHtml, startIndex + 16, endIndex, "default");
                        injectionPoint.Value = injectionPoint.Value.Replace("{siteroot}", ConfigurationManager.AppSettings["BaseChainWebsiteUrl"] + client.Name + "/");
                        injectionPoint.DefaultValue = injectionPoint.Value;

                        string group = this.GetAttributeValue(ref templateHtml, startIndex + 16, endIndex, "group");

                        List<InjectionPoint> groupInjectionPoints = null;
                        if (!injectionPoints.TryGetValue(group, out groupInjectionPoints))
                        {
                            groupInjectionPoints = new List<InjectionPoint>();
                            injectionPoints.Add(group, groupInjectionPoints);
                        }

                        groupInjectionPoints.Add(injectionPoint);
                    }
                }
                while (startIndex > -1);
            }
        }

        private void ParseInjectionTemplate(int chainId, Dictionary<string, string> injectValues)
        {
            Client client = ClientDao.FindById(chainId);
            string parsedTemplate = "";

            // Get the existing values from the database
            IList<ClientWebsiteCustomContent> clientWebsiteCustomContent = ClientWebsiteCustomContentDao.FindByChainId(chainId);

            string webSitesFolder = ConfigurationManager.AppSettings["WebSitesFolder"];
            string chainFolder = Path.Combine(webSitesFolder, client.Name);

            // Write the parsed template out to disk
            string impersonateUsername = ConfigurationManager.AppSettings["ImpersonateUsername"];
            string impersonateDomain = ConfigurationManager.AppSettings["ImpersonateDomain"];
            string impersonatePassword = ConfigurationManager.AppSettings["ImpersonatePassword"];

            if (this.ImpersonateValidUser(impersonateUsername, impersonateDomain, impersonatePassword))
            {
                // Does the chain folder exist?
                if (!Directory.Exists(chainFolder))
                {
                    // Create the chain folder
                    Directory.CreateDirectory(chainFolder);
                }

                string webTemplatesFolder = ConfigurationManager.AppSettings["WebTemplatesFolder"];

                // Sort out the image files
                foreach (string file in Request.Files)
                {
                    string key = file.Substring(4);

                    HttpPostedFileBase hpf = (HttpPostedFileBase)Request.Files[file];
                    if (hpf.ContentLength == 0)
                    {
                        // Use the existing value
                        foreach (ClientWebsiteCustomContent customContent in clientWebsiteCustomContent)
                        {
                            if (customContent.Key == key)
                            {
                                injectValues.Add(key, customContent.Value);
                                break;
                            }
                        }
                    }
                    else
                    {
                        string imageFilename = Path.Combine(chainFolder, Path.GetFileName(hpf.FileName));
                        hpf.SaveAs(imageFilename);

                        string imageUrl = ConfigurationManager.AppSettings["BaseChainWebsiteUrl"] + client.Name + "/" + Path.GetFileName(hpf.FileName);

                        injectValues.Add(key, imageUrl);
                    }
                }

                if (client.WebsiteTemplateName.Length > 0)
                {
                    string templateFilename = Path.Combine(webTemplatesFolder, Path.Combine(client.WebsiteTemplateName, "index.htm"));

                    string templateHtml = "";
                    using (StreamReader streamReader = new StreamReader(templateFilename))
                    {
                        templateHtml = streamReader.ReadToEnd();
                    }

                    int startIndex = 0;
                    int endIndex = 0;

                    do
                    {
                        // End index is the end of the last injection point
                        startIndex = templateHtml.IndexOf("<injectionPoint ", endIndex);

                        if (startIndex > -1)
                        {
                            if (endIndex == 0)
                            {
                                parsedTemplate += templateHtml.Substring(0, startIndex - endIndex);
                            }
                            else
                            {
                                parsedTemplate += templateHtml.Substring(endIndex + 2, startIndex - (endIndex + 2));
                            }

                            endIndex = templateHtml.IndexOf("/>", startIndex);

                            // Extract the id of the injection point
                            string injectionPointId = this.GetAttributeValue(ref templateHtml, startIndex + 16, endIndex, "id");
                            string injectionPointType = this.GetAttributeValue(ref templateHtml, startIndex + 16, endIndex, "type");

                            // Is there a value to inject?
                            string injectValue = "";
                            if (injectionPointType == "Border")
                            {
                                parsedTemplate += "border-style: solid; ";

                                if (injectValues.TryGetValue(injectionPointId + "_Colour", out injectValue))
                                {
                                    // Inject the value
                                    parsedTemplate += "border-color: " + injectValue + "; ";
                                }

                                if (injectValues.TryGetValue(injectionPointId + "_Top", out injectValue))
                                {
                                    // Inject the value
                                    parsedTemplate += "border-top-width: " + injectValue + "px; ";
                                }

                                if (injectValues.TryGetValue(injectionPointId + "_Right", out injectValue))
                                {
                                    // Inject the value
                                    parsedTemplate += "border-right-width: " + injectValue + "px; ";
                                }

                                if (injectValues.TryGetValue(injectionPointId + "_Bottom", out injectValue))
                                {
                                    // Inject the value
                                    parsedTemplate += "border-bottom-width: " + injectValue + "px; ";
                                }

                                if (injectValues.TryGetValue(injectionPointId + "_Left", out injectValue))
                                {
                                    // Inject the value
                                    parsedTemplate += "border-left-width: " + injectValue + "px; ";
                                }
                            }
                            // The web key is a special injection point that can't be customized
                            else if (injectionPointType == "WebKey")
                            {
                                // Inject the clients web key
                                parsedTemplate += client.WebKey;
                            }
                            else
                            {
                                if (injectValues.TryGetValue(injectionPointId, out injectValue))
                                {
                                    // Inject the value
                                    parsedTemplate += injectValue;
                                }
                            }
                        }
                    }
                    while (startIndex > -1);

                    parsedTemplate += templateHtml.Substring(endIndex + 2);
                }

                // Copy each file in the templates folder to the chains web site folder 
                string templateFolder = Path.Combine(webTemplatesFolder, client.WebsiteTemplateName);

                foreach (string fullSourceFilename in Directory.GetFiles(templateFolder))
                {
                    // Ignore the default.htm template file
                    if (!fullSourceFilename.EndsWith("index.htm", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string sourceFilename = Path.GetFileName(fullSourceFilename);
                        string destinationFilename = Path.Combine(chainFolder, sourceFilename);

                        if (!System.IO.File.Exists(destinationFilename))
                        {
                            // Copy the template file to the chains web site folder
                            System.IO.File.Copy(fullSourceFilename, destinationFilename, true);
                        }
                    }
                }

                // Write out the parsed html file to the chains web site folder
                using (StreamWriter streamWriter = new StreamWriter(Path.Combine(chainFolder, "index.htm")))
                {
                    streamWriter.Write(parsedTemplate);
                }

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = (HttpPostedFileBase)Request.Files[file];
                    if (hpf.ContentLength == 0) continue;

                    string imageFilename = Path.Combine(chainFolder, Path.GetFileName(hpf.FileName));
                    hpf.SaveAs(imageFilename);
                }

                this.UndoImpersonation();
            }
            else
            {
                //Your impersonation failed. Therefore, include a fail-safe mechanism here.
            }
        }

        private string GetAttributeValue(ref string templateHtml, int startIndex, int endIndex, string attribute)
        {
            string value = "";

            int nameStartIndex = templateHtml.IndexOf(attribute, startIndex);
            int nameValueStartIndex = templateHtml.IndexOf("\"", nameStartIndex + attribute.Length);
            int nameValueEndIndex = templateHtml.IndexOf("\"", nameValueStartIndex + 1);

            if (nameValueEndIndex < endIndex)
            {
                value = templateHtml.Substring(nameValueStartIndex+ 1, nameValueEndIndex - nameValueStartIndex - 1);
            }

            return value;
        }

        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        private bool ImpersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        private void UndoImpersonation()
        {
            impersonationContext.Undo();
        }
    }
}
