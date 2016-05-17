module MyAndromeda.Hubs {
    export class StoreHub {

        public static GetInstance(options: Models.IHubParameters): StoreHub {
            Logger.Notify("Get Store Hub");
            if (StoreHub._storeHubInstance) { return StoreHub._storeHubInstance; }

            StoreHub._storeHubInstance = new StoreHub(options);
            return StoreHub._storeHubInstance;
        }

        public static MenuItemChangeEvent: string = "MenuItemChangeEvent";
        private static _storeHubInstance: StoreHub;
        private menuItemChangeEvents: Array<Function> = [];
        private eventMap: Models.ILoggingMessageEvent;
        private menuHub: any;

        public user: Users.Models.IUser
        public options: Models.IHubParameters;
        public myAndromedaHubConnection: MyAndromedaHubConnection;

        constructor(options: Models.IHubParameters) {
            this.options = options;
            this.connect();


            if (StoreHub._storeHubInstance) {
                throw Error("The class has already been initialized. Use StoreHub.GetInstance");
            }

            var hubs = this.myAndromedaHubConnection.hubConnection.proxies,
                menuHub = <any>hubs["storehub"];

            this.eventMap = {};
            this.menuHub = menuHub;
            this.setupEvents();

            

        }

        private connect(): void {
            this.myAndromedaHubConnection = MyAndromedaHubConnection.GetInstance(this.options);

            this.myAndromedaHubConnection.connect();
        }

        private setupEvents(): void {
            var internal = this;
            this.menuHub.client.user = function (user) {
                internal.user = <Users.Models.IUser>user;
                StoreHub.log("User:" + internal.user.Username);
            };

            this.menuHub.client.transactionLog = function (message) {
                if ($("#MenuFtpTransactionLog").length > 0) {
                    $("#MenuFtpTransactionLog").append("<div>" + message + "</div>")
                }
            };
            this.menuHub.client.getStoreMenuVersion = function (data) {
                StoreHub.log("GetStoreMenuVersion");
                StoreHub.log(data);
            };

            this.menuHub.client.ping = function (data) {
                StoreHub.log(data);
            };

            /* valid changes have been sent from the server */
            this.menuHub.client.updateLocalItems = function (data) {
                StoreHub.log("local items need changing?");
                StoreHub.log(data);

                internal.menuItemChangeEvents.forEach((value: Function, index) => {
                    value(data);
                });
            };

            this.menuHub.client.logFtp = function (data) {
                StoreHub.log("logftp");
                StoreHub.log(data);
            };

            this.menuHub.client.storeInfo = function (data) {
                StoreHub.log(data);
            };

            this.menuHub.client.menuInfo = function (data) {
                StoreHub.log(data);
            };

            this.menuHub.client.onDebug = this.createEventMapping("onDebug");
            this.menuHub.client.onNotifierDebug = this.createEventMapping("onNotifierDebug");

            this.menuHub.client.onError = this.createEventMapping("onError");
            this.menuHub.client.onNotifierError = this.createEventMapping("onNotifierError");

            this.menuHub.client.onNotifyMail = this.createEventMapping("onNotifyMail");


            this.menuHub.client.onNotify = this.createEventMapping("onNotify");
            this.menuHub.client.onNotifierNotify = this.createEventMapping("onNotifierNotify");
            this.menuHub.client.onNotifierSuccess = this.createEventMapping("onNotifierSuccess");

            this.menuHub.client.checkingDatabaseEvent = function (data) {
                StoreHub.log("1. checkingDatabaseEvent");
                StoreHub.log(data);
            };

            this.menuHub.client.downloadingDatabaseEvent = function (data) {
                StoreHub.log("2. downloadingDatabaseEvent");
                StoreHub.log(data);
            };
            this.menuHub.client.downloadedDatabaseEvent = function (data) {
                StoreHub.log("3. downloadingDatabaseEvent");
                StoreHub.log(data);
            };

            this.menuHub.client.extractedDatabaseEvent = function (data) {
                StoreHub.log("4. extractedDatabaseEvent");
                StoreHub.log(data);
            };
            //opened a connection to both to compare altered data and versions
            this.menuHub.client.comparingDatabaseEvent = function (data) {
                StoreHub.log("5. comparingDatabaseEvent");
                StoreHub.log(data);
            };
            //database altered or version number is the same or lower 
            this.menuHub.client.notChangedDatabaseEvent = function (data) {
                StoreHub.log("5.1  notChangedDatabaseEvent");
                StoreHub.log(data);
            };
            //database is newer - taking the ftp one. 
            this.menuHub.client.copiedDatabaseEvent = function (data) {
                StoreHub.log("5.2. copiedDatabaseEvent");

            };
        }

        private createEventMapping(key: string): Function {
            this.eventMap[key] = new Array<Function>();
            var internal = this,
                action = (data) => {
                    var dispatch = internal.eventMap[key];
                    dispatch.forEach((listener) => {
                        listener(data);
                    });
                };

            return action;
        }

        public static log(data): void {
            if (console && console.log) {
                try { console.log(data); }
                catch (e) { }
            }
        }


        public bind(types: string, listener: (ev: any) => void): void {
            if (types === StoreHub.MenuItemChangeEvent) {
                this.menuItemChangeEvents.push(listener);

                return;
            }

            types.split(" ").forEach((type) => {
                var collection = this.eventMap[type];
                if (collection) { collection.push(listener); }
                else {
                    StoreHub.log("There is no type: " + type + " to bind to");
                }
            });
        }

        public getStoreMenuVersion(handler: (data: Models.IDebugDatabaseMenuViewModel) => void): void {
            this.menuHub.server.getStoreMenuVersion().done((data) => {
                handler(data);
            });
        }
    }

} 