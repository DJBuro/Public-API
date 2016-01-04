using System;
using System.Linq;
using MyAndromeda.Data.Model;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromedaDataAccess.DataAccess;

namespace MyAndromeda.Data.DataAccess
{
    public class AddressDataAccess : IAddressDataAccess
    {

        public string UpsertBySiteId(int siteId, Data.Domain.Address address)
        {
            using (var dbContext = new AndroAdminDbContext())
            {
                var query = from c in dbContext.Countries
                            where c.Id == address.CountryId
                            select c;

                var countryEntity = query.FirstOrDefault();

                var query2 = from s in dbContext.Stores
                             join a in dbContext.Addresses on s.AddressId equals a.Id
                             where s.Id == siteId
                             select a;

                Address addressEntity = query2.FirstOrDefault();

                // Insert or update?
                if (addressEntity == null)
                {
                    addressEntity = new Address();
                }

                // Make the changes
                addressEntity.Country = countryEntity;
                addressEntity.County = address.County;
                addressEntity.Locality = address.Locality;
                addressEntity.Org1 = address.Org1;
                addressEntity.Org2 = address.Org2;
                addressEntity.Org3 = address.Org3;
                addressEntity.PostCode = address.Postcode;
                addressEntity.Prem1 = address.Prem1;
                addressEntity.Prem2 = address.Prem2;
                addressEntity.Prem3 = address.Prem3;
                addressEntity.Prem4 = address.Prem4;
                addressEntity.Prem5 = address.Prem5;
                addressEntity.Prem6 = address.Prem6;
                addressEntity.RoadName = address.RoadName;
                addressEntity.RoadNum = address.RoadNum;
                addressEntity.State = address.State;
                addressEntity.Town = address.Town;
                addressEntity.DPS = address.Dps;

                var v = dbContext.GetNextDataVersionForEntity();
                addressEntity.DataVersion = v;

                foreach (var store in addressEntity.Stores) 
                {
                    store.DataVersion = v;
                }

                // Add a new address
                if (addressEntity == null)
                {
                    dbContext.Addresses.Add(addressEntity);
                }

                dbContext.SaveChanges();
            }

            return string.Empty;
        }
    }
}