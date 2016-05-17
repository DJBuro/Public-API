using System.Data.Entity;
using MyAndromeda.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.SendGridService
{
    public interface IEmailHistoryDataService : IDependency
    {
        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <value>The emails.</value>
        IDbSet<MyAndromeda.Data.DataWarehouse.Models.Email> Emails { get; }

        /// <summary>
        /// Gets the email history.
        /// </summary>
        /// <value>The email history.</value>
        IDbSet<EmailHistory> EmailHistory { get; }

        /// <summary>
        /// Saves the async.
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }
}
