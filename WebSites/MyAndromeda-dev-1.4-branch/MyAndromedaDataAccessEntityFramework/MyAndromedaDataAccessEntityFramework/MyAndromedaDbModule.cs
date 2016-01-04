using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Web.Common;

namespace MyAndromedaDataAccessEntityFramework
{
    public class MyAndromedaDbModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Model.MyAndromeda.MyAndromedaDbContext>().To<Model.MyAndromeda.MyAndromedaDbContext>()
                .InRequestScope();
            Bind<Model.AndroAdmin.AndroAdminDbContext>().To<Model.AndroAdmin.AndroAdminDbContext>()
                .InRequestScope();
        }
    }
}
