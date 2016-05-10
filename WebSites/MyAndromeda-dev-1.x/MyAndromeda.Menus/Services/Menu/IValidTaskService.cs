using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Logging;

namespace MyAndromeda.Menus.Services.Menu
{
    public interface IValidTaskService : ITransientDependency
    {
        /// <summary>
        /// Checks that the tasks are valid. That none have cancelled in a fatal way such that it could not be defined to restart
        /// </summary>
        /// <param name="utcNow">The UTC now.</param>
        void CheckAndCorrectTasks(DateTime utcNow);
    }

}