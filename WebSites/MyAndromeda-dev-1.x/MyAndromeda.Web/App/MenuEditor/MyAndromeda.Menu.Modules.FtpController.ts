/// <reference path="MyAndromeda.Menu.Controllers.FtpController.ts" />
module MyAndromeda.Menu.Modules {
    export var MenuFtpModuleName ="MenuFtpModule";
    
    var FtpModule = angular.module(MenuFtpModuleName, [
        "kendo.directives"
    ]);

    FtpModule.factory(Services.FtpService.Name, [
        '$http',
        ($http) => {
            var instance = new Services.FtpService($http);

            return instance;
        }
    ]);
    
    FtpModule.controller(Controllers.FtpController.Name, [
        '$scope',
        '$timeout',
        Services.FtpService.Name,
        ($scope, $timeout, ftpService: MyAndromeda.Menu.Services.FtpService) => {

            Controllers.FtpController.SetupScope($scope, $timeout, ftpService);
        } 
    ]);
} 