using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Recipients
{
    public class GetPeopleResponseModel<TModel> : List<TModel>
        where TModel : Person
    {
    }

    public class GetPeopleCountResponseModel 
    {
        public int Count { get; set; }
        public string ListName { get; set; }
    }
}