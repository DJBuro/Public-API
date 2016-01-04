using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andromeda.WebOrdering.Model;

namespace Andromeda.WebOrdering.PaymentProviders
{
    public interface IPaymentProvider
    {
        string BuildPaymentRollbackFailEmailBody(OrderDetails orderDetails, out Email templateEmail);
        bool ProcessPayment(OrderDetails orderDetails);
        bool RollbackPayment(OrderDetails orderDetails, string reference);
    }
}
