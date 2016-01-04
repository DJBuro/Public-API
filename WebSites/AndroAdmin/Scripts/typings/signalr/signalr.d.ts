// Type definitions for SignalR 1.0
// Project: http://www.asp.net/signalr
// Definitions by: Boris Yankov <https://github.com/borisyankov/>
// Modified by: T. Michael Keesey <https://github.com/keesey/>
// Definitions: https://github.com/borisyankov/DefinitelyTyped



// Simplified JQueryPromise and JQueryDefered, renamed to prevent name potential clashes with jquery
interface IPromise<T> {
    always(...alwaysCallbacks: any[]): IPromise<T>;
    done(...doneCallbacks: any[]): IPromise<T>;
    fail(...failCallbacks: any[]): IPromise<T>;
    progress(...progressCallbacks: any[]): IPromise<T>;
    then<U>(onFulfill: (...values: any[]) => U, onReject?: (...reasons: any[]) => U, onProgress?: (...progression: any[]) => any): IPromise<U>;
}
interface IDeferred<T> extends IPromise<T> {
    always(...alwaysCallbacks: any[]): IDeferred<T>;
    done(...doneCallbacks: any[]): IDeferred<T>;
    fail(...failCallbacks: any[]): IDeferred<T>;
    progress(...progressCallbacks: any[]): IDeferred<T>;
    notify(...args: any[]): IDeferred<T>;
    notifyWith(context: any, ...args: any[]): IDeferred<T>;
    reject(...args: any[]): IDeferred<T>;
    rejectWith(context: any, ...args: any[]): IDeferred<T>;
    resolve(val: T): IDeferred<T>;
    resolve(...args: any[]): IDeferred<T>;
    resolveWith(context: any, ...args: any[]): IDeferred<T>;
    state(): string;
    promise(target?: any): IPromise<T>;
}

interface HubMethod {
    (callback: (data: string) => void);
}

interface SignalREvents {
    onStart: string;
    onStarting: string;
    onReceived: string;
    onError: string;
    onConnectionSlow: string;
    onReconnect: string;
    onStateChanged: string;
    onDisconnect: string;
}

interface SignalRStateChange {
    oldState: number;
    newState: number;
}

interface SignalR {
    events: SignalREvents;
    connectionState: any;
    transports: any;

    hub: HubConnection;
    id: string;
    logging: boolean;
    messageId: string;
    url: string;

    (url: string, queryString?: any, logging?: boolean): SignalR;
    hubConnection(url?: string): SignalR;

    log(msg: string, logging: boolean): void;
    isCrossDomain(url: string): boolean;
    changeState(connection: SignalR, expectedState: number, newState: number): boolean;
    isDisconnecting(connection: SignalR): boolean;

    // createHubProxy(hubName: string): SignalR;

    start(): IPromise<any>;
    start(callback: () => void): IPromise<any>;
    start(settings: ConnectionSettings): IPromise<any>;
    start(settings: ConnectionSettings, callback: () => void): IPromise<any>;

    send(data: string): void;
    stop(async?: boolean, notifyServer?: boolean): void;

    starting(handler: () => void): SignalR;
    received(handler: (data: any) => void): SignalR;
    error(handler: (error: string) => void): SignalR;
    stateChanged(handler: (change: SignalRStateChange) => void): SignalR;
    disconnected(handler: () => void): SignalR;
    connectionSlow(handler: () => void): SignalR;
    sending(handler: () => void): SignalR;
    reconnecting(handler: () => void): SignalR;
    reconnected(handler: () => void): SignalR;
}

interface HubProxy {
    (connection: HubConnection, hubName: string): HubProxy;
    state: any;
    connection: HubConnection;
    hubName: string;
    init(connection: HubConnection, hubName: string): void;
    hasSubscriptions(): boolean;
    on(eventName: string, callback: (...msg) => void): HubProxy;
    off(eventName: string, callback: (msg) => void): HubProxy;
    invoke(methodName: string, ...args: any[]): any; // IDeferred<any>;
}

interface HubConnectionSettings {
    queryString?: string;
    logging?: boolean;
    useDefaultPath?: boolean;
}

