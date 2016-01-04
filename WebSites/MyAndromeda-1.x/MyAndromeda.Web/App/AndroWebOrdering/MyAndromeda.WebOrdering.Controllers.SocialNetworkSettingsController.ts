/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(SocialNetworkSettingsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    SocialNetworkSettingsController.SetupValidatorOptions($scope, $timeout, webOrderingWebApiService);
                    SocialNetworkSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    SocialNetworkSettingsController.SetupKendoMvvm($scope,  $timeout, contextService);
                }
            ]);
    });

    export class SocialNetworkSettingsController {
        public static Name: string = "SocialNetworkSettingsController";

        public static OnLoad(
            $scope: Scopes.ISocialNetworkSettingsScope,
            $timout: ng.ITimeoutService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) 
        {
            //$scope.SocialNetworkSettingsValidator.ru
           

            $scope.SaveChanges = () => {
                if($scope.SocialNetworkSettingsValidator.validate())
                {
                    webOrderingWebApiService.Update();
                }
            };

            var s = <any>$scope;
            s.FacebookSettings = {};
        }

        public static SetupValidatorOptions($scope: Scopes.ISocialNetworkSettingsScope, $timout: ng.ITimeoutService, contextService: Services.ContextService)
        {
            var validatorOptions : kendo.ui.ValidatorOptions =  {
                name : "",
                rules: {
                    FacebookUrlRequired : function(input : JQuery){
                        if(!input.is("[data-required-if-facebook]"))
                        {
                            return true;
                        }

                        var isEnabled =  contextService.Model.SocialNetworkSettings.FacebookSettings.get("IsEnable"); 

                        var text: string = input.val();
                        return text.length > 0;
                    },
                    TwitterRequired: function(intput){}  
                }
            };
        }

        public static SetupKendoMvvm($scope: Scopes.ISocialNetworkSettingsScope, $timout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {
                    var viewElement = $("#SocialNetworkSettingsController");
                    kendo.bind(viewElement, websiteSettings.SocialNetworkSettings);
                    $scope.ShowFacebookSettings = websiteSettings.SocialNetworkSettings.FacebookSettings.get("IsEnable");
                    $scope.ShowTwitterSettings = websiteSettings.SocialNetworkSettings.TwitterSettings.get("IsEnable");
                    $scope.ShowInstagramSettings = websiteSettings.SocialNetworkSettings.InstagramSettings.get("IsEnable");                    
                   // $scope.ShowTripAdvisorSettings = websiteSettings.TripAdvisorSettings.get("IsEnable");
          });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
} 