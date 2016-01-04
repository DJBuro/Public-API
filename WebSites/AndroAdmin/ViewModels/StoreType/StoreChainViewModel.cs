using AndroAdminDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.ViewModels.StoreType
{
    public class StoreChainViewModel
    {
        public string Name { set; get; }
        public int Id { set; get; }
        public string AndromedaSiteId { set; get; }
        public StoreStatus StoreStatus { set; get; }
        public bool IsMappedToChain { set; get; }
    }

    public static class StoreChainViewModelExtension
    {
        public static StoreChainViewModel ToViewModel(this Store store) {
            
            return new StoreChainViewModel { 
                Name = store.Name,
                StoreStatus = store.StoreStatus,
                Id = store.Id,
                AndromedaSiteId = store.AndromedaSiteId.ToString()                
            };
        }        
    }
}