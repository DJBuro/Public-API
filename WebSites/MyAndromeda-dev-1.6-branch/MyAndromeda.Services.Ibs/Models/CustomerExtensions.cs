using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Services.Ibs.IbsWebOrderApi;

namespace MyAndromeda.Services.Ibs.Models
{
    public static class CustomerExtensions 
    {
        internal static cUpdateCustomerRec Transform(this Customer customer) 
        {
            var email = 
                customer.Contacts.FirstOrDefault(e => e.ContactTypeId == 0);
            var phone = 
                customer.Contacts.FirstOrDefault(e => e.ContactTypeId == 1);
           
            var model = new cUpdateCustomerRec()
            {
                m_eAccountStatus = eAccountStatus.eUnverified,
                m_iDOBDay = 0,
                m_iDOBMonth = 0,
                m_iDOBYear = 0,
                m_szAddress1 = customer.Address == null 
                    ? "Unknown" 
                    : customer.Address.RoadNum,
                m_szAddress2 = customer.Address == null 
                    ? "Unknown"
                    : customer.Address.RoadName,
                m_szAddress3 = customer.Address == null 
                    ? "Unknown"
                    : customer.Address.Town,
                m_szAddress4 = 
                customer.Address == null 
                    ? "Unknown"
                    : customer.Address.State,
                //m_szBuildingName = customer.Address.b
                //
                //m_szContact = phone.Value,
                m_szEmail = email == null 
                    ? "unknown@unknown.com" 
                    : email.Value,
                m_szFirstName = customer.FirstName,
                m_szLastName = customer.LastName,
                m_szMobile = phone == null 
                    ? "0123456789"
                    : phone.Value,
                m_szPhone = phone.Value,
                m_szPostCode = customer.Address == null
                    ? ""
                    : customer.Address.PostCode,
                m_szTitle = customer.Title,
                //m_szWebPassword
            };

            return model;
        }
    }
}