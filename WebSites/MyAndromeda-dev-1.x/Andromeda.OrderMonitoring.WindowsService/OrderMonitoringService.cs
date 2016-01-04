using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using MyAndromeda.Services.Orders.OrderMonitoring.Services;
using MyAndromeda.Services.Orders.OrderMonitoring.Models;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Configuration;

namespace Andromeda.OrderMonitoring.WindowsService
{
    public partial class OrderMonitoringService : ServiceBase
    {
        EventLog eLog = new EventLog();
        public OrderMonitoringService()
        {
            InitializeComponent();
            this.AutoLog = false;
            if (!System.Diagnostics.EventLog.SourceExists("OrderMonitoring"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "OrderMonitoring", "Application");
            }
            eLog.Source = "OrderMonitoring";
        }

        protected override void OnStart(string[] args)
        {
            eLog.WriteEntry("In OnStart");
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["OrderMonitoringServiceIntervalInSeconds"]) * 1000; 
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            eLog.WriteEntry("Timer started with interval(ms): " + timer.Interval);
        }

        protected override void OnStop()
        {
            eLog.WriteEntry("In OnStop");
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            eLog.WriteEntry("Timer..");
            ProcessOrders();
        }

        public void ProcessOrders()
        {
            eLog.WriteEntry("In Process Orders");
            OrderMonitoringWindowsService serv = new OrderMonitoringWindowsService();
            double minutes = Convert.ToInt32(ConfigurationManager.AppSettings["OrderMonitoringMinutes"]);
            int bufferTimeInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["OrderMonitoringBufferTimeInMinutes"]);
            int status = Convert.ToInt32(ConfigurationManager.AppSettings["OrderMonitoringServiceStatus"]);

            List<Guid> list = serv.GetOrderIds(minutes, bufferTimeInMinutes, status);

            eLog.WriteEntry("Order Count: " + list.Count);
            if (list != null && list.Count > 0)
            {
                OrderList orderList = new OrderList();
                orderList.OrderIds = list;
                orderList.Created = DateTime.UtcNow;
                string postData = JsonConvert.SerializeObject(orderList);
                SendRequest(postData);
            }
        }

        private void SendRequest(string postData)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["OrderMonitoringService"]);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    eLog.WriteEntry("web-service Post Successful");
                }
            }
            catch (Exception ex)
            {
                eLog.WriteEntry("web-service Error: " + ex.Message);
            }
        }
    }
}
