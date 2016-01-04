/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("GeneralSettingsController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    GeneralSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);
                    /* going to leave kendo to manage the observable object */
                    GeneralSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                });
            });
            var GeneralSettingsController = (function () {
                function GeneralSettingsController() {
                }
                GeneralSettingsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
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
                            GeneralSettingsController.ShowFacebookIdInput($scope, $timeout, false);
                        }
                    };
                };
                GeneralSettingsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    //$scope.$watch("$scope.DineInServiceChargeLimit",)
                    var getDineInServiceCharge = function () {
                        var element = $("#DineInServiceCharge");
                        return element.data("kendoNumericTextBox");
                    };
                    var getDineInServiceChargeLimit = function () {
                        var element = $("#LegalDineInServiceChargeLimit");
                        return element.data("kendoNumericTextBox");
                    };
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#GeneralSettingsController");
                        var generalSettings = websiteSettings.GeneralSettings;
                        var customerAccountSettings = websiteSettings.CustomerAccountSettings;
                        /* todo remove the rest of the kendo mvvm binding with the angular ones*/
                        kendo.bind(viewElement, websiteSettings);
                        var minDeliveryValue = websiteSettings.GeneralSettings.get("MinimumDeliveryAmount");
                        $scope.MinimumDeliveryAmount = minDeliveryValue ? minDeliveryValue / 100 : 0;
                        var visible = customerAccountSettings.get("EnableFacebookLogin");
                        GeneralSettingsController.ShowFacebookIdInput($scope, $timeout, visible);
                        GeneralSettingsController.WatchForValidLoginSettings($scope, $timeout, customerAccountSettings);
                        $scope.JivoChatSettings = websiteSettings.JivoChatSettings;
                        $scope.GeneralSettings = websiteSettings.GeneralSettings;
                    });
                    $scope.DineInServiceChargeOptions = {
                        min: 0
                    };
                    $scope.DineInServiceChargeLimitOptions = {
                        min: 0,
                        change: function () {
                            WebOrdering.Logger.Notify("limit change");
                            var charge = getDineInServiceCharge();
                            var limit = getDineInServiceChargeLimit();
                            if (limit.value() < charge.value()) {
                                //some reason is not updating the model...
                                charge.value(limit.value());
                                //force model update
                                $scope.GeneralSettings.set("DineInServiceCharge", limit.value());
                            }
                            charge.max(limit.value());
                        }
                    };
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
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
