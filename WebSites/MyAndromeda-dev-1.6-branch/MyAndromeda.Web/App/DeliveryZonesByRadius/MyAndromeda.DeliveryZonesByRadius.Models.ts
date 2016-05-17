/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
declare module MyAndromeda.DeliveryZonesByRadius.Models {
    export interface IObservable {
        get?: (key: string) => any;
        set?: (key: string, value: any) => void;
        bind? (eventName: string, handler: Function): IObservable;
    }

    export interface IDeliveryZonesByRadiusSettings extends IObservable {
        StoreId: number;
        DeliveryZoneName: IDeliveryZoneNameViewModelSettings;
    }

    export interface IDeliveryZoneNameViewModelSettings extends IObservable {
        OriginPostCode: string;
        Id: number;
        Name: string;
        StoreId: number;
        RadiusCovered: number;
        IsCustom: boolean;
        HasPostCodes: boolean;
        PostCodeSectors: kendo.data.ObservableArray; //IPostCodeSectorsViewModel[];
    }

    export interface IPostCodeSectorsViewModel extends IObservable {
        Id: number;
        DeliveryZoneId: number;
        PostCodeSector: string;
        IsSelected: boolean;
        DataVersion: number;
    }

} 