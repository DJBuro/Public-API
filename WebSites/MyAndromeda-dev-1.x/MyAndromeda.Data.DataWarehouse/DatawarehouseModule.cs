using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Ninject.Web.Common;
using MyAndromeda.Data.DataWarehouse.Models;
using Ninject;
using Ninject.Planning.Bindings;

namespace MyAndromeda.Data.DataWarehouse
{


    public class DatawarehouseModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DataWarehouseDbContext>()
                .To<DataWarehouseDbContext>()
                .InRequestScope();

            //create a readonly strategy ... to talk to the secondary database for reporting. 
            Bind<DataWarehouseDbContext>()
                .ToMethod((c) => {
                    var dataContext = new DataWarehouseDbContext();
                    dataContext.Configuration.AutoDetectChangesEnabled = false;

                    return dataContext;
                })
                .InRequestScope()
                //see ReadOnlyDataAttribute
                .WithMetadata(key: "Readonly", value: true);
        }
    }
}
