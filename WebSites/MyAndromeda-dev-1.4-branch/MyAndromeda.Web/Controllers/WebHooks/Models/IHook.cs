namespace MyAndromeda.Web.Controllers.WebHooks.Models
{
    public interface IHook
    {
        /// <summary>
        /// Gets or sets the andromeda site id.
        /// </summary>
        /// <value>The andromeda site id.</value>
        int AndromedaSiteId { get; set; }
    }
}