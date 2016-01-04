using System.Net;
using MyAndromeda.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService.MarketingApi.Models.Lists;
using System.Net.Http;
using Newtonsoft.Json;
using MyAndromeda.SendGridService.MarketingApi.Models.Recipients;

namespace MyAndromeda.SendGridService.MarketingApi
{
    public interface IRecipientListService : IDependency
    {
        /// <summary>
        /// Creates the async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> CreateAsync(Models.Lists.CreateListModel model);

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<Models.Lists.GetListResponseModel> GetAsync(Models.Lists.GetListRequestModel model);

        /// <summary>
        /// Deletes the async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task DeleteAsync(Models.Lists.DeleteRequestModel model);

        /// <summary>
        /// Adds the people async.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<AddPeopleResponseModel> AddPeopleAsync<TModel>(AddPeopleRequestModel<TModel> model)
            where TModel : Person;

        /// <summary>
        /// Gets the people async.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<GetPeopleResponseModel<TModel>> GetPeopleAsync<TModel>(GetPeopleRequestModel model)
            where TModel : Person;

        /// <summary>
        /// Gets the person count async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<GetPeopleCountResponseModel> GetPersonCountAsync(GetPeopleRequestModel model);

        /// <summary>
        /// Removes the emails async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<RemoveEmailsResponseModel> RemoveEmailsAsync(RemoveEmailsRequestModel model);
    }

    public class RecipientListService : IRecipientListService
    {
        private readonly ISendGridEmailSettings emailSettings;
        private readonly IMyAndromedaLogger logger;

        public const string CreateList = "https://api.sendgrid.com/api/newsletter/lists/add.json";
        public const string GetRoute = "https://api.sendgrid.com/api/newsletter/lists/get.json";
        public const string DeleteRoute = "https://api.sendgrid.com/api/newsletter/lists/delete.json";

        public const string RemoveEmailsRoute = "https://api.sendgrid.com/api/newsletter/lists/email/delete.json";
        public const string AddPeopleRoute = "https://api.sendgrid.com/api/newsletter/lists/email/add.json";
        public const string GetPeopleRoute = "https://api.sendgrid.com/api/newsletter/lists/email/get.json";
        public const string GetPeopleCountRoute = "https://api.sendgrid.com/api/newsletter/lists/email/count.json";

        public RecipientListService(ISendGridEmailSettings emailSettings, IMyAndromedaLogger logger) 
        {
            this.emailSettings = emailSettings;
            this.logger = logger;
        }

        public async Task<bool> CreateAsync(CreateListModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(CreateList, model);

                    result = request.IsSuccessStatusCode;
                    //if (request.IsSuccessStatusCode)
                    //{

                    //    var r = await request.Content.ReadAsStringAsync();
                    //    result = JsonConvert.DeserializeObject<AddOrEditSenderAddressModel>(r);
                    //    //incorrect content type response.
                    //    //result = await request.Content.ReadAsAsync<AddOrEditSenderAddressModel>();
                    //}
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }

            return result;
        }

        public async Task<GetListResponseModel> GetAsync(GetListRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            GetListResponseModel result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(CreateList, model);

                    if (request.IsSuccessStatusCode)
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<GetListResponseModel>(r);
                    }
                    else 
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        this.logger.Debug(r);
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

        public async Task DeleteAsync(DeleteRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            GetListResponseModel result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(CreateList, model);

                    //if (request.IsSuccessStatusCode)
                    //{

                    //    var r = await request.Content.ReadAsStringAsync();
                    //    result = JsonConvert.DeserializeObject<GetListResponseModel>(r);

                    //}
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }

        }
        
        public async Task<AddPeopleResponseModel> AddPeopleAsync<TModel>(AddPeopleRequestModel<TModel> model) where TModel : Person
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            AddPeopleResponseModel result = null;
            try
            {

                var json = JsonConvert.SerializeObject(model);

                this.logger.Debug("Add People:");
                this.logger.Debug(json);

                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(AddPeopleRoute, model);

                    if (request.IsSuccessStatusCode)
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<AddPeopleResponseModel>(r);
                    }
                    else
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        this.logger.Error("Could not add people");
                        throw new WebException(r);
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

        public async Task<GetPeopleResponseModel<TModel>> GetPeopleAsync<TModel>(GetPeopleRequestModel model) where TModel : Person
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            GetPeopleResponseModel<TModel> result = null;
            try
            {
                using (var client = new HttpClient())
                {


                    string route = GetPeopleRoute + client.CreateUrlEncodedQueryString(model);

                    var request = await client.GetAsync(route);

                    if (request.IsSuccessStatusCode)
                    {

                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<GetPeopleResponseModel<TModel>>(r);
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

        public async Task<GetPeopleCountResponseModel> GetPersonCountAsync(GetPeopleRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            GetPeopleCountResponseModel result = null;
            try
            {
                using (var client = new HttpClient())
                {

                    string route = GetPeopleCountRoute + client.CreateUrlEncodedQueryString(model);

                    var request = await client.GetAsync(route);

                    if (request.IsSuccessStatusCode)
                    {

                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<GetPeopleCountResponseModel>(r);

                        result.ListName = model.ListName;
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

        public async Task<RemoveEmailsResponseModel> RemoveEmailsAsync(RemoveEmailsRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            RemoveEmailsResponseModel result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(RemoveEmailsRoute, model);

                    if (request.IsSuccessStatusCode)
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        
                        result = JsonConvert.DeserializeObject<RemoveEmailsResponseModel>(r);

                        //incorrect content type response.
                        //result = await request.Content.ReadAsAsync<AddOrEditSenderAddressModel>();
                    }
                    else 
                    {
                        if (model.Emails.Length > 0) 
                        {
                            var r = await request.Content.ReadAsStringAsync();
                            throw new Exception(r);    
                        }
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

        public async Task ClearRecipientList() 
        {
            
        }
    }
}
