/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers 
{
    Angular.ControllersInitilizations.push((app) => {
        app.controller(PickThemeController.Name, 
        [
            '$scope','$timeout',
            
            Services.ContextService.Name,
            Services.WebOrderingWebApiService.Name,
            Services.WebOrderingThemeWebApiService.Name,

            ($scope, $timeout, contextService, webOrderingWebApiService, webOrderingThemeWebApiService) => {

                PickThemeController.OnLoad($scope, $timeout, webOrderingWebApiService, webOrderingThemeWebApiService);
                PickThemeController.SetupScope($scope);
                PickThemeController.SetupSelection($scope, webOrderingWebApiService);
                PickThemeController.SetupCurrentSelection($scope, $timeout, contextService);

            }    
        ]);
    });

    export class PickThemeController 
    {
        public static Name: string = "PickThemeController";
        public static Route: string = "/";

        public static OnLoad($scope: Scopes.IThemeScope, $timout: ng.ITimeoutService, 
            webOrderingWebApiService: Services.WebOrderingWebApiService,
            webOrderingThemeWebApiService: Services.WebOrderingThemeWebApiService)
        {
            var isThemesBusySubscription = webOrderingThemeWebApiService.IsBusy.subscribe((value) => {
                $timout(()=> {
                    $scope.IsThemesBusy = value;
                });
            });
            var isDataBusySubscription = webOrderingWebApiService.IsWebOrderingBusy.subscribe((value) => {
                $timout(()=> {
                    $scope.IsDataBusy = value;
                });
            });

            $scope.ListViewTemplate = $("#ListViewTemplate").html();
            $scope.DataSource = webOrderingThemeWebApiService.GetThemeDataSource();
            $scope.HasPreviewTheme = false;
            $scope.HasCurrentTheme = false;
            $scope.SearchTemplates = () => {
                webOrderingThemeWebApiService.SearchText($scope.SearchText);
            };

            $scope.$on('$destroy', () => {
                isThemesBusySubscription.dispose();
            });
        }

        public static SetupCurrentSelection($scope: Scopes.IThemeScope, $timout: ng.ITimeoutService, contextService: Services.ContextService)
        {
            var modelSubscription = contextService.ModelSubject.where((value)=>{
                return value !== null;
            }).subscribe((settings) => {
                var s :any = settings;
                    s.bind("change", () => { 
                        $timout(()=> { 
                        //console.log("set current theme settings");
                        console.log(settings.ThemeSettings);
                        $scope.CurrentTheme = settings.ThemeSettings; 
                        $scope.HasCurrentTheme = true;
                    });
                });

                console.log(settings.ThemeSettings);
                $scope.CurrentTheme = settings.ThemeSettings; 
                $scope.HasCurrentTheme = true;
                
            });

            $scope.$on("$destroy", ()=>{
                modelSubscription.dispose();
            });
        }

        public static SetupSelection($scope : Scopes.IThemeScope, webOrderingWebApiService: Services.WebOrderingWebApiService)
        {
            $scope.SelectTemplate = (id: number) => {
                var dataSource = $scope.DataSource;
                var previewItem = dataSource.data().find((item: Models.IWebOrderingTheme) => {
                    return item.Id === id;
                });

                $scope.HasPreviewTheme = true;
                $scope.SelectedTheme = previewItem;
            };

            $scope.SelectPreviewTheme = (theme : Models.IWebOrderingTheme) => {
                console.log(theme);
                webOrderingWebApiService.UpdateThemeSettings(theme);

            };
        }

        public static SetupScope($scope: ng.IScope)
        {
            $scope.$on('$destroy', () => {});
        }
    }
} 