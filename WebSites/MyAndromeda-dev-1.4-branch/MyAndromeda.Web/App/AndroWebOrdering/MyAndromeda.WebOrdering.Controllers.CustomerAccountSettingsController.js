/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CustomerAccountSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CustomerAccountSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        CustomerAccountSettingsController.SetupKendoMvvm($scope, contextService);
                    }
                ]);
            });
            var CustomerAccountSettingsController = (function () {
                function CustomerAccountSettingsController() {
                }
                CustomerAccountSettingsController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                };
                CustomerAccountSettingsController.SetupKendoMvvm = function ($scope, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#CustomerAccountSettingsController");
                        kendo.bind(viewElement, websiteSettings.CustomerAccountSettings);
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                CustomerAccountSettingsController.Name = "CustomerAccountSettingsController";
                return CustomerAccountSettingsController;
            })();
            Controllers.CustomerAccountSettingsController = CustomerAccountSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
