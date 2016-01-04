var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(LegalNoticesController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        LegalNoticesController.OnLoad($scope, $timeout, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        LegalNoticesController.SetupKendoMvvm($scope, contextService);
                    }
                ]);
            });

            var LegalNoticesController = (function () {
                function LegalNoticesController() {
                }
                LegalNoticesController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                };

                LegalNoticesController.SetupKendoMvvm = function ($scope, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var viewElement = $("#LegalNoticesController");
                        kendo.bind(viewElement, websiteSettings.LegalNotices);
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                LegalNoticesController.Name = "LegalNoticesController";
                return LegalNoticesController;
            })();
            Controllers.LegalNoticesController = LegalNoticesController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
