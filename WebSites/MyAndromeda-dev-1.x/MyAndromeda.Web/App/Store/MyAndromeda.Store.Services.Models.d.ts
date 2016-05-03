declare module MyAndromeda.Store.Models
{
    export interface IStore
    {
        Name: string;
        ExternalId: string;
        ChainId: number;
    }

    ///... MyAndromeda.Web.Controllers.Api.User
    export interface IStoreModel {
        Id: number;
        AndromedaSiteId: number;
        ExternalSiteId: string;
        Name: string;
        HasRameses: boolean;
        StoreEnrollments: IStoreEnrollment[]
    }

    export interface IStoreEnrollment {
        Name: string;
    }

    export interface IStoreLowerCase
    {
        clientSiteName: string;
        latitude: string;
        longitude: string;   
    }

    export interface IStoreService
    {
        get(id: number, callback: Function);
        get(externalId: string, callback: Function);
        get(id: any, callback: (stores: IStore[]) => void);

        getStores(chainId: number, callback: (stores: IStore[]) => void);
    }

    export interface IStoreServiceRoutes
    {
        getById: string;
        getByExternalId: string;
        getByChainId: string;
    }

} 