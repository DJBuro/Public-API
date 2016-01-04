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
    public class PrivateAPIController : ApiController
    {
        [HttpGet]
        [ActionName("host")]
        public async Task<HttpResponseMessage> Host(string storeId, string licenseKey, string hardwareKey)
        {
            // EXAMPLE
            //<Hosts>
            //    <Host>
            //        <Url>https://test1.androcloudservices.com/AndroCloudPrivateAPI/privateapi</Url>
            //        <Order>1</Order>
            //        <SignalRUrl>http://test1.androcloudservices.com:8081</SignalRUrl>
            //    </Host>
            //</Hosts>

            // Get submitted and wanted data types from the http header
            DataTypes dataTypes = AndroCloudHelper.Helper.GetDataTypes();

            // Go get the hosts
            ResponseDetails responseDetails = await HostServices.GetHostsV1Async(new SignpostDataAccess(), dataTypes, storeId, licenseKey, hardwareKey);

            // Return the hosts in the requested data type
            var response = this.Request.CreateResponse(responseDetails.httpStatusCode);
            response.Content = new StringContent(responseDetails.ResponseText, Encoding.UTF8, DataTypes.DataTypeToText(dataTypes.WantsDataType));
            return response;
        }
    }
}
