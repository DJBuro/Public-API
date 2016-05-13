

module MyAndromeda.Menu.Controllers {
    Angular.ControllersInitilizations.push((app) => 
    {
        app.controller(ToppingsFilterController.Name,
        [
            '$scope',
            '$timeout',
            Services.MenuToppingsService.Name,
            Services.MenuToppingsFilterService.Name,
            ($scope, $timeout, menuToppingsService, menuToppingsFilterService) => {
                Logger.Debug("Setting up ToppingsFilterController");
                
                ToppingsFilterController.OnLoad($scope);
                ToppingsFilterController.SetupScope($scope, $timeout, menuToppingsFilterService);

                Logger.Debug("Set up ToppingsFilterController");
            }
        ]);
    });

    export class ToppingsFilterController 
    {
        public static Name: string = "ToppingsFilterController";

        public static OnLoad($scope: IToppingFilterScope)
        {
            Logger.Notify("Toppings Filter Controller Loaded");
        }
        
        public static SetupScope($scope: IToppingFilterScope, $timeout : ng.ITimeoutService, menuToppingsFilterService : Services.MenuToppingsFilterService)
        {
            $scope.Name =  menuToppingsFilterService.GetName();
            $scope.$watch("Name", (newValue, olderValue) => {
                Logger.Debug("ToppingsFilterController : Name changed");
                if(newValue === olderValue) { return; } 

                menuToppingsFilterService.ChangeNameFilter(newValue);
            });

            $scope.ResetFilters = () => {
                Logger.Debug("Reset button clicked");
                menuToppingsFilterService.ResetFilters();
            };
            
            var observable = menuToppingsFilterService.ResetFiltersObservable.subscribe(() => {
                $timeout(() => {
                    $scope.Name = "";
                }, 0);
            });

            $scope.$on("$destroy", () => {
                observable.dispose();
            });
        }


    }

}

 