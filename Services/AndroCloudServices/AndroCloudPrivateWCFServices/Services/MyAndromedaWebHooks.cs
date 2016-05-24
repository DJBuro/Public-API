using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AndroCloudDataAccess;
using System.Net.Http;
using System.Net.Http.Headers;
using AndroCloudServices.Domain;
using Newtonsoft.Json;

namespace AndroCloudPrivateWCFServices.Services
{
    public static class MyAndromedaWebHooks
    {
        public const string AuthHeader = "Hi";

        public static async Task<string> CallWebHooksForEtdChange(
            SiteUpdate update,
            string andromedaSiteId 
            ) 
        {
            try
            {
                var hosts = new List<AndroCloudDataAccess.Domain.HostV2>();

                string results = "";
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
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
                            client.DefaultRequestHeaders.Add(name: "MyAuthorization", value: AuthHeader);

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
                                string message = string.Format(format: "Notify - Could not call : {0}", 
                                    arg0: host.Url);

                                string responseMessage = await response.Content.ReadAsStringAsync();
                                //throw new WebException(message, new Exception(responseMessage));
                                results += message + " " + responseMessage;

                                string auditMessage = string.Format(format: "{'Call':'{0}','edt': {1},'asid':'{2}'}", 
                                    arg0: host.Url, 
                                    arg1: model.Edt, 
                                    arg2: model.AndromedaSiteId);

                                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                                   string.Empty, string.Empty, string.Empty, 
                                   action: "CallWebHooksForEtdChange-WebHooks failed", 
                                   responseTime: 0, 
                                   errorCode: 0, 
                                   extraInfo: auditMessage);
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
            AndroCloudDataAccess.Domain.Site site,
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
                string results = string.Empty;
                DataAccessHelper.DataAccessFactory.HostDataAccess.GetOrderStatusChangedHosts(out hosts);

                if (hosts.Count == 0)
                {
                    DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                                  string.Empty, string.Empty, string.Empty, 
                                  action: "OrderStatusChange-WebHooks", 
                                  responseTime: 0, 
                                  errorCode: 0, 
                                  extraInfo: "No web hook endpoints to send to for CallWebHooksForOrderStatusChange");
                }

                var model = new
                {
                    AndromedaSiteId = site.AndroId,
                    ExternalSiteId = site.ExternalId,
                    Source = "AndroCloudPrivateWCFServices.Services.MyAndromedaWebHooks",
                    RamesesOrderNum = ramesesOrderNum,
                    //ExternalOrderId = externalOrderId,
                    //AcsApplicationId = acsApplicationId,
                    Status = update.Status
                };
                string body = JsonConvert.SerializeObject(model);

                foreach (var host in hosts)
                {
                    //call: MyAndromeda - probably 
                    DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                                string.Empty, string.Empty, string.Empty,
                                action: "OrderStatusChange-WebHooks",
                                responseTime: 0,
                                errorCode: 0,
                                extraInfo: string.Format(format: "send to: '{0}' | {1}", arg0: host.Url, arg1: body));
                    
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            // New code:
                            client.BaseAddress = new Uri(host.Url);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
                            client.DefaultRequestHeaders.Add(name: "MyAuthorization", value: AuthHeader);

                            

                            HttpResponseMessage response = await client.PostAsJsonAsync(host.Url, model);

                            if (!response.IsSuccessStatusCode)
                            {
                                string message = string.Format(format: "Notify - Could not call : {0}", arg0: host.Url);
                                string responseMessage = await response.Content.ReadAsStringAsync();
                                //throw new WebException(message, new Exception(responseMessage));
                                results += message + " " + responseMessage;

                                string auditMessage = string.Format(format: "{'Call':'{0}','status': {1},'asid':'{2}'}", arg0: host.Url, arg1: model.Status, arg2: model.AndromedaSiteId);
                                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                                   string.Empty, string.Empty, string.Empty, 
                                   action: "OrderStatusChange-WebHooks", 
                                   responseTime: 0, 
                                   errorCode: 0, 
                                   extraInfo: auditMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format(format: "Notify - Could not call : {0}", arg0: host.Url);
                        string responseMessage = ex.Message;
                        //throw new WebException(message, new Exception(responseMessage));
                        results += message + " " + responseMessage;

                        string auditMessage = string.Format(format: "{'Call':'{0}', 'Error': '{1}'}", arg0: host.Url, arg1: results);
                        DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                           string.Empty, string.Empty, string.Empty, 
                           action: "CallWebHooksForOrderStatusChange-WebHooks", 
                           responseTime: 0, 
                           errorCode: 0, 
                           extraInfo: auditMessage);
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                          string.Empty, string.Empty, string.Empty,
                          action: "CallWebHooksForOrderStatusChange-WebHooks",
                          responseTime: 0,
                          errorCode: 0,
                          extraInfo: string.Format("error: {0}", ex.Message));
            }

            return string.Empty;
        }

        public static async Task<string> CallWebHooksForMenuUpdate(string androSiteId, string version)
        {
            //list places to call for version updated - DataAccessHelper;
            var hosts = new List<AndroCloudDataAccess.Domain.HostV2>();
            string results = "";
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
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
                        client.DefaultRequestHeaders.Add(name: "MyAuthorization", value: AuthHeader);

                        var model = new
                        {
                            Source = "AndroCloudPrivateWCFServices.Services.MyAndromedaWebHooks",
                            AndromedaSiteId = androSiteId,
                            Version = version
                        };


                        HttpResponseMessage response = await client.PostAsJsonAsync(host.Url, model);

                        if (!response.IsSuccessStatusCode)
                        {
                            string message = string.Format(format: "Notify - Could not call : {0}", arg0: host.Url);
                            string responseMessage = await response.Content.ReadAsStringAsync();
                            //throw new WebException(message, new Exception(responseMessage));
                            results += message + " " + responseMessage;

                            string auditMessage = string.Format(format: "{'Call':'{0}','v': '{1}','asid':'{2}'}", arg0: host.Url, arg1: model.Version, arg2: model.AndromedaSiteId);
                            DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                               string.Empty, string.Empty, string.Empty,
                               action: "CallWebHooksForMenuUpdate-WebHooks", 
                               responseTime: 0, 
                               errorCode: 0, 
                               extraInfo: auditMessage);
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