declare module MyAndromeda.Loyalty.Models {
    export interface IRouteParamsEdit {
        providerName: string;
    }


    export interface ILoyaltyType {
        Name: string;
        Subscribed: boolean;
    }


    export interface IProviderLoyalty {
        Id?: string;
        ProviderName : string;
        Configuration : IAndromedaStoreLoyalty
    }


    export interface IStoreLoyalty {
        Enabled?: boolean;
    }

    export interface IAndromedaStoreLoyalty extends IStoreLoyalty {
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
        //the maximum total of available points a customer can have
        MaximumObtainablePoints?: number;
        //round the points up or down to the nearest whole number 
        RoundUp : boolean
    }

    export interface ILoyaltyStartScope extends ng.IScope {
        AvailableLoyaltyTypeNames: string[];
        CurrentLoyaltyTypes: IProviderLoyalty[];

        ShowAddList: boolean;
        ShowEditList : boolean;
    } 

    export interface IEditAndromedaLoyaltyScope extends ng.IScope {
        Name: string;
        Model: IAndromedaStoreLoyalty;
        ProviderAvailable: boolean;

        Currency: string;
        Currency10: string;
        Currency100: string;
        ProviderLoyalty: IProviderLoyalty;

        //convert the number from points 
        ValueOf: (points: number) => string;
        //format a number as currency
        DisplayAsCurrency: (value: number) => string;

        SetDefaults: () => void;
        Save: () => void;
        SaveBusy: boolean;

        AndromedaLoyaltyValidator: kendo.ui.Validator;
        AwardingPointsNumericTextBox: kendo.ui.NumericTextBox;
        MinimumPointsBeforeAvailableNumericTextBox: kendo.ui.NumericTextBox;
        MaximumObtainablePointsNumericTextBox: kendo.ui.NumericTextBox;


        IsAutoSpendPointsOverThisPeakEnabled: () => boolean;
        IsMinimumPointsBeforeAvailableEnabled: () => boolean;
        IsMaximumObtainablePointsEnabled: () => boolean;

        IsMaximumAndMinimumRulesEqual: () => boolean;
        IsMaximumAndMinimumRulesInvalid: () => boolean;
    }
 }