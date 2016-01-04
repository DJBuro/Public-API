using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.SendGridService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService.Handlers
{
    public class CreatedTransactionalEmailHandler : ICreatedTransactinoalEmailEvent
    {
        private readonly IEmailHistoryDataService dataService;

        public CreatedTransactionalEmailHandler(IEmailHistoryDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<Email> CreatedTransactionalEmailAsync(
            SendGrid.SendGridMessage email,
            IEnumerable<string> categories,
            int andromedaSiteId)
        {
            var entity = this.dataService.Emails.Create();

            entity.Id = Guid.NewGuid();
            entity.TimeStampUtc = DateTime.UtcNow;
            entity.To = email.To.First().Address;
            entity.Subject = email.Subject;
            entity.AndromedaSiteId = andromedaSiteId;
            entity.Content = email.Html;

            foreach (var category in categories.Distinct())
            {
                entity.EmailCategories.Add(new Data.DataWarehouse.Models.EmailCategory() { Name = category });
            }

            this.dataService.Emails.Add(entity);

            await this.dataService.SaveAsync();

            return entity;
        }

        public async Task<Email> CreatedTransactionalOrderEmailAsync(SendGrid.SendGridMessage email, IEnumerable<string> categories, int andromedaSiteId, Guid? orderId = null, Guid? customerId = null)
        {
            var anyEntities = this.dataService.Emails.FirstOrDefault(e => e.OrderHeaderId == orderId);

            if (anyEntities != null)
            {
                return anyEntities;
            }

            var entity = this.dataService.Emails.Create();

            entity.Id = Guid.NewGuid();
            entity.TimeStampUtc = DateTime.UtcNow;
            entity.To = email.To.First().Address;
            entity.Subject = email.Subject;
            entity.AndromedaSiteId = andromedaSiteId;
            entity.Content = email.Html;

            if (orderId.HasValue)
            {
                entity.OrderHeaderId = orderId;
            }
            if (customerId.HasValue)
            {
                entity.CustomerId = customerId;
            }

            foreach (var category in categories.Distinct())
            {
                entity.EmailCategories.Add(new Data.DataWarehouse.Models.EmailCategory() { Name = category });
            }

            this.dataService.Emails.Add(entity);


            await this.dataService.SaveAsync();

            return entity;
        }
    }

}
