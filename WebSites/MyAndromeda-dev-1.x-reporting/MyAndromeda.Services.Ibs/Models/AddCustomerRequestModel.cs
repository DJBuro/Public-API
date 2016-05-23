using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Services.Ibs.IbsWebOrderApi;
using System.Linq;

namespace MyAndromeda.Services.Ibs.Models
{
    public class AddCustomerRequestModel 
    { 
        public string Email { get; set; }

        public MyAndromeda.Services.Ibs.IbsWebOrderApi.cLoyaltyCardRec LoyaltyCard { get; private set; }

        public MyAndromeda.Services.Ibs.IbsWebOrderApi.cUpdateCustomerRec Customer { get; set; }
    }
}