using MyAndromeda.Core;

namespace MyAndromeda.Services.Vouchers.Events
{
    public interface IInvalidCodeEventHandler : IDependency 
    {
        void InvalidCodeEntered(InvalidCodeEventContext context);
    }

    public class InvalidCodeEventContext 
    {
        public string VoucherCode { get; set; }

        public MyAndromeda.Data.Domain.Site Site { get; set; }

        public MyAndromeda.Data.DataWarehouse.Models.Voucher VoucherFound { get; set; }

        public MyAndromeda.Data.DataWarehouse.Models.Customer Customer { get; set; }
        
        /// <summary>
        /// Gets or sets the invalid reason... could even be not found
        /// </summary>
        /// <value>The invalid reason.</value>
        public string InvalidReason { get; set; }
    }
}