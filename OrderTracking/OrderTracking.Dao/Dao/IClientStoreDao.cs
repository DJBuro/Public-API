using System.Collections.Generic;
using OrderTracking.Dao.Domain;
using System;

namespace OrderTracking.Dao
{
    public interface IClientStoreDao : IGenericDao<ClientStore, int>
    {
        IList<ClientStore> FindByChainId(Int64 chainId);
        IList<ClientStore> FindByStoreId(Int64 storeId);
        void DeleteByStoreId(Int64 storeId);
    }
}
