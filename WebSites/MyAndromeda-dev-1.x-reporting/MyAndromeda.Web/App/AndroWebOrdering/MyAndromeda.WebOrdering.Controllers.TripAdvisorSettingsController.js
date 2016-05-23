/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(TripAdvisorSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        TripAdvisorSettingsController.SetupValidatorOptions($scope, $timeout, webOrderingWebApiService);
                        TripAdvisorSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        TripAdvisorSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var TripAdvisorSettingsController = (function () {
                function TripAdvisorSettingsController() {
                }
                TripAdvisorSettingsController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    //$scope.SaveChanges = () => {
                    //    if ($scope.TripAdvisorSettingsValidator.validate()) {
                    //        webOrderingWebApiService.Update();
                    //    }
                    //};
                    //var s = <any>$scope;
                    //s.FacebookSettings = {};
                };
                TripAdvisorSettingsController.SetupValidatorOptions = function ($scope, $timout, contextService) {
                    var validatorOptions = {
                        name: "",
                        rules: {
                            TripadvisorScirptRequired: function (input) {
                                if (!input.is("[data-required-if-tripadvisor]")) {
                                    return true;
                                }
                                var isEnabled = contextService.Model.TripAdvisorSettings.get("IsEnable");
                                var text = input.val();
                                return text.length > 0;
                            }
                        }
                    };
                };
                TripAdvisorSettingsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#TripAdvisorSettingsController");
                        kendo.bind(viewElement, websiteSettings.TripAdvisorSettings);
                        $scope.ShowTripAdvisorSettings = websiteSettings.TripAdvisorSettings.get("IsEnable");
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                TripAdvisorSettingsController.Name = "TripAdvisorSettingsController";
                return TripAdvisorSettingsController;
            }());
            Controllers.TripAdvisorSettingsController = TripAdvisorSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
