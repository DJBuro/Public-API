using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Services;
using MyAndromeda.Framework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.WebApiServices.Services
{
    public interface IOrderService : IDependency
    {
        Postal.Email CreateEmail(int orderId, Models.GprsPrinterIbacs model);

        void UpdateOrderStatus(int orderId, bool success, string msg);
    }
}