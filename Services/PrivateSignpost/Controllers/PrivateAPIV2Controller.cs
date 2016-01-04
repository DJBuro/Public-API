using AndroCloudHelper;
using PrivateSignpost.Models;
using PrivateSignpost.Services;
using SignpostDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PrivateSignpost.Controllers
{
    public class PrivateAPIV2Controller : ApiController
    {
        [HttpGet]
        [ActionName("host")]
        public async Task<HttpResponseMessage> Host(string storeId, string licenseKey, string hardwareKey)
        {
            // EXAMPLE
            //<Hosts>
            //    <Host><Type>PrivateHubWebService</Type><Url>http://test1.androcloudservices.com/ACSHubWCFServicesV2/ACSHubServices/order/</Url><Version>1</Version><Order>1</Order></Host>
            //    <Host><Type>PrivateHubWebService</Type><Url>http://test1.androcloudservices.com/ACSHubWCFServicesV2/ACSHubServices/order/</Url><Version>2</Version><Order>2</Order></Host>
            //    <Host><Type>PrivateWebOrderingAPI</Type><Url>https://test1.androcloudservices.com/AndroCloudPrivateAPI/privateapi</Url><Version>1</Version><Order>1</Order></Host>
            //    <Host><Type>PrivateWebOrderingAPI</Type><Url>https://test1.androcloudservices.com/AndroCloudPrivateAPI/privateapiv2</Url><Version>2</Version><Order>2</Order></Host>
            //    <Host><Type>WarehouseRealtime</Type><Url>https://test1.androcloudservices.com/AndroCloudPrivateAPI/privateapiv2</Url><Version>2</Version><Order>1</Order></Host>
            //    <Host><Type>WebHooks - Update EDT</Type><Url>https://test.myandromeda.co.uk/web-hooks/store/update-estimated-delivery-time</Url><Version>1</Version><Order>0</Order></Host>
            //    <Host><Type>WebHooks - Update Menu</Type><Url>https://test.myandromeda.co.uk/web-hooks/store/update-menu</Url><Version>1</Version><Order>0</Order></Host>
            //    <Host><Type>WebHooks - Update Order Status</Type><Url>https://test.myandromeda.co.uk/web-hooks/store/orders/update-order-status</Url><Version>1</Version><Order>0</Order></Host>
            //    <Host><Type>WebHooks - Update Store Status</Type><Url>https://test.myandromeda.co.uk/web-hooks/store/update-status</Url><Version>1</Version><Order>0</Order></Host>
            //    <Host><Type>WebOrderingAPI</Type><Url>https://test1.androcloudservices.com/AndroCloudAPI/weborderapi</Url><Version>1</Version><Order>1</Order></Host>
            //    <Host><Type>WebOrderingAPI</Type><Url>https://test1.androcloudservices.com/AndroCloudAPI/weborderapiv2</Url><Version>2</Version><Order>2</Order></Host>
            //    <Host><Type>WebOrderingSignalR</Type><Url>http://test1.androcloudservices.com:8082/</Url><Version>2</Version><Order>1</Order></Host>
            //</Hosts>

            // Get submitted and wanted data types from the http header
            DataTypes dataTypes = AndroCloudHelper.Helper.GetDataTypes();

            // Go get the hosts
            ResponseDetails responseDetails = await HostServices.GetHostsV2Async(new SignpostDataAccess(), dataTypes, storeId, licenseKey, hardwareKey);

            // Return the hosts in the requested data type
            var response = this.Request.CreateResponse(responseDetails.httpStatusCode);
            response.Content = new StringContent(responseDetails.ResponseText, Encoding.UTF8, DataTypes.DataTypeToText(dataTypes.WantsDataType));
            return response;
        }

        [HttpGet]
        [ActionName("dataversion")]
        public async Task<HttpResponseMessage> GetDataVersion(string secretKey)
        {
            // Go get the data version
            ResponseDetails responseDetails = await SyncServices.GetDataVersionAsync(new SignpostDataAccess(), secretKey);

            // Return the current data version
            var response = this.Request.CreateResponse(responseDetails.httpStatusCode);
            response.Content = new StringContent(responseDetails.ResponseText, Encoding.UTF8, DataTypes.DataTypeToText(DataTypeEnum.JSON));
            return response;
        }

        [HttpPost]
        [ActionName("sync")]
        public async Task<HttpResponseMessage> Sync(string secretKey, [FromBody]Sync sync)
        {
            // Update the signpost server data
            ResponseDetails responseDetails = await SyncServices.SyncAsync(new SignpostDataAccess(), secretKey, sync);

            // Return the result of the sync
            var response = this.Request.CreateResponse(responseDetails.httpStatusCode);
            response.Content = new StringContent(responseDetails.ResponseText, Encoding.UTF8, DataTypes.DataTypeToText(DataTypeEnum.JSON));
            return response;
        }
    }
}
