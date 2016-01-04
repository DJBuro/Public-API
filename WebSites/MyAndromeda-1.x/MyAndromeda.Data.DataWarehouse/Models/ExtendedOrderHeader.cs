using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Models
{
    public static class OrderHeaderExtensions
    {
        public static DateTime? TranslateToLocalDateTime(this OrderHeader header, Expression<Func<OrderHeader, DateTime?>> path) 
        {
            var delegateStatement = path.Compile();
            
            var dateTime = delegateStatement(header);

            if (dateTime.HasValue) 
            {
                var dateService = dateTime.Value.ToLocalTime();
            }

            return null;
        } 
    }
}
