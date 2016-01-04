var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Controllers) {
            Menu.Angular.ControllersInitilizations.push(function (app) {
                app.controller(MenuNavigationController.Name, [
                    '$scope',
                    Menu.Services.MenuNavigationService.Name,
                    Menu.Services.PublishingService.Name,
                    function ($scope, menuNavigationService, publishingService) {
                        Menu.Logger.Notify("Menu navigation controller loaded");

                        MenuNavigationController.ToolbarOptions($scope, publishingService);
                        MenuNavigationController.OnLoad($scope, menuNavigationService);
                    }
                ]);
            });

            var MenuNavigationController = (function () {
                function MenuNavigationController() {
                }
                MenuNavigationController.ToolbarOptions = function ($scope, publishingService) {
                    publishingService.init();
                    $scope.Publish = function () {
                        publishingService.openWindow();
                    };
                    $scope.ToolbarOptions = {
                        items: [
                            //{ type: "button", text: "Menu Items" },
                            //{ type: "button", text: "Menu Sequencing" },
                            //{ type: "button", text: "Toppings" },
                            //{ type: "separator" },
                            { type: "button", text: "Publish", click: function () {
                                    publishingService.openWindow();
                                } }
                        ]
                    };
                };

                MenuNavigationController.OnLoad = function ($scope, menuNavigationService) {
                    $scope.$on("$destroy", function () {
                    });
                };
                MenuNavigationController.Name = "MenuNavigationController";
                return MenuNavigationController;
            })();
            Controllers.MenuNavigationController = MenuNavigationController;
        })(Menu.Controllers || (Menu.Controllers = {}));
        var Controllers = Menu.Controllers;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
