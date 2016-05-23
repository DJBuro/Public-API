using System;
using System.Data.Entity;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService
{
    public interface IMarketingTemplateDataService : IDependency 
    {
        IDbSet<MyAndromeda.Data.Model.AndroAdmin.Address> Addresses { get; }

        IDbSet<MarketingEventCampaign> MarketingEventCampaigns { get; }
        IDbSet<MarketingContact> MarketingContacts { get; }
        
        Task SaveAsync(); 
    }

    public class MarketingTemplateDataService : IMarketingTemplateDataService 
    {
        private readonly MyAndromedaDbContext dbContext;
        private readonly MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androadminDbContext; 

        public MarketingTemplateDataService(MyAndromedaDbContext dbContext, MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androadminDbContext) 
        {
            this.androadminDbContext = androadminDbContext;
            this.dbContext = dbContext;

            this.MarketingContacts = dbContext.MarketingContacts;
            this.MarketingEventCampaigns = dbContext.MarketingEventCampaigns;

            this.Addresses = androadminDbContext.Addresses;
        }

        public IDbSet<Address> Addresses
        {
            get;
            private set;
        }

        public IDbSet<MarketingEventCampaign> MarketingEventCampaigns { get;private set; }

        public IDbSet<MarketingContact> MarketingContacts
        {
            get;
            private set;
        }

        public async Task SaveAsync()
        {
            await this.dbContext.SaveChangesAsync();
        }
    }
}