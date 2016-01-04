var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("StoresController", function ($scope, contextService) {
                    var dataSource = new kendo.data.DataSource();
                    $scope.storeGridOptions = {
                        dataSource: dataSource,
                        sortable: true,
                        columns: [{
                                field: "Name",
                                title: "Store Name"
                            }]
                    };
                    contextService.StoreSubject.subscribe(function (stores) {
                        WebOrdering.Logger.Notify("I have stores" + stores.length);
                        dataSource.data(stores);
                    });
                });
            });
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
