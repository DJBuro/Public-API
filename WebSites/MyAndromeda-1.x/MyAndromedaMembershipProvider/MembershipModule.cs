using System;
using System.Linq;
using Ninject.Modules;

namespace Ninject.Extensions.MyAndromeda
{
    public class MembershipModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<MyAndromedaMembershipProvider.MyAndromedaMembershipProvider>().To<MyAndromedaMembershipProvider.MyAndromedaMembershipProvider>();
        }
    }
}