

module MyAndromeda.Menu.Controllers {
    Angular.ControllersInitilizations.push((app) => 
    {
        app.controller(ToppingsController.Name,
        [
            '$scope',
            Services.MenuToppingsService.Name,
            ($scope, menuToppingsService) => {
                Logger.Debug("Start: Setting up Toppings controller");
                ToppingsController.OnLoad($scope);
                ToppingsController.SetupScope($scope);
                ToppingsController.SetupItemTemplate($scope);
                ToppingsController.SetupDataSource($scope, menuToppingsService);
                Logger.Debug("Complete: Setting up Toppings controller");
            }
        ]);
    });

    export class ToppingsController 
    {
        public static Name: string = "ToppingsController";
        public static Route: string = "/";
        public static Template: () => string = () => { return $("#MenuTemplate").html(); };
        

        public static OnLoad($scope: IToppingsScope)
        {
            Logger.Notify("Toppings Controller Loaded");
        }
        
        public static SetupScope($scope: IToppingsScope)
        {
        }

        public static SetupItemTemplate($scope: IToppingsScope){
            $scope.MenuToppingsListViewTemplate = $("#MenuToppingsListViewTemplate").html();
            $scope.MenuToppingsEditListViewTemplate = $("#MenuToppingsEditListViewTemplate").html();
        }

        public static SetupDataSource($scope: IToppingsScope, service: Services.MenuToppingsService){
            var dataSource = service.GetDataSource();
            $scope.DataSource = dataSource;

            var start = new Rx.Subject<boolean>();
            var end = new Rx.Subject<boolean>();

            var startLoading = start.subscribe((e)=> {
                Logger.Debug("Start Loading");
                kendo.ui.progress($("body"), true);
            }); 

            var endLoading = end.subscribe((e) => {
                Logger.Debug("Loading complete");
                kendo.ui.progress($("body"), false);
            });

            dataSource.bind("requestStart", ()=> { start.onNext(true); }); 
            dataSource.bind("requestEnd", () => { end.onNext(true); });

            $scope.$on("$destroy", () => {
                startLoading.dispose();
                endLoading.dispose();
            });
        }
    }

    export interface IToppingsScope extends ng.IScope {
        ListViewTemplate: string;
        MenuToppingsListViewTemplate: string;
        MenuToppingsEditListViewTemplate: string;
        DataSource: kendo.data.DataSource;
    }

    export interface IToppingFilterScope extends ng.IScope {
        Name: string;
        ResetFilters: () => void;
    }
}

