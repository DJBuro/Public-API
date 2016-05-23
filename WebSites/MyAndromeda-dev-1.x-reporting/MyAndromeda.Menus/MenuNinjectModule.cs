using System;
using System.Linq;
using MyAndromeda.Menus.Ftp;
using Ninject;
using Ninject.Modules;

namespace MyAndromeda.Menus
{
    public class MenuNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind<Lazy<IMenuFtpService>>().ToMethod((context) =>
            {
                return new Lazy<IMenuFtpService>(() =>
                {
                    return context.Kernel.Get<IMenuFtpService>();
                });
            }).InTransientScope();
        }
    }
}
