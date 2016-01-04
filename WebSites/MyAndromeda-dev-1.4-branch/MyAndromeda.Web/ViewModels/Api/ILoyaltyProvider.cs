using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.ViewModels.Api
{

    public class LoyaltyProviderViewModel
    {
        public Guid? Id { get; set; }
        public dynamic Configuration { get; set; }
        public string ProviderName { get; set; }
    }

    public static class LoyaltyProviderViewModelExtension 
    {
        public static LoyaltyProviderViewModel ToViewModel(this StoreLoyalty model) 
        {
            if (model == null) { return null; }

            return new LoyaltyProviderViewModel()
            {
                Id = model.Id,
                ProviderName = model.ProviderName,
                Configuration = model.Configuration == null ? null : JsonConvert.DeserializeObject(model.Configuration)
            };
        }

        public static StoreLoyalty UpdateFromViewModel(this StoreLoyalty storeLoyalty, LoyaltyProviderViewModel loyaltyProvider) 
        {
            if (storeLoyalty == null) 
            {
                storeLoyalty = new StoreLoyalty();
            }

            if (loyaltyProvider.Configuration != null) {
                storeLoyalty.ProviderName = loyaltyProvider.ProviderName;
                storeLoyalty.Configuration = JsonConvert.SerializeObject(loyaltyProvider.Configuration);
            }

            return storeLoyalty;
        }

    }
}