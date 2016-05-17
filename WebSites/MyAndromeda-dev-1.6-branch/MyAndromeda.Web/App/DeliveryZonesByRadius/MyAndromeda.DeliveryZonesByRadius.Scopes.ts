/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
declare module MyAndromeda.DeliveryZonesByRadius.Scopes {
    export interface IDeliveryZonesByRadiusScope extends ng.IScope {
        
    }

    export interface IStatusControllerScope extends IDeliveryZonesByRadiusScope {
        Modal: kendo.mobile.ui.ModalView;       
    }
 
    export interface IDeliveryZonesNamesScope extends IDeliveryZonesByRadiusScope {
        IsBusy: boolean;
        IsSaveBusy: boolean;
        PostCodeValidator: kendo.ui.Validator;
        PostCodeOptionsListView: kendo.ui.ListView;
        
        SelectAll: boolean;        
        ViewModel: Models.IDeliveryZoneNameViewModelSettings;
        //DataSource: kendo.data.DataSource;      
        ValidateOriginPostCode: (e: Object) => void;        
        SaveChanges: () => void;
        GeneratePostCodeSectors: () => void;
        SelectAllChange: () => void;
        UpdateSelectAll: () => void;
        
    }

    export interface IPostCodesScope extends IDeliveryZonesByRadiusScope {
       
    }
}  