using System.Linq;
using MyAndromeda.Data.DataAccess.Chains;
using Domain = MyAndromeda.Data.Domain;
using System.Collections.Generic;
using System.Web.Http.Routing;

namespace MyAndromeda.Framework.Contexts
{
    public class CurrentChain : ICurrentChain 
    {
        private readonly ICurrentRequest currentRequest;

        private readonly ICurrentUser currentUser;
        private readonly IChainDataService chainDataService;

        public CurrentChain(ICurrentRequest currentRequest, ICurrentUser currentUser, IChainDataService chainDataService) 
        { 
            this.currentRequest = currentRequest;
            this.chainDataService = chainDataService;
            this.currentUser = currentUser;
        }

        private Domain.Site[] sitesBelongingToChain;

        public Domain.Site[] SitesBelongingToChain
        {
            get
            {
                if (!this.Available)
                    return new Domain.Site[0];

                return sitesBelongingToChain ?? (sitesBelongingToChain = chainDataService.GetSiteList(this.Chain.Id).ToArray());
            }
        }

        public bool Available
        {
            get
            {
                return this.Chain != null;
            }
        }

        private bool? authorizedAtChainLevel;

        public bool AuthorizedAtChainLevel
        {
            get
            {
                if (!authorizedAtChainLevel.HasValue)
                { 
                    LoadChain();       
                }
                          
                return authorizedAtChainLevel.GetValueOrDefault();
            }
        }

        private Domain.Chain chain;

        public Domain.Chain Chain
        {
            get
            {
                return chain ?? LoadChain();
            }
        }

        private Domain.Chain LoadChain() 
        {
            if (!this.currentUser.Available)
                return null; //not going to bother loading a chain for a context that is not logged in 
              
            if (!this.currentRequest.Available)
                return null; //no chain data to load from

            var chainIdValue = this.currentRequest.GetRouteData("ChainId");

            if (chainIdValue == null || string.IsNullOrWhiteSpace(chainIdValue.ToString()))
            {
                IDictionary<string, object> allValues = this.currentRequest.RouteData.Values;
                var webApiParameterRoutes = allValues
                    .Where(e => e.Value is IEnumerable<IHttpRouteData>)
                    .Select(e => e.Value as IEnumerable<IHttpRouteData>)
                    .SelectMany(e => e)
                    .SelectMany(e => e.Values)
                    .ToArray();

                var webapiChainId = webApiParameterRoutes.FirstOrDefault(e => e.Key.Equals("ChainId", System.StringComparison.InvariantCultureIgnoreCase));


                if(!string.IsNullOrWhiteSpace(webapiChainId.Value.ToString()))
                {
                    chainIdValue = webapiChainId.Value;
                }

                if(chainIdValue == null)
                {
                    return null;
                }
            }
            var chainIdString = chainIdValue.ToString();
            if (string.IsNullOrWhiteSpace(chainIdString))
            {
                return null;
            }

            int chainId;
            int.TryParse(chainIdString, out chainId);

            var userChains = this.currentUser.FlattenedChains; //userChainsDataService.GetChainsForUser(this.currentUser.User.Id);
            var chainBelongingToUser = userChains.FirstOrDefault(e => e.Id == chainId);
            if (chainBelongingToUser != null) 
            {
                authorizedAtChainLevel = true;
                this.chain = chainBelongingToUser;
                  
                return chainBelongingToUser;
            }

            //load chain and flag it as not an owner. 
            var chain = chainDataService.Get(chainId);
            this.authorizedAtChainLevel = false;
              
            return chain;
        }
    }
}