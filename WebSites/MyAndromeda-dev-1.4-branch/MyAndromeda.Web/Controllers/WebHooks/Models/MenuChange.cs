namespace MyAndromeda.Web.Controllers.WebHooks.Models
{
    public class MenuChange : IHook 
    {
        public int AndromedaSiteId { get; set; }

        public string Version { get; set; }
    }
}