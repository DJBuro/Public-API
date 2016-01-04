var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
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
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
