using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;

namespace MyAndromeda.Services.Vouchers.Events
{
    public interface IVoucherEventHandler : IDependency
    {
        void Created(VoucherUpdateEventContext context);
        void Creating(VoucherUpdateEventContext context);
        void Updated(VoucherUpdateEventContext context);
        void Updating(VoucherUpdateEventContext context);
        void Deleting(VoucherUpdateEventContext context);
        void Deleted(VoucherUpdateEventContext context);
    }

    public class VoucherUpdateEventContext
    {
        /// <summary>
        /// Gets or sets the cancel flag.
        /// </summary>
        /// <value>The cancel.</value>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the voucher.
        /// </summary>
        /// <value>The voucher.</value>
        public MyAndromeda.Data.DataWarehouse.Models.Voucher Voucher { get; set; }
    }
}
