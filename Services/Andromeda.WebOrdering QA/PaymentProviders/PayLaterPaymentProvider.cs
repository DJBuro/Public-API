using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Andromeda.WebOrdering.Model;

namespace Andromeda.WebOrdering.PaymentProviders
{
    public class PayLaterPaymentProvider : IPaymentProvider
    {
        #region IPaymentProvider

        public string BuildPaymentRollbackFailEmailBody(OrderDetails orderDetails, out Email templateEmail)
        {
            templateEmail = null;
            return "";
        }
        public bool ProcessPayment(Model.OrderDetails orderDetails)
        {
            // Default to pay at door
            orderDetails.Payment = new Payment()
            {
                PaymentType = "PayLater",
                Value = ((decimal)orderDetails.OrderElement["pricing"]["priceAfterDiscount"]).ToString(),
                PaytypeName = null,
                AuthCode = null,
                LastFourDigits = null,
                CVVStatus = null,
                PayProcessor = null,
                PSPSpecificDetails = null
            };

            return true;
        }
        public bool RollbackPayment(Model.OrderDetails orderDetails, string reference)
        {
            return false;
        }

        #endregion IPaymentProvider
    }
}