using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.SendGridService.Events
{
    public interface ICreatedTransactinoalEmailEvent : IEventContext
    {
        /// <summary>
        /// Event - Created the transactional email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<Email> CreatedTransactionalEmailAsync(
            SendGrid.SendGridMessage email,
            IEnumerable<string> categories, 
            int andromedaSiteId);

        /// <summary>
        /// Event - Created the transactional order email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="orderId">The order id.</param>
        /// <returns></returns>
        Task<Email> CreatedTransactionalOrderEmailAsync(
            SendGrid.SendGridMessage email, 
            IEnumerable<string> categories, 
            int andromedaSiteId, 
            Guid? orderId = null, 
            Guid? customerId = null);


    }

}
