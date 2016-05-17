using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs.Models
{
    public class CustomerResultModel 
    {
        public long CustomerNumber { get; set; }
        public string FirstName { get; set; }
        public List<CustomerDeliveryAddressResultModel> DeliveryAddresses { get; set; }
       
    }
}

