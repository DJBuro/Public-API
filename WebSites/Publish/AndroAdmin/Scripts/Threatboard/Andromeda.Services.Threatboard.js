///// <reference path="Andromeda.Hubs.ts" />
///// <reference path="Andromeda.Hubs.Threatboard.ts" />
///// <reference path="ThreatboardDefinitions.d.ts" />
///// <reference path="../typings/kendo/kendo.all.d.ts" />
///// <reference path="../typings/signalr/signalr.d.ts" />
//module Andromeda.Services {
//    class ThreatDashboardDiagnostics
//    {
//        public static hub: string = "ThreatDashboardDiagnosticsHub";
//        constructor(connection: any)
//        {
//            var hub = connection.createHubProxy(ThreatDashboardDataService.ThreatDashboardStoreHub);
//            hub.on("Notification", (result) => {
//            });
//        }
//    }
//    class ThreatDashboardAcsServerHubDataService implements IDashboardDataService
//    {
//        /* hub for acs server hub connection notifications */
//        public static ThreatDashboardAcsServerHub: string = "ThreatDashboardHubs";
//        public static OfflineMessage: string = "{0} hub went offline.";
//        public static OnlineMessgage: string = "{0} hub has come back online";
//        public static UpdateMessage: string = "{0} hub has been updated"; 
//        private options: IDashboardDataServiceLiveOptions;
//        private dataSource: kendo.data.DataSource;
//        public Connection: any;
//        constructor(options: IDashboardDataServiceLiveOptions, connection: any)
//        {
//            this.options = options;
//            this.Connection = connection;
//            this.SetupDataSource();
//        }
//        private NotifyMessage(type: string, message: string): void {
//            if (!this.options.ThreatboardNotifications) {
//                return;
//            }
//            this.options.ThreatboardNotifications.warning(message);
//        }
//        private SetupDataSource(): void {
//            var storeHub = this.Connection.createHubProxy(ThreatDashboardAcsServerHubDataService.ThreatDashboardAcsServerHub);
//            var hubStart = this.Connection.start();
//            this.dataSource = new kendo.data.DataSource({
//                type: "signalr",
//                //autoSync: true,
//                push: (e) => {
//                    console.log(e);
//                    //if (e.type === "create") {
//                    //    this.NotifyMessage("create", kendo.format(ThreatDashboardAcsServerHubDataService.OfflineMessage, e.items[0].Host));
//                    //}
//                    //if (e.type === "update") {
//                    //    this.NotifyMessage("update", kendo.format(ThreatDashboardAcsServerHubDataService.UpdateMessage, e.items[0].Host));
//                    //}
//                    //if (e.type === "destroy") {
//                    //    this.NotifyMessage("destroy", kendo.format(ThreatDashboardAcsServerHubDataService.OnlineMessgage, e.items[0].Host));
//                    //}
//                },
//                sort: [],
//                transport: {
//                    signalr: {
//                        promise: hubStart,
//                        hub: storeHub,
//                        server: {
//                            read: "Read",
//                            create: "Create",
//                            update: "Update",
//                            destroy: "Destroy"
//                        },
//                        client: {
//                            read: "Read",
//                            create: "Create",
//                            update: "Update",
//                            destroy: "Destroy"
//                        }
//                    }
//                },
//                schema: {
//                    model: {
//                        id: "Id",
//                        fields: {
//                            CreateAtUtc: { type: "date" }
//                        }
//                    },
//                    total: (data) => {
//                        console.log("total");
//                        console.log(data);
//                        return data.length;
//                    }
//                }
//            });
//            this.dataSource.sync();
//        }
//        public GetDataSource(): kendo.data.DataSource {
//            return this.dataSource;
//        }
//        public GetConnection(): any {
//            return this.Connection;
//        }
//    }
//    class ThreatDashboardDataService implements IDashboardDataService {
//        /* Hub for store online and offline notifications  */
//        public static ThreatDashboardStoreHub: string = "ThreatDashboardStores";
//        public static OfflineMessage: string = "{0} store went offline.";
//        public static OnlineMessgage: string = "{0} stores have come back online";
//        public static UpdateMessage: string = "{0} stores have been updated";
//        private options: IDashboardDataServiceLiveOptions;
//        private dataSource: kendo.data.DataSource;
//        public Connection: any;
//        constructor(options: IDashboardDataServiceLiveOptions)
//        {
//            this.options = options;
//            this.SetupConnection();
//            this.SetupDataSource();
//        }
//        private SetupConnection()
//        {
//            var hubUrl = this.options.List;
//            this.Connection = $.hubConnection(hubUrl);
//        }
//        private SetupDataSource()
//        {
//            var storeHub = this.Connection.createHubProxy(ThreatDashboardDataService.ThreatDashboardStoreHub);
//            var hubStart = this.Connection.start();
//            this.dataSource = new kendo.data.DataSource({
//                type: "signalr",
//                //autoSync: true,
//                push: (e) => {
//                    console.log(e);
//                    if (e.type === "create") {
//                        this.NotifyMessage("create", kendo.format(ThreatDashboardDataService.OfflineMessage, e.items.length));
//                    }
//                    if (e.type === "update") {
//                        this.NotifyMessage("update", kendo.format(ThreatDashboardDataService.UpdateMessage, e.items.length));
//                    }
//                    if (e.type === "destroy") {
//                        this.NotifyMessage("destroy", kendo.format(ThreatDashboardDataService.OnlineMessgage, e.items.length));
//                    }
//                },
//                sort: [],
//                transport: {
//                    signalr: {
//                        promise: hubStart,
//                        hub: storeHub,
//                        server: {
//                            read: "Read",
//                            create: "Create",
//                            update: "Update",
//                            destroy: "Destroy"
//                        },
//                        client: {
//                            read: "Read",
//                            create: "Create",
//                            update: "Update",
//                            destroy: "Destroy"
//                        }
//                    }
//                },
//                schema: {
//                    model: {
//                        id: "Id",
//                        fields: {
//                            CreateAtUtc: { type: "date" }
//                        }
//                    },
//                    total: (data) => {
//                        console.log("total");
//                        console.log(data);
//                        return data.length;
//                    }
//                }
//            });
//            this.dataSource.sync();
//        }
//        private NotifyMessage(type: string, message: string): void {
//            if (!this.options.ThreatboardNotifications)
//            {
//                return;
//            }
//            if (type === "create")
//            {
//                this.options.ThreatboardNotifications.warning(message);
//            } 
//        }
//        public GetDataSource(): kendo.data.DataSource {
//            return this.dataSource;
//        }
//        public GetConnection(): any {
//            return this.Connection;
//        }
//    }
//    class ThreatDashboardTestData implements IDashboardDataService {
//        private options: IDashboardDataServiceTestOptions;
//        private dataSource: kendo.data.DataSource;
//        constructor(options: IDashboardDataServiceTestOptions)
//        {
//            this.options = options;
//            this.dataSource = new kendo.data.DataSource({
//                transport: {
//                    read: this.options.List
//                },
//                schema: {
//                    data: "Data",
//                    total: "Total",
//                    errors: "Errors",
//                    aggregates: "AggregateResults"
//                },
//                serverPaging: false,
//                serverFiltering: false,
//                pageSize: 8*5
//            });
//        }
//        public GetDataSource(): kendo.data.DataSource {
//            return this.dataSource;
//        }
//        public GetConnection(): any {
//            return null;
//        }
//    }
//    export class TheatDashboard implements IDashboardAppService {
//        private options: IDashboardAppOptions;
//        private storeUpdatesDataSource: IDashboardDataService;
//        private hubUpdatesDataSource: IDashboardDataService;
//        private hub: Andromeda.Hubs.Threatboard; 
//        private threatBoardDiagnositicHub: ThreatDashboardDiagnostics;
//        private vm: kendo.data.ObservableObject;
//        private notifications: kendo.ui.Notification;
//        constructor(options: IDashboardAppOptions) {
//            this.options = options;
//            this.initNotifications();
//            this.initDataSource();
//            this.initDiagnositicsHub(); 
//            //this.hub = new Andromeda.Hubs.Threatboard({});
//            this.initViewModel();
//            this.initView();
//        }
//        private initNotifications(): void {
//            this.notifications = $("#notification").kendoNotification({
//                width: "100%",
//                position: { top: 0, left: 0 }
//            }).data("kendoNotification");
//            this.options.LiveDataSourceOptions.ThreatboardNotifications = this.notifications;
//        }
//        private initDataSource(): void {
//            if (this.options.TestDataSourceOptions) {
//                this.storeUpdatesDataSource = new ThreatDashboardTestData(this.options.TestDataSourceOptions);
//            }
//            else
//            {
//                console.log("setup store updates");
//                this.storeUpdatesDataSource = new ThreatDashboardDataService(this.options.LiveDataSourceOptions);
//                console.log("setup hub updates");
//                this.hubUpdatesDataSource = new ThreatDashboardAcsServerHubDataService(this.options.LiveDataSourceOptions, this.storeUpdatesDataSource.GetConnection());
//                console.log("setup all hub data sources");
//            }
//        }
//        private initDiagnositicsHub(): void {
//        }
//        private initViewModel() : void
//        {
//            this.vm = kendo.observable({
//                DataSource: this.GetDataSource(),
//                HubAcsServersDataSource: this.hubUpdatesDataSource.GetDataSource()
//            });
//        }
//        private initView(): void
//        {
//            kendo.bind($("#app"), this.vm);
//        }
//        public GetDataSource(): kendo.data.DataSource
//        {
//            return this.storeUpdatesDataSource.GetDataSource();
//        }
//    }
//}
//# sourceMappingURL=Andromeda.Services.Threatboard.js.map