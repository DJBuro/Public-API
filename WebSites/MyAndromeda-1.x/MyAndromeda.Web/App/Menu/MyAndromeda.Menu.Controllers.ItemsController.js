var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Controllers) {
            Menu.Angular.ControllersInitilizations.push(function (app) {
                app.controller(MenuItemsController.Name, [
                    '$scope',
                    function ($scope) {
                        MenuItemsController.OnLoad($scope);
                        MenuItemsController.SetupScope($scope);
                    }
                ]);
            });

            var MenuItemsController = (function () {
                function MenuItemsController() {
                }
                MenuItemsController.OnLoad = function ($scope) {
                    $scope.$on('$destroy', function () {
                    });
                };

                MenuItemsController.SetupScope = function ($scope) {
                    $scope.$on('$destroy', function () {
                    });
                };
                MenuItemsController.Template = "Templates/MenuItems";
                MenuItemsController.Name = "MenuItemsController";
                MenuItemsController.Route = "/MenuItems";
                return MenuItemsController;
            })();
            Controllers.MenuItemsController = MenuItemsController;
        })(Menu.Controllers || (Menu.Controllers = {}));
        var Controllers = Menu.Controllers;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
