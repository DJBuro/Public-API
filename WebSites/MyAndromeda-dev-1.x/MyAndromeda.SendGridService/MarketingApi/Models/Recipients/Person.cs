using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Dynamic;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Recipients
{
    public class Person : DynamicObject
    {
        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        /// <value>The customer id.</value>
        [JsonProperty("customerId")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [JsonProperty("title")]
        public string Title { get; set; }

        readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties.ContainsKey(binder.Name))
            {
                result = properties[binder.Name];
                return true;
            }
            else
            {
                result = "Invalid Property!";
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            properties[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic method = properties[binder.Name];
            result = method(args[0].ToString(), args[1].ToString());
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.properties.Keys;
        }
    }

}
