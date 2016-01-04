///// <reference path="../typings/jquery/jquery.d.ts" />
///// <reference path="../typings/signalr/signalr.d.ts" />
//module Andromeda.Hubs
//{
//    export class HubAbstract
//    {
//        public options: IHubParameters;
//        public connection: AndromedaHubsConnection;
//        constructor(options: IHubParameters) {
//            if (!options) {
//                //always demand options. It needs to know whether it knows 
//                //if the chain id and external id is present. 
//                //null is acceptable.
//                throw new Error("hub parameters are required.");
//                //options = {
//                //    chainId: null,
//                //    externalSiteId: null
//                //};
//            }
//            this.options = options;
//            this.connect();
//        }
//        private connect(): void {
//            this.connection =
//                Andromeda.Hubs.AndromedaHubsConnection.GetInstance(this.options);
//            this.connection.connect();
//        }
//    }
//    export class AndromedaHubsConnection {
//        private static _instance: AndromedaHubsConnection;
//        private options: IHubParameters;
//        private connected: boolean;
//        private connecting: boolean;
//        private setup: boolean;
//        public hubConnection: HubConnection;
//        public static GetInstance(options: IHubParameters): AndromedaHubsConnection {
//            if (AndromedaHubsConnection._instance) { return AndromedaHubsConnection._instance; }
//            return (AndromedaHubsConnection._instance = new AndromedaHubsConnection(options));
//        }
//        constructor(options: IHubParameters)
//        {
//            this.options = options;
//            this.connect();
//        }
//        public connect(): HubConnection
//        {
//            if (this.connected) { return; }
//            if (this.connecting) { return; }
//            if (this.setup) { return; }
//            var internal = this,
//                hubConnection = $.connection.hub;
//            this.hubConnection = hubConnection;
//            //setup route parameters for MyAndromeda 
//            if (!this.setupQueryString()) { return hubConnection; };
//            hubConnection.starting(() => {
//                internal.connecting = true;
//                AndromedaHubsConnection.log("hub connection starting");
//            });
//            hubConnection.start({
//                transport: 'longPolling'
//                //transport: kendo.support.mobileOS ? 'longPolling' : 'webSockets'
//            }).done(() => {
//                internal.connecting = false;
//                internal.connected = true;
//                AndromedaHubsConnection.log("hub connection started!");
//            });
//            this.setup = true;
//            return hubConnection;
//        } 
//        private setupQueryString(): boolean {
//            var connection = <any>this.hubConnection;
//            if (!this.options)
//            {
//                return false;
//            }
//            connection.qs = {
//            };
//            return true;
//        }
//        public static log(data): void {
//            if (console && console.log) {
//                try { console.log(data); }
//                catch (e) { }
//            }
//        }  
//    }
//}
//interface IRouteParameters
//{
//}
//interface IHubParameters extends IRouteParameters 
//{
//} 
//# sourceMappingURL=Andromeda.Hubs.js.map