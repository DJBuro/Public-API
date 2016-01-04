var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(SiteDetailsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        SiteDetailsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        SiteDetailsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var SiteDetailsController = (function () {
                function SiteDetailsController() {
                }
                SiteDetailsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        if ($scope.SiteDetailsValidator.validate()) {
                            webOrderingWebApiService.Update();
                        }
                    };

                    $scope.HasWebsiteLogo = false;
                    $scope.HasMobileLogo = false;
                    $scope.TempWebsiteLogoPath = "";
                    $scope.TempMobileLogoPath = "";
                };

                SiteDetailsController.SetupUploaders = function ($scope, $timeout, contextService) {
                    var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "website");
                    var mobileImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "mobile");
                    var FaviconUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "favicon");

                    if (!$scope.MainImageUpload) {
                        alert("MainImageUpload hasnt been created. Pester Matt");
                    }

                    $scope.MainImageUpload.setOptions({ async: { saveUrl: webSiteImageUploadRoute, autoUpload: true }, showFileList: false, multiple: false });
                    $scope.MainImageUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.SiteDetails;
                        observableObject.set("WebsiteLogoPath", result.response.Url);

                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempWebsiteLogoPath = result.response.Url + "?k=" + r;
                            $scope.HasWebsiteLogo = true;
                        });
                    });

                    if (!$scope.MobileImageUpload) {
                        alert("MobileImageUpload hasn't been created. Pester Matt");
                    }

                    $scope.MobileImageUpload.setOptions({
                        async: {
                            saveUrl: mobileImageUploadRoute,
                            autoUpload: true
                        },
                        showFileList: false,
                        multiple: false
                    });
                    $scope.MobileImageUpload.bind("success", function (result) {
                        console.log(result);

                        var observableObject = contextService.Model.SiteDetails;
                        observableObject.set("MobileLogoPath", result.response.Url);

                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempMobileLogoPath = result.response.Url + "?k=" + r;
                            $scope.HasMobileLogo = true;
                        });
                    });

                    $scope.FaviconImageUpload.setOptions({ async: { saveUrl: FaviconUploadRoute, autoUpload: true }, showFileList: false, multiple: false });
                    $scope.FaviconImageUpload.bind("success", function (result) {
                        console.log(result);

                        var observableObject = contextService.Model.SiteDetails;
                        observableObject.set("FaviconPath", result.response.Url);

                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempFaviconLogoPath = result.response.Url + "?k=" + r;
                            $scope.HasFaviconLogo = true;
                        });
                    });
                };

                SiteDetailsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var _this = this;
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (webSiteSettings) {
                        var viewElement = $("#SiteDetailsController");
                        kendo.bind(viewElement, webSiteSettings.SiteDetails);

                        //added 500ms timeout as there are random issues.
                        $timeout(function () {
                            _this.SetupUploaders($scope, $timeout, contextService);

                            var r = Math.floor(Math.random() * 99999) + 1;

                            if (webSiteSettings.SiteDetails.WebsiteLogoPath && webSiteSettings.SiteDetails.WebsiteLogoPath !== null) {
                                $scope.TempWebsiteLogoPath = webSiteSettings.SiteDetails.WebsiteLogoPath + "?k=" + r;
                            }
                            if (webSiteSettings.SiteDetails.MobileLogoPath && webSiteSettings.SiteDetails.MobileLogoPath !== null) {
                                $scope.TempMobileLogoPath = webSiteSettings.SiteDetails.MobileLogoPath + "?k=" + r;
                            }
                            if (webSiteSettings.SiteDetails.FaviconPath && webSiteSettings.SiteDetails.FaviconPath !== null) {
                                $scope.TempFaviconLogoPath = webSiteSettings.SiteDetails.FaviconPath + "?k=" + r;
                            }

                            $scope.HasWebsiteLogo = webSiteSettings.SiteDetails.WebsiteLogoPath && webSiteSettings.SiteDetails.WebsiteLogoPath.length > 0;
                            $scope.HasMobileLogo = webSiteSettings.SiteDetails.MobileLogoPath && webSiteSettings.SiteDetails.MobileLogoPath.length > 0;
                            $scope.HasFaviconLogo = webSiteSettings.SiteDetails.FaviconPath && webSiteSettings.SiteDetails.FaviconPath.length > 0;
                        }, 500, true);
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                SiteDetailsController.Name = "SiteDetailsController";
                return SiteDetailsController;
            })();
            Controllers.SiteDetailsController = SiteDetailsController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
