using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Services.Vouchers.Events
{
    public class InvalidCodeEventContext 
    {
        public string VoucherCode { get; set; }

        public SiteDomainModel Site { get; set; }

        public Voucher VoucherFound { get; set; }

        public Customer Customer { get; set; }
        
        /// <summary>
        /// Gets or sets the invalid reason... could even be not found
        /// </summary>
        /// <value>The invalid reason.</value>
        public string InvalidReason { get; set; }
    }

}