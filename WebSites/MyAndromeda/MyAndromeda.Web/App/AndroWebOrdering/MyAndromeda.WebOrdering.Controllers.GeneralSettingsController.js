/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(GeneralSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        GeneralSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        GeneralSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var GeneralSettingsController = (function () {
                function GeneralSettingsController() {
                }
                GeneralSettingsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    var _this = this;
                    $scope.$watch("MinimumDeliveryAmount", function (newValue, oldValue) {
                        newValue = isNaN(newValue) ? null : newValue * 100;

                        if (!contextService.Model) {
                            return;
                        }

                        contextService.Model.GeneralSettings.set("MinimumDeliveryAmount", newValue);
                    });

                    $scope.SaveChanges = function () {
                        if ($scope.GeneralSettingsValidator.validate()) {
                            webOrderingWebApiService.Update();
                        }
                    };

                    $scope.ResetToDefault = function () {
                        if (confirm("Are you sure you want to update general settings")) {
                            contextService.Model.set("GeneralSettings", kendo.observable(GeneralSettingsController.GeneralSettingsDefault));
                            $scope.MinimumDeliveryAmount = 0;
                        }

                        if (confirm("Are you sure you want to update the customer account settings")) {
                            contextService.Model.set("CustomerAccountSettings", kendo.observable(GeneralSettingsController.CustomerAccountsDefault));
                            GeneralSettingsController.WatchForValidLoginSettings($scope, $timeout, contextService.Model.get("CustomerAccountSettings"));

                            _this.ShowFacebookIdInput($scope, $timeout, false);
                        }
                    };
                };

                GeneralSettingsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var viewElement = $("#GeneralSettingsController");
                        var generalSettings = websiteSettings.GeneralSettings;
                        var customerAccountSettings = websiteSettings.CustomerAccountSettings;

                        kendo.bind(viewElement, websiteSettings);

                        var minDeliveryValue = websiteSettings.GeneralSettings.get("MinimumDeliveryAmount");
                        $scope.MinimumDeliveryAmount = minDeliveryValue ? minDeliveryValue / 100 : 0;

                        var visible = customerAccountSettings.get("EnableFacebookLogin");
                        GeneralSettingsController.ShowFacebookIdInput($scope, $timeout, visible);

                        GeneralSettingsController.WatchForValidLoginSettings($scope, $timeout, customerAccountSettings);
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };

                GeneralSettingsController.ShowFacebookIdInput = function ($scope, $timeout, visible) {
                    $timeout(function () {
                        $scope.ShowFacebookAppId = visible;
                    });
                };

                GeneralSettingsController.WatchForValidLoginSettings = function ($scope, $timeout, customerAccountSettings) {
                    $timeout(function () {
                        var hasFacebookLogin = customerAccountSettings.get("EnableFacebookLogin");
                        var hasAndromedaLogin = customerAccountSettings.get("EnableAndromedaLogin");

                        $scope.HasLoginOptions = hasAndromedaLogin || hasFacebookLogin;

                        console.log("$scope.HasLoginOptions: " + $scope.HasLoginOptions);
                    });

                    customerAccountSettings.bind("change", function (e) {
                        if (e.field === "EnableFacebookLogin" || e.field === "EnableAndromedaLogin") {
                            var hasFacebookLogin = customerAccountSettings.get("EnableFacebookLogin");
                            var hasAndromedaLogin = customerAccountSettings.get("EnableAndromedaLogin");

                            $timeout(function () {
                                $scope.HasLoginOptions = hasAndromedaLogin || hasFacebookLogin;
                            });
                        }
                    });

                    $scope.$watch("HasLoginOptions", function (newValue, oldValue) {
                        customerAccountSettings.set("IsEnable", newValue);
                    });
                };
                GeneralSettingsController.Name = "GeneralSettingsController";

                GeneralSettingsController.GeneralSettingsDefault = {
                    EnableHomePage: true,
                    MinimumDeliveryAmount: 0
                };

                GeneralSettingsController.CustomerAccountsDefault = {
                    IsEnable: true,
                    EnableAndromedaLogin: true,
                    IsEnableAndromedaLogin: true
                };
                return GeneralSettingsController;
            })();
            Controllers.GeneralSettingsController = GeneralSettingsController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
