using System.Linq;
using MyAndromedaDataAccess.Domain.DataWarehouse;

namespace MyAndromeda.Data.DataWarehouse.Domain.Marketing
{
    public static class CustomerExtensions
    {
        public static string GetEmail(this CustomerModel customer) 
        {
            var contactDetail = customer.ContactDetails.FirstOrDefault(e => e.ContactType == ContactType.Email);

            return contactDetail == null ? string.Empty : contactDetail.Value;
        }
    }
}