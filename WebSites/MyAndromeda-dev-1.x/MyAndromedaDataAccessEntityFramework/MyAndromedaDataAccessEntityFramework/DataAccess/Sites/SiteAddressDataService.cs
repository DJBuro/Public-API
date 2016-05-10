using MyAndromeda.Data.Model.AndroAdmin;
using Domain = MyAndromeda.Data.Domain;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MyAndromeda.Data.DataAccess.Sites;

namespace MyAndromeda.Data.DataAccess.Sites
{
    public class SiteAddressDataService : ISiteAddressDataService 
    {
        public SiteAddressDataService() 
        {
        }

        public Domain.AddressDomainModel GetSiteAddress(int storeId) 
        {
            Domain.AddressDomainModel address; 
            using (var dbContext = new AndroAdminDbContext()) 
            {
                var table = dbContext.Addresses;
                var query = table
                                 .Include(e => e.Country)
                                 .Where(e => e.Stores.Any(store => store.Id == storeId));

                var result = query.Single();

                address = new Domain.AddressDomainModel()
                {
                    Country = new Domain.CountryDomainModel()
                    {
                        CountryName = result.Country.CountryName,
                        Id = result.Country.Id,
                        ISO3166_1_alpha_2 = result.Country.ISO3166_1_alpha_2,
                        ISO3166_1_numeric = result.Country.ISO3166_1_numeric
                    },
                    County = result.County,
                    Dps = result.DPS,
                    Lat = result.Lat,
                    Locality = result.Locality,
                    Long = result.Long,
                    Org1 = result.Org1,
                    Org2 = result.Org2,
                    Org3 = result.Org3,
                    Postcode = result.PostCode,
                    Prem1 = result.Prem1,
                    Prem2 = result.Prem2,
                    Prem3 = result.Prem3,
                    Prem4 = result.Prem4,
                    Prem5 = result.Prem5,
                    Prem6 = result.Prem6,
                    RoadName = result.RoadName,
                    RoadNum = result.RoadNum,
                    State = result.State,
                    Town = result.Town
                };
            }

            return address;
        }

        public Domain.AddressDomainModel GetSiteAddressByExternalSiteId(string externalStoreId)
        {
            Domain.AddressDomainModel address;
            using (var dbContext = new AndroAdminDbContext())
            {
                var table = dbContext.Addresses;
                var query = table
                                 .Include(e => e.Country)
                                 .Where(e => e.Stores.Any(store => store.ExternalId == externalStoreId));

                var result = query.Single();

                address = new Domain.AddressDomainModel()
                {
                    Country = new Domain.CountryDomainModel()
                    {
                        CountryName = result.Country.CountryName,
                        Id = result.Country.Id,
                        ISO3166_1_alpha_2 = result.Country.ISO3166_1_alpha_2,
                        ISO3166_1_numeric = result.Country.ISO3166_1_numeric
                    },
                    County = result.County,
                    Dps = result.DPS,
                    Lat = result.Lat,
                    Locality = result.Locality,
                    Long = result.Long,
                    Org1 = result.Org1,
                    Org2 = result.Org2,
                    Org3 = result.Org3,
                    Postcode = result.PostCode,
                    Prem1 = result.Prem1,
                    Prem2 = result.Prem2,
                    Prem3 = result.Prem3,
                    Prem4 = result.Prem4,
                    Prem5 = result.Prem5,
                    Prem6 = result.Prem6,
                    RoadName = result.RoadName,
                    RoadNum = result.RoadNum,
                    State = result.State,
                    Town = result.Town
                };
            }

            return address;
        }
    }
}