using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface ICustomerDao : IGenericDao<Customer, int>
    {
        IList<Customer> FindByStore(Store store);
        Customer FindByUserCredentials(string userCredentials, Store store);
        Customer FindByUserCredentialsOnly(string userCredentials);
    }
}
