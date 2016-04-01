/// <reference path="../../scripts/typings/signalr/signalr.d.ts" />

module MyAndromeda.Hubs {
    export class SynchronizationHub {
        static STARTED: string = "started";
        static COMPLETED: string = "completed";
        static ERROR: string = "error";

        public static log(data): void {
            if (console && console.log) {
                try { console.log(data); }
                catch (e) { }
            }
        }
        private client: Models.ISynchronizationHub;

        public options: Models.IHubParameters;
        public myAndromedaHubConnection: MyAndromedaHubConnection;

        constructor(options: Models.IHubParameters) {
            this.options = options;
            
            var hubs = <any>$.connection,
                hub = hubs.hub,
                cloudHub = hubs.cloudSynchronizationHub;

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
            this.client = <Models.ISynchronizationHub>cloudHub.client;

            this.connect();

            //hub.start()
            //    .done(function () {
            //        SynchronizationHub.log("connected");
            //    })
            //    .fail(function () {
            //        SynchronizationHub.log("connection failed");
            //    });
        }

        private connect(): void {
            this.myAndromedaHubConnection = MyAndromedaHubConnection.GetInstance(this.options);

            this.myAndromedaHubConnection.connect();
        }

        //produceConnection(): any {
        //    var connection = $.hubConnection();

        //    var proxy = connection.createHubProxy("CloudSynchronizationHub");

        //    //proxy.on("startedSynchronization", (msg: any) => {
        //    //    this.client.startedSynchronization(msg);
        //    //});
        //    //proxy.on("completedSynchronization", (msg: any) => {
        //    //});
        //    //proxy.on("completedSynchronization", function (d) {
        //    //    this.client.completedSynchronization(d);
        //    //});
        //    //proxy.on("errorSynchronization", function (d) {
        //    //    this.client.errorSynchronization(d);
        //    //});

        //    connection.start()
        //        .done(function () { SynchronizationHub.log("Connected"); })
        //        .fail(function () { SynchronizationHub.log("Could not connect"); });

        //    return proxy;
        //}
    }

    export class SynchronizationHubService {
        private hub: SynchronizationHub;
        public options: Models.ISynchronizationHubServiceOptions;
        public viewModel: Models.IDisplayModelObservable;

        constructor(options: Models.ISynchronizationHubServiceOptions) {
            this.options = options;
            this.hub = new SynchronizationHub(options);

            this.viewModel = kendo.observable({
                started: [],
                completed: [],
                errors: []
            });
        }

        initEvents(): void {
            var internal = this,
                hub = <any>this.hub.myAndromedaHubConnection.hubConnection.proxies.cloudsynchronizationhub,
                client = hub.client;

            client.startedSynchronization = function (data) {
                SynchronizationHub.log("started fired");
                var models: kendo.data.ObservableArray = internal.viewModel.get("started");
                models.push(data);
            };
            client.completedSynchronization = function (data) {
                SynchronizationHub.log("completed fired");
                var models: kendo.data.ObservableArray = internal.viewModel.get("completed");
                models.push(data);
            };
            client.errorSynchronization = function (data) {
                SynchronizationHub.log("error fired");
                var models: kendo.data.ObservableArray = internal.viewModel.get("errors");
                models.push(data);
            };
            client.tasks = function (data) {
                var models: kendo.data.ObservableArray = internal.viewModel.get("started");
                models.push({
                    Note: "Checking how many tasks",
                    TimeStamp: new Date(),
                    Count: data
                });
            }
        }

        init(): void {
            kendo.bind(this.options.id, this.viewModel);

            this.initEvents();
        }
    }

}
