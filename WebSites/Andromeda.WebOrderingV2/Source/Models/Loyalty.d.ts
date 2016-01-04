declare module AndroWeb.Models
{
    export interface ICheckoutLoyalty
    {
        providerName: string;
        values: {
            awardedPoints: number;
            awardedPointsValue: number;
            redeemedPoints: number;
            redeemedPointsValue: number
        }
    }


    export interface ICustomerLoyalty
    {
        AvailablePoints: KnockoutObservable<number>;
    }

    export interface ICustomerLoyaltySession
    {
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

    export interface IAndromedaStoreLoyalty {
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