/// <reference path="androwebordering.models.d.ts" />
declare module AndroWeb.Services
{
    export interface IAcsServices
    {
        getSites: (success: (values: Array<Models.ISiteDetails>) => void, error: () => void, deliveryZone: string) => void;
        getSiteList: (success: (values: Array<Models.ISiteDetails>) => void, error: () => void, deliveryZone: string) => void;

        getSite: (siteId, gotMenuVersion, callback, statusCheck, errorCallback) => void;
        getSiteDetails: (siteId, callback, errorCallback) => void;


        putMercuryPayment: (siteId, callback, errorCallback) => void;
        putDataCashPayment: (siteId, callback, errorCallback) => void;
        putMercanetPayment: (siteId, order, callback, errorCallback) => void;
        putPayPalPayment: (siteId, paymentDetails, callback, errorCallback) => void;
        postPayPalCallback: (orderRef, paymentDetails, callback, errorCallback) => void;
        checkForOrderError: (data) => {
            hasError: boolean; errorMessage: string; errorCode: string; orderNumber: string; isProvisional: boolean;
        }
        putOrder: (siteId, order, callback, errorCallback) => void;
        checkOrderVouchers: (siteId, order, callback, errorCallback) => void;
        checkForError: (data, alwaysReturnAnError, result) => void;

        getCustomer: (username, password, callback, errorCallback) => void;
        putCustomer: (username, password, customer, callback, errorCallback) => void;
        postCustomer: (username, existingPassword, newPassword, customer, callback, errorCallback) => void;
        putPasswordResetRequest: (username, callback, errorCallback) => void;
        postPasswordResetRequest: (username, passwordResetToken, newPassword, callback, errorCallback) => void;

        getCustomerOrders: (username, password, callback, errorCallback) => void;
        getCustomerOrder: (orderId, username, password, callback, errorCallback) => void;
        
        putFeedback: (siteId, feedback, callback) => void;
        encode64: (data: any) => string;
    }

}

declare var acsapi: AndroWeb.Services.IAcsServices;