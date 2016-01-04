using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataWarehouseDataAccess.DataAccess;
using DataWarehouseDataAccessEntityFramework;

namespace AndroAdmin.DataAccess
{
    public class DataWarehouseDataAccessFactory
    {
        public static IOrderMetricsDataAccess GetOrderMetricsDataAccess()
        {
            return new DataWarehouseDataAccessEntityFramework.DataAccess.OrderMetricsDataAccess();
        }
    }
}