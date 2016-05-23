using Newtonsoft.Json;
using System;
using System.Linq;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Contact
{
    public class GetSenderAddressModel : Auth
    {
        [JsonProperty(PropertyName = "identity")]
        public string Identity { get; set; }
        //identity=Sender_Address&
        //api_user=your_sendgrid_username&
        //api_key=your_sendgrid_password
    }


}
