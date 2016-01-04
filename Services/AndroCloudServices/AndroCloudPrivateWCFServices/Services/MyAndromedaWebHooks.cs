using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AndroCloudDataAccess;
using System.Net.Http;
using System.Net.Http.Headers;
using AndroCloudServices.Domain;

namespace AndroCloudPrivateWCFServices.Services
{
    public static class MyAndromedaWebHooks
    {
        public const string AuthHeader = "Hi";

        public static async Task<string> CallWebHooksForEtdChange(
            AndroCloudServices.Domain.SiteUpdate update,
            string andromedaSiteId 
            ) 
        {
            try
            {
                var hosts = new List<AndroCloudDataAccess.Domain.HostV2>();

                var results = "";
                DataAccessHelper.DataAccessFactory.HostDataAccess.GetEdtChangedHosts(out hosts);


                foreach (var host in hosts)
                {
                    //call: MyAndromeda - probably 
                    //lookup host v2 
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            // New code:
                            client.BaseAddress = new Uri(host.Url);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("MyAuthorization", AuthHeader);

                            var model = new
                            {
                                AndromedaSiteId = andromedaSiteId,
                                //ExternalSiteId = externalSiteId,
                                Source = "AndroCloudPrivateWCFServices.Services.MyAndromedaWebHooks",
                                Edt = update.ETD
                            };

                            HttpResponseMessage response = await client.PostAsJsonAsync(host.Url, model);

                            if (!response.IsSuccessStatusCode)
                            {
                                string message = string.Format("Notify - Could not call : {0}", host.Url);
                                string responseMessage = await response.Content.ReadAsStringAsync();
                                //throw new WebException(message, new Exception(responseMessage));
                                results += message + " " + responseMessage;

                                var auditMessage = string.Format("{'Call':'{0}','edt': {1},'asid':'{2}'}", host.Url, model.Edt, model.AndromedaSiteId);
                                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                                   "",
                                   "",
                                   "",
                                   "PostMenu-WebHooks",
                                   0,
                                   0,
                                  auditMessage);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return results;
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        public static async Task<string> CallWebHooksForOrderStatusChange(
            OrderStatusUpdate update,
            string andromedaSiteId,
            string ramesesOrderNum
            //string externalOrderId,
            //string acsApplicationId,
            //int status,
            //string statusDescription
            ) 
        {
            //list places to call for version updated - DataAccessHelper;
            try
            {
                var hosts = new List<AndroCloudDataAccess.Domain.HostV2>();
                var results = "";
                DataAccessHelper.DataAccessFactory.HostDataAccess.GetOrderStatusChangedHosts(out hosts);

                if (hosts.Count == 0)
                {
                    DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                                  "",
                                  "",
                                  "",
                                  "PostMenu-WebHooks",
                                  0,
                                  0,
                                 "No web hook endpoints to send to.");
                }
                else
                {
                    DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                                  "",
                                  "",
                                  "",
                                  "PostMenu-WebHooks",
                                  0,
                                  0,
                                 "Where am i? ");
                }


                foreach (var host in hosts)
                {
                    //call: MyAndromeda - probably 
                    //lookup host v2 
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            // New code:
                            client.BaseAddress = new Uri(host.Url);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("MyAuthorization", AuthHeader);

                            var model = new
                            {
                                AndromedaSiteId = andromedaSiteId,
                                //ExternalSiteId = externalSiteId,
                                Source = "AndroCloudPrivateWCFServices.Services.MyAndromedaWebHooks",
                                RamesesOrderNum = ramesesOrderNum,
                                //ExternalOrderId = externalOrderId,
                                //AcsApplicationId = acsApplicationId,
                                Status = update.Status
                            };

                            HttpResponseMessage response = await client.PostAsJsonAsync(host.Url, model);

                            if (!response.IsSuccessStatusCode)
                            {
                                string message = string.Format("Notify - Could not call : {0}", host.Url);
                                string responseMessage = await response.Content.ReadAsStringAsync();
                                //throw new WebException(message, new Exception(responseMessage));
                                results += message + " " + responseMessage;

                                var auditMessage = string.Format("{'Call':'{0}','status': {1},'asid':'{2}'}", host.Url, model.Status, model.AndromedaSiteId);
                                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                                   "",
                                   "",
                                   "",
                                   "PostMenu-WebHooks",
                                   0,
                                   0,
                                  auditMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Notify - Could not call : {0}", host.Url);
                        string responseMessage = ex.Message;
                        //throw new WebException(message, new Exception(responseMessage));
                        results += message + " " + responseMessage;

                        var auditMessage = string.Format("{'Call':'{0}', 'Error': '{1}'}", host.Url, results);
                        DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                           "",
                           "",
                           "",
                           "PostMenu-WebHooks",
                           0,
                           0,
                          auditMessage);
                    }
                }

                return results;
            }
            catch (Exception)
            {
                
            }

            return string.Empty;
        }

        public static async Task<string> CallWebHooksForMenuUpdate(string androSiteId, string version)
        {
            //list places to call for version updated - DataAccessHelper;
            var hosts = new List<AndroCloudDataAccess.Domain.HostV2>();
            var results = "";
            DataAccessHelper.DataAccessFactory.HostDataAccess.GetMenuChangedHosts(out hosts);

            foreach (var host in hosts)
            {
                //call: MyAndromeda - probably 
                //lookup host v2 
                try
                {
                    using (var client = new HttpClient())
                    {
                        // New code:
                        client.BaseAddress = new Uri(host.Url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("MyAuthorization", AuthHeader);

                        var model = new
                        {
                            Source = "AndroCloudPrivateWCFServices.Services.Menus",
                            AndromedaSiteId = androSiteId,
                            Version = version
                        };


                        HttpResponseMessage response = await client.PostAsJsonAsync(host.Url, model);

                        if (!response.IsSuccessStatusCode)
                        {
                            string message = string.Format("Notify - Could not call : {0}", host.Url);
                            string responseMessage = await response.Content.ReadAsStringAsync();
                            //throw new WebException(message, new Exception(responseMessage));
                            results += message + " " + responseMessage;

                            var auditMessage = string.Format("{'Call':'{0}','v': '{1}','asid':'{2}'}", host.Url, model.Version, model.AndromedaSiteId);
                            DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                               "",
                               "",
                               "",
                               "PostMenu-WebHooks",
                               0,
                               0,
                              auditMessage);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return results;
        }
    }
}