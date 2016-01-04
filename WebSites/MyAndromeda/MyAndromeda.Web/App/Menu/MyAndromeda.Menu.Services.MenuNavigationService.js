var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(MenuNavigationService.Name, [
                    function () {
                        var instance = new MenuNavigationService();

                        return instance;
                    }
                ]);
            });

            var MenuNavigationService = (function () {
                function MenuNavigationService() {
                }
                MenuNavigationService.Name = "MenuNavigationService";
                return MenuNavigationService;
            })();
            Services.MenuNavigationService = MenuNavigationService;
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
