var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Controllers;
        (function (Controllers) {
            Menu.Angular.ControllersInitilizations.push(function (app) {
                app.controller(ToppingsController.Name, [
                    '$scope',
                    Menu.Services.MenuToppingsService.Name,
                    function ($scope, menuToppingsService) {
                        Menu.Logger.Debug("Start: Setting up Toppings controller");
                        ToppingsController.OnLoad($scope);
                        ToppingsController.SetupScope($scope);
                        ToppingsController.SetupItemTemplate($scope);
                        ToppingsController.SetupDataSource($scope, menuToppingsService);
                        Menu.Logger.Debug("Complete: Setting up Toppings controller");
                    }
                ]);
            });
            var ToppingsController = (function () {
                function ToppingsController() {
                }
                ToppingsController.OnLoad = function ($scope) {
                    Menu.Logger.Notify("Toppings Controller Loaded");
                };
                ToppingsController.SetupScope = function ($scope) {
                };
                ToppingsController.SetupItemTemplate = function ($scope) {
                    $scope.MenuToppingsListViewTemplate = $("#MenuToppingsListViewTemplate").html();
                    $scope.MenuToppingsEditListViewTemplate = $("#MenuToppingsEditListViewTemplate").html();
                };
                ToppingsController.SetupDataSource = function ($scope, service) {
                    var dataSource = service.GetDataSource();
                    $scope.DataSource = dataSource;
                    var start = new Rx.Subject();
                    var end = new Rx.Subject();
                    var startLoading = start.subscribe(function (e) {
                        Menu.Logger.Debug("Start Loading");
                        kendo.ui.progress($("body"), true);
                    });
                    var endLoading = end.subscribe(function (e) {
                        Menu.Logger.Debug("Loading complete");
                        kendo.ui.progress($("body"), false);
                    });
                    dataSource.bind("requestStart", function () { start.onNext(true); });
                    dataSource.bind("requestEnd", function () { end.onNext(true); });
                    $scope.$on("$destroy", function () {
                        startLoading.dispose();
                        endLoading.dispose();
                    });
                };
                ToppingsController.Name = "ToppingsController";
                ToppingsController.Route = "/";
                ToppingsController.Template = function () { return $("#MenuTemplate").html(); };
                return ToppingsController;
            })();
            Controllers.ToppingsController = ToppingsController;
        })(Controllers = Menu.Controllers || (Menu.Controllers = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
