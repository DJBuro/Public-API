using MyAndromeda.Core;
using MyAndromeda.Services.Ibs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs
{
    public interface IGetPaymentTypes : IDependency
    {
        Task<IEnumerable<PaymentTypeModel>> List(
            int andromedaSiteId,
            TokenResult token,
            LocationResult location);
    }
}
