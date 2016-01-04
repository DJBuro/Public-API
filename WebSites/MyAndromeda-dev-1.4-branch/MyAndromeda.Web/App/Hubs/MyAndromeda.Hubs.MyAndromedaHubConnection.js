var MyAndromeda;
(function (MyAndromeda) {
    var Hubs;
    (function (Hubs) {
        var MyAndromedaHubConnection = (function () {
            function MyAndromedaHubConnection(options) {
                this.options = options;
                this.connect();
            }
            MyAndromedaHubConnection.GetInstance = function (options) {
                if (MyAndromedaHubConnection._instance) {
                    return MyAndromedaHubConnection._instance;
                }
                return (MyAndromedaHubConnection._instance = new MyAndromedaHubConnection(options));
            };
            MyAndromedaHubConnection.prototype.connect = function () {
                if (this.connected) {
                    return;
                }
                if (this.connecting) {
                    return;
                }
                if (this.setup) {
                    return;
                }
                //$.connection.hub.logging = true;
                var internal = this, hubConnection = $.connection.hub;
                //if (this.hubConnection)
                //{
                //    return this.hubConnection;
                //}
                this.hubConnection = hubConnection;
                //setup route parameters for MyAndromeda 
                if (!this.setupQueryString()) {
                    return hubConnection;
                }
                ;
                hubConnection.starting(function () {
                    internal.connecting = true;
                    MyAndromedaHubConnection.log("hub connection starting");
                });
                var transportType = "longPolling";
                //if(document.URL.indexOf("localhost") >= 0){
                //    transportType = "webSockets";
                //}
                hubConnection.start({
                    transport: transportType //['webSockets', 'longPolling'] 
                }).done(function () {
                    internal.connecting = false;
                    internal.connected = true;
                    MyAndromedaHubConnection.log("hub connection started!");
                });
                this.setup = true;
                return hubConnection;
            };
            MyAndromedaHubConnection.prototype.setupQueryString = function () {
                var connection = this.hubConnection;
                if (!this.options.chainId) {
                    return false;
                }
                connection.qs = {
                    'externalSiteId': this.options.externalSiteId,
                    'chainId': this.options.chainId
                };
                return true;
            };
            MyAndromedaHubConnection.log = function (data) {
                if (console && console.log) {
                    try {
                        console.log(data);
                    }
                    catch (e) { }
                }
            };
            return MyAndromedaHubConnection;
        })();
        Hubs.MyAndromedaHubConnection = MyAndromedaHubConnection;
    })(Hubs = MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
})(MyAndromeda || (MyAndromeda = {}));
