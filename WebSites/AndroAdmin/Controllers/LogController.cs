using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.Helpers;
using System.Web.Mvc;

namespace AndroAdmin.Controllers
{
    [Authorize]
    public class LogController : BaseController
    {
        public ActionResult Index()
        {
            ActionResult actionResult = null;

            try
            {
                // Get all the logs
                IEnumerable<AndroAdminDataAccess.Domain.Log> logs = this.LogDAO.GetAll();

                actionResult = View(logs);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("LogController.Index", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }
    }
}