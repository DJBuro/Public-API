declare module AndroWeb.Models
{
    export interface ICart
    {
        
    }

    export interface ICartDetails
    {
        totalPrice: number;
        areDisabledItems: boolean;
        checkoutEnabled: boolean;
        totalPriceItemsOnly: number;
    }

    export interface ICartObservable extends KnockoutObservable<ICart>
    {
        displayTotalPrice: KnockoutObservable<number>;

        //need to describe later
        cartItems: KnockoutObservableArray<any>; 

        subTotalPrice: KnockoutObservable<number>;
        preLoyaltySubTotalPrice: KnockoutObservable<number>;
        totalPrice: KnockoutObservable<number>;
        hasItems: KnockoutObservable<boolean>;
        //mercuryPaymentId: KnockoutObservable<string>;
        //dataCashPaymentDetails: any;
        //deals: KnockoutObservableArray<any>,
        //areDisabledItems: ko.observable(),
        //checkoutEnabled: ko.observable(),
        //hasItems: ko.observable(),
        //displaySubTotalPrice: ko.observable(),
        //subTotalPrice: ko.observable(),
        //discountName: ko.observable(),
        //discountAmount: ko.observable(),
        //displayDiscountAmount: ko.observable(),
        //vouchers: ko.observable([]),
        //totalItemCount: ko.observable(0)
    }
}