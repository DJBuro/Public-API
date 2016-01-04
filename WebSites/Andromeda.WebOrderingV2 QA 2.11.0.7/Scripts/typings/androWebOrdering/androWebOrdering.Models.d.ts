declare module AndroWeb.Models 
{
    export interface IMenuItemWrapper 
    {
        src: string;
        templateName: string;
        name: string;
        description: string;
        price: string;
        className: string;
        id: number;
        quantity: KnockoutObservable<number>;
        menuItem: any; IMenuItem;
    }
    export interface IMenuItem 
    {
        Desc: string;
        ColPric: number;
        DelPrice: number;
        DineInPrice: number;
    }

    export interface IThumbnail 
    {
        Src: string;
    }

    export interface IMyAndromedaThumbnailObject 
    {
        Server: { Endpoint: string }
    }

    export interface ISite 
    {
        siteId?: string;
        name?: string;
        isOpen?: boolean;
        estDelivTime?: number;
    }

    export interface ISiteDetails extends ISite 
    {
        siteLoyalties?: ILoyaltyProvider[];
    }

    export interface ICart
    {
    }
    export interface ICartItem
    {
    }

    export interface ICustomer
    {
        accountNumber: string;
        address: any;
        contacts: IContact[];
        facebookId: string;
        facebookUsername: string;
        firstName: string;
        surname: string;
        title: string;

        loyalties: ICustomerLoyalty[]
    }

    export interface IContact
    {
    }

    export interface ICustomerObservable 
    {
        firstName: KnockoutObservable<string>;
        loyalties: KnockoutObservableArray<ICustomerLoyalty>;
    }

    export interface ICustomerLoyalty
    {
        Id: string;
        CustomerId: string;
        ProviderName: string;
        Points?: number;
        PointsGained?: number;
        PointsUsed?: number;
    }

    export interface ICartDetails
    {
        totalPrice: number;
        areDisabledItems: boolean;
        checkoutEnabled: boolean;
        totalPriceItemsOnly: number;
    }

    export interface ICheckoutDetails
    {
        firstName: KnockoutObservable<string>;

        voucherCodes: KnockoutObservableArray<any>;
        loyaltyPoints: KnockoutObservable<ICheckoutLoyaltyPoints>;

        wantedTime: KnockoutObservable<any>;

        tableNumber: KnockoutObservable<string>;
    }

    export interface ICheckoutLoyaltyPoints
    {
        gainedPoints: KnockoutObservable<number>;
        gainedPointsValue: KnockoutObservable<number>;

        redeemedPoints: KnockoutObservable<number>;
        redeemedPointsValue: KnockoutObservable<number>;
    }

    export interface ICheckoutLoyalty
    {
        providerName: string;
        awardedPoints: number;
        awardedPointsValue: number;
        redeemedPoints: number;
        redeemedPointsValue: number
    }


    export interface ICustomerLoyalty
    {
        AvailablePoints: KnockoutObservable<number>;
    }

    export interface ICustomerLoyaltySession
    {
        minimumPointsMessage: KnockoutObservable<string>;

        /* Potential values ... */
        canGainThesePoints: KnockoutObservable<number>;
        canGainThesePointsValue: KnockoutObservable<number>;
        canGainThesePointsValueLabel: KnockoutObservable<string>;

        canSpendThesePoints: KnockoutObservable<number>;
        canSpendThesePointsValue: KnockoutObservable<number>;
        canSpendThesePointsValueLabel: KnockoutObservable<string>;
        /* end of potential values */

        /* spending values */
        isSpendingThePoints: KnockoutObservable<boolean>;
        spentPoints: KnockoutObservable<number>;
        spentPointsValue: KnockoutObservable<number>;
        spentPointsValueLabel: KnockoutObservable<string>;
        gainedPoints: KnockoutObservable<number>;
        gainedPointsValue: KnockoutObservable<number>;
        gainedPointsValueLabel: KnockoutObservable<string>;
        /* end of spending values */

        /* loaded values */
        customersAvailableLoyaltyPoints: KnockoutObservable<number>;
        customersAvailableLoyaltyPointsValue: KnockoutObservable<number>;
        customersAvailableLoyaltyPointsValueLabel: KnockoutObservable<string>;

        /* checkout helper */
        displayPointsLeft: KnockoutObservable<number>;
        displayPointsValue: KnockoutObservable<number>;
        displayPointsValueLabel: KnockoutObservable<string>;
    }

    export interface ICustomerLoyaltyCheckout
    {
        SpentPoints: number;
        GainedPoints: number;
    }

    export interface IAndromedaStoreLoyalty
    {
        Enabled?: boolean;

        //how much points equal to a £ ie £1 = 100 points
        PointValue?: number;
        //how much points to receive per £ spent
        AwardPointsPerPoundSpent?: number;
        //how much points to be awarded on registration
        AwardOnRegiration?: number;
        //when x points are reached they must spend some to be under x - ie they cannot horde points 
        AutoSpendPointsOverThisPeak?: number;
        //the minimum amount needed left in the basket after the loyalty has been applied. 
        MinimumOrderTotalAfterLoyalty?: number;
        //how much points need to be claimed before they are accessible to the user
        MinimumPointsBeforeAvailable?: number;
        //how much of the order can be claimed with points
        MaximumPercentThatCanBeClaimed?: number;
        //how highest monetary value that can be spent by points. 
        MaximumValueThatCanBeClaimed?: number;
        MaximumObtainablePoints?: number;
        //round the points up or down to the nearest whole number 
        RoundUp: boolean
    }

    export interface ILoyaltyProvider
    {
        Id: string;
        ProviderName: string;
        Enabled: boolean;
        Configuration: IAndromedaStoreLoyalty;
    }
}