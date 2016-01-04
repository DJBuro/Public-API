/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />

module MyAndromeda.DeliveryZonesByRadius.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(DeliveryZoneNamesController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.DeliveryZonesByRadiusWebApiService.Name,

                ($scope, $timeout, contextService, deliveryZonesByRadiusApiService) => {

                    DeliveryZoneNamesController.OnLoad($scope, $timeout, contextService, deliveryZonesByRadiusApiService);

                    DeliveryZoneNamesController.SetupSubscriptions($scope, $timeout, contextService);
                }
            ]);
    });

    export class DeliveryZoneNamesController {
        public static Name: string = "DeliveryZoneNamesController";

        public static OnLoad(
            $scope: Scopes.IDeliveryZonesNamesScope,
            $timeout: ng.ITimeoutService,
            contextService: Services.ContextService,
            deliveryZonesByRadiusApiService: Services.DeliveryZonesByRadiusWebApiService) {

            $scope.SaveChanges = () => {
                if ($scope.PostCodeValidator.validate()) {
                    deliveryZonesByRadiusApiService.Update();
                    $timeout(() => {
                        var vm = $scope.ViewModel;
                        var postCodeOptions = vm.PostCodeSectors;
                        $scope.PostCodeOptionsListView.dataSource.data(postCodeOptions);
                        var unselectedItems = contextService.Model.PostCodeSectors.filter((e: Models.IPostCodeSectorsViewModel) => { return !e.IsSelected; })
                        $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                    });
                }
            };

            $scope.GeneratePostCodeSectors = () => {
                var validInput = $scope.PostCodeValidator.validate();
                if (!validInput)
                {
                    alert("Correct the input.");
                    return;
                }

                if ($scope.ViewModel.Id == 0 || confirm("The existing post code sectors selection will be lost and reset.Are you sure you want to Regenerate the post code sectors?")) {
                    if ($scope.PostCodeValidator.validate()) {
                        $("#loader").removeClass('hidden')
                        $scope.ViewModel.HasPostCodes = false;
                        deliveryZonesByRadiusApiService.GeneratePostCodes();
                    }
                }
            };

            $scope.SelectAllChange = () => {
                var selectAll = $scope.SelectAll;
                $timeout(() => {
                    var data = $scope.PostCodeOptionsListView.dataSource.data();
                    data.forEach((item: Models.IPostCodeSectorsViewModel) => {
                        item.IsSelected = selectAll;
                    });
                });
                //var data = $scope.PostCodeOptionsListView.dataSource.data();
                //contextService.Model.PostCodeSectors = data;
            }

            $scope.UpdateSelectAll = () => {
                console.log("update select all");
                var data = $scope.PostCodeOptionsListView.dataSource.data();
                //contextService.Model.PostCodeSectors = data;
                var unselectedItems = data.filter((e: Models.IPostCodeSectorsViewModel) => { return !e.IsSelected; });
                if (unselectedItems.length === 0) {
                    $scope.SelectAll = true;
                }
                else if (unselectedItems.length < data.length) {
                    $scope.SelectAll = false;
                }
                else { $scope.SelectAll = false; }
            };
        }

        public static SetupSubscriptions($scope: Scopes.IDeliveryZonesNamesScope, $timeout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .subscribe((deliveryZoneByRadius) => {
                    $timeout(() => {
                        $scope.ViewModel = deliveryZoneByRadius;
                        $scope.PostCodeOptionsListView.dataSource.data($scope.ViewModel.PostCodeSectors);
                        $scope.ViewModel.HasPostCodes = deliveryZoneByRadius.PostCodeSectors.length === 0 ? false : true;
                        var unselectedItems = contextService.Model.PostCodeSectors.filter((e: Models.IPostCodeSectorsViewModel) => { return !e.IsSelected; })
                        $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                    });

                });

            var settingsPostcodeSubscription = contextService.PostcodeModels
                .subscribe((newDeliveryOptions) => {
                    $scope.ViewModel.HasPostCodes = newDeliveryOptions.length === 0 ? false : true;
                    $timeout(() => {
                        var vm = $scope.ViewModel;
                        var postCodeOptions = vm.PostCodeSectors;
                        postCodeOptions = new kendo.data.ObservableArray(newDeliveryOptions);
                        vm.PostCodeSectors = postCodeOptions;
                        $scope.PostCodeOptionsListView.dataSource.data(postCodeOptions);
                        var unselectedItems = contextService.Model.PostCodeSectors.filter((e: Models.IPostCodeSectorsViewModel) => { return !e.IsSelected; })
                        $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                        $scope.ViewModel.HasPostCodes = (contextService.Model.PostCodeSectors == null || contextService.Model.PostCodeSectors.length === 0) ? false : true;
                    });

                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}  