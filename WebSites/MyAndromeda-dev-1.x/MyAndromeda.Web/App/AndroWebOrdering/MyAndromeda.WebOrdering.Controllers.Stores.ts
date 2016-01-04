module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller("StoresController",($scope, contextService: Services.ContextService) => {
            var dataSource = new kendo.data.DataSource();
            $scope.storeGridOptions = {
                dataSource: dataSource,
                sortable: true,
                columns: [{
                    field: "Name",
                    title: "Store Name",
                }]
            };

            contextService.StoreSubject.subscribe((stores) => {
                Logger.Notify("I have stores" + stores.length);
                dataSource.data(stores);
            });
        });
    });
}
