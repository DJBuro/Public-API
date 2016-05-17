using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Data.DataWarehouse.Services;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Services.Orders.Events;
using System.Collections.Generic;

namespace MyAndromeda.Services.Orders
{
    public interface IOrderEmailingService : IDependency
    {
        Postal.Email CreateEmail(string externalSiteId, int ramesesOrderId, string message, string deliveryTime, bool success);
    }

}
