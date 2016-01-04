var MyAndromeda;
(function (MyAndromeda) {
    (function (Hubs) {
        var HubAbstract = (function () {
            function HubAbstract(options) {
                if (!options) {
                    throw new Error("hub parameters are required.");
                    //options = {
                    //    chainId: null,
                    //    externalSiteId: null
                    //};
                }

                this.options = options;
                this.connect();
            }
            HubAbstract.prototype.connect = function () {
                this.myAndromedaHubConnection = MyAndromeda.Hubs.MyAndromedaHubsConnection.GetInstance(this.options);

                this.myAndromedaHubConnection.connect();
            };
            return HubAbstract;
        })();
        Hubs.HubAbstract = HubAbstract;

        var MyAndromedaHubsConnection = (function () {
            function MyAndromedaHubsConnection(options) {
                this.options = options;
                this.connect();
            }
            MyAndromedaHubsConnection.GetInstance = function (options) {
                if (MyAndromedaHubsConnection._instance) {
                    return MyAndromedaHubsConnection._instance;
                }

                return (MyAndromedaHubsConnection._instance = new MyAndromedaHubsConnection(options));
            };

            MyAndromedaHubsConnection.prototype.connect = function () {
                if (this.connected) {
                    return;
                }
                if (this.connecting) {
                    return;
                }
                if (this.setup) {
                    return;
                }

                var internal = this, hubConnection = $.connection.hub;

                this.hubConnection = hubConnection;

                //setup route parameters for MyAndromeda
                if (!this.setupQueryString()) {
                    return hubConnection;
                }
                ;

                hubConnection.starting(function () {
                    internal.connecting = true;
                    MyAndromedaHubsConnection.log("hub connection starting");
                });

                hubConnection.start({
                    //transport: 'longPolling'
                    transport: kendo.support.mobileOS ? 'longPolling' : 'webSockets'
                }).done(function () {
                    internal.connecting = false;
                    internal.connected = true;

                    MyAndromedaHubsConnection.log("hub connection started!");
                });

                this.setup = true;

                return hubConnection;
            };

            MyAndromedaHubsConnection.prototype.setupQueryString = function () {
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

            MyAndromedaHubsConnection.log = function (data) {
                if (console && console.log) {
                    try  {
                        console.log(data);
                    } catch (e) {
                    }
                }
            };
            return MyAndromedaHubsConnection;
        })();
        Hubs.MyAndromedaHubsConnection = MyAndromedaHubsConnection;
    })(MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
    var Hubs = MyAndromeda.Hubs;
})(MyAndromeda || (MyAndromeda = {}));
