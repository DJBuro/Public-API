using System.Data.Entity;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService;
using MyAndromeda.SendGridService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity.Validation;
using System.Reactive.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Converters;

namespace MyAndromeda.Web.Controllers.Api.WebHooks
{
    public class SendGridWebHookController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IEmailHistoryDataService dataService;
        private readonly IDateServices dateServices;

        public SendGridWebHookController(IMyAndromedaLogger logger, IEmailHistoryDataService dataService, IDateServices dateServices) 
        {
            this.dateServices = dateServices;
            this.dataService = dataService;
            this.logger = logger;
        }

        [HttpPost]
        [Route("api/webhooks/sendgrid")]
        public async Task<HttpResponseMessage> Listen([FromBody]List<SendGridEventModel> models) 
        {
            this.logger.Debug("SendGrid webhooks hit: " + models.Count);

            foreach (var model in models) 
            {
                System.Diagnostics.Trace.TraceError(model.Email); // For debugging to the Azure Streaming logs

                this.logger.Debug(model.Email);
            }

            var emailIds = models.Where(e => !string.IsNullOrWhiteSpace(e.EmailId))
                .Select(e => new Guid(e.EmailId))
                .ToArray();
            

            var body = await this.Request.Content.ReadAsStringAsync();
            
            var converter = new ExpandoObjectConverter();
            List<ExpandoObject> rows = JsonConvert.DeserializeObject<List<ExpandoObject>>(body, converter);
           
            var emailEntities = await this.dataService.Emails
                .Where(e => emailIds.Contains(e.Id))
                .ToArrayAsync();

            var history = models
                .Where(e => !string.IsNullOrWhiteSpace(e.EmailId)).GroupBy(e=> e.EmailId);

            foreach (var historyGroup in history) 
            {
                var id = new Guid(historyGroup.Key);

                historyGroup.ToObservable()
                    //.Distinct(e => e.SendgridEvent) //don't want to distinct click events :-/ 
                    .Subscribe(onNext: (item) => {

                        var email = emailEntities.FirstOrDefault(e => e.Id == id);
                        if (email == null) { return; } //nothing to add history to apparently.

                        var historyEntity = this.dataService.EmailHistory.Create();

                        historyEntity.Id = Guid.NewGuid();
                        historyEntity.EmailId = email.Id;
                        historyEntity.TimeStampUtc = item.TimeStampUtc;
                        historyEntity.EventType = item.SendgridEvent;
                        //historyEntity.Message = string.Empty;

                        ExpandoObject row = rows.First(e => ((dynamic)e).sg_event_id == item.SendgridEventId);
                        historyEntity.Message = JsonConvert.SerializeObject(row);

                        this.dataService.EmailHistory.Add(historyEntity);
                    });

            }

            try
            {
                await this.dataService.SaveAsync();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult validationResult in e.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        this.logger.Error("{0}.{1} - {2}", entityName, error.PropertyName, error.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw e;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/{andromedaSiteId}/email-history/order/{orderId}")]
        public async Task<object> ListOrderHistory(Guid orderId) 
        {
            MyAndromeda.Data.DataWarehouse.Models.Email data = await this.dataService.Emails
                .Include(e=> e.EmailCategories)
                .Include(e=> e.EmailHistories)
                .Where(e => e.OrderHeaderId == orderId)
                .SingleOrDefaultAsync();

            if (data == null) { return null; }

            return new
            {
                Subject = data.Subject,
                Content = data.Content,
                Categories = data.EmailCategories.Select(e => e.Name),
                History = data.EmailHistories
                    .OrderByDescending(e=> e.TimeStampUtc)
                    .Select(e => new { 
                        e.EventType,
                        TimeStamp = this.dateServices.ConvertToLocalFromUtc(e.TimeStampUtc),
                        e.TimeStampUtc,
                        e.Message
                    })
            };
        }
    }
}
