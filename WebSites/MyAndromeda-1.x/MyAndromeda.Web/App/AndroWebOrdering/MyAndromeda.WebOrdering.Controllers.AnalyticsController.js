var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(AnalyticsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        AnalyticsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        AnalyticsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var AnalyticsController = (function () {
                function AnalyticsController() {
                }
                AnalyticsController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        //console.log("save");
                        webOrderingWebApiService.Update();
                    };
                    //going to move to just the analytics id
                    //$scope.EncodeScript = (analyticsScript : string) => {
                    //    webOrderingWebApiService.Context.Model.AnalyticsSettings.set("AnalyticsScript", encodeURI(analyticsScript));
                    //};
                };

                AnalyticsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var viewElement = $("#AnalyticsController");
                        kendo.bind(viewElement, websiteSettings.AnalyticsSettings);

                        $timout(function () {
                            //want to decode here
                            //websiteSettings.AnalyticsSettings.AnalyticsScript = decodeURI(websiteSettings.AnalyticsSettings.AnalyticsScript);
                            //$scope.AnalyticsScript = websiteSettings.AnalyticsSettings.get("AnalyticsScript")
                        });
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                AnalyticsController.Name = "AnalyticsController";
                return AnalyticsController;
            })();
            Controllers.AnalyticsController = AnalyticsController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
