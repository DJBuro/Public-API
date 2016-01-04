using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using PublicSignpost.Services;
using PublicSignpost.Models;
using System.Text;
using AndroCloudHelper;
using SignpostDataAccessLayer;

namespace PublicSignpost.Controllers
{
    public class WeborderAPIController : ApiController
    {
        [HttpGet]
        [ActionName("host")]
        public async Task<HttpResponseMessage> Host(string partnerId, string applicationId)
        {
            // Get submitted and wanted data types from the http header
            DataTypes dataTypes = Helper.GetDataTypes();

            // Go get the hosts
            ResponseDetails responseDetails = await HostServices.GetHostsV1Async(new SignpostDataAccess(), dataTypes, partnerId, applicationId);

            // Return the hosts in the requested data type
            var response = this.Request.CreateResponse(responseDetails.httpStatusCode);
            response.Content = new StringContent(responseDetails.ResponseText, Encoding.UTF8, DataTypes.DataTypeToText(dataTypes.WantsDataType));
            return response;
        }
    }
}
