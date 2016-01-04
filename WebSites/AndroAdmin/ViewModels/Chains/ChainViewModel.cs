using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.Domain;
using AndroAdmin.ViewModels.StoreType;
using System.Web.Mvc;

namespace AndroAdmin.ViewModels.Chains
{
    public class ChainViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Culture { get; set; }
        public string MasterMenuId { set; get; }
        public string StoresCount { set; get; }
        public string ParentChainId { get; set; }

        public IList<StoreChainViewModel> AllStores { set; get; }
        public IEnumerable<SelectListItem> MappedStores { set; get; }
        public string UpdatedMappedStores { set; get; }
             
        public IEnumerable<int> SelectedMapStores { set; get; }

        public IEnumerable<ChainSelector> ChainList { get; set; }
    }

    public static class ChainViewModelExtension
    {
        public static ChainViewModel ToViewModel(this Chain model)
        {
            ChainViewModel viewModel = new ChainViewModel();
            viewModel.Id = (int?)model.Id;
            viewModel.Name = model.Name;
            viewModel.Description = model.Description;
            viewModel.ParentChainId = model.ParentChainId.HasValue ? model.ParentChainId.ToString() : "";
            if (model.Stores.Where(s => s.Id == model.MasterMenuId).FirstOrDefault() != null)
            {
                int? masterMenuId = model.Stores.Where(s => s.Id == model.MasterMenuId).FirstOrDefault().Id;
                viewModel.MasterMenuId = masterMenuId.HasValue ? masterMenuId.ToString() : "-";
            }
            else
            {
                viewModel.MasterMenuId = "-";
            }
            viewModel.StoresCount = model.Stores == null ? string.Empty : model.Stores.Count().ToString();
            viewModel.MappedStores = model.Stores.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            viewModel.SelectedMapStores = model.Stores.Select(x => x.Id).ToList();
            viewModel.UpdatedMappedStores = string.Join(",", model.Stores.Select(x => x.Id).ToList());

            return viewModel;
        }

        public static Chain ToDataModel(this ChainViewModel model)
        {
            Chain chain = new Chain
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                
                Stores = model.SelectedMapStores == null ? new List<Store>() : model.SelectedMapStores.Select(x => new Store
                {
                    Id = x
                }).ToList()
            };

            int masterMenuId = 0;
            if (Int32.TryParse(model.MasterMenuId, out masterMenuId)) chain.MasterMenuId = masterMenuId;

            int parentChainId = 0;
            if (Int32.TryParse(model.ParentChainId, out parentChainId)) chain.ParentChainId = parentChainId;

            return chain;
        }
    }

    public class ChainSelector
    {
        public string ChainName { get; set; }
        public string ChainId { get; set; }
    }
}