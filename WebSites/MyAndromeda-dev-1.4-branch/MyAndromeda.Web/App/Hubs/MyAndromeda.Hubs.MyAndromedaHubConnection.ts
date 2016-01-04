module MyAndromeda.Hubs {
    export class MyAndromedaHubConnection {

        private static _instance: MyAndromedaHubConnection;
        private options: Models.IHubParameters;

        private connected: boolean;
        private connecting: boolean;
        private setup: boolean;

        public hubConnection: HubConnection;


        public static GetInstance(options: Models.IHubParameters): MyAndromedaHubConnection {
            if (MyAndromedaHubConnection._instance) { return MyAndromedaHubConnection._instance; }

            return (MyAndromedaHubConnection._instance = new MyAndromedaHubConnection(options));
        }

        constructor(options: Models.IHubParameters)
        {
            this.options = options;
            this.connect();
        }

        public connect(): HubConnection
        {
            if (this.connected) { return; }
            if (this.connecting) { return; }
            if (this.setup) { return; }

            //$.connection.hub.logging = true;

            var internal = this,
                hubConnection = $.connection.hub;

            //if (this.hubConnection)
            //{
            //    return this.hubConnection;
            //}

            this.hubConnection = hubConnection;
            //setup route parameters for MyAndromeda 
            if (!this.setupQueryString()) { return hubConnection; };
            
            hubConnection.starting(() => {
                internal.connecting = true;
                MyAndromedaHubConnection.log("hub connection starting");
            });

            var transportType = //"webSockets";
                "longPolling";
            
            //if(document.URL.indexOf("localhost") >= 0){
            //    transportType = "webSockets";
            //}

            hubConnection.start({
                transport: transportType //['webSockets', 'longPolling'] 
                //transport: transportType
                //transport: kendo.support.mobileOS ? 'longPolling' : 'webSockets'
            }).done(() => {
                internal.connecting = false;
                internal.connected = true;

                MyAndromedaHubConnection.log("hub connection started!");
            });

            this.setup = true;

            return hubConnection;
        } 

        private setupQueryString(): boolean {
            var connection = <any>this.hubConnection;

            if (!this.options.chainId)
            {
                return false;
            }

            connection.qs = {
                'externalSiteId': this.options.externalSiteId,
                'chainId': this.options.chainId
            };
            return true;
        }

        public static log(data): void {
            if (console && console.log) {
                try { console.log(data); }
                catch (e) { }
            }
        }  
    }
} 