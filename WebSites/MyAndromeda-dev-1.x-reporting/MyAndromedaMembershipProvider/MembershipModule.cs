using System;
using System.Linq;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Ninject.Extensions.MyAndromeda
{
    public class MembershipModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<MyAndromedaMembershipProvider.MyAndromedaMembershipProvider>()
                .To<MyAndromedaMembershipProvider.MyAndromedaMembershipProvider>().InRequestScope();
        }
    }
}