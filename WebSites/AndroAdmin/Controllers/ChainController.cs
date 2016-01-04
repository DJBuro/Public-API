using AndroAdminDataAccess.DataAccess;
using AndroAdminDataAccess.EntityFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdmin.ViewModels.Chains;
using AndroAdmin.ViewModels.StoreType;
using AndroAdmin.Helpers;
using AndroAdminDataAccess.Domain;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace AndroAdmin.Controllers
{
    public class ChainController : BaseController
    {
        private readonly IChainDAO chainDataService;
        private readonly IStoreDAO storeDataService;

        public ChainController()
        {
            chainDataService = new ChainDAO();
            storeDataService = new StoreDAO();
        }

        public ActionResult Index()
        {
            // Get a list of all chains
            IList<ChainViewModel> chainsView = new List<ChainViewModel>();
            chainsView = chainDataService.GetAll().Select(s => s.ToViewModel()).ToList();

            return View(chainsView);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ChainViewModel viewModel = this.GetChainViewModel(id);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(ChainViewModel chainViewModel)
        {            
            List<int> mappedStoreIds = new List<int>();
            List<string> mappedStores = chainViewModel.UpdatedMappedStores == null ? new List<string>() : chainViewModel.UpdatedMappedStores.Split(',').ToList();
         
            foreach (var item in mappedStores)
            {
                mappedStoreIds.Add(Convert.ToInt32(item));
            }

            chainViewModel.SelectedMapStores = mappedStoreIds;
            
            int chainId = chainDataService.Save(chainViewModel.ToDataModel());

            return RedirectToAction("Index");
        }

        private ChainViewModel GetChainViewModel(int? id)
        {
            ChainViewModel viewModel = (id == null ? (new ChainViewModel() { MappedStores = new List<SelectListItem>() }) : (chainDataService.GetChainById(Convert.ToInt32(id)).ToViewModel()));
            
            // Get the chains stores
            viewModel.AllStores = storeDataService.GetAllStores().Select(s => s.ToViewModel()).ToList();

            if (id != null)
            {
                if (viewModel.AllStores != null)
                {
                    viewModel.AllStores.ToList().ForEach(f => f.IsMappedToChain = (viewModel.MappedStores.Any(a => Convert.ToInt32(a.Value) == Convert.ToInt32(f.Id))));
                }
            }

            // Get all the chains - for selecting the parent chain
            IList<ChainViewModel> chainsView = new List<ChainViewModel>();
            chainsView = chainDataService.GetAll().Select(s => s.ToViewModel()).ToList();

            List<ChainSelector> chainList = new List<ChainSelector>();
            foreach (ChainViewModel chainViewModel in chainsView)
            {
                chainList.Add(new ChainSelector() { ChainName = chainViewModel.Name, ChainId = chainViewModel.Id.ToString() });
            }
            viewModel.ChainList = chainList;

            // Get the current parent chain
            // In theory the database schema allows multiple parents but we'll just use the first


            return viewModel;
        }

        public JsonResult All([DataSourceRequest]DataSourceRequest request)
        {
            IList<ChainViewModel> chains = new List<ChainViewModel>();
            chains = chainDataService.GetAll().Select(s => s.ToViewModel()).ToList();

            var chainsTreeData = ((IEnumerable<ChainViewModel>)chains).ToDataSourceResult(request);

            return Json(chainsTreeData, JsonRequestBehavior.AllowGet);
        }
    }
}
