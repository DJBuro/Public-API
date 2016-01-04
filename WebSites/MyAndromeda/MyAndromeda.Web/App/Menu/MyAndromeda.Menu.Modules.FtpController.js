var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        /// <reference path="MyAndromeda.Menu.Controllers.FtpController.ts" />
        (function (Modules) {
            Modules.MenuFtpModuleName = "MenuFtpModule";

            var FtpModule = angular.module(Modules.MenuFtpModuleName, []);

            FtpModule.factory(Menu.Services.FtpService.Name, [
                '$http',
                function ($http) {
                    var instance = new Menu.Services.FtpService($http);

                    return instance;
                }
            ]);

            FtpModule.controller(Menu.Controllers.FtpController.Name, [
                '$scope',
                '$timeout',
                Menu.Services.FtpService.Name,
                function ($scope, $timeout, ftpService) {
                    Menu.Controllers.FtpController.SetupScope($scope, $timeout, ftpService);
                }
            ]);
        })(Menu.Modules || (Menu.Modules = {}));
        var Modules = Menu.Modules;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
