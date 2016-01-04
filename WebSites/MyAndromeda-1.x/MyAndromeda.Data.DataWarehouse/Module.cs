using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Ninject.Web.Common;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse
{
    public class DatawarehouseModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DataWarehouseEntities>().To<DataWarehouseEntities>()
                .InRequestScope();
        }
    }
}
