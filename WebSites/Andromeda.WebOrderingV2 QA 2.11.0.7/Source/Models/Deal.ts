/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
module AndroWeb.Models
{
    export class Deal
    {
        public AvailableTimes: any[];
        public CollectionAmount: number;
        public DealLines: any[];
        public DealName: string = "";
        public DeliveryAmount: number;
        public Description: string = "";
        public Display: number;
        public DisplayOrder: number;
        public ForCollection: boolean;
        public ForDelivery: boolean;
        public ForDineIn: boolean;
        public ForceCheapestFree: boolean;
        public FullOrderDiscountCollectionAmount: number;
        public FullOrderDiscountDeliveryAmount: number;
        public FullOrderDiscountMagicNumber: number;
        public FullOrderDiscountType: any;
        public Id: number;
        public MinimumOrderValue: number;
    }
};































