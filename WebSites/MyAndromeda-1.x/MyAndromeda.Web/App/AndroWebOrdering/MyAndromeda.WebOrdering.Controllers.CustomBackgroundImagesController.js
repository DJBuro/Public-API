var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CustomBackgroundImagesController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CustomBackgroundImagesController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        CustomBackgroundImagesController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var CustomBackgroundImagesController = (function () {
                function CustomBackgroundImagesController() {
                }
                CustomBackgroundImagesController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        console.log("save");
                        webOrderingWebApiService.Update();
                    };

                    $scope.DeleteCustomBackgroundImage = function (imageType) {
                        var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/DeleteBackgroundImage/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, imageType);

                        var deleteImage = $.ajax({
                            url: webSiteImageUploadRoute,
                            type: "POST",
                            contentType: 'application/json',
                            dataType: "json"
                        });

                        $timeout(function () {
                            if (imageType == 'desktop') {
                                $scope.HasDesktopBackgroundImage = false;
                                $scope.TempDesktopBackgroundImagePath = contextService.Model.CustomThemeSettings.DesktopBackgroundImagePath = null;
                            } else {
                                $scope.HasMobileBackgroundImage = false;
                                $scope.TempMobileBackgroundImagePath = contextService.Model.CustomThemeSettings.MobileBackgroundImagePath = null;
                            }
                            $scope.SaveChanges();
                        });
                    };
                    $scope.HasDesktopBackgroundImage = false;
                    $scope.HasMobileBackgroundImage = false;
                    $scope.TempDesktopBackgroundImagePath = "";
                    $scope.TempMobileBackgroundImagePath = "";
                };

                CustomBackgroundImagesController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var customThemeSettings = websiteSettings.CustomThemeSettings;
                        var viewElement = $("#CustomBackgroundImagesController");
                        kendo.bind(viewElement, websiteSettings.CustomThemeSettings);
                        $timeout(function () {
                            var r = Math.floor(Math.random() * 99999) + 1;
                            $scope.TempDesktopBackgroundImagePath = customThemeSettings.DesktopBackgroundImagePath + "?k=" + r;
                            $scope.TempMobileBackgroundImagePath = customThemeSettings.MobileBackgroundImagePath + "?k=" + r;

                            $scope.HasDesktopBackgroundImage = customThemeSettings.DesktopBackgroundImagePath && customThemeSettings.DesktopBackgroundImagePath.length > 0;
                            $scope.HasMobileBackgroundImage = customThemeSettings.MobileBackgroundImagePath && customThemeSettings.MobileBackgroundImagePath.length > 0;
                        });
                    });

                    var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadBackgroundImage/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "desktop");
                    var desktopUpload = $("#DesktopBackgroundUpload").kendoUpload({
                        async: {
                            saveUrl: webSiteImageUploadRoute,
                            autoUpload: true,
                            batch: false
                        },
                        showFileList: false
                    }).data("kendoUpload");

                    desktopUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.CustomThemeSettings;

                        observableObject.set("DesktopBackgroundImagePath", result.response.Url);

                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempDesktopBackgroundImagePath = result.response.Url + "?k=" + r;
                            $scope.HasDesktopBackgroundImage = true;
                        });
                    });

                    var mobileImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadBackgroundImage/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "mobile");
                    var mobileUpload = $("#MobileBackgroundUpload").kendoUpload({
                        async: {
                            saveUrl: mobileImageUploadRoute,
                            autoUpload: true,
                            batch: false
                        },
                        showFileList: false
                    }).data("kendoUpload");

                    mobileUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.CustomThemeSettings;
                        observableObject.set("MobileBackgroundImagePath", result.response.Url);

                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempMobileBackgroundImagePath = result.response.Url + "?k=" + r;
                            $scope.HasMobileBackgroundImage = true;
                        });
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                CustomBackgroundImagesController.Name = "CustomBackgroundImagesController";
                return CustomBackgroundImagesController;
            })();
            Controllers.CustomBackgroundImagesController = CustomBackgroundImagesController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
