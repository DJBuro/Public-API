using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.Domain;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.DataAccess;
using AndroAdmin.Helpers;
using AndroAdmin.Model;
using CloudSync;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewPaymentProviders")]
    public class PaymentProviderController : BaseController
    {
        public PaymentProviderController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.Stores;
            ViewBag.SelectedStoreMenu = StoresMenuItemEnum.PaymentProviders;
        }

        [Security(Permissions = "ViewPaymentProviders")]
        public ActionResult Index()
        {
            try
            {
                // Get all the payment providers
                IEnumerable<AndroAdminDataAccess.Domain.StorePaymentProvider> paymentProviders = this.StorePaymentProviderDAO.GetAll();

                return View(paymentProviders);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PaymentTypesController.Index", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [Security(Permissions = "AddPaymentProvider")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Security(Permissions = "AddPaymentProvider")]
        public ActionResult Add(AndroAdminDataAccess.Domain.StorePaymentProvider storePaymentProvider)
        {
            ActionResult actionResult = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    actionResult = View(storePaymentProvider);
                }

                if (actionResult == null)
                {
                    // Add the store payment provider
                    this.StorePaymentProviderDAO.Add(storePaymentProvider);

                    // Push the change out to the ACS servers
                    string errorMessage = SyncHelper.ServerSync();

                    // Success
                    if (errorMessage.Length == 0)
                    {
                        TempData["message"] = "Payment provider successfully added";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to add payment provider";
                    }

                    actionResult = RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PaymentTypesController.Add (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [Security(Permissions = "UpdatePaymentProvider")]
        public ActionResult Update(int? id)
        {
            ActionResult actionResult = null;

            try
            {
                //if (id.HasValue)
                //{
                //    // Get the store
                //    AndroAdminDataAccess.Domain.Store store = this.StoreDAO.GetById(id.Value);

                //    if (store == null)
                //    {
                //        actionResult = RedirectToAction("Index");
                //    }

                //    // Get a list of store statuses
                //    IList<AndroAdminDataAccess.Domain.StoreStatus> storeStatuses = this.StoreStatusDAO.GetAll();

                //    if (storeStatuses == null)
                //    {
                //        actionResult = RedirectToAction("Index");
                //    }

                //    // Get a list of countries
                //    List<AndroAdminDataAccess.Domain.Country> countries = this.CountryDAO.GetAll();

                //    if (countries == null)
                //    {
                //        actionResult = RedirectToAction("Index");
                //    }

                //    // Build the model
                //    StoreModel storeModel = null;
                //    if (actionResult == null)
                //    {
                //        storeModel = new StoreModel()
                //        {
                //            Store = store,
                //            StoreStatuses = storeStatuses,
                //            StoreStatusId = store.StoreStatus.Id,
                //            CountryId = store.Address.Country.Id,
                //            Countries = countries
                //        };
                //    }

                //    {
                //        actionResult = View(storeModel);
                //    }
                //}
                //else
                //{
                //    actionResult = RedirectToAction("Index");
                //}
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PaymentTypesController.Update", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        [HttpPost]
        [Security(Permissions = "UpdatePaymentProvider")]
        public ActionResult Update(StoreModel storeModel)
        {
            ActionResult actionResult = null;

            try
            {
              //  // Get a list of store statuses
              //  IList<AndroAdminDataAccess.Domain.StoreStatus> storeStatuses = null;
              //  if (actionResult == null)
              //  {
              //      storeStatuses = this.StoreStatusDAO.GetAll();

              //      if (storeStatuses == null)
              //      {
              //          actionResult = RedirectToAction("Index");
              //      }
              //  }

              //  // Get a list of countries
              //  List<AndroAdminDataAccess.Domain.Country> countries = null;
              //  if (actionResult == null)
              //  {
              //      countries = this.CountryDAO.GetAll();

              //      if (countries == null)
              //      {
              //          actionResult = RedirectToAction("Index");
              //      }
              //  }

              //  if (!ModelState.IsValid)
              //  {
              //      storeModel.StoreStatuses = storeStatuses; // The model returned by MVC no longer includes the store statuses
              //      storeModel.Countries = countries; // The model returned by MVC no longer includes the countries
              //      actionResult = View(storeModel);
              //  }

              //  // Has the store name already been used?
              //  if (actionResult == null)
              //  {
              //      Store existingStore = this.StoreDAO.GetByName(storeModel.Store.Name);
              //      if (existingStore != null && existingStore.Id != storeModel.Store.Id)
              //      {
              //          storeModel.StoreStatuses = storeStatuses; // The model returned by MVC no longer includes the store statuses
              //          ModelState.AddModelError("Store.Name", "Store name already used");
              //          actionResult = View(storeModel);
              //      }
              //  }

              //  // Has the store id already been used?
              //  if (actionResult == null)
              //  {
              //      Store existingStore = this.StoreDAO.GetByAndromedaId(storeModel.Store.AndromedaSiteId);
              //      if (existingStore != null && existingStore.Id != storeModel.Store.Id)
              //      {
              //          storeModel.StoreStatuses = storeStatuses; // The model returned by MVC no longer includes the store statuses
              //          ModelState.AddModelError("Store.AndromedaSiteId", "Store id already used");
              //          actionResult = View(storeModel);
              //      }
              //  }

              //  // Find the selected status
              //  StoreStatus storeStatus = null;
              //  if (actionResult == null)
              //  {
              //      foreach (StoreStatus checkStoreStatus in storeStatuses)
              //      {
              //          if (checkStoreStatus.Id == storeModel.StoreStatusId)
              //          {
              //              storeStatus = checkStoreStatus;
              //              break;
              //          }
              //      }
              //  }

              //  // Find the selected country
              //  Country country = null;
              //  if (actionResult == null)
              //  {
              //      foreach (Country checkCountry in countries)
              //      {
              //          if (checkCountry.Id == storeModel.CountryId)
              //          {
              //              //storeModel.Store.Address = new Address();
              //              //storeModel.Store.Address.Country = country;
              //              country = checkCountry;
              //              break;
              //          }
              //      }
              //  }

              //  if (actionResult == null)
              //  {
              //      Store store = this.StoreDAO.GetById(storeModel.Store.Id);
              ////      store.AndromedaSiteId = storeModel.Store.AndromedaSiteId;
              //      store.Name = storeModel.Store.Name;
              //      store.CustomerSiteId = storeModel.Store.CustomerSiteId;
              //      store.StoreStatus = storeStatus;
              //      store.Address.Country = country;

              //      // Update the store
              //      this.StoreDAO.Update(store);

              //      // Push the change out to the ACS servers
              //      string errorMessage = SyncHelper.ServerSync();

              //      // Success
              //      if (errorMessage.Length == 0)
              //      {
              //          TempData["message"] = "ApplicationStores successfully updated";
              //      }
              //      else
              //      {
              //          TempData["errorMessage"] = "Failed to update Application stores";
              //      }

              //      actionResult = RedirectToAction("Index");
              //  }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("PaymentTypesController.Update (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }
    }
}
