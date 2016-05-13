 
module MyAndromeda.Data.Services
{
    export module Models
    {
        export interface IOrderHeader {
            Id: string;
            ItemCount: number;
            FinalPrice: number;
            OrderPlacedTime: number;
            OrderWantedTime: number; 

            Customer: ICustomer;
        }

        export interface ICustomer {
            Id: string;
            Name: string;
            Email: string;
            Phone: string;
            Latitude: any;
            Longitude: any;
            GeoLocation: number[];
            Postcode: string;
        }

        export interface IOrderHeaderChangeStatus
        {
            StatusId: number;
        }

    }

    var m = angular.module("MyAndromeda.Data.Orders",[]);

    export class OrderService {
        private $http: ng.IHttpService;
        constructor($http: ng.IHttpService)
        {
            this.$http = $http;
        }


        public ListOrdersForMap(andromedaSiteId, start, end) {

            var read = kendo.format("/data/{0}/orders/map", andromedaSiteId);

            var sort: kendo.data.DataSourceSortItem = {
                field: "OrderPlacedTime",
                dir: "desc"
            };
            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: (options: kendo.data.DataSourceTransportReadOptions) => {
                        if (!options.data) { options.data = {}; }

                        var f = kendo.toString(start, "u");
                        var t = kendo.toString(end, "u");

                        Logger.Notify(f);
                        Logger.Notify(t);

                        var data = $.extend({}, options.data, {
                            From: f,
                            To: t
                        });
                        var promise = this.$http.post(read, data);

                        Rx.Observable
                            .fromPromise(promise)
                            .subscribe((r) => { options.success(r.data); }, (ex) => { });

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
                                var model: Models.IOrderHeader = this;
                                var lat = model.Customer.Latitude;
                                var long = model.Customer.Longitude;

                                //return [0, 0];

                                if (!lat) { return [0, 0]; }
                                return [lat, long];
                            }
                        },

                    }
                },
                //filter: [
                    
                //],
                //group: [
                //    { field: "Customer.Email" }
                //]
            });

            dataSource.bind("change", () => {

                Logger.Notify(dataSource.data());
            });

            return dataSource;

        }

        public ListOrders(andromedaSiteId): kendo.data.DataSource {
            
            var read = kendo.format("/data/{0}/debug-orders", andromedaSiteId);

            var sort: kendo.data.DataSourceSortItem = {
                field: "OrderPlacedTime",
                dir: "desc"
            };
            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: (options: kendo.data.DataSourceTransportReadOptions) => {
                        Logger.Notify("read: options");
                        Logger.Notify(options);
                        var data = <any>options.data;
                        var a = {
                            aggregate: data.aggregate,
                            filter: data.filter,
                            filters: data.filter,
                            group: data.group,
                            groups: data.group,
                            models: data.models,
                            page: data.page,
                            pageSize: data.pageSize,
                            skip: data.skip,
                            sort: data.sort,
                            sorts: data.sort,
                            take: data.take
                        };


                        var promise = this.$http.post(read, a);
                        

                        Rx.Observable
                            .fromPromise(promise)
                            .subscribe((r) => {
                                options.success(r.data);
                            }, (ex) => { });
                    },
                    parameterMap: function (data, type) {
                        //return kendo.stringify(data);

                        var a = {
                            aggregate: data.aggregate,
                            filter: data.filter,
                            filters: data.filter,
                            group: data.group,
                            groups: data.group,
                            models: data.models,
                            page: data.page,
                            pageSize: data.pageSize,
                            skip: data.skip,
                            sort: data.sort,
                            sorts: data.sort,
                            take: data.take
                        };
                        Logger.Notify("param map");
                        Logger.Notify(a);
                        return kendo.stringify(a);
                    } 
                },
                //data: "Data",
                pageSize: 10,
                page: 1,
                serverPaging: true,
                serverSorting: true,
                schema: {
                    data: "Data",
                    total: function (response) {
                        return response.Total;
                    },
                    model: {
                        id: "Id",
                        fields: {
                            OrderPlacedTime: { "type": "date" },
                            OrderWantedTime: { "type": "date" },
                            CustomerGeoLocation: function () {
                                var model: Models.IOrderHeader = this;
                                var lat = model.Customer.Latitude;
                                var long = model.Customer.Longitude;

                                //return [0, 0];

                                if (!lat) { [0, 0] }
                                return [lat, long];
                            }
                        },
                        
                    }
                },
                sort:sort
            });

            return dataSource;
        }

        public GetOrderFood(orderId: string): any {
            let orderFoodUrl: string = kendo.format('data/debug-orders/{0}/orders/food', orderId);
            return this.$http.get(orderFoodUrl);
        }

        public GetOrderDetails(orderId: string): any {
            let orderDetailsUrl: string = kendo.format('data/debug-orders/{0}/orders/details', orderId);
            return this.$http.get(orderDetailsUrl);
        }

        public GetOrderPayment(orderId: string): any {
            let orderPaymentUrl: string = kendo.format('data/debug-orders/{0}/orders/payment', orderId);
            return this.$http.get(orderPaymentUrl);
        }

        public GetOrderStatus(orderId: string): any {
            let orderStatusUrl: string = kendo.format('data/debug-orders/{0}/orders/status', orderId);
            return this.$http.get(orderStatusUrl);
        }

        public ChangeOrderStatus(andromedaSiteId, orderId, change: Models.IOrderHeaderChangeStatus)
        {
            var route = kendo.format("/data/{0}/orders/{1}/updateStatus", andromedaSiteId, orderId);

            return this.$http.post(route, change);
        }

    }

    m.service("orderService", OrderService);
}