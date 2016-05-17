using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService.MarketingApi.Models;
using MyAndromeda.SendGridService.MarketingApi.Models.Categories;
using System.Net.Http;
using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi
{
    public interface ICategoryService : IDependency
    {
        Task<bool> CreateAsync(CategoryCreateRequestModel model);
        Task<bool> AddCategoryToMarketingEmail(CategoryAddOrRemoveRequestModel model);
        Task<bool> RemoveCategoryFromMarketingEmail(CategoryAddOrRemoveRequestModel model);
        Task<CategoriesResponseModel> ListAsync();
    }

    public class CategoryService : ICategoryService
    {
        public const string CreateRoute = "https://api.sendgrid.com/api/newsletter/category/create.json";
        public const string AddRoute = "https://api.sendgrid.com/api/newsletter/category/add.json";
        public const string RemoveRoute = "https://api.sendgrid.com/api/newsletter/category/remove.json";
        public const string ListRoute = "https://api.sendgrid.com/api/newsletter/category/list.json";

        private readonly ISendGridEmailSettings emailSettings;
        private readonly IMyAndromedaLogger logger;

        public CategoryService(ISendGridEmailSettings emailSettings, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.emailSettings = emailSettings;
        }

        public async Task<bool> CreateAsync(CategoryCreateRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(CreateRoute, model);

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

        public async Task<bool> AddCategoryToMarketingEmail(CategoryAddOrRemoveRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(AddRoute, model);

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

        public async Task<bool> RemoveCategoryFromMarketingEmail(CategoryAddOrRemoveRequestModel model)
        {
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            bool result = false;
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

        public async Task<CategoriesResponseModel> ListAsync()
        {
            var model = new Auth();
            model.ApiUser = this.emailSettings.UserName;
            model.ApiKey = this.emailSettings.Password;

            CategoriesResponseModel result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.PostAsFormPostAsync(ListRoute, model);

                    if (request.IsSuccessStatusCode) 
                    {
                        var r = await request.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<CategoriesResponseModel>(r);
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
    }
}
