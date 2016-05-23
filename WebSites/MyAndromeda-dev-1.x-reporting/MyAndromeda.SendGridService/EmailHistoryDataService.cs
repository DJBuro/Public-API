using System.Data.Entity;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.SendGridService
{
    public class EmailHistoryDataService : IEmailHistoryDataService 
    {
        private readonly DataWarehouseDbContext dbContext;

        public EmailHistoryDataService(DataWarehouseDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IDbSet<MyAndromeda.Data.DataWarehouse.Models.Email> Emails
        {
            get
            {
                return this.dbContext.Emails;
            }
        }

        public IDbSet<EmailHistory> EmailHistory
        {
            get
            {
                return this.dbContext.EmailHistories;
            }
        }

        public async Task SaveAsync() 
        {
            await this.dbContext.SaveChangesAsync();
        }
    }
}