/// <reference path="MyAndromeda.WebOrdering.App.ts" />

module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(GeneralSettingsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    GeneralSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    GeneralSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                }
            ]);
    });

    export class GeneralSettingsController {
        public static Name: string = "GeneralSettingsController";

        public static GeneralSettingsDefault : Models.IGeneralSettings = {
            EnableHomePage : true,
            MinimumDeliveryAmount: 0
        }

        public static CustomerAccountsDefault : Models.ICustomerAccountSettings = {
            IsEnable: true,
            EnableAndromedaLogin: true,
            IsEnableAndromedaLogin: true
        };

        public static OnLoad(
            $scope: Scopes.IGeneralSettingsScope,
            $timeout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) 
        {
            $scope.$watch("MinimumDeliveryAmount", (newValue, oldValue) => {
                newValue = isNaN(newValue) ? null : newValue * 100;

                if(!contextService.Model){ return; }

                contextService.Model.GeneralSettings.set("MinimumDeliveryAmount", newValue );
            });
             
            $scope.SaveChanges = () => {
                if ($scope.GeneralSettingsValidator.validate()) {
                    webOrderingWebApiService.Update();
                }
            };

            $scope.ResetToDefault = () => {
                if(confirm("Are you sure you want to update general settings"))
                {
                    contextService.Model.set("GeneralSettings", kendo.observable(GeneralSettingsController.GeneralSettingsDefault));
                    $scope.MinimumDeliveryAmount = 0;
                }

                if(confirm("Are you sure you want to update the customer account settings")) 
                { 
                    
                    contextService.Model.set("CustomerAccountSettings", kendo.observable(GeneralSettingsController.CustomerAccountsDefault));
                    GeneralSettingsController.WatchForValidLoginSettings($scope, $timeout, contextService.Model.get("CustomerAccountSettings"));

                    this.ShowFacebookIdInput($scope, $timeout, false);
                }
            };
        }

        public static SetupKendoMvvm($scope: Scopes.IGeneralSettingsScope, $timeout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {

                    var viewElement = $("#GeneralSettingsController");
                    var generalSettings = websiteSettings.GeneralSettings;
                    var customerAccountSettings = websiteSettings.CustomerAccountSettings;
                    
                    kendo.bind(viewElement, websiteSettings);
                                         
                    var minDeliveryValue = websiteSettings.GeneralSettings.get("MinimumDeliveryAmount");
                    $scope.MinimumDeliveryAmount = minDeliveryValue ? minDeliveryValue / 100 : 0;
                    
                    var visible = customerAccountSettings.get("EnableFacebookLogin"); 
                    GeneralSettingsController.ShowFacebookIdInput($scope, $timeout, visible);

                    
                    GeneralSettingsController.WatchForValidLoginSettings($scope, $timeout, customerAccountSettings);
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }

        public static ShowFacebookIdInput($scope: Scopes.IGeneralSettingsScope, $timeout: ng.ITimeoutService, visible: boolean)
        {
            $timeout(()=> {
                $scope.ShowFacebookAppId = visible;           
            });
        }

        public static WatchForValidLoginSettings($scope: Scopes.IGeneralSettingsScope, $timeout: ng.ITimeoutService, customerAccountSettings : Models.ICustomerAccountSettings)
        {
            $timeout(() => {
                var hasFacebookLogin = customerAccountSettings.get("EnableFacebookLogin");
                var hasAndromedaLogin = customerAccountSettings.get("EnableAndromedaLogin");

                $scope.HasLoginOptions = hasAndromedaLogin || hasFacebookLogin;

                console.log("$scope.HasLoginOptions: " + $scope.HasLoginOptions);
            });


            customerAccountSettings.bind("change", (e) => {
                if(e.field === "EnableFacebookLogin" || e.field === "EnableAndromedaLogin")
                {
                    var hasFacebookLogin = customerAccountSettings.get("EnableFacebookLogin");
                    var hasAndromedaLogin = customerAccountSettings.get("EnableAndromedaLogin");

                    $timeout(() => {
                        $scope.HasLoginOptions = hasAndromedaLogin || hasFacebookLogin;
                    });
                }
            });

             $scope.$watch("HasLoginOptions", (newValue, oldValue) => {
                customerAccountSettings.set("IsEnable", newValue);
            });
        }
    }
} 