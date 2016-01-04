/// <reference path="Loyalty.Services.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Loyalty;
    (function (Loyalty) {
        Loyalty.ControllersName = "LoyaltyControllers";
        var controllersModule = angular.module(Loyalty.ControllersName, [
            Loyalty.ServicesName
        ]);
        Loyalty.StartController = "StartController";
        controllersModule.controller(Loyalty.StartController, function ($scope, $timeout, loyaltyService) {
            loyaltyService.ListLoyaltyTypes();
            loyaltyService.List();
            $scope.ShowAddList = false;
            $scope.ShowEditList = false;
            var allTypesSubscription = loyaltyService.AllLoyaltyTypeList.subscribe(function (list) {
                $timeout(function () {
                    $scope.AvailableLoyaltyTypeNames = list;
                    $scope.ShowAddList = list.length > 0;
                });
            });
            var currentStoreLoyalties = loyaltyService.StoreLoyalties.subscribe(function (list) {
                $timeout(function () {
                    $scope.CurrentLoyaltyTypes = list;
                    $scope.ShowEditList = list.length > 0;
                });
            });
            $scope.$on("$destroy", function () {
                allTypesSubscription.dispose();
                currentStoreLoyalties.dispose();
            });
        });
        Loyalty.EditLoyaltyController = "EditLoyaltyController";
        controllersModule.controller(Loyalty.EditLoyaltyController, function ($scope, $timeout, $routeParams, loyaltyService) {
            $scope.SaveBusy = false;
            $scope.Currency = kendo.toString(1.00, "c");
            $scope.Currency10 = kendo.toString(10.00, "c");
            //lets explicitly look for this one:
            $scope.ProviderAvailable = false;
            loyaltyService.Get($routeParams.providerName);
            var modelAvailableSubscription = loyaltyService.LoyaltyProvider.subscribe(function (provider) {
                $timeout(function () {
                    $scope.Name = provider.ProviderName;
                    $scope.ProviderLoyalty = provider;
                    $scope.Model = provider.Configuration;
                    $scope.ProviderAvailable = true;
                });
            });
            var saveBusySubscription = loyaltyService.UpdateBusy.subscribe(function (value) {
                $timeout(function () {
                    $scope.SaveBusy = value;
                });
            });
            $scope.SetDefaults = function () {
                $scope.Model || ($scope.Model = {
                    RoundUp: true
                });
                var m = $scope.Model;
                if (!confirm("Setting defaults will wipe any existing change")) {
                    return;
                }
                m.Enabled = true;
                m.AutoSpendPointsOverThisPeak = null;
                m.AwardOnRegiration = 500;
                m.AwardPointsPerPoundSpent = 10;
                m.MaximumPercentThatCanBeClaimed = 1; /* 100% */
                m.MaximumValueThatCanBeClaimed = 10.00; /* 10.00 */
                m.MinimumPointsBeforeAvailable = null;
                m.PointValue = 100;
                //more defaults 
                m.MaximumObtainablePoints = null;
                m.MinimumOrderTotalAfterLoyalty = null;
            };
            $scope.IsAutoSpendPointsOverThisPeakEnabled = function () {
                if (typeof ($scope.Model) === 'undefined' || $scope.Model === null) {
                    return false;
                }
                var m = $scope.Model;
                if (m.AutoSpendPointsOverThisPeak === null) {
                    return false;
                }
                var t = (m.AutoSpendPointsOverThisPeak >= 0);
                return t;
            };
            $scope.IsMinimumPointsBeforeAvailableEnabled = function () {
                if (typeof ($scope.Model) === 'undefined' || $scope.Model === null) {
                    return false;
                }
                var m = $scope.Model;
                if (m.MinimumPointsBeforeAvailable === null) {
                    return false;
                }
                var t = m.MinimumPointsBeforeAvailable >= 0;
                return t;
            };
            $scope.IsMaximumObtainablePointsEnabled = function () {
                if (typeof ($scope.Model) === 'undefined' || $scope.Model === null) {
                    return false;
                }
                var m = $scope.Model;
                if (m.MaximumObtainablePoints === null) {
                    return false;
                }
                var t = m.MaximumObtainablePoints >= 0;
                return t;
            };
            $scope.Save = function () {
                var validator = $scope.AndromedaLoyaltyValidator;
                if (validator.validate()) {
                    loyaltyService.Update($scope.ProviderLoyalty);
                }
            };
            $scope.$watch("Model.PointValue", function () {
                if (!$scope.AwardingPointsNumericTextBox) {
                    return;
                }
                if ($scope.Model.PointValue) {
                    console.log("update points");
                    var numericPicker = $scope.AwardingPointsNumericTextBox;
                    numericPicker.max($scope.Model.PointValue);
                }
            });
            $scope.ValueOf = function (points) {
                points || (points = 0);
                if (!$scope.Model) {
                    return "";
                }
                //£1 = pointValue
                var pointValue = $scope.Model.PointValue;
                if (!pointValue) {
                    return "";
                }
                var worth = (1.00 / pointValue) * points;
                return kendo.toString(worth, "c"); //$1,234.57
            };
            $scope.DisplayAsCurrency = function (monies) {
                monies || (monies = 0);
                return kendo.toString(monies, "c");
            };
            $scope.$on("$destroy", function () {
                modelAvailableSubscription.dispose();
                saveBusySubscription.dispose();
            });
        });
    })(Loyalty = MyAndromeda.Loyalty || (MyAndromeda.Loyalty = {}));
})(MyAndromeda || (MyAndromeda = {}));
