declare module AndroWeb.Models
{
    export interface ICheckoutDetails
    {
        firstName: KnockoutObservable<string>;

        voucherCodes: KnockoutObservableArray<IVoucherCode>;
        loyaltyPoints: KnockoutObservable<ICheckoutLoyaltyPoints>;
    }

    

    export interface IVoucherCode
    {
        
    }

    export interface ICheckoutLoyaltyPoints
    {

        gainedPoints: KnockoutObservable<number>;
        gainedPointsValue: KnockoutObservable<number>;
        
        redeemedPoints: KnockoutObservable<number>;
        redeemedPointsValue: KnockoutObservable<number>;
    }
} 

//checkoutDetails:
//{
//    firstName: ko.observable(''),
//    surname: ko.observable(''),
//    telephoneNumber: ko.observable(''),
//    emailAddress: ko.observable(''),
//    deliveryTime: ko.observable(undefined),
//    address: new Address(),
//    payNow: ko.observable(undefined),
//    marketing: ko.observable('true'),
//    wantedTime: ko.observable(undefined),
//    orderNotes: ko.observable(''),
//    chefNotes: ko.observable(''),
//    rememberAddress: true,
//    rememberContactDetails: true,
//    voucherCodes: ko.observable([]),
//    isMissingAddress: ko.observable(false)
//},