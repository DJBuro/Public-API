/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (DeliveryZonesByRadius) {
        (function (Controllers) {
            DeliveryZonesByRadius.Angular.ControllersInitilizations.push(function (app) {
                app.controller(DeliveryZoneNamesController.Name, [
                    '$scope', '$timeout',
                    DeliveryZonesByRadius.Services.ContextService.Name,
                    DeliveryZonesByRadius.Services.DeliveryZonesByRadiusWebApiService.Name,
                    function ($scope, $timeout, contextService, deliveryZonesByRadiusApiService) {
                        DeliveryZoneNamesController.OnLoad($scope, $timeout, contextService, deliveryZonesByRadiusApiService);

                        DeliveryZoneNamesController.SetupSubscriptions($scope, $timeout, contextService);
                    }
                ]);
            });

            var DeliveryZoneNamesController = (function () {
                function DeliveryZoneNamesController() {
                }
                DeliveryZoneNamesController.OnLoad = function ($scope, $timeout, contextService, deliveryZonesByRadiusApiService) {
                    $scope.SaveChanges = function () {
                        if ($scope.PostCodeValidator.validate()) {
                            deliveryZonesByRadiusApiService.Update();
                            $timeout(function () {
                                var vm = $scope.ViewModel;
                                var postCodeOptions = vm.PostCodeSectors;
                                $scope.PostCodeOptionsListView.dataSource.data(postCodeOptions);
                                var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) {
                                    return !e.IsSelected;
                                });
                                $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                            });
                        }
                    };

                    $scope.GeneratePostCodeSectors = function () {
                        var validInput = $scope.PostCodeValidator.validate();
                        if (!validInput) {
                            alert("Correct the input.");
                            return;
                        }

                        if ($scope.ViewModel.Id == 0 || confirm("The existing post code sectors selection will be lost and reset.Are you sure you want to Regenerate the post code sectors?")) {
                            if ($scope.PostCodeValidator.validate()) {
                                $("#loader").removeClass('hidden');
                                $scope.ViewModel.HasPostCodes = false;
                                deliveryZonesByRadiusApiService.GeneratePostCodes();
                            }
                        }
                    };

                    $scope.SelectAllChange = function () {
                        var selectAll = $scope.SelectAll;
                        $timeout(function () {
                            var data = $scope.PostCodeOptionsListView.dataSource.data();
                            data.forEach(function (item) {
                                item.IsSelected = selectAll;
                            });
                        });
                        //var data = $scope.PostCodeOptionsListView.dataSource.data();
                        //contextService.Model.PostCodeSectors = data;
                    };

                    $scope.UpdateSelectAll = function () {
                        console.log("update select all");
                        var data = $scope.PostCodeOptionsListView.dataSource.data();

                        //contextService.Model.PostCodeSectors = data;
                        var unselectedItems = data.filter(function (e) {
                            return !e.IsSelected;
                        });
                        if (unselectedItems.length === 0) {
                            $scope.SelectAll = true;
                        } else if (unselectedItems.length < data.length) {
                            $scope.SelectAll = false;
                        } else {
                            $scope.SelectAll = false;
                        }
                    };
                };

                DeliveryZoneNamesController.SetupSubscriptions = function ($scope, $timeout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.subscribe(function (deliveryZoneByRadius) {
                        $timeout(function () {
                            $scope.ViewModel = deliveryZoneByRadius;
                            $scope.PostCodeOptionsListView.dataSource.data($scope.ViewModel.PostCodeSectors);
                            $scope.ViewModel.HasPostCodes = deliveryZoneByRadius.PostCodeSectors.length === 0 ? false : true;
                            var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) {
                                return !e.IsSelected;
                            });
                            $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                        });
                    });

                    var settingsPostcodeSubscription = contextService.PostcodeModels.subscribe(function (newDeliveryOptions) {
                        $scope.ViewModel.HasPostCodes = newDeliveryOptions.length === 0 ? false : true;
                        $timeout(function () {
                            var vm = $scope.ViewModel;
                            var postCodeOptions = vm.PostCodeSectors;
                            postCodeOptions = new kendo.data.ObservableArray(newDeliveryOptions);
                            vm.PostCodeSectors = postCodeOptions;
                            $scope.PostCodeOptionsListView.dataSource.data(postCodeOptions);
                            var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) {
                                return !e.IsSelected;
                            });
                            $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                            $scope.ViewModel.HasPostCodes = (contextService.Model.PostCodeSectors == null || contextService.Model.PostCodeSectors.length === 0) ? false : true;
                        });
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                DeliveryZoneNamesController.Name = "DeliveryZoneNamesController";
                return DeliveryZoneNamesController;
            })();
            Controllers.DeliveryZoneNamesController = DeliveryZoneNamesController;
        })(DeliveryZonesByRadius.Controllers || (DeliveryZonesByRadius.Controllers = {}));
        var Controllers = DeliveryZonesByRadius.Controllers;
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
