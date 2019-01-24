using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using AndroCloudHelper;
using System.Collections.Generic;
using System.Configuration;

namespace AndroCloudServices.Services
{
    public class HostService
    {
        public static Response GetAllForApplication(
            string externalApplicationId,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the externalApplicationId
            sourceId = externalApplicationId;

            // Was a valid applicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // Application id was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Check the application details
            Response error = SecurityHelper.CheckHostsGetAccess(externalApplicationId, dataAccessFactory, dataType);
            if (error != null)
            {
                // Security check failed
                return error;
            }

            // Get the hosts
            List<Host> applicationHostList = null;
            dataAccessFactory.HostDataAccess.GetBestPublicHosts(externalApplicationId, out applicationHostList);

            // Was a host returned?
            if (applicationHostList == null || applicationHostList.Count == 0)
            {
                return new Response(Errors.InternalError, dataType);
            }

            // Do we need to override the host names to localhost?
            HostService.CheckForDevOverride<Host>(applicationHostList);

            // Success
            Hosts serializableHosts = new Hosts();

            serializableHosts.AddRange(applicationHostList);

            return new Response(SerializeHelper.Serialize<Hosts>(serializableHosts, dataType));
        }

        public static Response GetAllForApplicationV2(
            string externalApplicationId,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the externalApplicationId
            sourceId = externalApplicationId;

            // Was a valid applicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // Application id was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Check the application details
            Response error = SecurityHelper.CheckHostsGetAccess(externalApplicationId, dataAccessFactory, dataType);
            if (error != null)
            {
                // Security check failed
                return error;
            }

            // Get the hosts
            List<HostV2> applicationHostList = null;
            dataAccessFactory.HostDataAccess.GetBestPublicHostsV2(externalApplicationId, out applicationHostList);

            // Was a host returned?
            if ((applicationHostList == null || applicationHostList.Count == 0))
            {
                return new Response(Errors.InternalError, dataType);
            }

            // Do we need to override the host names to localhost?
            HostService.CheckForDevOverride<HostV2>(applicationHostList);

            // Success
            HostsV2 serializableHosts = new HostsV2();

            //all ready decided if null or empty ... time to finish off     
            serializableHosts.AddRange(applicationHostList);

            return new Response(SerializeHelper.Serialize<HostsV2>(serializableHosts, dataType));
        }

        public static Response GetAllForSite(
            string andromedaSiteIdParameter,
            string licenseKey,
            string hardwareKey,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the andromedaSiteId
            sourceId = andromedaSiteIdParameter;
            int andromedaSiteId = 0;

            // Was an andromedaSiteId provided?
            if (andromedaSiteIdParameter == null || andromedaSiteIdParameter.Length == 0)
            {
                // andromedaSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Is andromedaSiteId an integer?
            if (!int.TryParse(andromedaSiteIdParameter, out andromedaSiteId))
            {
                // andromedaSiteId is not an integer
                return new Response(Errors.InvalidSiteId, dataType);
            }

            // Was a licenseKey provided?
            if (licenseKey == null || licenseKey.Length == 0)
            {
                // licenseKey was not provided
                return new Response(Errors.MissingLicenseKey, dataType);
            }

            // Was a hardwareKey provided?
            if (hardwareKey == null || hardwareKey.Length == 0)
            {
                // hardwareKey was not provided
                return new Response(Errors.MissingHardwareKey, dataType);
            }

            // Check the details
            Guid siteId = Guid.Empty;
            Response response = SecurityHelper.CheckHostsGetAccess(andromedaSiteId, licenseKey, dataAccessFactory, dataType, out siteId);
            if (response != null)
            {
                return response;
            }

            // Get the hosts V1
            List<PrivateHost> hosts = null;
            dataAccessFactory.HostDataAccess.GetBestPrivate(andromedaSiteId, out hosts);

            // Was a host returned?
            if (hosts == null)
            {
                return new Response(Errors.InternalError, dataType);
            }

            // Do we need to override the host names to localhost?
            HostService.CheckForDevOverride<PrivateHost>(hosts);

            // Success
            PrivateHosts serializableHosts = new PrivateHosts();
            serializableHosts.AddRange(hosts);

            return new Response(SerializeHelper.Serialize<PrivateHosts>(serializableHosts, dataType));
        }

        public static Response GetAllForSiteV2(
            string andromedaSiteIdParameter,
            string licenseKey,
            string hardwareKey,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the andromedaSiteId
            sourceId = andromedaSiteIdParameter;
            int andromedaSiteId = 0;

            // Was an andromedaSiteId provided?
            if (andromedaSiteIdParameter == null || andromedaSiteIdParameter.Length == 0)
            {
                // andromedaSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Is andromedaSiteId an integer?
            if (!int.TryParse(andromedaSiteIdParameter, out andromedaSiteId))
            {
                // andromedaSiteId is not an integer
                return new Response(Errors.InvalidSiteId, dataType);
            }

            // Was a licenseKey provided?
            if (licenseKey == null || licenseKey.Length == 0)
            {
                // licenseKey was not provided
                return new Response(Errors.MissingLicenseKey, dataType);
            }

            // Was a hardwareKey provided?
            if (hardwareKey == null || hardwareKey.Length == 0)
            {
                // hardwareKey was not provided
                return new Response(Errors.MissingHardwareKey, dataType);
            }

            // Check the details
            Guid siteId = Guid.Empty;
            Response response = SecurityHelper.CheckHostsGetAccess(andromedaSiteId, licenseKey, dataAccessFactory, dataType, out siteId);
            if (response != null)
            {
                return response;
            }

            // Get the hosts
            List<PrivateHostV2> hosts = null;
            dataAccessFactory.HostDataAccess.GetBestPrivateV2(andromedaSiteId, out hosts);

            // Was a host returned?
            if (hosts == null)
            {
                return new Response(Errors.InternalError, dataType);
            }

            // Do we need to override the host names to localhost?
            HostService.CheckForDevOverride<PrivateHostV2>(hosts);

            // Success
            PrivateHostsV2 serializableHosts = new PrivateHostsV2();
            serializableHosts.AddRange(hosts);

            return new Response(SerializeHelper.Serialize<PrivateHostsV2>(serializableHosts, dataType));
        }

        public static void CheckForDevOverride<T>(List<T> hosts) where T : IHost
        {
            // Do we need to override the host names to localhost?
            string devHostsOverride = ConfigurationManager.AppSettings["DevHostsOverride"];
            if (devHostsOverride != null && devHostsOverride.Length > 0)
            {
                foreach (T host in hosts)
                {
                    string url = host.Url;
                    string startBit = "";
                    string[] chunks = url.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
                    if (chunks.Length > 1)
                    {
                        url = chunks[1];
                        startBit = chunks[0] + "//";
                    }

                    string[] chunks2 = url.Split(new char[] { '/' });
                    chunks2[0] = devHostsOverride;
                    host.Url = startBit + String.Join("/", chunks2);
                }
            }
        }
    }
}
