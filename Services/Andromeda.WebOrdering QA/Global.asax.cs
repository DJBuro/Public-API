using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using log4net;
using Microsoft.AspNet.SignalR.Client;
using Andromeda.WebOrdering.Services;

namespace Andromeda.WebOrdering
{
    public class Global : System.Web.HttpApplication
    {
        internal static readonly ILog Log = LogManager.GetLogger(typeof(Global));
        internal static HubConnection HubConnection = null;
        internal static IHubProxy HubProxy = null;
        internal static Thread BackgroundServicesThread = null;

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            try
            {
                Global.Log.Info("Application_Start");

                // Setup routes
                this.RegisterRoutes();

                //// Inject the data access factory
                //WebOrderingDataAccess.DataAccessHelper.DataAccessFactory = new WebOrderingDataAccessMongoDb.DataAccessFactory();

                // Get the mappings
                Global.GetMappings();

                // Create a new FileSystemWatcher and set its properties.
                FileSystemWatcher watcher = new FileSystemWatcher();

                string mappingFilename = ConfigurationManager.AppSettings["MappingFile"];
                watcher.Path = Path.GetDirectoryName(mappingFilename);
                watcher.Filter = Path.GetFileName(mappingFilename);

                Global.Log.Info("Filewatcher Path=\"" + watcher.Path + "\" Filter=\"" + watcher.Filter + "\"");

                // Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                // Add event handlers.
                watcher.Changed += new FileSystemEventHandler(OnChanged);

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Create a connection to the SignalR hub
                string signalRHub = ConfigurationManager.AppSettings["SignalRHub"];

                if (signalRHub == null || signalRHub.Length == 0)
                {
                    Global.Log.Error("signalRHub setting missing or invalid");
                }
                else
                {
                    try
                    {
                        Global.HubConnection = new HubConnection(signalRHub);
                        Global.HubProxy = Global.HubConnection.CreateHubProxy("OrdersHub");
                        Global.HubConnection.Start().Wait();
                        Global.Log.Info("Connected to signalRHub as connectionId " + Global.HubConnection.ConnectionId);
                    }
                    catch (Exception exception)
                    {
                        Global.Log.Error("Failed to connect to SignalR", exception);
                    }
                }

                // Create the background services thread
                Global.BackgroundServicesThread = new Thread(new ThreadStart(BackgroundServices.StartAsync));

                // Start the thread
                Global.BackgroundServicesThread.Start();
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                Global.GetMappings();
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
            }
        }

        private static void GetMappings()
        {
            Global.Log.Debug("Getting mappings");

            lock (Helper.DomainConfiguration)
            {
                Helper.DomainConfiguration.Clear();

                string mappingFilename = ConfigurationManager.AppSettings["MappingFile"];

                Global.LoadMappingFile(mappingFilename);

                foreach (string mappingFile in Directory.GetFiles(WebSiteServices.GetMappingFolder()))
                {
                    Global.LoadMappingFile(mappingFile);
                }
            }
        }

        public static void LoadMappingFile(string mappingFilename)
        {
            using (StreamReader streamReader = new StreamReader(mappingFilename))
            {
                string xml = streamReader.ReadToEnd();

                // Parse the mapping xml
                XElement mappingXml = XElement.Parse(xml);

                // Process each site
                var mappings = mappingXml.Elements("Mapping");
                foreach (var mapping in mappings)
                {
                    string domainName = mapping.Elements("Domain").FirstOrDefault().Value;
                    string applicationId = mapping.Elements("ApplicationId").FirstOrDefault().Value;

                    string signpostServerText = mapping.Elements("SignpostServerUrl").FirstOrDefault().Value;

                    // Signpost servers are comma delimited
                    List<string> signPostServers = new List<string>();
                    string[] signpostServerSplit = signpostServerText.Split(',');
                    foreach (string signpostServer in signpostServerSplit)
                    {
                        signPostServers.Add(signpostServer);
                    }

                    // Get the default Datacash page set id
                    string defaultDatacashPageSetId = ConfigurationManager.AppSettings["defaultDatacashPageSetId"];
                    defaultDatacashPageSetId = defaultDatacashPageSetId == null || defaultDatacashPageSetId.Length == 0 ? "939" : defaultDatacashPageSetId;

                    // Get the datacash page set id - use the default if none is specified
                    XElement datacashPageSetIdElement = mapping.Elements("DataCashPageSetId").FirstOrDefault();
                    string datacashPageSetId = datacashPageSetIdElement == null ? defaultDatacashPageSetId : datacashPageSetIdElement.Value;

                    string enable3DSecure = ConfigurationManager.AppSettings["enable3DSecure"];
                    bool is3DSecureEnabled = false;
                    bool.TryParse(enable3DSecure, out is3DSecureEnabled);

                    DomainConfiguration domainConfiguration = new DomainConfiguration()
                    {
                        ApplicationId = applicationId,
                        SignpostServers = signPostServers,
                        TemplateEmails = new Dictionary<string, Email>(),
                        DataCashPageSetId = datacashPageSetId,
                        Is3DSecureEnabled = is3DSecureEnabled
                    };

                    XElement emailTemplatesElements = mapping.Elements("EmailTemplates").FirstOrDefault();

                    if (emailTemplatesElements != null)
                    {
                        IEnumerable<XElement> emailTemplateElements = emailTemplatesElements.Elements("EmailTemplate");

                        // Get each email template for the site
                        foreach (XElement xelement in emailTemplateElements)
                        {
                            XElement serverTypeElement = xelement.Elements("ServerType").FirstOrDefault();
                            string serverType = serverTypeElement == null ? "" : serverTypeElement.Value;

                            XElement serverElement = xelement.Elements("Server").FirstOrDefault();
                            string server = serverElement == null ? "" : serverElement.Value;

                            XElement usernameElement = xelement.Elements("Username").FirstOrDefault();
                            string username = usernameElement == null ? "" : usernameElement.Value;

                            XElement passwordElement = xelement.Elements("Password").FirstOrDefault();
                            string password = passwordElement == null ? "" : passwordElement.Value;

                            XElement nameElement = xelement.Elements("Name").FirstOrDefault();
                            string name = nameElement == null ? "" : nameElement.Value;

                            XElement fromElement = xelement.Elements("From").FirstOrDefault();
                            string from = fromElement == null ? "" : fromElement.Value;

                            XElement toElement = xelement.Elements("To").FirstOrDefault();
                            string to = toElement == null ? "" : toElement.Value;

                            XElement subjectElement = xelement.Elements("Subject").FirstOrDefault();
                            string subject = subjectElement == null ? "" : subjectElement.Value;

                            XElement bodyElement = xelement.Elements("Body").FirstOrDefault();
                            string body = bodyElement == null ? "" : bodyElement.Value;

                            domainConfiguration.TemplateEmails.Add
                            (
                                name,
                                new Email()
                                {
                                    ServerType = serverType,
                                    Server = server,
                                    Username = username,
                                    Password = password,
                                    To = to,
                                    From = from,
                                    Subject = subject,
                                    Body = body
                                }
                            );
                        }
                    }

                    if (Helper.DomainConfiguration.ContainsKey(domainName.ToUpper()))
                    {
                        Helper.DomainConfiguration.Remove(domainName.ToUpper());
                    }

                    // Got the config for a new site
                    Helper.DomainConfiguration.Add(domainName.ToUpper(), domainConfiguration);

                    Global.Log.Debug("Added mapping: domain=" + domainName + " appId=" + applicationId + " signpostServerText=" + signpostServerText);
                }
            }
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("webordering", new WebServiceHostFactory(), typeof(RESTServices)));
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}