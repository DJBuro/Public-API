var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var UpSelling;
        (function (UpSelling) {
            var upSellModule = angular.module("androweb-upsell-module", []);
            upSellModule.controller("upSellController", function ($scope, $timeout, contextService, upSellDataService, webOrderingWebApiService) {
                var menuPromise = upSellDataService.GetMenu();
                var getMultiSelect = function () {
                    var multiSelect = $scope.UpSellMultiSelect;
                    return multiSelect;
                };
                var dataSource = new kendo.data.DataSource({
                    autoSync: false
                });
                var multiselectOptions = {
                    placeholder: "Select a menu section",
                    autoBind: false,
                    dataTextField: "Name",
                    dataValueField: "Id",
                    dataSource: dataSource
                };
                $scope.MultiselectOptions = multiselectOptions;
                //$scope.SelectedDisplayCategories = [];
                $scope.DisplayCategoryDataSource = dataSource;
                $scope.SaveChanges = function () {
                    if ($scope.UpSellingValidator.validate()) {
                        webOrderingWebApiService.Update();
                    }
                };
                $scope.Settings = null;
                var menuObservable = Rx.Observable.fromPromise(menuPromise);
                var settingsObservable = contextService.ModelSubject
                    .where(function (e) { return e !== null; });
                var both = Rx.Observable.combineLatest(menuObservable, settingsObservable, function (menuResponse, settings) {
                    return {
                        Menu: menuResponse.data,
                        AndroWebSettings: settings
                    };
                });
                var bothSubscription = both.subscribe(function (settings) {
                    //var multiSelect = getMultiSelect();
                    //$scope.SelectedDisplayCategories = settings.AndroWebSettings.UpSelling.DisplayCategories;
                    if (!settings.AndroWebSettings.UpSelling) {
                        settings.AndroWebSettings.UpSelling = {
                            Enabled: false,
                            DisplayCategories: []
                        };
                    }
                    $timeout(function () {
                        $scope.Settings = settings.AndroWebSettings;
                        dataSource.data(settings.Menu.DisplayCategories);
                    });
                });
                $scope.$on('$destroy', function () {
                    bothSubscription.dispose();
                });
            });
            var UpSellDataService = (function () {
                function UpSellDataService($http) {
                    this.$http = $http;
                }
                UpSellDataService.prototype.GetMenu = function () {
                    var promise = this.$http.get(UpSellDataService.GetMemuRoute);
                    return promise;
                };
                //set by cshtml 
                UpSellDataService.GetMemuRoute = "";
                return UpSellDataService;
            })();
            UpSelling.UpSellDataService = UpSellDataService;
            upSellModule.service("upSellDataService", UpSellDataService);
        })(UpSelling = WebOrdering.UpSelling || (WebOrdering.UpSelling = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
