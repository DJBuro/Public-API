using System;
using System.Linq;
using Acs.Tests.Devices.Services;

namespace Acs.Tests.Devices
{
    class Program
    {

        //const string DeviceName = "iBT8000";

        static void Main(string[] args)
        {
            ISiteDevicesService service = new Services.SiteDevicesService();

            Console.WriteLine("=======================================================");
            Console.WriteLine("==================-- ACS Values --====================="); 
            Console.WriteLine("=======================================================");
                    
            using(var dbContext = new Model.Entities())
            {
                var sites = dbContext.Sites;

                foreach (var site in sites)
                {
                    Console.WriteLine("=======================================================");
                    Console.WriteLine();

                    Console.WriteLine("AndroId: {0}", site.AndroID);
                    Console.WriteLine("External Site Name: {0}", site.ExternalSiteName);
                    Console.WriteLine();
                    bool gprsDevice = service.ContainsAnyDevice(site.ID);

                    foreach (var device in site.SiteDevices) 
                    {

                        Console.WriteLine("Device Name: {0}", device.Device.Name);
                    }

                    Console.WriteLine(string.Format("Contains a device: {0}", gprsDevice));
                    
                    if (!gprsDevice)
                        continue;

                    dynamic settings = service.GetSettings(site.ID, "");

                    foreach (dynamic setting in settings) 
                    {

                        Console.WriteLine("Property: {0}", setting.Key);
                        Console.WriteLine("Value   : {0}", setting.Value);

                    }

                    Console.WriteLine();
                    Console.WriteLine("=======================================================");

                    //var ibacsSettings = new 
                    //{ 
                    //    PrinterId = settings.printer_id,
                    //    Currency = settings.currency
                    //};

                    //Console.WriteLine("printer id: {0}", ibacsSettings.PrinterId);
                    //Console.WriteLine("currency: {0}", ibacsSettings.Currency);
                }
            }

            Console.ReadLine();
            
        }
    }
}
