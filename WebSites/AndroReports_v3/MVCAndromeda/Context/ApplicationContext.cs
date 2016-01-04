using MVCAndromeda.Models;
using MyAndromeda.Core;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MVCAndromeda.Context
{
    public class ApplicationContext : IDependency
    {
        private readonly ICurrentUser currentUser;
        private readonly ISiteDataService siteDataService;

        private readonly RouteData routeData;

        public ApplicationContext(ICurrentUser currentUser, ISiteDataService siteDataService, RouteData routeData) 
        {
            this.currentUser = currentUser;
            this.siteDataService = siteDataService;
            this.routeData = routeData;
        }

        public string FirstName 
        {
            get 
            {
                return 
                    this.currentUser.Available ? this.currentUser.User.Firstname : String.Empty;
            }
        }

        List<string> _chains;
        public List<string> Chains 
        {
            get 
            {
                if(_chains != null){ return _chains; }
                _chains = this.currentUser.FlattenedChains.Select(e=> e.Name).ToList();

                return _chains;
            }
        }

        public DateTime Date 
        {
            get 
            {
                if (!routeData.Values.Any(e => e.Key.Equals("year", StringComparison.CurrentCultureIgnoreCase)))
                    return DateTime.Today;

                int year = Convert.ToInt32(routeData.GetRequiredString("year"));
                var month = Convert.ToInt32(routeData.GetRequiredString("month"));
                var day = Convert.ToInt32(routeData.GetRequiredString("day"));

                return new DateTime(year, month, day);
            }
        }

        public string ChosenCountry 
        {
            get 
            {
                if (!routeData.Values.Any(e => e.Key.Equals("chosenCountry", StringComparison.CurrentCultureIgnoreCase)))
                    return null;

                return routeData.GetRequiredString("chosenCountry");
            }
        }

        Owner _owner;
        public Owner Owner 
        {
            get 
            {
                if (_owner != null) { return _owner; } 

                _owner = new Owner(this.FirstName, this.ChosenCountry, this.CountryAndStores);

                return _owner;
            }
        }

        Dictionary<string, List<string>> _countryAndStores;
        public Dictionary<string, List<string>> CountryAndStores 
        {
            get 
            {
                if(_countryAndStores != null) { return _countryAndStores; }

                var chainIds = this.currentUser.FlattenedChains.Select(e => e.Id).ToArray();
                var storething = this.siteDataService.ListAndTransform(site => chainIds.Any(e => e == site.ChainId), (e) => new
                {
                    e.Name,
                    e.Address.Country.CountryName
                });

                _countryAndStores = storething.GroupBy(e => e.CountryName)
                    .ToDictionary(e => e.Key, e => e.Select(store => store.Name).ToList());

                //_countryAndStores = CubeAdapter.GetCountriesAndStores(this.Chains);

                return _countryAndStores;
            }
        }
    }
}