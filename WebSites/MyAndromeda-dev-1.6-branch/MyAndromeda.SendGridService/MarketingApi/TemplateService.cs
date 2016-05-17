using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyAndromeda.SendGridService.MarketingApi.Models;
using Newtonsoft.Json;
using MyAndromeda.SendGridService.MarketingApi.Models.Template;

namespace MyAndromeda.SendGridService.MarketingApi
{
   
    public class TemplateService : ITemplateService
    {
        public const string AddRoute = "https://api.sendgrid.com/api/newsletter/add.json";
        public const string EditRoute = "https://api.sendgrid.com/api/newsletter/edit.json";
        public const string ReadRoute = "https://api.sendgrid.com/api/newsletter/get.json";

        private readonly ISendGridEmailSettings emailSettings;

        public TemplateService(ISendGridEmailSettings emailSettings) 
        {
            this.emailSettings = emailSettings;
        }

        public async Task<GetResponseTemplateModel> GetAsync(GetRequestTemplateModel model) 
        {
            GetResponseTemplateModel result = null;

            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;
            
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(ReadRoute, model);

                    if (request.IsSuccessStatusCode)
                    {

                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<GetResponseTemplateModel>(r);
                    }
                    else 
                    {
                        //if there is no good response continue.
                        //var r = await request.Content.ReadAsStringAsync();
                        //throw new WebException(r);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<SendGridMessage> AddAsync(AddTemplateModel model)
        {
            SendGridMessage result = null;

            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(AddRoute, model);

                    if (request.IsSuccessStatusCode)
                    {

                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<SendGridMessage>(r);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<SendGridMessage> EditAsync(EditTemplateModel model)
        {
            SendGridMessage result = null;

            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(EditRoute, model);

                    if (request.IsSuccessStatusCode)
                    {

                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<SendGridMessage>(r);
                        //incorrect content type response.
                        //result = await request.Content.ReadAsAsync<AddOrEditSenderAddressModel>();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
