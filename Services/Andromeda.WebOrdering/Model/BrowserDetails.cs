namespace Andromeda.WebOrdering.Model
{
    public class BrowserDetails
    {
        public string Accept { get; set; }
        public string UserAgent { get; set; }

        public BrowserDetails()
        {
            this.Accept = "*/*";
            this.UserAgent = "";
        }
    }
}