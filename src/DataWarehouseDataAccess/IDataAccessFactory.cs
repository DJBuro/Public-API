﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataWarehouseDataAccess.DataAccess;

namespace DataWarehouseDataAccess
{
    public interface IDataAccessFactory
    {
        ICustomerDataAccess CustomerDataAccess { get; set; }
        ICustomerGPSDataAccess CustomerGPSDataAccess { get; set; }
        IPasswordResetRequestDataAccess PasswordResetRequestDataAccess { get; set; }
        IOrderDataAccess OrderDataAccess { get; set; }
        IVoucherDataAccess VoucherDataAccess { get; set; }
        IOrderMetricsDataAccess OrderMetricsDataAccess { get; set; }
        IFeedbackDataAccess FeedbackDataAccess { get; set; }
        ITelemetryDataAccess TelemetryDataAccess { get; set; }
    }
}