interface HubConnection extends SignalR {
    //(url?: string, queryString?: any, logging?: boolean): HubConnection;
    proxies;
    received(callback: (data: { Id; Method; Hub; State; Args; }) => void): HubConnection;
    createHubProxy(hubName: string): HubProxy;
}

interface SignalRfn {
    init(url, qs, logging);
}

interface ConnectionSettings {
    transport? ;
    callback? ;
    waitForPageLoad?: boolean;
    jsonp?: boolean;
}

interface JQuery {
    signalR: SignalR;
    connection: SignalR;
    hubConnection(url?: string, queryString?: any, logging?: boolean): HubConnection;
}

//declare var $: {
//    (): any;
//    (any): any;

//};


/// <reference path="../jquery/jquery.d.ts" />

//interface HubMethod {
//    (callback: (data: string) => void): any;
//}

//interface SignalREvents {
//    onStart: string;
//    onStarting: string;
//    onReceived: string;
//    onError: string;
//    onConnectionSlow: string;
//    onReconnect: string;
//    onStateChanged: string;
//    onDisconnect: string;
//}

//interface SignalRStateChange {
//    oldState: number;
//    newState: number;
//}

//interface SignalR {
//    events: SignalREvents;
//    connectionState: any;
//    transports: any;

//    hub: HubConnection;
//    id: string;
//    logging: boolean;
//    messageId: string;
//    url: string;
//    qs: any;

//    (url: string, queryString?: any, logging?: boolean): SignalR;
//    hubConnection(url?: string): SignalR;

//    log(msg: string, logging: boolean): void;
//    isCrossDomain(url: string): boolean;
//    changeState(connection: SignalR, expectedState: number, newState: number): boolean;
//    isDisconnecting(connection: SignalR): boolean;

//    // createHubProxy(hubName: string): SignalR;

//    start(): JQueryPromise<any>;
//    start(callback: () => void): JQueryPromise<any>;
//    start(settings: ConnectionSettings): JQueryPromise<any>;
//    start(settings: ConnectionSettings, callback: () => void): JQueryPromise<any>;


//    send(data: string): void;
//    stop(async?: boolean, notifyServer?: boolean): void;

//    starting(handler: () => void): SignalR;
//    received(handler: (data: any) => void): SignalR;
//    error(handler: (error: string) => void): SignalR;
//    stateChanged(handler: (change: SignalRStateChange) => void): SignalR;
//    disconnected(handler: () => void): SignalR;
//    connectionSlow(handler: () => void): SignalR;
//    sending(handler: () => void): SignalR;
//    reconnecting(handler: () => void): SignalR;
//    reconnected(handler: () => void): SignalR;
//}

//interface HubProxy {
//    (connection: HubConnection, hubName: string): HubProxy;
//    state: any;
//    connection: HubConnection;
//    hubName: string;
//    init(connection: HubConnection, hubName: string): void;
//    hasSubscriptions(): boolean;
//    on(eventName: string, callback: (...msg: any[]) => void): HubProxy;
//    off(eventName: string, callback: (msg: any) => void): HubProxy;
//    invoke(methodName: string, ...args: any[]): JQueryDeferred<any>;
//}

//interface HubConnectionSettings {
//    queryString?: string;
//    logging?: boolean;
//    useDefaultPath?: boolean;
//}

//interface HubConnection extends SignalR {
//    //(url?: string, queryString?: any, logging?: boolean): HubConnection;
//    proxies: any;
//    received(callback: (data: { Id: any; Method: any; Hub: any; State: any; Args: any; }) => void): HubConnection;
//    createHubProxy(hubName: string): HubProxy;
//}

//interface SignalRfn {
//    init(url: any, qs: any, logging: any): any;
//}

//interface ConnectionSettings {
//    transport?: any;
//    callback?: any;
//    waitForPageLoad?: boolean;
//    jsonp?: boolean;
//}

//interface JQueryStatic {
//    signalR: SignalR;
//    connection: SignalR;
//    hubConnection(url?: string, queryString?: any, logging?: boolean): HubConnection;
//}


