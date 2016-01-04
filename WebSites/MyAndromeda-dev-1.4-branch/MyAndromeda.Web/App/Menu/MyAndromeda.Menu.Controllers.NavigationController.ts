module MyAndromeda.Menu.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(MenuNavigationController.Name, [
            '$scope',
            Services.MenuNavigationService.Name,
            Services.PublishingService.Name,
            ($scope, menuNavigationService, publishingService) => {
                Logger.Notify("Menu navigation controller loaded");

                MenuNavigationController.ToolbarOptions($scope, publishingService);
                MenuNavigationController.OnLoad($scope, menuNavigationService);
            }
        ]);
    });

    export class MenuNavigationController {
        public static Name: string = "MenuNavigationController";

        public static ToolbarClick: Function;
        public static SaveAllChangesClick: Function;
        public static CancelAllChangesClick: Function;
        public static PublishClick: Function;

        public static ToolbarOptions($scope: IMenuNavigationControllerScope, publishingService: Services.PublishingService) {
            publishingService.init();
            $scope.Publish = () => { publishingService.openWindow(); };
            $scope.ToolbarOptions = {
                items: [
                    //{ type: "button", text: "Menu Items" },
                    //{ type: "button", text: "Menu Sequencing" },
                    //{ type: "button", text: "Toppings" },
                    //{ type: "separator" },
                    { type: "button", text: "Publish", click: function(){ publishingService.openWindow(); } }
                    //{
                    //    type: "buttonGroup",
                    //    buttons: [
                    //        {
                    //            spriteCssClass: "k-icon k-update", text: "Save changes", togglable: true, group: "save-publish",
                    //        },
                    //        {
                    //            spriteCssClass: "k-icon k-i-cancel", text: "Cancel all changes", togglable: true, group: "save-publish",
                    //        },
                    //        {
                    //            text: "Publish", togglable: true, group: "save-publish",
                    //            click: function(){
                    //                console.log("Hi");
                    //                publishingService.openWindow();
                    //            }
                    //        }
                    //    ]
                    //}
                ]
            };

        }

        public static OnLoad($scope: IMenuNavigationControllerScope, menuNavigationService: Services.MenuNavigationService): void 
        {
            $scope.$on("$destroy", () => { });
        }
    }

    export interface IMenuNavigationControllerScope extends ng.IScope {
        ToolbarOptions: kendo.ui.ToolBarOptions;
        Publish: () => void;
    }
} 