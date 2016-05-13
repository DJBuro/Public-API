module MyAndromeda.Debug {

    var gridApp = angular.module("MyAndromeda.Debug.OrdersApp",
        [
            "kendo.directives",
            "MyAndromeda.Resize",
            "MyAndromeda.Data.Orders",
            "MyAndromeda.Data.Drivers"
        ])
        .controller('OrderDetailsController', [OrderDetailsController]);

    gridApp.run(($templateCache: ng.ITemplateCacheService) => {
        Logger.Notify("OrdersApp Started");

        angular.element('script[type="text/template"]').each((i, element: HTMLElement) => {
            $templateCache.put(element.id, element.innerHTML);
        });
    });

    gridApp.controller("startController", ($scope) => {
        $scope.onSelect = (e: kendo.ui.MenuSelectEvent) => {
            //e.preventDefault();
        };

    });

    var gridTempaltes = {
        orderId: "<grid-order-id></grid-order-id>",
        OrderItemCount: "<grid-order-item-count></<grid-order-item-count>",
        OrderPlacedTime: "<grid-order-placed-time></grid-order-placed-time>",
        OrderWantedTime: "<grid-order-wanted-time></grid-order-wanted-time>",
        OrderCustomer: "<grid-order-customer></grid-order-customer>"
    };

    function OrderDetailsController() {
        var vm = this;
        let orderId: string = ''; //TODO: Get actual ID of order
        let orderService: Data.Services.OrderService;
        vm.food = orderService.GetOrderFood(orderId);
        vm.details = orderService.GetOrderDetails(orderId);
        vm.payments = orderService.GetOrderPayment(orderId);
        vm.status = orderService.GetOrderStatus(orderId);
    }
    ///need controllers. 
    //function getFood(orderId: string) {
    //    console.log(1);

    //    //let orderService: Data.Services.OrderService;
    //    //let food = orderService.GetOrderFood(orderId);
    //}

    //function getDetails(orderId: string) {
    //    console.log(2);

    //    //let orderService: Data.Services.OrderService;
    //    //let details = orderService.GetOrderDetails(orderId);
    //    //console.log(details);
    //}

    //function getPayments(orderId: string) {
    //    console.log(3);

    //    //let orderService: Data.Services.OrderService;
    //    //let payment = orderService.GetOrderPayment(orderId);
    //    //console.log(payment);
    //}

    //function getStatus(orderId: string) {
    //    console.log(4);

    //    //let orderService: Data.Services.OrderService;
    //    //let status = orderService.GetOrderStatus(orderId);
    //    //console.log(status);
    //}

    gridApp.directive("ordersGrid", () => {
        return {
            name: "ordersGrid",
            restrict: "E",
            scope: {
                andromedaSiteId: "@"
            },
            templateUrl: "GridTemplate.html",
            transclude: true,
            link: ($scope,
                instanceElement: ng.IAugmentedJQuery,
                instanceAttributes: ng.IAttributes,
                controller: any,
                transclude: ng.ITranscludeFunction
                ) => {

                transclude($scope, (clone, scope) => {
                    instanceElement.append(clone);
                });
            },
            controller: ($scope,
                orderService: Data.Services.OrderService,
                driverService: Data.Services.DriverService) => {
                var andromedaSiteId = $scope.andromedaSiteId;
                var dataSource = orderService.ListOrders(andromedaSiteId);

                var rowTemplate = "<tr orders-grid-row-template></tr>";
                var detailTemplate = "<order-detail-template></order-detail-template>";

                var gridOptions: kendo.ui.GridOptions = {
                    dataSource: dataSource,
                    sortable: true,
                    filterable: true,
                    pageable: {
                        numeric: true,
                        refresh: true,
                        pageSizes: [10, 25, 50],
                        previousNext: true,
                        input: false,
                        info: false
                    },
                    columns: [
                        { title: "Id", field: "Id", template: gridTempaltes.orderId },
                        { title: "Items", field: "ItemCount", template: gridTempaltes.OrderItemCount },
                        { title: "Final Price", field: "FinalPrice" },
                        { title: "Placed Time", field: "OrderPlacedTime", template: gridTempaltes.OrderPlacedTime },
                        { title: "Wanted Time", field: "OrderWantedTime", template: gridTempaltes.OrderWantedTime },
                        { title: "Customer Name", field: "Customer.Name", template: gridTempaltes.OrderCustomer }
                    ],
                    detailTemplate: kendo.template(detailTemplate)
                    //rowTemplate: rowTemplate //"<orders-grid-row-template></orders-grid-row-template>"
                };

                var mapOptions = {
                    center: [0,0],
                    zoom: 2,
                    layers: [
                        {
                            type: "tile",
                            urlTemplate: "http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png",
                            subdomains: ["a", "b", "c"],
                            attribution: "&copy; <a href='http://osm.org/copyright'>OpenStreetMap contributors</a>"
                        },
                        {
                            type: "marker",
                            dataSource: dataSource,
                            locationField: "Customer.GeoLocation"
                        }
                    ],
                    //markers: [{
                    //    location: [30.268107, -97.744821],
                    //    shape: "pin",
                    //}]
                };


                $scope.createDriver = (orderId, driver) => {
                    driverService.AddToOrder(andromedaSiteId, orderId, driver);
                };
                $scope.changeStatus = (orderId, statusId) => {
                    orderService.ChangeOrderStatus(andromedaSiteId, orderId, {
                        StatusId: statusId
                    });
                };
                $scope.gridOptions = gridOptions;
                $scope.mapOptions = mapOptions;
            }
        };
    });

   

    var orderColumn = (name, template) => {
        var a = {
            name: name,
            transclude: true,
            link: ($scope,
                instanceElement: ng.IAugmentedJQuery,
                instanceAttributes: ng.IAttributes,
                controller: any,
                transclude: ng.ITranscludeFunction
                ) => {

                transclude($scope, (clone, scope) => {
                    instanceElement.append(clone);
                });
            },
            controller: null,
            //controller: ($scope) => {
            //},
            templateUrl: template
        };

        return () => a;
    };

    var orderIdColumn = () => {
        var col = orderColumn("gridOrderId", "order-id-column.html");
        var r = col();
        r.controller = ($scope, $element: JQuery) => {
            var target = $element.find(".hasMenu");
            $scope.target = target;

            $scope.openMenu = ($event) => {

                var menu: kendo.ui.ContextMenu = $scope.rowMenu;
                menu.open($event.clientX, $event.clientY);
                Logger.Notify($event);
            };
        };

        return () => r;
    };

    gridApp.directive("gridOrderId", orderIdColumn());

    //todo: need to rename the directive to something better 
    gridApp.directive("gridOrderItems", () => {
        //usage <grid-order-item-count orderId="[an order ref]"></grid-order-item-count
        return {
            name: "gridOrderItems",
            scope: { orderId: "=" },  
            templateUrl: "order-items.html",
            controller: ($scope, $timeout, orderService: MyAndromeda.Data.Services.OrderService) => {
                Logger.Notify("gridOrderItems controller");
                Logger.Notify("Scope Id:" + $scope.orderId);
                let orderId: string = $scope.orderId;

                //not found : 
                //http://localhost:50262/Debug/data/debug-orders/69540ecc-dfdc-49bf-bb01-f74efb009990/orders/food
                let promise = orderService.GetOrderFood(orderId);
                let context = {
                    foodItems : [] 
                }; 
                promise.then(result => {
                    Logger.Notify("food items data:");
                    Logger.Notify(result.data);
                    $timeout(() => {
                        context.foodItems = result.data;
                    });
                });

                $scope.context = context;

            }
        };
    });
    gridApp.directive("gridOrderPlacedTime", orderColumn("gridOrderPlacedTime", "order-placed-time-column.html"));
    gridApp.directive("gridOrderWantedTime", orderColumn("gridOrderWantedTime", "order-wanted-time-column.html"));
    gridApp.directive("gridOrderCustomer", orderColumn("gridOrderCustomer", "order-customer-column.html"));
    gridApp.directive("orderDetailTemplate", () => {
        return {
            name: "orderDetailTemplate",
            transclude: true,
            link: ($scope,
                instanceElement: ng.IAugmentedJQuery,
                instanceAttributes: ng.IAttributes,
                controller: any,
                transclude: ng.ITranscludeFunction
                ) => {

                transclude($scope, (clone, scope) => {
                    instanceElement.append(clone);
                });
            },
            controller: null,
            //controller: ($scope) => {
            //},
            templateUrl: "order-detail-template.html"
        };
    });
    gridApp.directive('tabs', function () {
        return {
            restrict: 'E',
            transclude: true,
            scope: {},
            controller: ["$scope", function ($scope) {
                var panes = $scope.panes = [];

                $scope.select = function (pane) {
                    angular.forEach(panes, function (pane) {
                        pane.selected = false;
                    });
                    pane.selected = true;
                }

                this.addPane = function (pane) {
                    if (panes.length == 0) $scope.select(pane);
                    panes.push(pane);
                }
            }],
            template:`
            <div class="tabbable">
                <ul class="nav nav-tabs">
                    <li ng-repeat="pane in panes" ng-class="{active:pane.selected}">
                        <a href="" ng-click="select(pane)">{{pane.title}}</a>
                    </li>
                </ul>
                <div class="tab-content" ng-transclude></div>
            </div>
            `,
            replace: true
        };
    });
    gridApp.directive('pane', function () {
        return {
            require: '^tabs',
            restrict: 'E',
            transclude: true,
            scope: { title: '@' },
            link: function (scope, element, attrs, tabsCtrl : any) {
                tabsCtrl.addPane(scope);
            },
            template:
            `<div class="tab-pane" ng-class="{active: selected}" ng-transclude>
            </div>`,
            replace: true
        };
    });

    
    //order-wanted-time-column.html
    //order-customer-column.html

    //gridApp.directive("ordersGridRowTemplate", () => {
    //    return {
    //        name: "ordersGridRowTemplate",
    //        transclude: true,
    //        link: ($scope,
    //            instanceElement: ng.IAugmentedJQuery,
    //            instanceAttributes: ng.IAttributes,
    //            controller: any,
    //            transclude: ng.ITranscludeFunction
    //            ) => {

    //            transclude($scope, (clone, scope) => {
    //                instanceElement.append(clone);
    //            });
    //        },
    //        //controller: ($scope) => {
    //        //},
    //        templateUrl: "GridRowTemplate.html"
    //    };   
    //});

    //gridApp.directive("")

    export var gridAppSetup = (id) => {
        var element = document.getElementById(id);
        angular.bootstrap(element, ["MyAndromeda.Debug.OrdersApp"]);
    };
}