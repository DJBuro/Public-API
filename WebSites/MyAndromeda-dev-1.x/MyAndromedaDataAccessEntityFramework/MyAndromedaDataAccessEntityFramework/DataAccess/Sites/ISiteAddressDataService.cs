using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Sites
{
    public interface ISiteAddressDataService : IDependency 
    {
        Address GetSiteAddress(int storeId);

        Address GetSiteAddressByExternalSiteId(string externalStoreId);
    }
}