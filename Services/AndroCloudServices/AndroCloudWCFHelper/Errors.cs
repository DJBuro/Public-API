using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroCloudHelper
{
    public class Errors
    {
        public static Error NoError = new Error("OK", 0, ResultEnum.NoError);
        public static Error InternalError = new Error("Internal server error", 1, ResultEnum.InternalServerError);
 //       public static Error MissingPartnerId = new Error("Missing partnerId", 1000, ResultEnum.BadRequest);
        public static Error MissingLongitude = new Error("Missing longitude", 1001, ResultEnum.BadRequest);
        public static Error MissingLatitude = new Error("Missing latitude", 1002, ResultEnum.BadRequest);
        public static Error MissingMaxDistance = new Error("Missing maxDistance", 1003, ResultEnum.BadRequest);
 //       public static Error UnknownPartnerId = new Error("Unknown partnerId", 1004, ResultEnum.Unauthorized);
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
 //       public static Error PartnerAccessToSiteDenied = new Error("PartnerId is not authorized to access this siteId", 1017, ResultEnum.Unauthorized);
        public static Error UnknownLicenseKey = new Error("Unknown licenseKey", 1018, ResultEnum.BadRequest); // Private services
        public static Error MissingOrder = new Error("Missing order", 1019, ResultEnum.BadRequest);
        public static Error MissingStatus = new Error("Missing status", 1020, ResultEnum.BadRequest); // Private services
        public static Error InvalidOrderStatusId = new Error("Invalid orderStatusId", 1021, ResultEnum.BadRequest); // Private services
        public static Error UnkownOrderStatusId = new Error("Unknown orderStatusId", 1022, ResultEnum.BadRequest); // Private services
        public static Error MissingOrderStatusId = new Error("Missing orderStatusId", 1022, ResultEnum.BadRequest); // Private services
        public static Error BadData = new Error("Bad data: {errorMessage}", 1023, ResultEnum.BadRequest); // Private services
        public static Error InvalidMaxDistance = new Error("Invalid max distance", 1024, ResultEnum.BadRequest);
        public static Error InvalidLongitude = new Error("Invalid longitude", 1025, ResultEnum.BadRequest);
        public static Error InvalidLatitude = new Error("Invalid latitude", 1026, ResultEnum.BadRequest);
        public static Error InvalidSiteId = new Error("Invalid site id", 1027, ResultEnum.BadRequest); // Private services
        public static Error MissingApplicationId = new Error("Missing applicationId", 1028, ResultEnum.BadRequest);
        public static Error UnknownApplicationId = new Error("Unknown applicationId", 1029, ResultEnum.Unauthorized);
        public static Error ApplicationAccessToSiteDenied = new Error("ApplicationId is not authorized to access this siteId", 1030, ResultEnum.Unauthorized);
        public static Error SyncAccessDenied = new Error("You are not authorized to call this web service", 1031, ResultEnum.Unauthorized);
        public static Error MissingETD = new Error("Missing ETD", 1032, ResultEnum.BadRequest);
        public static Error MissingSite = new Error("Missing site", 1033, ResultEnum.BadRequest);
        public static Error MissingUsername = new Error("Missing username", 1034, ResultEnum.BadRequest);
        public static Error MissingPassword = new Error("Missing password", 1035, ResultEnum.BadRequest);
       
        public static Error UnknownUsername = new Error("Unknown username", 1036, ResultEnum.BadRequest);
        public static Error InvalidCustomer = new Error("Invalid customer", 1037, ResultEnum.BadRequest);
        public static Error UnknownCountry = new Error("Unknown country", 1038, ResultEnum.BadRequest);
        public static Error UnknownMarketingLevel = new Error("Unknown marketing level", 1039, ResultEnum.BadRequest);
        public static Error UnknownContactType = new Error("Unknown contact type", 1040, ResultEnum.BadRequest);
        public static Error UsernameAlreadyUsed = new Error("Username already used", 1041, ResultEnum.BadRequest);
        public static Error IncorrectPassword = new Error("Incorrect password", 1042, ResultEnum.BadRequest);
        public static Error PasswordResetRequestExpired = new Error("Password reset request expired", 1043, ResultEnum.BadRequest);
        public static Error UnknownCustomerId = new Error("Unknown customer id", 1044, ResultEnum.BadRequest);
        public static Error PasswordResetNotAllowedForFacebookAccount = new Error("Password reset not allowed for facebook account", 1045, ResultEnum.BadRequest);

        public static Error Voucher_UnKnown = new Error("Voucher does not exist", 3000, ResultEnum.BadRequest);
        public static Error Voucher_MaxOneVoucher = new Error("More than one voucher to a single order", 3001, ResultEnum.BadRequest);
        public static Error Voucher_InActive = new Error("Voucher is not active", 3002, ResultEnum.BadRequest);
        public static Error Voucher_InvalidOccasion = new Error("Invalid Occasion", 3003, ResultEnum.BadRequest);
        public static Error Voucher_MinimumAmountNotMet = new Error("Order amount not in range with Voucher minumum order amount", 3004, ResultEnum.BadRequest);
        public static Error Voucher_InvalidDate = new Error("Order wanted time not in range with Voucher Availability", 3005, ResultEnum.BadRequest);
        public static Error Voucher_InvalidDayOfWeek = new Error("Order wanted time not in range with Voucher Availability Day", 3006, ResultEnum.BadRequest);
        public static Error Voucher_InvalidTimeOfDay = new Error("Order wanted time not in range with Voucher Availability Time", 3007, ResultEnum.BadRequest);
        public static Error Voucher_ExceededCustomerRepetitions = new Error("MaxRepetitions exceeded for the customer", 3008, ResultEnum.BadRequest);
        public static Error Voucher_CustomerId = new Error("Customer Id not available to process vouchers", 3009, ResultEnum.BadRequest);

        public static Error Loyalty_InvalidData = new Error("Invalid Loyalty data", 4000, ResultEnum.BadRequest);
        public static Error Loyalty_MismatchCustomerId = new Error("CustomerId mismatch", 4001, ResultEnum.BadRequest);
        public static Error Loyalty_MissingAction = new Error("Missing Action", 4002, ResultEnum.BadRequest);

        public static Error Sync_InvalidSecretKey = new Error("Invalid secret key", 5000, ResultEnum.BadRequest);
        public static Error Sync_MissingSyncJson = new Error("Missing sync JSON", 5001, ResultEnum.BadRequest);
        public static Error Sync_InvalidSyncJson = new Error("Invalid sync JSON", 5002, ResultEnum.BadRequest);
        public static Error Sync_InvalidACSApplicationId = new Error("Invalid ACS Application id", 5003, ResultEnum.BadRequest);
        public static Error Sync_InvalidACSApplicationEnvironmentId = new Error("Invalid ACS Application environment id", 5004, ResultEnum.BadRequest);
        
        
    }
}
