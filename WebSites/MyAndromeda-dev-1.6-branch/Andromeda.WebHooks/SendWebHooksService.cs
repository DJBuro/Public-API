﻿using MyAndromeda.Framework.Helpers;
using MyAndromeda.Logging;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.Services.WebHooks.Models.Settings;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyAndromeda.Data.DataAccess;

namespace MyAndromeda.Services.WebHooks
{

    public class SendWebHooksService : ISendWebHooksService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IWebHooksEvent[] eventHandlers;
        private readonly IAcsApplicationDataService dataService;

        public SendWebHooksService(
            IMyAndromedaLogger logger, 
            IWebHooksEvent[] eventHandlers, 
            IAcsApplicationDataService dataService)
        {
            this.dataService = dataService;
            this.eventHandlers = eventHandlers;
            this.logger = logger;
        }

        private async Task SendingActionHandlers<TModel>(TModel model, WebHookEnrolement enrollment)
            where TModel : IHook
        {
            await this.eventHandlers.ForEachAsync(async ev =>
            {
                try
                {
                    this.logger.Debug(format: "Notifying {0} - Sending handler {1}", args: new object[] { ev.Name, enrollment.Name });
                    await ev.SendingRequestAsync(model.AndromedaSiteId, enrollment, model);
                }
                catch (Exception ex)
                {
                    this.logger.Error("Event Failed: " + ev.Name);
                    this.logger.Error(ex);
                }
            });
        }

        private async Task SentActionHandlers<TModel>(TModel model, WebHookEnrolement enrollment)
            where TModel : IHook
        {
            await this.eventHandlers.ForEachAsync(async ev =>
            {
                try
                {
                    this.logger.Debug(format: "Notifying {0} - Sent handler {1}", args: new object[] { ev.Name, enrollment.Name });
                    await ev.SentRequestAsync(model.AndromedaSiteId, enrollment, model);
                }
                catch (Exception ex)
                {
                    this.logger.Error("Event Failed: " + ev.Name);
                    this.logger.Error(ex);
                }
            });
        }

        private async Task OnError<TModel>(TModel model, WebHookEnrolement enrolment, Exception ex)
             where TModel : IHook
        {
            this.logger.Error(format: "AndromedaSiteId: {0} - Failed task: {0} - {1}", args: new object[] { model.AndromedaSiteId, enrolment.Name, enrolment.CallBackUrl });
            this.logger.Error(ex);

            await this.eventHandlers.ForEachAsync(async ev =>
            {
                try
                {
                    await ev.FailedRequestAsync(model.AndromedaSiteId, enrolment, model);
                }
                catch (Exception)
                {
                    this.logger.Error("Error Failed :) -" + ev.Name);
                    this.logger.Error(ex);
                }
            });

        }


