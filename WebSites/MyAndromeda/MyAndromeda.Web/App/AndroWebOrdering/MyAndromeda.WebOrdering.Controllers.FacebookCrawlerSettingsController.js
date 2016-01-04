var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(FacebookCrawlerSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        FacebookCrawlerSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        FacebookCrawlerSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var FacebookCrawlerSettingsController = (function () {
                function FacebookCrawlerSettingsController() {
                }
                FacebookCrawlerSettingsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        if (contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath == null || contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath == "") {
                            $("#FacebookCrawlerLogo").attr("required", "required");
                            $("#FacebookCrawlerLogo").attr("title", "Please select an image");
                        } else {
                            if ($scope.FacebookCrawlerSettingsValidator.validate()) {
                                webOrderingWebApiService.Update();
                            }
                        }
                    };

                    $scope.HasFacebookProfileLogo = false;
                    $scope.TempFaceboolProfileLogoPath = "";
                };

                FacebookCrawlerSettingsController.SetupUploaders = function ($scope, $timeout, contextService) {
                    var facebookImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadFacebookLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "FacebookCrawler");

                    $scope.FacebookProfileLogoUpload.setOptions({ async: { saveUrl: facebookImageUploadRoute, autoUpload: true }, showFileList: false, multiple: false });
                    $scope.FacebookProfileLogoUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.FacebookCrawlerSettings;
                        observableObject.set("FacebookProfileLogoPath", result.response.Url);

                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempFaceboolProfileLogoPath = result.response.Url + "?k=" + r;
                            contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath = result.response.Url;
                            $("#FacebookCrawlerLogo").removeAttr("required");
                            $("#FacebookCrawlerLogo").removeAttr("title");
                            $scope.HasFacebookProfileLogo = true;
                        });
                    });
                };

                FacebookCrawlerSettingsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var _this = this;
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (webSiteSettings) {
                        var viewElement = $("#FacebookCrawlerSettingsController");
                        kendo.bind(viewElement, webSiteSettings.FacebookCrawlerSettings);

                        //added 500ms timeout as there are random issues.
                        $timeout(function () {
                            _this.SetupUploaders($scope, $timeout, contextService);

                            var r = Math.floor(Math.random() * 99999) + 1;
                            $scope.TempFaceboolProfileLogoPath = webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath + "?k=" + r;

                            $scope.HasFacebookProfileLogo = webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath && webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath.length > 0;
                        }, 500, true);
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                FacebookCrawlerSettingsController.Name = "FacebookCrawlerSettingsController";
                return FacebookCrawlerSettingsController;
            })();
            Controllers.FacebookCrawlerSettingsController = FacebookCrawlerSettingsController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
