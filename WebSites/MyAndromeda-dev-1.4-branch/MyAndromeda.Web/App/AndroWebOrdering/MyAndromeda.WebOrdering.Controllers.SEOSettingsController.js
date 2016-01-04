/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(SEOSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        SEOSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        SEOSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var SEOSettingsController = (function () {
                function SEOSettingsController() {
                }
                SEOSettingsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        if ($scope.SEOSettingsValidator.validate()) {
                            webOrderingWebApiService.Update();
                        }
                    };
                };
                SEOSettingsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (webSiteSettings) {
                        var viewElement = $("#SEOSettingsController");
                        kendo.bind(viewElement, webSiteSettings.SEOSettings);
                        $scope.ShowSEODescription = webSiteSettings.SEOSettings.get("IsEnableDescription");
                        //added 500ms timeout as there are random issues. 
                        $timeout(function () {
                        }, 500, true);
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                SEOSettingsController.Name = "SEOSettingsController";
                return SEOSettingsController;
            })();
            Controllers.SEOSettingsController = SEOSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
