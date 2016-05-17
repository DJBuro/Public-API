 declare module MyAndromeda.Chain.Models 
 {
    export interface IChain
    {
        Id: number;
        Name: string;
    }

    export interface IChainObservable extends IChain
    {
        get: Function;
        set: Function;
        dirty: boolean;
    }

    export interface IChainService
    {
        get(id: number, callback: { (chain: IChain): void }); 
    }

    export interface IChainServiceRoutes
    {
        getById: string;
    }
 }