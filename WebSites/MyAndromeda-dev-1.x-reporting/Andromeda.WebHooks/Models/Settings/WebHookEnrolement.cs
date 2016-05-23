namespace MyAndromeda.Services.WebHooks.Models.Settings
{
    public class WebHookEnrolement 
    {
        public string Name { get; set; }

        public string CallBackUrl { get; set; }

        public bool Enabled { get; set; }

        public RequestHeaders RequestHeaders { get; set; }
    }
}