using MyAndromeda.Data.Domain;

namespace MyAndromeda.Web.ViewModels
{
    public class ChainUpdateCultureViewModel
    {
        public string ReturnUrl { get; set; }
        public ChainDomainModel Chain { get; set; }
    }
}