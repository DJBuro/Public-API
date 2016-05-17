//createDriver(driverUpdate)

module MyAndromeda.Data.Services {
    module Models {
        export interface IDriver {
            Name: string;
            Phone: string;
            
        }
    }

    var m = angular.module("MyAndromeda.Data.Drivers", []);

    export class DriverService
    {
        private $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }

        public AddToOrder(andromedaSiteId: number, orderId: string, driver: Models.IDriver)
        {
            var route = kendo.format("/data/{0}/orders/{1}/addDriver", andromedaSiteId, orderId);

            var promise = this.$http.post(route, driver);

            return promise;
        }
    }

    m.service("driverService", DriverService);
    
}