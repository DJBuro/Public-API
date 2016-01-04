using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService.MarketingApi.Models;
using Newtonsoft.Json;
using System.Net.Http;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.SendGridService.MarketingApi.Models.Contact;

namespace MyAndromeda.SendGridService.MarketingApi
{

    public interface ISenderAddressService : IDependency 
    {
        Task<AddOrEditSenderAddressModel> GetAsync(GetSenderAddressModel model);
        Task<SendGridMessage> AddAsync(AddOrEditSenderAddressModel model);
        Task<SendGridMessage> EditAsync(AddOrEditSenderAddressModel model);
    }
    
    public class SenderAddressService : ISenderAddressService
    {
        private const string Get = "https://api.sendgrid.com/api/newsletter/identity/get.json";
        private const string List = "https://api.sendgrid.com/api/newsletter/identity/list.json";
        private const string Delete = "https://api.sendgrid.com/api/newsletter/identity/delete.json";
        private const string Update = "https://api.sendgrid.com/api/newsletter/identity/edit.json";
        private const string Add = "https://api.sendgrid.com/api/newsletter/identity/add.json";

        private readonly ISendGridEmailSettings emailSettings;
        private readonly IMyAndromedaLogger logger;

        public SenderAddressService(ISendGridEmailSettings emailSettings, IMyAndromedaLogger logger)
        {
            this.emailSettings = emailSettings;
            this.logger = logger;
        }

        public async Task<AddOrEditSenderAddressModel> GetAsync(GetSenderAddressModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            AddOrEditSenderAddressModel result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(Get, model);

                    if (request.IsSuccessStatusCode)
                    {

                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<AddOrEditSenderAddressModel>(r);
                        //incorrect content type response.
                        //result = await request.Content.ReadAsAsync<AddOrEditSenderAddressModel>();
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

        public async Task<SendGridMessage> AddAsync(AddOrEditSenderAddressModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            SendGridMessage result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(Add, model);

                    if (request.IsSuccessStatusCode)
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<SendGridMessage>(r);
                        //result = await request.Content.ReadAsAsync<SendGridMessage>();
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

        public async Task<SendGridMessage> EditAsync(AddOrEditSenderAddressModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            SendGridMessage result = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(Update, model);

                    if (request.IsSuccessStatusCode)
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<SendGridMessage>(r);
                        //result = await request.Content.ReadAsAsync<SendGridMessage>();
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }
            
        }
    }
}
