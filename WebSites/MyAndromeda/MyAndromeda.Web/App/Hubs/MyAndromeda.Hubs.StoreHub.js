var MyAndromeda;
(function (MyAndromeda) {
    (function (Hubs) {
        var StoreHub = (function () {
            function StoreHub(options) {
                this.menuItemChangeEvents = [];
                this.options = options;
                this.connect();

                if (StoreHub._storeHubInstance) {
                    throw Error("The class has already been initialized. Use StoreHub.GetInstance");
                }

                var hubs = this.myAndromedaHubConnection.hubConnection.proxies, menuHub = hubs.storehub;

                this.eventMap = {};
                this.menuHub = menuHub;
                this.setupEvents();
            }
            StoreHub.GetInstance = function (options) {
                if (StoreHub._storeHubInstance) {
                    return StoreHub._storeHubInstance;
                }

                StoreHub._storeHubInstance = new StoreHub(options);
                return StoreHub._storeHubInstance;
            };

            StoreHub.prototype.connect = function () {
                this.myAndromedaHubConnection = Hubs.MyAndromedaHubConnection.GetInstance(this.options);

                this.myAndromedaHubConnection.connect();
            };

            StoreHub.prototype.setupEvents = function () {
                var internal = this;
                this.menuHub.client.user = function (user) {
                    internal.user = user;
                    StoreHub.log("User:" + internal.user.Username);
                };

                this.menuHub.client.transactionLog = function (message) {
                    if ($("#MenuFtpTransactionLog").length > 0) {
                        $("#MenuFtpTransactionLog").append("<div>" + message + "</div>");
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

                    internal.menuItemChangeEvents.forEach(function (value, index) {
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
            };

            StoreHub.prototype.createEventMapping = function (key) {
                this.eventMap[key] = new Array();
                var internal = this, action = function (data) {
                    var dispatch = internal.eventMap[key];
                    dispatch.forEach(function (listener) {
                        listener(data);
                    });
                };

                return action;
            };

            StoreHub.log = function (data) {
                if (console && console.log) {
                    try  {
                        console.log(data);
                    } catch (e) {
                    }
                }
            };

            StoreHub.prototype.bind = function (types, listener) {
                var _this = this;
                if (types === StoreHub.MenuItemChangeEvent) {
                    this.menuItemChangeEvents.push(listener);

                    return;
                }

                types.split(" ").forEach(function (type) {
                    var collection = _this.eventMap[type];
                    if (collection) {
                        collection.push(listener);
                    } else {
                        StoreHub.log("There is no type: " + type + " to bind to");
                    }
                });
            };

            StoreHub.prototype.getStoreMenuVersion = function (handler) {
                this.menuHub.server.getStoreMenuVersion().done(function (data) {
                    handler(data);
                });
            };
            StoreHub.MenuItemChangeEvent = "MenuItemChangeEvent";
            return StoreHub;
        })();
        Hubs.StoreHub = StoreHub;
    })(MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
    var Hubs = MyAndromeda.Hubs;
})(MyAndromeda || (MyAndromeda = {}));
