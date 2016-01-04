using System.Configuration;
using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders.Context
{
    public class EmailConfiguration : ISingletonDependency
    {
        const string orderMonitoringEmailKey = "Myandromeda.Emails.OrderMonitoringDefault";
        
        public EmailConfiguration() 
        {
        
        }

        private string orderMonitoringEailToValue;

        public string OrderMonitoringEailToValue
        {
            get 
            {
                return orderMonitoringEailToValue ?? 
                    (orderMonitoringEailToValue = ConfigurationManager.AppSettings[orderMonitoringEmailKey]); 
            }
        }

    }
}
