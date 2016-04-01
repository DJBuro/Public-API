/// <reference path="../../scripts/typings/signalr/signalr.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Hubs;
    (function (Hubs) {
        var SynchronizationHub = (function () {
            function SynchronizationHub(options) {
                this.options = options;
                var hubs = $.connection, hub = hubs.hub, cloudHub = hubs.cloudSynchronizationHub;
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
                //if (!cloudHub) { cloudHub = this.produceConnection(); }
                this.client = cloudHub.client;
                this.connect();
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
                    try {
                        console.log(data);
                    }
                    catch (e) { }
                }
            };
            SynchronizationHub.prototype.connect = function () {
                this.myAndromedaHubConnection = Hubs.MyAndromedaHubConnection.GetInstance(this.options);
                this.myAndromedaHubConnection.connect();
            };
            SynchronizationHub.STARTED = "started";
            SynchronizationHub.COMPLETED = "completed";
            SynchronizationHub.ERROR = "error";
            return SynchronizationHub;
        })();
        Hubs.SynchronizationHub = SynchronizationHub;
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
                var internal = this, hub = this.hub.myAndromedaHubConnection.hubConnection.proxies.cloudsynchronizationhub, client = hub.client;
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
    })(Hubs = MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
})(MyAndromeda || (MyAndromeda = {}));
