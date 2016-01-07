declare module MyAndromeda.Reports.ChainDashboard.Models {
    export interface IChainTodaysSalesSummaryControllerModel {
        Total: number;
        TotalValue: number;
        Completed: number;
        CompletedValue: number;
        Cancelled: number;
        CancelledValue: number;
        Received: number;
        ReceivedValue: number;
        Oven: number;
        OvenValue: number;
        ReadyToDispatch: number;
        ReadyToDispatchValue: number;
        OutForDelivery: number;
        OutForDeliveryValue: number;
        FutureOrder: number;
        FutureOrderValue: number;
        /* pie values */
        Collection: number;
        CollectionValue: number;
        Delivery: number;
        DeliveryValue: number;
        InStore: number;
        InStoreValue: number;
        /* pie value */
        Name: string;
        AcsApplicationData: kendo.data.DataSource;
        Data: kendo.data.DataSource;
        OccasionData: kendo.data.DataSource;
    }

    export interface IDataWarehouseChain
    {
        Id: number;
        Name: string;
        Stores: IDataWareHouseStore[];
    }

    export interface IDataWareHouseStore
    {
        AndromedaSiteId: number;
        Name: string;
        ExternalSiteId: string;
        ExternalSiteName: string;
        Orders: IDataWarehouseOrder[];
    }

    export interface IDataWarehouseOrder {
        OrderType: string;
        ApplicationId: number;
        ExternalSiteId: string;
        FinalPrice: number;
        Status: number;
        WantedTime: Date;
    }

    export interface IChainResult {
        ChainId: number;
        ChainName: string;
        Data: IGroupedStoreResults[];

        Collection: ISummarySalesModelType;
        Delivery: ISummarySalesModelType;
        DineIn: ISummarySalesModelType;
        CarryOut: ISummarySalesModelType;
        Cancelled: ISummarySalesModelType;
        Total: ISummarySalesModelType;

        WeekData: IStoreSummaryModel[]
    }

    export interface IGroupedStoreResults {
        StoreId: number;

        DailyData: IStoreSummaryModel[];
        WeekData: IStoreSummaryModel[];

        Collection: ISummarySalesModelType;
        Delivery: ISummarySalesModelType;
        DineIn: ISummarySalesModelType;
        CarryOut: ISummarySalesModelType;
        Cancelled: ISummarySalesModelType;
        Total: ISummarySalesModelType;
    }

    export interface IQuery {
        FromObj: Date;
        FromLabel?: string;
        ToObj: Date;
        ToLabel?: string;
    }

    export interface IStoreSummaryModel {
        StoreId: number;
        ExternalSiteName: string;
        WeekOfYear: number;

        Collection: ISummarySalesModelType;
        Delivery: ISummarySalesModelType;
        DineIn: ISummarySalesModelType;
        CarryOut: ISummarySalesModelType;
        Cancelled: ISummarySalesModelType;
        Total: ISummarySalesModelType;

        NetSales: number;
        OrderCount: number;
        AverageMakeTime: number;
        RackTime: number;
        AverageOutTheDoorTime: number;
        AverageToTheDoorTime: number;
    }

    export interface ISummarySalesModelType {
        NetSales: number;
        OrderCount: number;
    }

    export interface IDailyModel {
    }
}