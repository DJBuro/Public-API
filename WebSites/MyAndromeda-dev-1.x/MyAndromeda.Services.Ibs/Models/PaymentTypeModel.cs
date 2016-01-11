using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs.Models
{
    public class PaymentTypeModel
    {

        public bool OtherCurrency { get; set; }

        public IbsWebOrderApi.eMediaType MediaType { get; set; }

        public long MediaNumber { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }
    }
}
