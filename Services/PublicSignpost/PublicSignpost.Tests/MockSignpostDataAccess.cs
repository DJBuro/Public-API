using SignpostDataAccessLayer;
using SignpostDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicSignpost.Tests
{
    public class MockSignpostDataAccess : ISignpostDataAccess
    {
        public string GetServicesByApplicationId(string applicationId, out List<SignpostDataAccessLayer.Models.HostV2> hosts)
        {
            hosts = new List<HostV2>();

            if (applicationId == "123")
            {
                hosts.Add(new HostV2() { Id = Guid.NewGuid(), Order = 0, Name = "test server", Url = "y", Version = 1 });
            }

            return "";
        }


        public string GetServicesBySiteId(int andromedaSiteId, out List<HostV2> hosts)
        {
            throw new NotImplementedException();
        }


        public string GetLegacyPrivateHostsBySiteId(int andromedaSiteId, out List<HostV1> hosts)
        {
            throw new NotImplementedException();
        }

        public string GetLegacyPublicHostsBySiteId(int andromedaSiteId, out List<HostV2> hosts)
        {
            throw new NotImplementedException();
        }


        public string GetDataVersion(out int version)
        {
            throw new NotImplementedException();
        }


        public string AddUpdateACSApplications(int fromVersion, int toVersion, List<SignpostDataAccessLayer.Models.ACSApplication> acsApplications)
        {
            throw new NotImplementedException();
        }
    }
}
