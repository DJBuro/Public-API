using MyAndromeda.CloudSynchronization.Services;
using MyAndromeda.Data.Domain;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Logging;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Areas.Store.Models;
using MyAndromeda.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Store.Controllers
{
    [MyAndromedaAuthorize]
    public class OpeningHoursController : Controller
    {
        private readonly IStoreOpeningTimesService storeOpeningTimesService;

        private readonly IMyAndromedaLogger logger;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;
        private readonly ITranslator translator;

        public OpeningHoursController(IStoreOpeningTimesService storeOpeningTimesService, INotifier notifier, IAuthorizer authorizer, IMyAndromedaLogger logger, ITranslator translator)
        {
            this.translator = translator;
            this.logger = logger;
            this.storeOpeningTimesService = storeOpeningTimesService;
            this.notifier = notifier;
            this.authorizer = authorizer;
        }

        public ActionResult Index()
        {
            if (!authorizer.Authorize(StoreUserPermissions.EditOpeningTimes)) 
            { 
                return new HttpUnauthorizedResult();
            }

            var model = new OpeningTimesViewModel() 
            {
                OpeningHours = this.storeOpeningTimesService.GetOpenTimes()
            };

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ListOpeningHours() 
        {
            var model = new OpeningTimesViewModel()
            {
                OpeningHours = this.storeOpeningTimesService.GetOpenTimes()
            };

            return PartialView(model);
        }

        public ActionResult UpdateTimes(OpeningTimesViewModel viewModel) 
        {
            if (!authorizer.Authorize(StoreUserPermissions.EditOpeningTimes)) 
            { 
                return new HttpUnauthorizedResult();
            }

            if (!viewModel.AddOpeningStartTime.HasValue) 
            {
                this.ModelState.AddModelError("AddOpeningStartTime", "Requires a value");
            }

            if (!viewModel.AddOpeningEndTime.HasValue) 
            {
                this.ModelState.AddModelError("AddOpeningEndTime", "Requires a value");
            }

            if (!ModelState.IsValid) 
            {
                viewModel.OpeningHours = this.storeOpeningTimesService.GetOpenTimes();

                return this.View("Index", viewModel);
            }

            // Parse the start time
            var addOpeningStartTime = viewModel.AddOpeningStartTime.Value;
            TimeSpan startTimeSpan = new TimeSpan(addOpeningStartTime.Hour, addOpeningStartTime.Minute, addOpeningStartTime.Second);

            // Parse the end time
            var addOpeningEndTime = viewModel.AddOpeningEndTime.Value;
            TimeSpan endTimeSpan = new TimeSpan(addOpeningEndTime.Hour, addOpeningEndTime.Minute, addOpeningEndTime.Second);

            this.storeOpeningTimesService.AddOpeningTime(viewModel.Day, startTimeSpan, endTimeSpan, this.ModelState);

            if (ModelState.IsValid)
            {
                this.notifier.Notify(translator.T("Store opening time has been added"));
            }
            else 
            {
                viewModel.OpeningHours = this.storeOpeningTimesService.GetOpenTimes();

                this.notifier.Error("The selected hours could not be added");
                return this.View("Index", viewModel);
            }

            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateAllDay(OpeningTimesViewModel viewModel) 
        {
            if (!authorizer.Authorize(StoreUserPermissions.EditOpeningTimes))
            {
                return new HttpUnauthorizedResult();
            }

            try 
            {
                this.storeOpeningTimesService.DeleteAllTimesForDay(viewModel.Day);

                if (viewModel.OpenAllDay) 
                { 
                    TimeSpanBlock timeSpanBlock = new TimeSpanBlock();
                    timeSpanBlock.Day = viewModel.Day;
                    timeSpanBlock.StartTime = "";
                    timeSpanBlock.EndTime = "";
                    timeSpanBlock.OpenAllDay = true;

                    this.storeOpeningTimesService.AddOpeningTime(timeSpanBlock);
                }
            }
            catch (Exception e) 
            {
                this.logger.Error("Failed updating the open all day field", e);
                string message = string.Format("Could not set the open all day value for {0}", viewModel.Day);
                
                this.notifier.Error(translator.T(message));
            }

            return RedirectToAction("Index");
        }


        public ActionResult DeleteOpeningHour(int openingHoursId) 
        {
            if (!authorizer.Authorize(StoreUserPermissions.EditOpeningTimes))
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                this.storeOpeningTimesService.DeleteOpeningTime(openingHoursId);

                this.notifier.Notify(translator.T("Store opening time has been removed"));
            }
            catch (Exception e) 
            {
                this.logger.Error("Failed removing the opening time.", e);
                this.notifier.Error(translator.T("There was an error removing the time from the database. Please try again."));
            }

            return RedirectToAction("Index");
        }

    }
}