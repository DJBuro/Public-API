using MyAndromeda.Data.Domain;

namespace MyAndromeda.Web.ViewModels
{
    public class ChainListViewModel
    {
        public ChainDomainModel[] FlatternedChains { get; set; }
        public ChainDomainModel[] TreeViewChain { get; set; }
    }
}