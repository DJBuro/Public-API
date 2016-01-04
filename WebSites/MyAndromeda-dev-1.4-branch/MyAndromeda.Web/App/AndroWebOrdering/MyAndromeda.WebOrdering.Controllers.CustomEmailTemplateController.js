/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("CustomEmailTemplateController", function ($scope, $timeout, ContextService, WebOrderingWebApiService) {
                    var setLiveColours = function () {
                        WebOrdering.Logger.Notify("Set live colours");
                        ContextService.Model.CustomEmailTemplate.LiveHeaderColour =
                            ContextService.Model.CustomEmailTemplate.HeaderColour;
                        ContextService.Model.CustomEmailTemplate.LiveFooterColour =
                            ContextService.Model.CustomEmailTemplate.FooterColour;
                    };
                    var resetColours = function () {
                        WebOrdering.Logger.Notify("Resetting email template colors");
                        var customEmailTemplate = ContextService.Model.CustomEmailTemplate;
                        customEmailTemplate.HeaderColour = customEmailTemplate.LiveHeaderColour;
                        customEmailTemplate.FooterColour = customEmailTemplate.LiveFooterColour;
                    };
                    var saveChanges = function () {
                        WebOrdering.Logger.Notify("Save");
                        setLiveColours();
                        WebOrderingWebApiService.Update();
                    };
                    $scope.SetLiveColors = setLiveColours;
                    $scope.ResetColors = resetColours;
                    $scope.SaveChanges = saveChanges;
                    ContextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        //set the scope
                        var customEmailTemplate = websiteSettings.CustomEmailTemplate;
                        var correct = function (key, value) {
                            if (!customEmailTemplate.get(key)) {
                                customEmailTemplate.set(key, "#EEEEEE");
                            }
                        };
                        correct("HeaderColour", "HeaderColour");
                        correct("FooterColour", "FooterColour");
                        setLiveColours();
                        $scope.CustomEmailTemplate = websiteSettings.CustomEmailTemplate;
                    });
                });
            });
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
