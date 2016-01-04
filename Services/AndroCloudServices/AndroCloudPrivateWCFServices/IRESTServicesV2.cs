using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;

namespace AndroCloudPrivateWCFServices
{
    [ServiceContract]
    public interface IRESTServicesV2
    {
        [OperationContract]
        Stream GetHosts(string siteId, string licenseKey, string hardwareKey);

        [OperationContract]
        Task<Stream> PostMenu(Stream input, string siteId, string licenseKey, string hardwareKey, string version);

        [OperationContract]
        Task<Stream> PutOrder(Stream input, string siteId, string orderId, string licenseKey, string hardwareKey);

        [OperationContract]
        Task<Stream> PostOrder(Stream input, string siteId, string orderId, string licenseKey, string hardwareKey);

        [OperationContract]
        Task<Stream> PostSite(Stream input, string siteId, string licenseKey, string hardwareKey);

        [OperationContract]
        Stream GetDataVersion(string key);

        [OperationContract]
        Stream PostSync(Stream input, string key);
    }
}