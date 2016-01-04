declare module MyAndromeda.Store.Models
{
    export interface IStore
    {
        Name: string;
        ExternalId: string;
        ChainId: number;
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
        get(id: any, callback: { (stores: IStore[]): void });

        getStores(chainId: number, callback: { (stores: IStore[]): void });
    }

    export interface IStoreServiceRoutes
    {
        getById: string;
        getByExternalId: string;
        getByChainId: string;
    }

} 