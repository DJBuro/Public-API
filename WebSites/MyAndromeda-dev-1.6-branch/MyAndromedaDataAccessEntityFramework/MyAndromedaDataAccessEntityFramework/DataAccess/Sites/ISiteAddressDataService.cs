using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Data.DataAccess.Sites
{
    public interface ISiteAddressDataService : IDependency 
    {
        AddressDomainModel GetSiteAddress(int storeId);

        AddressDomainModel GetSiteAddressByExternalSiteId(string externalStoreId);
    }
}