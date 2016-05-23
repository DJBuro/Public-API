using Microsoft.AspNet.Identity;
using Ninject.Modules;
using Ninject.Web.Common;

namespace MyAndromeda.Identity
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUserStore<MyAndromedaIdentityUser, int>>()
                .To<UserStore>()
                .InRequestScope();
        }
    }
}
