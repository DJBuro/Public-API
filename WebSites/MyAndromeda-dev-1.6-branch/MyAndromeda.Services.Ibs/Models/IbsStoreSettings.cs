using Newtonsoft.Json;
using System;
using System.Linq;

namespace MyAndromeda.Services.Ibs.Models
{
    public class IbsStoreSettings 
    {
        /// <summary>
        /// Gets or sets the company code.
        /// </summary>
        /// <value>The company code.</value>
        [JsonProperty("CompanyCode")]
        public string CompanyCode { get; set; }

        /// <summary>
        /// Gets or sets the location id.
        /// </summary>
        /// <value>The location id.</value>
        public string LocationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the app GUID.
        /// </summary>
        /// <value>The app GUID.</value>
        [JsonProperty("ApiKey")]
        public string AppGuid { get; set; }

        public string Url { get; set; }
    }
}
