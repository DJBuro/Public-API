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
    public interface IRESTServicesV3
    {
        [OperationContract]
        Stream GetSiteDetails3(string siteId, string applicationId);
    }
}