using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering
{
    public class Errors
    {
        public const string UnhandledException = "-1000";
        public const string InitialiseMercuryPaymentFailed = "-1001";
        public const string MercuryPaymentFailed = "-1002";
        public const string InitialiseDatacashPaymentFailed = "-1003";
        public const string UnknownCommsError = "-1004";
        public const string DataCashPaymentFailed = "-1005";
        public const string MercanetPaymentFailed = "-1006";
        public const string InitialiseMercanetPaymentFailed = "-1007";
        public const string PaymentRolledBack = "-1008";
        public const string PaymentRolledBackFailed = "-1009";
        public const string PaypalPaymentFailed = "-1010";
    }
}