using System;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;
using Ninject.Modules;
using Ninject.Web.Common;

namespace MyAndromeda.Data
{
    public class MyAndromedaDbModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<MyAndromedaDbContext>()
                .To<MyAndromedaDbContext>()
                .InRequestScope();
            this.Bind<AndroAdminDbContext>()
                .To<AndroAdminDbContext>()
                .InRequestScope();
        }
    }
}
