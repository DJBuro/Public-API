using System.Collections.Generic;
using OrderTracking.Dao.Domain;
using System;

namespace OrderTracking.Dao
{
    public interface IClientWebsiteCustomContentDao : IGenericDao<ClientWebsiteCustomContent, int>
    {
        IList<ClientWebsiteCustomContent> FindByChainId(Int64 chainId);
        void DeleteAllForChain(Int64 chainId);
    }
}
