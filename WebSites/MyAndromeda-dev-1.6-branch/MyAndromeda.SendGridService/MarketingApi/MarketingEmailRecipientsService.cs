using System;
using System.Net.Http;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService.MarketingApi.Models.Recipients;
using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi
{
    public class MarketingEmailRecipientsService : IMarketingEmailRecipientsService
    {
        public const string AddRoute = "https://api.sendgrid.com/api/newsletter/recipients/add.json";
        public const string GetRoute = "https://api.sendgrid.com/api/newsletter/recipients/get.json";
        public const string RemoveRoute = "https://api.sendgrid.com/api/newsletter/recipients/delete.json";

        private readonly ISendGridEmailSettings emailSettings;
        private readonly IMyAndromedaLogger logger;

        public MarketingEmailRecipientsService(ISendGridEmailSettings emailSettings, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.emailSettings = emailSettings;
        }

        public async Task<bool> AddAsync(AddOrRemoveRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            var result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(AddRoute, model);

                    result = request.IsSuccessStatusCode;

                    if (!result) 
                    {
                        var content = await request.Content.ReadAsStringAsync();
                        
                        throw new Exception(content);
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }

            return result;
        }

        public async Task<ListRecipientListNames> ListAsync(GetRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            ListRecipientListNames result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(GetRoute, model);
                    var r = await request.Content.ReadAsStringAsync();
                    
                    if (!request.IsSuccessStatusCode)
                    {
                        throw new Exception(r);
                    }
                    
                    result = JsonConvert.DeserializeObject<ListRecipientListNames>(r);
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }

            return result;
        }

        public async Task<bool> RemoveListFromMarketingEmailAsync(AddOrRemoveRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            var result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(RemoveRoute, model);

                    result = request.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }

            return result;
        }
    }
}