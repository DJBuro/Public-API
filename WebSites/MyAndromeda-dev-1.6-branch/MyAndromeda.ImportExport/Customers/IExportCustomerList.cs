using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Domain.Marketing;

namespace MyAndromeda.ImportExport.Customers
{
    public interface IExportCustomerList : ITransientDependency
    {
        /// <summary>
        /// Gets the CSV from a customer list.
        /// </summary>
        /// <param name="customers">The customers.</param>
        /// <returns></returns>
        MemoryStream GetCsv(IEnumerable<CustomerModel> customers);
    }
}