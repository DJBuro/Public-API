using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface IClientDao : IGenericDao<Client, int>
    {
    }
}
