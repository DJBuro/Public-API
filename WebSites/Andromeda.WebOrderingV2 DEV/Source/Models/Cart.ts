/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
module AndroWeb.Models
{
    export class Cart
    {
        public displayTotalPrice: KnockoutObservable<boolean> = ko.observable(true);
        public cartItems: KnockoutObservableArray<AndroWeb.Models.CartItem> = ko.observableArray([]);
        public totalPrice: KnockoutObservable<number> = ko.observable(0);
        public totalPriceBeforeLoyalty: KnockoutObservable<any> = ko.observable(0);
        public selectedCartItem: KnockoutObservable<any> = ko.observable(undefined);
        public mercuryPaymentId: KnockoutObservable<any> = ko.observable(undefined);
        public dataCashPaymentDetails: KnockoutObservable<any> = ko.observable(undefined);
        public deals: KnockoutObservableArray<any> = ko.observableArray([]);
        public areDisabledItems: KnockoutObservable<boolean> = ko.observable(true);
        public checkoutEnabled: KnockoutObservable<boolean> = ko.observable(true);
        public hasItems: KnockoutObservable<boolean> = ko.observable(true);
        public displaySubTotalPrice: KnockoutObservable<boolean> = ko.observable(true);
        public subTotalPrice: KnockoutObservable<number> = ko.observable(0);
        public preLoyaltySubTotalPrice: KnockoutObservable<number> = ko.observable(0);
        public discountName: KnockoutObservable<string> = ko.observable('');
        public discountAmount: KnockoutObservable<number> = ko.observable(0);
        public displayDiscountAmount: KnockoutObservable<number> = ko.observable(0);
        public vouchers: KnockoutObservableArray<any> = ko.observableArray([]);
        public totalItemCount: KnockoutObservable<number> = ko.observable(0);
        public deliveryCharge: number = 0;
        public displayDeliveryCharge: KnockoutObservable<string> = ko.observable('');
        public cardCharge: number = 0;
        public displayCardCharge: KnockoutObservable<boolean> = ko.observable(true);
        public displayCardChargePayment: KnockoutObservable<boolean> = ko.observable(true);
        public autoApplyVoucherText: KnockoutObservable<string> = ko.observable('');
    }
};































