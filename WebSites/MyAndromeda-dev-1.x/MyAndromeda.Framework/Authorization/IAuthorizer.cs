using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Framework.Authorization
{
    public interface IAuthorizer : IDependency
    {
        ICurrentSite Site { get; }
        ICurrentChain Chain { get; }

        bool Authorize(Permission permission);
        bool AuthorizeAll(params Permission[] permission);
        bool AuthorizeAny(params Permission[] permission);
        //ChainAuthorization AuthorizedForChainAccess();
        //ChainAndSiteAuthorization AuthorizedForLocationSiteAndStore();
        ChainAndSiteAuthorization AuthorizedForChainAndStore();
    }

}
