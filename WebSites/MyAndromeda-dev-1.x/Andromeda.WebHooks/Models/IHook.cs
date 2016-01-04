namespace MyAndromeda.Services.WebHooks.Models
{
    public interface IHook
    {
        /// <summary>
        /// Gets or sets the andromeda site id.
        /// </summary>
        /// <value>The andromeda site id.</value>
        int AndromedaSiteId { get; set; }

        /// <summary>
        /// Gets or sets the external site id.
        /// </summary>
        /// <value>The external site id.</value>
        string ExternalSiteId { get; set; }

        /// <summary>
        /// Gets or sets the source of the update.
        /// </summary>
        /// <value>The source.</value>
        string Source { get; set; }
    }
}