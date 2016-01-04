using System;
using System.Linq;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using AndroCloudHelper;
using CloudSync;

namespace AndroCloudServices.Services
{
    public class SyncService
    {
        public static Response GetDataVersion(
            string key,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory)
        {
            Response response = null;

            // Just as a secondary security mechanism - check for a hard coded key
            if (key == "791BB89009C544129F84B409738ACA4E")
            {
                // Get data version
                response = null;

                // Get the data version
                string value = "";
                dataAccessFactory.SettingsDataAccess.GetByName("DataVersion", out value);

                // Package the data version into a serializable object
                DataVersion dataVersion = new DataVersion();
                dataVersion.Version = value;

                // Success
                return new Response(AndroCloudHelper.SerializeHelper.Serialize<DataVersion>(dataVersion, dataType));
            }
            else
            {
                // Invalid key
                ErrorHelper.Log.Error("Invalid key:" + key);
                response = new Response(Errors.UnknownLicenseKey, dataType);
            }

            return response;
        }

        public static Response ImportSyncXml(
            string key,
            string syncXml, 
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory)
        {
            Response response = null;

            // Just as a secondary security mechanism - check for a hard coded key
            if (key == "791BB89009C544129F84B409738ACA4E")
            {
                // Process the sync
                AcsSyncHelper.ImportSyncXml(syncXml, 
                    (successMessage) => ErrorHelper.Log.Debug(successMessage), 
                    (failureMessage) => ErrorHelper.Log.Error(failureMessage)
                );

                // Serialize
                return new Response();
            }
            else
            {
                // Invalid key
                ErrorHelper.Log.Error("Invalid key:" + key);
                response = new Response(Errors.UnknownLicenseKey, dataType);
            }

            return response;
        }
    }
}
