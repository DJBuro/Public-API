var MyAndromeda;
(function (MyAndromeda) {
    var Data;
    (function (Data) {
        var Services;
        (function (Services) {
            var m = angular.module("MyAndromeda.Data.Orders", []);
            var OrderService = (function () {
                function OrderService($http) {
                    this.$http = $http;
                }
                OrderService.prototype.ListOrdersForMap = function (andromedaSiteId, start, end) {
                    var _this = this;
                    var read = kendo.format("/data/{0}/orders/map", andromedaSiteId);
                    var sort = {
                        field: "OrderPlacedTime",
                        dir: "desc"
                    };
                    var dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                if (!options.data) {
                                    options.data = {};
                                }
                                var f = kendo.toString(start, "u");
                                var t = kendo.toString(end, "u");
                                MyAndromeda.Logger.Notify(f);
                                MyAndromeda.Logger.Notify(t);
                                var data = $.extend({}, options.data, {
                                    From: f,
                                    To: t
                                });
                                var promise = _this.$http.post(read, data);
                                Rx.Observable
                                    .fromPromise(promise)
                                    .subscribe(function (r) { options.success(r.data); }, function (ex) { });
                            }
                        },
                        //data: "Data",
                        //pageSize: 10,
                        //page: 1,
                        serverPaging: true,
                        serverSorting: true,
                        schema: {
                            //data: "Data",
                            //total: "Total",
                            model: {
                                id: "Id",
                                fields: {
                                    OrderPlacedTime: { "type": "date" },
                                    OrderWantedTime: { "type": "date" },
                                    CustomerGeoLocation: function () {
                                        var model = this;
                                        var lat = model.Customer.Latitude;
                                        var long = model.Customer.Longitude;
                                        return [0, 0];
                                        if (!lat) {
                                            return null;
                                        }
                                        return [lat, long];
                                    }
                                }
                            }
                        }
                    });
                    dataSource.bind("change", function () {
                        MyAndromeda.Logger.Notify(dataSource.data());
                    });
                    return dataSource;
                };
                OrderService.prototype.ListOrders = function (andromedaSiteId) {
                    var _this = this;
                    var read = kendo.format("/data/{0}/debug-orders", andromedaSiteId);
                    var sort = {
                        field: "OrderPlacedTime",
                        dir: "desc"
                    };
                    var dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                var promise = _this.$http.post(read, options.data);
                                Rx.Observable
                                    .fromPromise(promise)
                                    .subscribe(function (r) { options.success(r.data); }, function (ex) { });
                            }
                        },
                        //data: "Data",
                        //pageSize: 10,
                        //page: 1,
                        serverPaging: true,
                        serverSorting: true,
                        schema: {
                            //data: "Data",
                            //total: "Total",
                            model: {
                                id: "Id",
                                fields: {
                                    OrderPlacedTime: { "type": "date" },
                                    OrderWantedTime: { "type": "date" },
                                    CustomerGeoLocation: function () {
                                        var model = this;
                                        var lat = model.Customer.Latitude;
                                        var long = model.Customer.Longitude;
                                        return [0, 0];
                                        if (!lat) {
                                            return null;
                                        }
                                        return [lat, long];
                                    }
                                }
                            }
                        }
                    });
                    return dataSource;
                };
                OrderService.prototype.ChangeOrderStatus = function (andromedaSiteId, orderId, change) {
                    var route = kendo.format("/data/{0}/orders/{1}/updateStatus", andromedaSiteId, orderId);
                    return this.$http.post(route, change);
                };
                return OrderService;
            })();
            Services.OrderService = OrderService;
            m.service("orderService", OrderService);
        })(Services = Data.Services || (Data.Services = {}));
    })(Data = MyAndromeda.Data || (MyAndromeda.Data = {}));
})(MyAndromeda || (MyAndromeda = {}));
