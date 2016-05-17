/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(AnalyticsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    AnalyticsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    AnalyticsController.SetupKendoMvvm($scope,$timeout, contextService);
                }
            ]);
    });

    export class AnalyticsController {
        public static Name: string = "AnalyticsController";

        public static OnLoad(
            $scope: Scopes.IAnalyticsScope,
            $timout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {
            
            $scope.SaveChanges = () => {
                //console.log("save");                
                webOrderingWebApiService.Update();
            }; 

            //going to move to just the analytics id 
            //$scope.EncodeScript = (analyticsScript : string) => {                               
            //    webOrderingWebApiService.Context.Model.AnalyticsSettings.set("AnalyticsScript", encodeURI(analyticsScript));
            //};
        }

        public static SetupKendoMvvm($scope: Scopes.IAnalyticsScope, $timout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {

                    var viewElement = $("#AnalyticsController");                    
                    kendo.bind(viewElement, websiteSettings.AnalyticsSettings);

                    $timout(() => {
                        //want to decode here
                        //websiteSettings.AnalyticsSettings.AnalyticsScript = decodeURI(websiteSettings.AnalyticsSettings.AnalyticsScript);
                        
                        //$scope.AnalyticsScript = websiteSettings.AnalyticsSettings.get("AnalyticsScript")
                    });

                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}  