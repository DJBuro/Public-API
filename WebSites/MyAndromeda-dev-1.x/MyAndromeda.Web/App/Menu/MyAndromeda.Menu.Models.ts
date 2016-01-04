module MyAndromeda.Menu.Models {
    export class thumbItem implements IMenuItemThumb {
        Width: number;
        Height: number;
        FileName: string;
        Url: string;
        Alt: string;
    }

    export interface IMenuPublishOptions {
        publishPanel: {
            mainButtonId: string;
            publishPanelId: string;
            publishUrlPath: string;
        };

    }

    export interface IMenuServiceOptions extends IMenuPublishOptions {
        ids: {
            listViewId: string;
            statusViewId: string;
            pagerId: string;
        };
        listview: {
            template: string;
            editTemplate: string;
        };
        //actionUrls: IRequiredActionUrls;
        filterIds: IFilterids;

        routeParameters: IRouteParameters;
        data: {
            endpoints: string[]
        };
    }

    export interface IRequiredActionUrls {
        listMenuItems: string;

        saveImageUrl: string;
        removeImageUrl: string;

        saveMenuItems: string;
        batch: boolean;
    }

    export interface IFilterids {
        toolBarId: string;

        itemNameId: string;
        displayCategoryId: string;
        category1Id: string;
        category2Id: string;
        resetId: string;
    }

    export interface IMenuItem {
        isNew: () => boolean;

        Id: number;
        Name: string;
        WebName: string;
        WebDescription: string;
        WebSequence: number;
        Enabled: boolean;
        Thumbs: kendo.data.ObservableArray; //IMenuItemThumb[];
        CategoryId1: number;
        Category1Name: string;
        CategoryId2: number;
        Category2Name: string;
        DisplayCategoryId: number;
        DisplayName: Function;
        CanEditNameAndDescription: Function;
        ShowCantEditMessage: Function;
        WebNameCount: Function;
        WebDescriptionCount: Function;
        Index: Function;
        ColorStatus: Function;

        ClearWebName: Function;
        ClearDescription: Function;

        Prices: {
            Collection: number;
            Delivery: number;
            Instore: number;
        }

        Enable: () => void;
        Disable: () => void;
    }


    export interface IMenuItemObservable extends kendo.data.Model, IMenuItem {
        //dirty: boolean; //kendo dirty value
        //get(field: string): any;
        //set(field: string, value: any): any;
        //bind: (eventName: string, handler: (e: any) => void) => void;
    }

    export interface IMenuItemThumb {
        FileName: string;
        Width: number;
        Height: number;
        Url: string;
        Alt: string;
    }

    export interface ICategory {
        Id: number;
        Name: string;
        ParentId: number;
    }

    export interface IToppingCollection {
        Items: ITopping[]
    }

    export interface IToppingGroup {
        Id: string;
        Name: string;
        Group: ICategory;
        ToppingVarients: IToppingVarient[];

        get: (key: string) => any;
        set: (key: string, value: any) => void;
        accept: (values: any) => void;
        trigger: (event: string) => void;
    }

    export interface IToppingVarient {
        Id: number;
        Category: ICategory;
        DineInPrice: number;
        CollectionPrice: number;
        DeliveryPrice: number;

        get: (key: string) => any;
        set: (key: string, value: any) => void;
        accept: (values: any) => void;
        trigger: (event: string) => void;
    }

    export interface ITopping {
        Id: number;
        Name: string;
        DeliveryPrice: number;
        CollectionPrice: number;
        DineInPrice: number;
        SubCategory: ICategory;

        get: (key: string) => any;
        set: (key: string, value: any) => void;

        accept: (values: any) => void;
        trigger: (event: string) => void;

        UpdateAllNames: (confirm: boolean) => void;
        UpdateAllToppingPrices: (confirm: boolean) => void;
        UpdateAllDineIn: (confirm: boolean) => void;
        UpdateAllCollection: (confirm: boolean) => void;
        UpdateAllDelivery: (confirm: boolean) => void;
    }

    export interface IDealLine
    {
        DeliveryAmount: number;
        CollectionAmount: number;
        AllowableItemsIds: number[];
    }

    export interface IDeal {
        DealId: number;
        DealName: string;
        Description: string;
        ForDelivery: boolean;
        ForCollection: boolean;
        DeliveryAmount: number;
        CollectionAmount: number;
        ForceCheapestFree: boolean;
        MinimumOrderValue: number;
        DealLines: IDealLine[]
    }

    export interface IMenu {
        DisplayCategories: ICategory[];
        Categories1: ICategory[];
        Categories2: ICategory[];
        MenuItems: IMenuItem[];
        DealItems: IDeal[];

    }

    /** May move to another file  ... more a global settings thing **/
    export interface IRouteParameters {
        externalSiteId: string;
        chainId: number;
    }

    export interface IMenuToppingsFilters {
        Name: string;
        ResetFilters: () => void;
    }


}