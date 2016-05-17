using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Services.Marketing.Models;

namespace MyAndromeda.Services.Marketing
{
    public class MarketingHistoryTrailService : IMarketingHistoryTrailService 
    {
        private readonly DataWarehouseDbContext dbContext;
        private readonly IMyAndromedaLogger logger;

        public MarketingHistoryTrailService(DataWarehouseDbContext dbContext, IMyAndromedaLogger logger)
        { 
            this.logger = logger;
            this.dbContext = dbContext;
            this.Emails = this.dbContext.Emails; 
        }

        public IDbSet<Email> Emails { get; set; }

        public IDbSet<EmailCategory> EmailCategory { get; set; }

        public async Task SentEmailsAsync(MarketingRecipientListMessage message)
        {
            foreach (var person in message.People) 
            {
                var entity = this.Emails.Create(); 
                
                this.Emails.Add(entity);
                
                entity.Id = Guid.NewGuid();
                entity.Subject = message.Template.Subject;
                entity.Content = message.Template.Html;
                entity.To = person.Email;
                entity.TimeStampUtc = DateTime.UtcNow;
                
                entity.AndromedaSiteId = message.AndromedaSiteId;
                entity.MarketingEventTypeName = message.MarketingCampaignType;

                entity.EmailCategories = message.Categories.Select(e => new EmailCategory()
                {
                    Name = e
                }).ToList();

                if (!person.CustomerId.HasValue)
                {
                    continue;
                }

                entity.CustomerId = person.CustomerId;
            }

            var recordsChanged = await this.dbContext.SaveChangesAsync();

            logger.Debug("Added historic records: " + recordsChanged);
        }
    }
}