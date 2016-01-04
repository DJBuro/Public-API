var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CustomThemeSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CustomThemeSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        CustomThemeSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var CustomThemeSettingsController = (function () {
                function CustomThemeSettingsController() {
                }
                CustomThemeSettingsController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    $scope.ResetColors = function () {
                        console.log("Resetting colors..");

                        contextService.Model.CustomThemeSettings.ColourRange1 = contextService.Model.CustomThemeSettings.LiveColourRange1;
                        contextService.Model.CustomThemeSettings.ColourRange2 = contextService.Model.CustomThemeSettings.LiveColourRange2;
                        contextService.Model.CustomThemeSettings.ColourRange3 = contextService.Model.CustomThemeSettings.LiveColourRange3;
                        contextService.Model.CustomThemeSettings.ColourRange4 = contextService.Model.CustomThemeSettings.LiveColourRange4;
                        contextService.Model.CustomThemeSettings.ColourRange5 = contextService.Model.CustomThemeSettings.LiveColourRange5;
                        contextService.Model.CustomThemeSettings.ColourRange6 = contextService.Model.CustomThemeSettings.LiveColourRange6;

                        var viewElement = $("#CustomThemeSettingsController");

                        kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
                    };

                    $scope.SaveChanges = function () {
                        console.log("save");
                        $scope.SetLiveColors();
                        webOrderingWebApiService.Update();
                        var viewElement = $("#CustomThemeSettingsController");
                        kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
                    };

                    $scope.SetLiveColors = function () {
                        contextService.Model.CustomThemeSettings.LiveColourRange1 = contextService.Model.CustomThemeSettings.ColourRange1;
                        contextService.Model.CustomThemeSettings.LiveColourRange2 = contextService.Model.CustomThemeSettings.ColourRange2;
                        contextService.Model.CustomThemeSettings.LiveColourRange3 = contextService.Model.CustomThemeSettings.ColourRange3;
                        contextService.Model.CustomThemeSettings.LiveColourRange4 = contextService.Model.CustomThemeSettings.ColourRange4;
                        contextService.Model.CustomThemeSettings.LiveColourRange5 = contextService.Model.CustomThemeSettings.ColourRange5;
                        contextService.Model.CustomThemeSettings.LiveColourRange6 = contextService.Model.CustomThemeSettings.ColourRange6;
                    };
                };

                CustomThemeSettingsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var customThemeSettings = websiteSettings.CustomThemeSettings;
                        var correct = function (key, value) {
                            if (!customThemeSettings.get(key)) {
                                customThemeSettings.set(key, null);
                            }
                        };

                        correct("ColourRange1", "ColourRange1");
                        correct("ColourRange2", "ColourRange2");
                        correct("ColourRange3", "ColourRange3");
                        correct("ColourRange4", "ColourRange4");
                        correct("ColourRange5", "ColourRange5");
                        correct("ColourRange6", "ColourRange6");

                        $scope.SetLiveColors();
                        var viewElement = $("#CustomThemeSettingsController");
                        kendo.bind(viewElement, websiteSettings.CustomThemeSettings);

                        $timout(function () {
                        });
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                CustomThemeSettingsController.Name = "CustomThemeSettingsController";
                return CustomThemeSettingsController;
            })();
            Controllers.CustomThemeSettingsController = CustomThemeSettingsController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
