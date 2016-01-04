/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(SocialNetworkSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        SocialNetworkSettingsController.SetupValidatorOptions($scope, $timeout, webOrderingWebApiService);
                        SocialNetworkSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        SocialNetworkSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var SocialNetworkSettingsController = (function () {
                function SocialNetworkSettingsController() {
                }
                SocialNetworkSettingsController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    //$scope.SocialNetworkSettingsValidator.ru
                    $scope.SaveChanges = function () {
                        if ($scope.SocialNetworkSettingsValidator.validate()) {
                            webOrderingWebApiService.Update();
                        }
                    };
                    var s = $scope;
                    s.FacebookSettings = {};
                };
                SocialNetworkSettingsController.SetupValidatorOptions = function ($scope, $timout, contextService) {
                    var validatorOptions = {
                        name: "",
                        rules: {
                            FacebookUrlRequired: function (input) {
                                if (!input.is("[data-required-if-facebook]")) {
                                    return true;
                                }
                                var isEnabled = contextService.Model.SocialNetworkSettings.FacebookSettings.get("IsEnable");
                                var text = input.val();
                                return text.length > 0;
                            },
                            TwitterRequired: function (intput) { }
                        }
                    };
                };
                SocialNetworkSettingsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#SocialNetworkSettingsController");
                        kendo.bind(viewElement, websiteSettings.SocialNetworkSettings);
                        $scope.SocialNetworkSettings = websiteSettings.SocialNetworkSettings;
                        $scope.GeneralSettings = websiteSettings.GeneralSettings;
                        $scope.CustomerAccountSettings = websiteSettings.CustomerAccountSettings;
                        $scope.ShowFacebookSettings = websiteSettings.SocialNetworkSettings.FacebookSettings.get("IsEnable");
                        $scope.ShowTwitterSettings = websiteSettings.SocialNetworkSettings.TwitterSettings.get("IsEnable");
                        $scope.ShowInstagramSettings = websiteSettings.SocialNetworkSettings.InstagramSettings.get("IsEnable");
                        // $scope.ShowTripAdvisorSettings = websiteSettings.TripAdvisorSettings.get("IsEnable");
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                SocialNetworkSettingsController.Name = "SocialNetworkSettingsController";
                return SocialNetworkSettingsController;
            })();
            Controllers.SocialNetworkSettingsController = SocialNetworkSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
