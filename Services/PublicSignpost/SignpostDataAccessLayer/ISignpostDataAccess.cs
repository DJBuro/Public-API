using SignpostDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignpostDataAccessLayer
{
    public interface ISignpostDataAccess
    {
        string GetServicesByApplicationId(string applicationId, out List<Models.HostV2> hosts);
        string GetServicesBySiteId(int andromedaSiteId, out List<Models.HostV2> hosts);
        string GetLegacyPrivateHostsBySiteId(int andromedaSiteId, out List<Models.HostV1> hosts);
        string GetLegacyPublicHostsBySiteId(int andromedaSiteId, out List<Models.HostV2> hosts);
        string GetDataVersion(out int version);
        string AddUpdateACSApplications(int fromVersion, int toVersion, List<SignpostDataAccessLayer.Models.ACSApplication> acsApplications);
    }
}
