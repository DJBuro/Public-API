using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;

namespace AndroCloudWCFServices
{
    [ServiceContract]
    public interface IRESTServices
    {
        [OperationContract]
        Stream Ping();

        [OperationContract]
        Stream GetHost(string partnerId, string applicationId);

        [OperationContract]
        Stream GetMenu(string siteId, string partnerId, string applicationId, string version);

        [OperationContract]
        Stream GetSite(string partnerId, string groupIdFilter, string maxDistanceFilter, string longitudeFilter, string latitudeFilter, string applicationId);

        [OperationContract]
        Stream GetSiteDetails(string siteId, string partnerId, string applicationId);

        [OperationContract]
        Task<Stream> PutOrder(Stream input, string siteId, string orderId, string partnerId, string applicationId);

        [OperationContract]
        Stream GetOrder(string siteId, string orderId, string partnerId, string applicationId);
    }
}