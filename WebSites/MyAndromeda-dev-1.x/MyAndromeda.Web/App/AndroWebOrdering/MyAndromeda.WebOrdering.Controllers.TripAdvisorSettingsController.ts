/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(TripAdvisorSettingsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    TripAdvisorSettingsController.SetupValidatorOptions($scope, $timeout, webOrderingWebApiService);
                    TripAdvisorSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    TripAdvisorSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                }
            ]);
    });

    export class TripAdvisorSettingsController {
        public static Name: string = "TripAdvisorSettingsController";

        public static OnLoad(
            $scope: Scopes.ITripAdvisorSettingsScope,
            $timout: ng.ITimeoutService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {
            
            //$scope.SaveChanges = () => {
            //    if ($scope.TripAdvisorSettingsValidator.validate()) {
            //        webOrderingWebApiService.Update();
            //    }
            //};

            //var s = <any>$scope;
            //s.FacebookSettings = {};
        }

        public static SetupValidatorOptions($scope: Scopes.ITripAdvisorSettingsScope, $timout: ng.ITimeoutService, contextService: Services.ContextService) {
            var validatorOptions: kendo.ui.ValidatorOptions = {
                name: "",
                rules: {
                    TripadvisorScirptRequired: function (input: JQuery) {
                        if (!input.is("[data-required-if-tripadvisor]")) {
                            return true;
                        }

                        var isEnabled = contextService.Model.TripAdvisorSettings.get("IsEnable");

                        var text: string = input.val();
                        return text.length > 0;
                    }
                }
            };
        }

        public static SetupKendoMvvm($scope: Scopes.ITripAdvisorSettingsScope, $timout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {
                    var viewElement = $("#TripAdvisorSettingsController");
                    kendo.bind(viewElement, websiteSettings.TripAdvisorSettings);                    
                    $scope.ShowTripAdvisorSettings = websiteSettings.TripAdvisorSettings.get("IsEnable");
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
} 