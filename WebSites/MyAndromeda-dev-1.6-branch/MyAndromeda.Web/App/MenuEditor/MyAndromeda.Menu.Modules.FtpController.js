/// <reference path="MyAndromeda.Menu.Controllers.FtpController.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Modules;
        (function (Modules) {
            Modules.MenuFtpModuleName = "MenuFtpModule";
            var FtpModule = angular.module(Modules.MenuFtpModuleName, [
                "kendo.directives"
            ]);
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
        })(Modules = Menu.Modules || (Menu.Modules = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
