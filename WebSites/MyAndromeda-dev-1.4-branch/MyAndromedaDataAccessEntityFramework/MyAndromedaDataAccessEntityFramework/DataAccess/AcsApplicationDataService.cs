using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;

namespace MyAndromedaDataAccessEntityFramework.DataAccess
{
    public interface IAcsApplicationDataService : IDependency 
    {
        ACSApplication Get(int id); 
    }
    
    public class AcsApplicationDataService : IAcsApplicationDataService
    {
        private readonly Model.AndroAdmin.AndroAdminDbContext dbContext;

        public AcsApplicationDataService(Model.AndroAdmin.AndroAdminDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public ACSApplication Get(int id)
        {
            var model = this.dbContext.ACSApplications.Find(id);

            return model;
        }
    }
}
