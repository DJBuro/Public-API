using System.Collections.Generic;
using MyAndromeda.SendGridService.MarketingApi.Models.Recipients;
using MyAndromeda.SendGridService.MarketingApi.Models.Template;

namespace MyAndromeda.Services.Marketing.Models
{
    public class MarketingStoreEventQueueMessage
    {
        /// <summary>
        /// Gets or sets the type of the marketing campaign.
        /// </summary>
        /// <value>The type of the marketing campaign.</value>
        public string MarketingCampaignType { get; set; }

        /// <summary>
        /// Gets or sets the andromeda site id.
        /// </summary>
        /// <value>The andromeda site id.</value>
        public int AndromedaSiteId { get; set; }
    }

    /// <summary>
    /// Message to that history needs to 
    /// </summary>
    public class MarketingRecipientListMessage 
    {
        public List<Person> People { get; set; }
        public string MarketingCampaignType { get; set; }

        public int AndromedaSiteId { get; set; }
        public GetResponseTemplateModel Template { get; set; }
        public string[] Categories { get; set; }
    }
}