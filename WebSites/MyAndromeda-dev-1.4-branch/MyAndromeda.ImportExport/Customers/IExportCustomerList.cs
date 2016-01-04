using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyAndromeda.Core;

namespace MyAndromeda.ImportExport.Customers
{
    public interface IExportCustomerList : ITransientDependency
    {
        /// <summary>
        /// Gets the CSV from a customer list.
        /// </summary>
        /// <param name="customers">The customers.</param>
        /// <returns></returns>
        MemoryStream GetCsv(IEnumerable<MyAndromedaDataAccess.Domain.Marketing.Customer> customers);
    }
}