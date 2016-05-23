using System.Net;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService.MarketingApi.Models.Schedule;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService.MarketingApi
{
    public class ScheduleService : IScheduleService
    {
        public const string AddRoute = "https://api.sendgrid.com/api/newsletter/schedule/add.json";
        public const string GetRoute = "https://api.sendgrid.com/api/newsletter/schedule/get.json";
        public const string DeleteRoute = "https://api.sendgrid.com/api/newsletter/schedule/delete.json";

        private readonly ISendGridEmailSettings emailSettings;
        private readonly IMyAndromedaLogger logger;

        public ScheduleService(ISendGridEmailSettings emailSettings, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.emailSettings = emailSettings;
        }

        public async Task<ScheduleGetResponseModel> GetAsync(ScheduleGetRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            ScheduleGetResponseModel result =null;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(GetRoute, model);

                    if (request.IsSuccessStatusCode) 
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<ScheduleGetResponseModel>(r);
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

        public async Task<bool> AddAsync(ScheduleAddRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(AddRoute, model);

                    if (!request.IsSuccessStatusCode) 
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        
                        throw new WebException(r);
                    }
                

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

        public async Task<bool> DeleteAsync(ScheduleDeleteRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(DeleteRoute, model);

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
