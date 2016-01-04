var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CustomEmailTemplateController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CustomEmailTemplateController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        CustomEmailTemplateController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var CustomEmailTemplateController = (function () {
                function CustomEmailTemplateController() {
                }
                CustomEmailTemplateController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    $scope.ResetColors = function () {
                        console.log("Resetting email template colors..");
                        contextService.Model.CustomEmailTemplate.HeaderColour = contextService.Model.CustomEmailTemplate.LiveHeaderColour;
                        contextService.Model.CustomEmailTemplate.FooterColour = contextService.Model.CustomEmailTemplate.LiveFooterColour;
                        var viewElement = $("#CustomEmailTemplateController");
                        kendo.bind(viewElement, contextService.Model.CustomEmailTemplate);
                    };

                    $scope.SaveChanges = function () {
                        console.log("save");
                        $scope.SetLiveColors();
                        webOrderingWebApiService.Update();
                        var viewElement = $("#CustomEmailTemplateController");
                        kendo.bind(viewElement, contextService.Model.CustomEmailTemplate);
                    };

                    $scope.SetLiveColors = function () {
                        contextService.Model.CustomEmailTemplate.LiveHeaderColour = contextService.Model.CustomEmailTemplate.HeaderColour;
                        contextService.Model.CustomEmailTemplate.LiveFooterColour = contextService.Model.CustomEmailTemplate.FooterColour;
                    };
                };

                CustomEmailTemplateController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var customEmailTemplate = websiteSettings.CustomEmailTemplate;
                        var correct = function (key, value) {
                            if (!customEmailTemplate.get(key)) {
                                customEmailTemplate.set(key, "#EEEEEE");
                            }
                        };
                        correct("HeaderColour", "HeaderColour");
                        correct("FooterColour", "FooterColour");
                        $scope.SetLiveColors();
                        var viewElement = $("#CustomEmailTemplateController");
                        kendo.bind(viewElement, websiteSettings.CustomEmailTemplate);
                        $timout(function () {
                        });
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                CustomEmailTemplateController.Name = "CustomEmailTemplateController";
                return CustomEmailTemplateController;
            })();
            Controllers.CustomEmailTemplateController = CustomEmailTemplateController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
