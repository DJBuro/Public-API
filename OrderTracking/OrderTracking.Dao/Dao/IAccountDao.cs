using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface IAccountDao : IGenericDao<Account, int>
    {
        bool Verify(string userName, string password, out Account account);
        Account FindByStoreRamesesId(string storeId);
    }
}
