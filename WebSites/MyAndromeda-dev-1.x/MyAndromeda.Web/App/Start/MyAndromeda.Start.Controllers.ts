module MyAndromeda.Start
{
    var controllers = angular.module("MyAndromeda.Start.Controllers", ["MyAndromeda.Start.Services"]);

    controllers.controller("chainListController", ($scope, userChainDataService: Services.UserChainDataService) => {

        var chainListOptionsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    parentId: "ParentId"
                }
            }
        });
        var chainListOptions: kendo.ui.TreeListOptions = {
            sortable: true,
            editable: false,
            filterable: true,
            dataSource: chainListOptionsDataSource,
            columns: [
                { field: "Name", title: "Name" },
                { field: "Stores", title: "Store Count" }
            ]
        };

        var chainsPromise = userChainDataService.List();
        chainsPromise.then((callback) => {

            var data = callback.data;
            chainListOptions.dataSource.data(data);

        }).catch(() => {
            alert("Something went wrong");
            });


        $scope.chainListOptions = chainListOptions;

    });

    controllers.controller("storeListController", ($scope) => {

        var storeListDataSource = new kendo.data.DataSource({});
        var storeListOptions: kendo.ui.GridOptions = {
            dataSource: storeListDataSource,
            columns: [
            ]
        };

        $scope.storeListOptions = storeListOptions;

    });

}