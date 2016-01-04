namespace MyAndromeda.Web.Controllers.WebHooks.Models
{
    public class StoreOnlineStatus : IHook
    {
        /// <summary>
        /// Gets or sets the andromeda site id.
        /// </summary>
        /// <value>The andromeda site id.</value>
        public int AndromedaSiteId { get; set; }

        /// <summary>
        /// Gets or sets who reported the store as online. Hub1, Hub2 etc.
        /// </summary>
        /// <value>The reported by.</value>
        public string ReportedBy { get; set; }
    }
}