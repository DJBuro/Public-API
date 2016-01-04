using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using System.Text;
using AndroAdminDataAccess.Domain;
using AndroAdmin.DataAccess;
using AndroAdminDataAccess.DataAccess;

namespace AndroAdmin
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AndroAdminServices
    {
        [WebGet(UriTemplate = "Test")]
        public Stream Test()
        {
            // Stream the result back
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            byte[] returnBytes = Encoding.UTF8.GetBytes("<html><body>Ping</body></html>");
            return new MemoryStream(returnBytes);
        }

        [WebGet(UriTemplate = "GetAMSFTPInfoForSite?password={password}&siteId={id}")]
        public Stream GetAMSFTPInfoForSite(string password, string id)
        {
            string responseText = "";
            int siteId = 0;

            try
            {
                // Was a password provided?
                if (password == null || password != "429C19EE237245358F8E1189ABDB1388")
                {
                    responseText = "<Response errorMessage=\"Invalid password\" />";
                }

                if (responseText.Length == 0)
                {
                    // Was a site id provided?
                    if (id == null)
                    {
                        responseText = "<Response errorMessage=\"Siteid missing\" />";
                    }
                    // Is the siteid a number?
                    else if (!int.TryParse(id, out siteId))
                    {
                        responseText = "<Response errorMessage=\"Siteid is not a valid number: " + id + " />";
                    }
                }

                // Get the store details
                Store store = null;
                if (responseText.Length == 0)
                {
                    // Get an object that can talk to the database
                    IStoreDAO storeDAO = DataAccessFactory.GetStoreDAO();

                    // Get the store
                    store = storeDAO.GetByAndromedaId(siteId);
                }

                // Get the ftp sites that the store needs to upload to
                IList<StoreAMSServerFtpSite> storeAmsServerFtpSites = null;
                if (responseText.Length == 0)
                {
                    // Get an object that can talk to the database
                    IStoreAMSServerFTPSiteDAO storeAmsServerFtpSiteDao = DataAccessFactory.GetStoreAMSServerFTPSiteDAO();

                    storeAmsServerFtpSites = storeAmsServerFtpSiteDao.GetBySiteId(store.Id);
                }

                StringBuilder siteinfo = new StringBuilder();

                if (store == null)
                {
                    responseText = "<Response errorMessage=\"Unknown site id: " + id + "\" />";
                }
                else
                {
                    siteinfo.Append("<SiteInfo name=\"");
                    siteinfo.Append(store.Name);
                    siteinfo.Append("\" andromedaStoreId=\"");
                    siteinfo.Append(store.AndromedaSiteId.ToString());
                    siteinfo.Append("\" customerStoreId=\"");
                    siteinfo.Append(store.CustomerSiteId.ToString());
                    siteinfo.Append("\"");
                    if (store.LastFTPUploadDateTime.HasValue)
                    {
                        siteinfo.Append(" lastFTPUploadDateTime=\"");
                        siteinfo.Append(store.LastFTPUploadDateTime.Value.ToString("s"));
                        siteinfo.Append("\"");
                    }
                    siteinfo.Append(">\r\n");

                    siteinfo.Append("<FTPSites priority=\"0\">\r\n");

                    foreach (StoreAMSServerFtpSite storeAmsServerFtpSite in storeAmsServerFtpSites)
                    {
                        siteinfo.Append("<FTPSite ");

                        siteinfo.Append("isPrimary=\"true\" ");

                        siteinfo.Append("url=\"");
                        siteinfo.Append(storeAmsServerFtpSite.FTPSite.Url);
                        siteinfo.Append("\" ");

                        siteinfo.Append("port=\"");
                        siteinfo.Append(storeAmsServerFtpSite.FTPSite.Port.ToString());
                        siteinfo.Append("\" ");

                        siteinfo.Append("type=\"");
                        siteinfo.Append(storeAmsServerFtpSite.FTPSite.FTPSiteType.Name);
                        siteinfo.Append("\" ");

                        siteinfo.Append("username=\"");
                        siteinfo.Append(storeAmsServerFtpSite.FTPSite.Username);
                        siteinfo.Append("\" ");

                        siteinfo.Append("password=\"");
                        siteinfo.Append(storeAmsServerFtpSite.FTPSite.Password);
                        siteinfo.Append("\" ");

                        siteinfo.Append("folder=\"");
                        siteinfo.Append(storeAmsServerFtpSite.StoreAMSServer.AMSServer.Name);
                        siteinfo.Append("\" ");

                        siteinfo.Append("/>\r\n");
                    }

                    siteinfo.Append("</FTPSites>\r\n");
                    siteinfo.Append("</SiteInfo>\r\n");
                }

                // Was there an error?
                if (responseText.Length == 0)
                {
                    // All ok
                    responseText = "<Response>\r\n" + siteinfo.ToString() + "</Response>\r\n";
                }
                else
                {
                    // Get an object that can talk to the database
                    ILogDAO logDAO = DataAccessFactory.GetLogDAO();

                    // Log the error
                    Log log = new Log();
                    log.Created = DateTime.Now;
                    log.Message = responseText;
                    log.Method = ".GetAMSFTPInfoForSite";
                    log.Severity = "ERROR";
                    log.Source = "AndroAdminServices";
                    log.StoreId = siteId.ToString();
                    logDAO.Add(log);
                }
            }
            catch (Exception exception)
            {
                responseText = "<Response errorMessage=\"" + exception.Message + "\" />";

                try
                {
                    // Get an object that can talk to the database
                    ILogDAO logDAO = DataAccessFactory.GetLogDAO();

                    // Log the error
                    Log log = new Log();
                    log.Created = DateTime.Now;
                    log.Message = exception.Message;
                    log.Method = ".GetAMSFTPInfoForSite";
                    log.Severity = "ERROR";
                    log.Source = "AndroAdminServices";
                    log.StoreId = "";
                    logDAO.Add(log);
                }
                catch { }
            }

            // Stream the result back
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            byte[] returnBytes = Encoding.UTF8.GetBytes(responseText);
            return new MemoryStream(returnBytes);
        }

        [WebGet(UriTemplate = "GetSitesForAMS?password={password}&amsId={amsServerName}")]
        public Stream GetSitesForAMS(string password, string amsServerName)
        {
            string responseText = "";

            try
            {
                // Was a password provided?
                if (password == null || password != "429C19EE237245358F8E1189ABDB1388")
                {
                    responseText = "<Response errorMessage=\"Invalid password\" />";
                }

                AndroAdminDataAccess.Domain.AMSServer amsServer = null;
                if (responseText.Length == 0)
                {
                    // Get an object that can talk to the database
                    IAMSServerDAO amsServerDAO = DataAccessFactory.GetAMSServerDAO();

                    // Get the ams server
                    amsServer = amsServerDAO.GetByName(amsServerName);
                }

                StringBuilder siteinfo = new StringBuilder();

                if (amsServer == null)
                {
                    responseText = "<Response errorMessage=\"Unknown AMS server id: " + amsServerName + "\" />";
                }
                else
                {
                    // Get an object that can talk to the database
                    IStoreAMSServerDAO storeAMSServerDAO = DataAccessFactory.GetStoreAMSServerDAO();

                    // Get the ams server ftp site pair
                    IList<StoreAMSServer> storeAmsServers = storeAMSServerDAO.GetByAMServerName(amsServerName);

                    foreach (StoreAMSServer storeAMSServer in storeAmsServers)
                    {
                        siteinfo.Append("<SiteInfo name=\"");
                        siteinfo.Append(storeAMSServer.Store.Name);
                        siteinfo.Append("\" andromedaStoreId=\"");
                        siteinfo.Append(storeAMSServer.Store.AndromedaSiteId.ToString());
                        siteinfo.Append("\" customerStoreId=\"");
                        siteinfo.Append(storeAMSServer.Store.CustomerSiteId.ToString());

                        if (storeAMSServer.Store.LastFTPUploadDateTime.HasValue)
                        {
                            siteinfo.Append("\" lastFTPUploadDateTime=\"");
                            siteinfo.Append(storeAMSServer.Store.LastFTPUploadDateTime.Value.ToString("s"));
                        }

                        siteinfo.Append("\" storeStatus=\"");
                        siteinfo.Append(storeAMSServer.Store.StoreStatus.Status);

                        siteinfo.Append("\" iso3166_1_numeric=\"");
                        siteinfo.Append(storeAMSServer.Store.Address.Country == null ? "" : storeAMSServer.Store.Address.Country.ISO3166_1_numeric.ToString());

                        siteinfo.Append("\"");
                        siteinfo.Append(">\r\n");


                        siteinfo.Append("<FTPSites>\r\n");

                        foreach (FTPSite ftpSite in storeAMSServer.FTPSites)
                        {
                            siteinfo.Append("<FTPSite ");

                            siteinfo.Append("isPrimary=\"true\" ");

                            siteinfo.Append("url=\"");
                            siteinfo.Append(ftpSite.Url);
                            siteinfo.Append("\" ");

                            siteinfo.Append("port=\"");
                            siteinfo.Append(ftpSite.Port.ToString());
                            siteinfo.Append("\" ");

                            siteinfo.Append("type=\"");
                            siteinfo.Append(ftpSite.FTPSiteType.Name);
                            siteinfo.Append("\" ");

                            siteinfo.Append("username=\"");
                            siteinfo.Append(ftpSite.Username);
                            siteinfo.Append("\" ");

                            siteinfo.Append("password=\"");
                            siteinfo.Append(ftpSite.Password);
                            siteinfo.Append("\" ");

                            siteinfo.Append("folder=\"");
                            siteinfo.Append(storeAMSServer.AMSServer.Name);
                            siteinfo.Append("\" ");

                            siteinfo.Append(" />\r\n");
                        }

                        siteinfo.Append("</FTPSites>\r\n");
                        siteinfo.Append("</SiteInfo>\r\n");
                    }
                }

                // Was there an error?
                if (responseText.Length == 0)
                {
                    // All ok
                    responseText = "<Response>\r\n" + siteinfo.ToString() + "</Response>\r\n";
                }
                else
                {
                    // Get an object that can talk to the database
                    ILogDAO logDAO = DataAccessFactory.GetLogDAO();

                    // Log the error
                    Log log = new Log();
                    log.Created = DateTime.Now;
                    log.Message = responseText;
                    log.Method = ".GetSitesForAMS";
                    log.Severity = "ERROR";
                    log.Source = "AndroAdminServices";
                    log.StoreId = "";
                    logDAO.Add(log);
                }
            }
            catch (Exception exception)
            {
                responseText = "<Response errorMessage=\"" + exception.Message + "\" />";

                try
                {
                    // Get an object that can talk to the database
                    ILogDAO logDAO = DataAccessFactory.GetLogDAO();

                    // Log the error
                    Log log = new Log();
                    log.Created = DateTime.Now;
                    log.Message = exception.Message;
                    log.Method = ".GetSitesForAMS";
                    log.Severity = "ERROR";
                    log.Source = "AndroAdminServices";
                    log.StoreId = "";
                    logDAO.Add(log);
                }
                catch { }
            }

            // Stream the result back
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            byte[] returnBytes = Encoding.UTF8.GetBytes(responseText);
            return new MemoryStream(returnBytes);
        }

        [WebGet(UriTemplate = "UpdateLastUploadedDateTimeForSite?password={password}&siteId={id}&dateTime={dateTime}")]
        public Stream UpdateLastUploadedDateTimeForSite(string password, string id, string dateTime)
        {
            string responseText = "";
            int siteId = 0;

            try
            {
                // Was a password provided?
                if (password == null || password != "429C19EE237245358F8E1189ABDB1388")
                {
                    responseText = "<Response errorMessage=\"Invalid password\" />";
                }

                if (responseText.Length == 0)
                {
                    // Was a site id provided?
                    if (id == null)
                    {
                        responseText = "<Response errorMessage=\"Siteid missing\" />";
                    }
                    // Is the siteid a number?
                    else if (!int.TryParse(id, out siteId))
                    {
                        responseText = "<Response errorMessage=\"Siteid is not a valid number: " + id + " />";
                    }
                }

                // Was a datetime provided?
                DateTime lastUploadedDateTime = DateTime.Now;
                if (responseText.Length == 0)
                {
                    if (!DateTime.TryParse(dateTime, out lastUploadedDateTime))
                    {
                        responseText = "<Response errorMessage=\"DateTime is not a valid date time: " + dateTime + " />";
                    }
                }

                Store store = null;
                IStoreDAO storeDAO = DataAccessFactory.GetStoreDAO();
                if (responseText.Length == 0)
                {
                    store = storeDAO.GetByAndromedaId(siteId);

                    if (store == null)
                    {
                        responseText = "<Response errorMessage=\"Unknown store id: " + siteId.ToString() + " />";
                    }
                }

                if (responseText.Length == 0)
                {
                    store.LastFTPUploadDateTime = lastUploadedDateTime;
                    storeDAO.Update(store);
                }
            }
            catch (Exception exception)
            {
                responseText = "<Response errorMessage=\"" + exception.Message + "\" />";
            }

            // Stream the result back
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            byte[] returnBytes = Encoding.UTF8.GetBytes(responseText);
            return new MemoryStream(returnBytes);
        }

        [WebInvoke(Method = "POST", UriTemplate = "site?password={password}")]
        public Stream Site(Stream input, string password)
        {
            string responseText = "";

            try
            {
//                // Was a password provided?
//                if (password == null || password != "429C19EE237245358F8E1189ABDB1388")
//                {
//                    responseText = "<Response errorMessage=\"Invalid password\" />";
//                }

//                if (responseText.Length == 0)
//                {
//                    // Get the store xml
//                    string xml = AndroAdminServices.StreamToString(input);

//                    // Deserialize the store object
//                    AndroAdminClientDomain.NewUpdateStore store = null;
//                    string errorMessage = SerializeHelper.Deserialize<AndroAdminClientDomain.NewUpdateStore>(xml, out store);

//                    if (errorMessage.Length == 0)
//                    {
//                        // Add/update the store
//                        IStoreDAO storeDAO = DataAccessFactory.GetStoreDAO();
//                        IAddressDAO addressDAO = DataAccessFactory.GetAddressDAO();
//                        IStoreStatusDAO storeStatusDAO = DataAccessFactory.GetStoreStatusDAO();
//                        ICountryDAO countryDAO = DataAccessFactory.GetCountryDAO();

//                        // Get the country
//                        Country country = countryDAO.GetById(store.Address.CountryNumber);

//                        // Copy the address data into an object that can be used with the data access layer
//                        AndroAdminDataAccess.Domain.Address address = new Address()
//                        {
//                            Org1 = store.Address.Org1,
//                            Org2 = store.Address.Org2,
//                            Org3 = store.Address.Org3,
//                            Prem1 = store.Address.Prem1,
//                            Prem2 = store.Address.Prem2,
//                            Prem3 = store.Address.Prem3,
//                            Prem4 = store.Address.Prem4,
//                            Prem5 = store.Address.Prem5,
//                            Prem6 = store.Address.Prem6,
//                            RoadNum = store.Address.RoadNum,
//                            RoadName = store.Address.RoadName,
//                            Locality = store.Address.Locality,
//                            Town = store.Address.Town,
//                            County = store.Address.County,
//                            State = store.Address.State,
//                            PostCode = store.Address.PostCode,
//                            DPS = store.Address.DPS,
//                            Lat = store.Address.Lat,
//                            Long = store.Address.Long,
//                            Country = country
//                        };

//                        // See if the store already exists
//                        Store existingStore = storeDAO.GetByAndromedaId(store.AndromedaSiteId);
//                        // Create the address
//                        int addressId = addressDAO.Add();

//                        // Get the store status
//                        int storeStatusId = storeStatusDAO.GetByStatus(store.StoreStatus);

//                        // Does the store already exist?
//                        if (existingStore == null)
//                        {
////                            existingStore.

//                            // Update the satore
////                            storeDAO.Update(store.AndromedaSiteId);
//                        }
//                        else
//                        {
//                            // Create the store

//                            Store newStore = new Store()
//                            {
//                                AndromedaSiteId = store.AndromedaSiteId,
//                                ClientSiteName = store.ClientSiteName == null || store.ClientSiteName.Length == 0 ? store.Name : store.ClientSiteName,
//                                CustomerSiteId = store.CustomerSiteId == null ? store.AndromedaSiteId.ToString() : store.CustomerSiteId,
//                                ExternalSiteName = store.ExternalSiteName == null ? store.ExternalSiteId.ToString() : store.ExternalSiteId,
//                                Name = store.Name,
//                                //StoreStatus = 
//                                Address = new Address() { Id = addressId } // We only need the address entity id
//                            };

//                            // A new store object has a new GUID as the exgternalid. Only overwrite it if an externalid is provided
//                            newStore.ExternalSiteId = store.ExternalSiteId == null ? newStore.ExternalSiteId : store.ExternalSiteId;

//                            // Add the store
//                            storeDAO.Add(store);
//                        }
//                    }
//                }
            }
            catch (Exception exception)
            {
                responseText = "<Response errorMessage=\"" + exception.Message + "\" />";
            }

            // Stream the result back
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;

            byte[] returnBytes = Encoding.UTF8.GetBytes(responseText);
            return new MemoryStream(returnBytes);
        }

        public static string StreamToString(Stream input)
        {
            string output = "";

            using (StreamReader streamReader = new StreamReader(input))
            {
                if (streamReader != null)
                {
                    output = streamReader.ReadToEnd();
                }
            }

            return output;
        }
    }
}