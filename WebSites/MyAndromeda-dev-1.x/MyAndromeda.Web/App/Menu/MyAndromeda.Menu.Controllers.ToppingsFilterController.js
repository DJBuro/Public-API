var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Controllers;
        (function (Controllers) {
            Menu.Angular.ControllersInitilizations.push(function (app) {
                app.controller(ToppingsFilterController.Name, [
                    '$scope',
                    '$timeout',
                    Menu.Services.MenuToppingsService.Name,
                    Menu.Services.MenuToppingsFilterService.Name,
                    function ($scope, $timeout, menuToppingsService, menuToppingsFilterService) {
                        Menu.Logger.Debug("Setting up ToppingsFilterController");
                        ToppingsFilterController.OnLoad($scope);
                        ToppingsFilterController.SetupScope($scope, $timeout, menuToppingsFilterService);
                        Menu.Logger.Debug("Set up ToppingsFilterController");
                    }
                ]);
            });
            var ToppingsFilterController = (function () {
                function ToppingsFilterController() {
                }
                ToppingsFilterController.OnLoad = function ($scope) {
                    Menu.Logger.Notify("Toppings Filter Controller Loaded");
                };
                ToppingsFilterController.SetupScope = function ($scope, $timeout, menuToppingsFilterService) {
                    $scope.Name = menuToppingsFilterService.GetName();
                    $scope.$watch("Name", function (newValue, olderValue) {
                        Menu.Logger.Debug("ToppingsFilterController : Name changed");
                        if (newValue === olderValue) {
                            return;
                        }
                        menuToppingsFilterService.ChangeNameFilter(newValue);
                    });
                    $scope.ResetFilters = function () {
                        Menu.Logger.Debug("Reset button clicked");
                        menuToppingsFilterService.ResetFilters();
                    };
                    var observable = menuToppingsFilterService.ResetFiltersObservable.subscribe(function () {
                        $timeout(function () {
                            $scope.Name = "";
                        }, 0);
                    });
                    $scope.$on("$destroy", function () {
                        observable.dispose();
                    });
                };
                ToppingsFilterController.Name = "ToppingsFilterController";
                return ToppingsFilterController;
            }());
            Controllers.ToppingsFilterController = ToppingsFilterController;
        })(Controllers = Menu.Controllers || (Menu.Controllers = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
