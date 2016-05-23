/// <reference path="SyncronizationHubDefinitions.d.ts" />
/// <reference path="MenuHubDefinitions.d.ts" />
/// <reference path="../../typings/signalr/signalr.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MyAndromeda;
(function (MyAndromeda) {
    (function (Hubs) {
        var SynchronizationHub = (function (_super) {
            __extends(SynchronizationHub, _super);
            function SynchronizationHub(options) {
                _super.call(this, options);

                var hubs = $.connection, hub = hubs.hub, cloudHub = hubs.cloudSynchronization;

                cloudHub.client.ping = function (date) {
                    SynchronizationHub.log("ping fired");
                    SynchronizationHub.log(date);
                };

                cloudHub.client.startedSynchronization = function (data) {
                    SynchronizationHub.log("started fired");
                };

                cloudHub.client.completedSynchronization = function (data) {
                    SynchronizationHub.log("completed fired");
                };

                cloudHub.client.errorSynchronization = function (data) {
                    SynchronizationHub.log("error fired");
                };

                cloudHub.client.skippedSynchronization = function (data) {
                    SynchronizationHub.log("skip fired");
                };

                if (!cloudHub) {
                    cloudHub = this.produceConnection();
                }
                this.client = cloudHub.client;
                //hub.start()
                //    .done(function () {
                //        SynchronizationHub.log("connected");
                //    })
                //    .fail(function () {
                //        SynchronizationHub.log("connection failed");
                //    });
            }
            SynchronizationHub.log = function (data) {
                if (console && console.log) {
                    try  {
                        console.log(data);
                    } catch (e) {
                    }
                }
            };

            SynchronizationHub.prototype.produceConnection = function () {
                var connection = $.hubConnection();

                var proxy = connection.createHubProxy("CloudSynchronization");

                //proxy.on("startedSynchronization", (msg: any) => {
                //    this.client.startedSynchronization(msg);
                //});
                //proxy.on("completedSynchronization", (msg: any) => {
                //});
                //proxy.on("completedSynchronization", function (d) {
                //    this.client.completedSynchronization(d);
                //});
                //proxy.on("errorSynchronization", function (d) {
                //    this.client.errorSynchronization(d);
                //});
                connection.start().done(function () {
                    SynchronizationHub.log("Connected");
                }).fail(function () {
                    SynchronizationHub.log("Could not connect");
                });

                return proxy;
            };
            SynchronizationHub.STARTED = "started";
            SynchronizationHub.COMPLETED = "completed";
            SynchronizationHub.ERROR = "error";
            return SynchronizationHub;
        })(MyAndromeda.Hubs.HubAbstract);

        var SynchronizationHubService = (function () {
            function SynchronizationHubService(options) {
                this.options = options;
                this.hub = new SynchronizationHub(options);

                this.viewModel = kendo.observable({
                    started: [],
                    completed: [],
                    errors: []
                });
            }
            SynchronizationHubService.prototype.initEvents = function () {
                var internal = this, hub = this.hub.myAndromedaHubConnection.hubConnection.proxies.cloudsynchronization, client = hub.client;

                client.startedSynchronization = function (data) {
                    SynchronizationHub.log("started fired");
                    var models = internal.viewModel.get("started");
                    models.push(data);
                };
                client.completedSynchronization = function (data) {
                    SynchronizationHub.log("completed fired");
                    var models = internal.viewModel.get("completed");
                    models.push(data);
                };
                client.errorSynchronization = function (data) {
                    SynchronizationHub.log("error fired");
                    var models = internal.viewModel.get("errors");
                    models.push(data);
                };
                client.tasks = function (data) {
                    var models = internal.viewModel.get("started");
                    models.push({
                        Note: "Checking how many tasks",
                        TimeStamp: new Date(),
                        Count: data
                    });
                };
            };

            SynchronizationHubService.prototype.init = function () {
                kendo.bind(this.options.id, this.viewModel);

                this.initEvents();
            };
            return SynchronizationHubService;
        })();
        Hubs.SynchronizationHubService = SynchronizationHubService;
    })(MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
    var Hubs = MyAndromeda.Hubs;
})(MyAndromeda || (MyAndromeda = {}));
