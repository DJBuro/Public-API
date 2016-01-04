 
module MyAndromeda.Menu.Controllers 
{
    Angular.ControllersInitilizations.push((app) => {
        app.controller(MenuItemsController.Name, 
        [
            '$scope',
            ($scope) => {
                MenuItemsController.OnLoad($scope);
                MenuItemsController.SetupScope($scope);
            }    
        ]);
    });

    export class MenuItemsController 
    {
        public static Template: string = "Templates/MenuItems";
        public static Name: string = "MenuItemsController";
        public static Route: string = "/MenuItems";

        public static OnLoad($scope: ng.IScope)
        {
            
            $scope.$on('$destroy', () => {});
        }

        public static SetupScope($scope: ng.IScope)
        {

            $scope.$on('$destroy', () => {});
        }
    }
}