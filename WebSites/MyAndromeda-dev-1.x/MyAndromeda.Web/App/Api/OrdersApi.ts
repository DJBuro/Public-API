 
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

                                return [0, 0];

                                if (!lat) { return null; }
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
                        var promise = this.$http.post(read, options.data);

                       

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

                                return [0, 0];

                                if (!lat) { return null; }
                                return [lat, long];
                            }
                        },
                        
                    }
                },
                //sort:sort
            });

            return dataSource;
        }

        public ChangeOrderStatus(andromedaSiteId, orderId, change: Models.IOrderHeaderChangeStatus)
        {
            var route = kendo.format("/data/{0}/orders/{1}/updateStatus", andromedaSiteId, orderId);

            return this.$http.post(route, change);
        }

    }

    m.service("orderService", OrderService);
}