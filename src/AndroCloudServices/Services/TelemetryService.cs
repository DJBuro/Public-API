using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using DataWarehouseDataAccess;
using AndroCloudHelper;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using DataWarehouseDataAccess.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace AndroCloudServices.Services
{
    public class TelemetryService
    {
        public static Response PutSession
        (
            string externalApplicationId,
            string externalSiteId,
            string data,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory
        )
        {
            // Was a externalApplicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalApplicationId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Deserialize the XML/JSON into an object
            DataWarehouseDataAccess.Domain.TelemetrySession telemetrySession = null;
            string result = SerializeHelper.Deserialize<TelemetrySession>(data, dataType, out telemetrySession);

            if (result != null && result.Length > 0)
            {
                return new Response(Errors.InvalidCustomer, dataType);
            }

            // Create a new session id
            telemetrySession.Id = Guid.NewGuid();

            // Check the application details
            int? applicationId = null;
            Guid siteId;

            Response response = SecurityHelper.CheckSiteDetailsGetAccess(externalApplicationId, externalSiteId, androCloudDataAccessFactory, dataType, out applicationId, out siteId);

            if (response != null)
            {
                return response;
            }

            // Put the telemetry session
            dataWarehouseDataAccessFactory.TelemetryDataAccess.AddTelemetrySession(applicationId.Value, telemetrySession, externalSiteId);

            return new Response() { ResponseText = "{ \"sessionId\":\"" + telemetrySession.Id.ToString() + "\" }" };
        }

        public static Response Put
        (
            string externalApplicationId,
            string externalSiteId,
            string data,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory
        )
        {
            // Was a externalApplicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalApplicationId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Deserialize the XML/JSON into an object
            DataWarehouseDataAccess.Domain.Telemetry telemetry = null;
            string result = SerializeHelper.Deserialize<Telemetry>(data, dataType, out telemetry);

            if (result != null && result.Length > 0)
            {
                return new Response(Errors.InvalidCustomer, dataType);
            }

            // Check the application details
            int? applicationId = null;
            Guid siteId;

            Response response = SecurityHelper.CheckSiteDetailsGetAccess(externalApplicationId, externalSiteId, androCloudDataAccessFactory, dataType, out applicationId, out siteId);

            if (response != null)
            {
                return response;
            }

            // Put the telemetry
            dataWarehouseDataAccessFactory.TelemetryDataAccess.AddTelemetry(applicationId.Value, telemetry);

            // Do we need to update the telemetry session?
            if (telemetry.Action != "Account/Login/Failed" && telemetry.Action.StartsWith("Account/Login/"))
            {
                string username = telemetry.ExtraInfo;

                // Update the customer associated with the telemetry session
                dataWarehouseDataAccessFactory.TelemetryDataAccess.UpdateTelemetrySession(applicationId.Value, username, telemetry.SessionId);
            }

            return new Response();
        }
    }
}
