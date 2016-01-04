using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsOrders.Model;

namespace MyAndromeda.Data.AcsOrders.Services
{
    public interface IAcsOrdersDbContext : IDependency
    {
        AcsOrderDbContext GetContext();
    }

    public class AcsOrdersDbContext : IAcsOrdersDbContext 
    {
        private readonly AcsOrderDbContext dbContext;

        public AcsOrdersDbContext() 
        {
            this.dbContext = new Model.AcsOrderDbContext();
        }

        public AcsOrderDbContext GetContext()
        {
            return this.dbContext;
        }
    }
}
