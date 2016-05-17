module MyAndromeda.WebOrdering.UpSelling {
    var upSellModule = angular.module("androweb-upsell-module", []);

    upSellModule.controller("upSellController", ($scope, $timeout,
        contextService: Services.ContextService,
        upSellDataService: UpSellDataService,
        webOrderingWebApiService: Services.WebOrderingWebApiService) => {

        var menuPromise = upSellDataService.GetMenu();

        var getMultiSelect = () => {
            var multiSelect: kendo.ui.MultiSelect = $scope.UpSellMultiSelect;
            return multiSelect;
        }; 

        var dataSource = new kendo.data.DataSource({
            autoSync: false
        });
        var multiselectOptions: kendo.ui.MultiSelectOptions = {
            placeholder: "Select a menu section",
            autoBind: false,
            dataTextField: "Name",
            dataValueField: "Id",
            dataSource: dataSource
        }; 

        

        $scope.MultiselectOptions = multiselectOptions;
        //$scope.SelectedDisplayCategories = [];
        $scope.DisplayCategoryDataSource = dataSource;
        $scope.SaveChanges = () => {
            if ($scope.UpSellingValidator.validate()) {
                webOrderingWebApiService.Update();
            }
        };      
        $scope.Settings = null;

        var menuObservable = Rx.Observable.fromPromise(menuPromise);
        var settingsObservable = contextService.ModelSubject
            .where((e) => { return e !== null; });

        var both = Rx.Observable.combineLatest(menuObservable, settingsObservable, (menuResponse, settings) => {
            return {
                Menu: menuResponse.data,
                AndroWebSettings: settings
            };
        });

        var bothSubscription = both.subscribe(settings => {
            //var multiSelect = getMultiSelect();
            //$scope.SelectedDisplayCategories = settings.AndroWebSettings.UpSelling.DisplayCategories;
            if (!settings.AndroWebSettings.UpSelling) {
                settings.AndroWebSettings.UpSelling = {
                    Enabled: false,
                    DisplayCategories : []
                };
            }

            $timeout(() => {
                $scope.Settings = settings.AndroWebSettings;
                dataSource.data(settings.Menu.DisplayCategories);
            });

            
        });

        $scope.$on('$destroy', () => {
            bothSubscription.dispose();
        });
    });


    export class UpSellDataService
    {
        //set by cshtml 
        public static GetMemuRoute = "";

        private $http: ng.IHttpService;
        constructor($http : ng.IHttpService)
        {
            this.$http = $http;
        }


        public GetMenu() : ng.IHttpPromise<MyAndromeda.Menu.Models.IMenu>
        {
            var promise = this.$http.get(UpSellDataService.GetMemuRoute);   

            return promise;
        }
    }

    upSellModule.service("upSellDataService", UpSellDataService);
}
