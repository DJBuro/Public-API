using System.Linq;
using Kendo.Mvc.UI;
using MyAndromeda.Data.AcsServices;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Menus.Services.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using MyAndromeda.Web.Areas.Menu.ViewModels;

namespace MyAndromeda.Web.Areas.Menu.Controllers
{
	public class ModifiersController : Controller
	{
		private readonly IWorkContext workContext;
		private readonly IGetAcsAddressesService getAcsAddressesService;
		private readonly IAcsMenuServiceAsync acsMenuServiceService;

		private readonly IToppingItemService toppingsItemService;

		public ModifiersController(
			IWorkContext workContext,
			IGetAcsAddressesService getAcsAddressesService,
			IAcsMenuServiceAsync acsMenuServiceService,
			IToppingItemService toppingsItemService)
		{ 
			this.acsMenuServiceService = acsMenuServiceService;
			this.toppingsItemService = toppingsItemService;
			this.getAcsAddressesService = getAcsAddressesService;
			this.workContext = workContext;
		}

		//
		// GET: /Menu/Modifiers/
		public ActionResult Index()
		{
			return View();
		}

		public async Task<JsonResult> List([DataSourceRequest]DataSourceRequest request) 
		{
			var endpoints = await this.getAcsAddressesService.GetMenuEndpointsAsync(this.workContext.CurrentSite.Store);
			var menu = await acsMenuServiceService.GetMenuDataFromEndpointsAsync(
				this.workContext.CurrentSite.AndromediaSiteId, 
				this.workContext.CurrentSite.ExternalSiteId, 
				endpoints);

			foreach (var item in menu.Toppings) 
			{
				if (item.DineInPrice.HasValue) { item.DineInPrice = item.DineInPrice / 100; }
				if (item.CollectionPrice.HasValue) { item.CollectionPrice = item.CollectionPrice / 100; }
				if (item.DeliveryPrice.HasValue) { item.DeliveryPrice = item.DeliveryPrice / 100; }
			}

			var groupedMenuToppings = menu.Toppings
				.GroupBy(e => new { Name = e.Name, GroupName = e.Group.Name })
				.Select(e => e.ToViewModel(e.Key.Name));

			return Json(groupedMenuToppings.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

			//return Json(menu.Toppings.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Update([DataSourceRequest]DataSourceRequest request, IEnumerable<ToppingViewModelGroup> models) 
		{
			var thing = toppingsItemService;
			var myAndromedaToppingModels = models.Select(e=> e.ToModel()).SelectMany(e=> e).ToArray();

			//foreach (var item in myAndromedaToppingModels) 
			{
				thing.UpdateAccessDatabaseMenuItemsAsync(myAndromedaToppingModels);
			}
			
			return Json(models.ToDataSourceResult(request));
		}
	}
}