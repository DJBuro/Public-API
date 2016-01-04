using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroCloudServices.Domain;
using AndroCloudDataAccess.Domain;
using AndroCloudHelper;

namespace AndroCloudServices
{
    public class Errors
    {
        public static Error InternalError = new Error("Internal server error", 1000, ResultEnum.InternalServerError);
        public static Error MissingPartnerId = new Error("Missing partnerId", 1000, ResultEnum.BadRequest);
        public static Error MissingLongitude = new Error("Missing longitude", 1001, ResultEnum.BadRequest);
        public static Error MissingLatitude = new Error("Missing latitude", 1002, ResultEnum.BadRequest);
        public static Error MissingMaxDistance = new Error("Missing maxDistance", 1003, ResultEnum.BadRequest);
        public static Error UnknownPartnerId = new Error("Unknown partnerId", 1004, ResultEnum.Unauthorized);
        public static Error UnknownGroupId = new Error("Unknown groupId", 1005, ResultEnum.BadRequest);
        public static Error MissingSiteId = new Error("Missing siteId", 1006, ResultEnum.BadRequest);
        public static Error UnknownSiteId = new Error("Unknown siteId", 1007, ResultEnum.BadRequest);
        public static Error MenuNotFound = new Error("Menu not found", 1008, ResultEnum.BadRequest);
        public static Error MissingLicenseKey = new Error("Missing licenseKey", 1009, ResultEnum.BadRequest); // Private services
        public static Error MissingHardwareKey = new Error("Missing hardwareKey", 1010, ResultEnum.BadRequest); // Private services
        public static Error MissingVersion = new Error("Missing version", 1011, ResultEnum.BadRequest); // Private services
        public static Error VersionIsNotAValidInteger = new Error("Version is not a valid Integer", 1012, ResultEnum.BadRequest); // Private services
        public static Error InvalidLicenceKey = new Error("Invalid licenceKey", 1013, ResultEnum.BadRequest); // Private services
        public static Error MissingOrderId = new Error("Missing orderId", 1014, ResultEnum.BadRequest);
        public static Error InvalidOrderId = new Error("Invalid orderId", 1015, ResultEnum.BadRequest);
        public static Error UnknownOrderId = new Error("Unknown orderId", 1016, ResultEnum.BadRequest);
        public static Error PartnerAccessToSiteDenied = new Error("PartnerId is not authorized to access this siteId", 1017, ResultEnum.Unauthorized);
        public static Error UnknownLicenseKey = new Error("Unknown licenseKey", 1018, ResultEnum.BadRequest); // Private services
        public static Error MissingOrder = new Error("Missing order", 1019, ResultEnum.BadRequest);
        public static Error MissingStatus = new Error("Missing status", 1020, ResultEnum.BadRequest); // Private services
        public static Error InvalidOrderStatusId = new Error("Invalid orderStatusId", 1021, ResultEnum.BadRequest); // Private services
        public static Error UnkownOrderStatusId = new Error("Unknown orderStatusId", 1022, ResultEnum.BadRequest); // Private services
        public static Error MissingOrderStatusId = new Error("Missing orderStatusId", 1022, ResultEnum.BadRequest); // Private services
        public static Error BadData = new Error("Bad data: {errorMessage}", 1023, ResultEnum.BadRequest); // Private services
    }
}
