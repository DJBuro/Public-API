module MyAndromeda.Start
{
    var controllers = angular.module("MyAndromeda.Start.Controllers",
        ["MyAndromeda.Start.Services", "kendo.directives"]);

    controllers.controller("chainListController", ($scope, userChainDataService: Services.UserChainDataService) => {

        var chainActionTemplate = $("#chain-actions-template").html();
        var storeTemplate = $("#chain-template").html();

        Logger.Notify("store template");
        Logger.Notify(storeTemplate);

        var chainListOptionsDataSource = new kendo.data.TreeListDataSource({
            //data: [
            //    { Id: 1, Name: "test", ParentId: null },
            //    { Id: 2, Name: "test 2", ParentId: 1 }
            //],
            schema: {
                model: {
                    id: "Id",
                    parentId: "ParentId",
                    fields: {
                        Name: {
                            field: "Name", type: "string"
                        },
                        StoreCount: { field: "StoreCount", type: "number" },
                        ParentId: { field: "ParentId", nullable: true },
                        
                    },
                    StoreCountLabel: function () {
                        var model: Services.Models.IChain = this;
                        if (model.ChildChainCount > 0) {
                            var chainTotal = model.ChildStoreCount + model.StoreCount;
                            return model.StoreCount + "/" + chainTotal;
                            
                        }
                        return "" + model.StoreCount;
                    },
                    expanded: true
                }
            }
        });

        var chainListOptions: kendo.ui.TreeListOptions = {
            sortable: true,
            editable: false,
            filterable: {
                //mode: "row"
            },
            //autoBind: false,
            resizable: true,
            dataSource: chainListOptionsDataSource,
            columns: [
                //{ title: "Actions", width: 170, template: chainActionTemplate, filterable: false }
                { field: "Name", title: "Name", template: storeTemplate },
                //{ field: "StoreCount", title: "Store Count", width: 100 }
            ]
        };

        var chainsPromise = userChainDataService.List();
        chainsPromise.then((callback) => {
            Logger.Notify("chain call back data");
            var data = callback.data;
            chainListOptionsDataSource.data(data);

            var dataSourceData = chainListOptionsDataSource.data()

            Logger.Notify(dataSourceData);
        }).catch(() => {
            alert("Something went wrong");
        });


        $scope.chainListOptions = chainListOptions;

    });

    controllers.directive("chainGrid", () => {
        return {
            name: "chainGrid",
            controller: "storeListController",
            restrict: "E",
            scope: {
                chain : "="
            },
            templateUrl: "store-list.html"
        };
    });

    controllers.controller("storeListController", ($scope, $timeout, userStoreDataService: Services.UserStoreDataService) => {
        var chain: Services.Models.IChain = $scope.chain;

        var status = {
            hasStores: false,
            hideStores: true
        };

        $scope.status = status;

        var storeListDataSource = new kendo.data.DataSource({
            transport: {
                read: (options) => {

                    var promise = userStoreDataService.ListStoresByChainId(chain.Id);

                    promise.then((callback) => {
                        options.success(callback.data);

                        if (callback.data.length > 0) {
                            $timeout(() => {
                                status.hasStores = true;
                            });
                        }
                    });
                }
            },
            sort: [
                { field: "Name", dir: "asc" }
            ],
            serverSorting: false,
            serverFiltering: false,
            serverPaging: false
        });

        storeListDataSource.read();

        var storeTemplate = $("#store-list-template").html();
        

        var storeListOptions: kendo.ui.GridOptions = {
            autoBind: true,
            sortable: true,
            dataSource: storeListDataSource,
            filterable: {
                mode: "row"
            },
            columns: [
                { title: "Name", field: "Name", width: 200 },
                { title: "Actions", template: storeTemplate }
            ]
        };

        $scope.storeListOptions = storeListOptions;

    });

}