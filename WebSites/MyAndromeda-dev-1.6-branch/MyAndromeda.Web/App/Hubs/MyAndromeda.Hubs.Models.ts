module MyAndromeda.Hubs.Models 
{
    export interface IRouteParameters
    {
        externalSiteId: string;
        chainId: number;
    }
    export interface IHubParameters extends Models.IRouteParameters {
    }

    export interface IMenuHubServiceOptions extends Models.IHubParameters {
        id: string;
        debugInfoUrl: string;
    }

    export interface IMenuHubContext {
    
    }

    export interface ISynchronizationHubServiceOptions extends IHubParameters {
        id: string;
    }

    export interface ISynchronizationHub {
        startedSynchronization: Function//(data: SynchronizationHubContext) => void);
        completedSynchronization: Function// (data: SynchronizationHubContext) => void);
        errorSynchronization: Function//(data: SynchronizationHubErrorContext) => void);
        tasks: Function
    }

    export interface SynchronizationHubContext {
        TaskCount: number;
        TimeStamp: Date;
    }

    export interface SynchronizationHubErrorContext {
        Message: string;
        Context: SynchronizationHubContext;
    }

    export interface IDisplayModelObservable {
        get: Function;
        set: Function;
    }

    export interface IDebugDatabaseMenuViewModel
    {
        Available: boolean;
        DbSiteId: string;
        DbMasterSiteId: string;
        MenuVersion: number;
        MenuVersionLastUpdated: Date;
        LastError: string;

        TempMenuVersion: number;
        TempMenuVersionLastUpdated: Date;

        Message: string;
    }

    export interface ILoggingMessageEvent
    {
        [key: string] : Array<Function> 
    }
}