        public async Task CallEndpoints<TModel>(
            TModel model,
            Func<WebhookSettings, List<WebHookEnrolement>> fetchEnrollments)
            where TModel : IHook
        {
            var acsApplications = await dataService.Query()
                .Where(e => e.ACSApplicationSites.Any(acsApplication => acsApplication.Store.AndromedaSiteId == model.AndromedaSiteId))
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.WebHookSettings
                }).ToListAsync();

            try
            {
                if (acsApplications.All(e => string.IsNullOrEmpty(e.WebHookSettings)))
                {
                    await this.eventHandlers.ForEachAsync(async ev =>
                    {
                        await ev.NoWebHooksAsync(model.AndromedaSiteId, model);
                    });
                    //return as there is nothing left to do. 
                    //return;
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(message: "Failed processing NoWebHooksAsync");
                this.logger.Error(ex);
                throw;
            }
            
            try
            {
                this.logger.Info(message: "webhooks warming up. Alert handlers");
                //notify events before sending this externally
                //useful to attach other activities when someone happens. 
                await this.eventHandlers.ForEachAsync(async ev =>
                {
                    try
                    {
                        await ev.BeforeDistributionAsync(model.AndromedaSiteId, model);
                    }
                    catch (Exception ex)
                    {
                        this.logger.Error(ev.Name + " exception");
                        this.logger.Error(ex);
                        //throw;
                    }
                });
            }
            catch (Exception ex)
            {
                this.logger.Error(message: "Failed processing BeforeDistributionAsync");
                this.logger.Error(ex);
                throw;
            }

            try
            {
                //notify each subscription that we are sending. 
                await acsApplications
                    .Where(e => !string.IsNullOrWhiteSpace(e.WebHookSettings))
                    .ForEachAsync(async e =>
                    {
                        if (string.IsNullOrWhiteSpace(e.WebHookSettings)) 
                        {
                            this.logger.Info("(skipping) There are no webhook settings for ACS Application: " + e.Name);
                            return; 
                        }

                        WebhookSettings webhookSettings = JsonConvert.DeserializeObject<WebhookSettings>(e.WebHookSettings);

                        List<WebHookEnrolement> webhooks = fetchEnrollments(webhookSettings) ?? new List<WebHookEnrolement>();

                        try
                        {
                            await webhooks.Where(r => r.Enabled).ForEachAsync(async enrollment =>
                            {
                                this.logger.Info(format: "{0} - {1} start: SendingActionHandlers", args: new object[] { e.Name, enrollment.Name });
                                await this.SendingActionHandlers(model, enrollment);
                                this.logger.Info(format: "{0} - {1} start: CreateRequest", args: new object[] { e.Name, enrollment.Name });
                                await this.CreateRequest(model, enrollment, OnError);
                                this.logger.Info(format: "{0} - {1} start: SentActionHandlers", args: new object[] { e.Name, enrollment.Name });
                                await this.SentActionHandlers(model, enrollment);
                            });
                        }
                        catch (Exception ex)
                        {
                            this.logger.Error("Error processing webhook activities for application:" + e.Name);
                            this.logger.Error(ex);
                            //one fails ... the rest *should* continue. 
                        }
                    });
            }
            catch (Exception ex)
            {
                this.logger.Error(message: "Failed processing each request");
                this.logger.Error(ex);
                throw;
            }


            //notify events after sending this externally
            this.logger.Info(message: "All webhooks completed. Alert handlers");
            await this.eventHandlers.ForEachAsync(async ev =>
            {
                try
                {
                    await ev.AfterDistributionAsync(model.AndromedaSiteId, model);
                }
                catch (Exception ex)
                {
                    this.logger.Error(format: "Failed processing: {0} - AfterDistributionAsync", args: new object[] { ev.Name });
                    this.logger.Error(ex);
                }
            });


        }

        private async Task<WebHookEnrolement> CreateRequest<TModel>(TModel payload, WebHookEnrolement enrollment, Func<TModel, WebHookEnrolement, Exception, Task> onError)
            where TModel : IHook
        {
            Exception ex = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(enrollment.CallBackUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));

                    if (enrollment.RequestHeaders != null)
                    {
                        //has not been done yet unfortunately 
                        foreach (var header in enrollment.RequestHeaders)
                        {
                            if (string.IsNullOrWhiteSpace(header.Key)) { continue; }
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    this.logger.Info("Created client. PostAsJson to: " + enrollment.CallBackUrl);
                    string content = JsonConvert.SerializeObject(payload);
                    this.logger.Info("Content: " + content);

                    HttpResponseMessage response = await client.PostAsJsonAsync(enrollment.CallBackUrl, payload);

                    this.logger.Info("Response received for: " + enrollment.CallBackUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        string message = string.Format(format: "{0} - Could not call : {1}", arg0: enrollment.Name, arg1: enrollment.CallBackUrl);
                        string responseMessage = await response.Content.ReadAsStringAsync();

                        this.logger.Error(message);
                        this.logger.Error(responseMessage);

                        throw new WebException(message, new Exception(responseMessage));
                    }
                    else
                    {
                        this.logger.Debug(format: "{0} Called webhook endpoint successfully", args: new object[] { enrollment.CallBackUrl });
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.Error("Calling endpoint failed:" + enrollment.CallBackUrl);
                ex = e;
            }

            if (ex != null) { await onError(payload, enrollment, ex); }

            return enrollment;
        }
    }
}
