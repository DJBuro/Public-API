/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(SiteDetailsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    SiteDetailsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    SiteDetailsController.SetupKendoMvvm($scope, $timeout, contextService);
                }
            ]);
    });

    export class SiteDetailsController {
        public static Name: string = "SiteDetailsController";

        public static OnLoad(
            $scope: Scopes.ISiteDetailsScope,
            $timeout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {

            $scope.SaveChanges = () => {
                if ($scope.SiteDetailsValidator.validate()) {
                    webOrderingWebApiService.Update();
                }
            };           

            $scope.HasWebsiteLogo = false;
            $scope.HasMobileLogo = false;
            $scope.TempWebsiteLogoPath = "";
            $scope.TempMobileLogoPath = "";
        }

        private static SetupUploaders($scope: Scopes.ISiteDetailsScope, $timeout: ng.ITimeoutService, contextService: Services.ContextService) {
            
            if(!$scope.MainImageUpload){ alert("MainImageUpload hasn't been created. Pester Matt"); }
            if(!$scope.MobileImageUpload) { alert("MobileImageUpload hasn't been created. Pester Matt"); }

            var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", Settings.AndromedaSiteId, Settings.WebSiteId, "website");
            $scope.MainImageUpload.setOptions({
                async: {
                    saveUrl: webSiteImageUploadRoute,
                    autoUpload: true
                },
                showFileList: false,
                multiple: false
            });

            var mobileImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", Settings.AndromedaSiteId, Settings.WebSiteId, "mobile");
            $scope.MobileImageUpload.setOptions({
                async:
                {
                    saveUrl: mobileImageUploadRoute,
                    autoUpload: true
                },
                showFileList: false,
                multiple: false
            });

            var FaviconUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", Settings.AndromedaSiteId, Settings.WebSiteId, "favicon");
            $scope.FaviconImageUpload.setOptions({
                async: {
                    saveUrl: FaviconUploadRoute,
                    autoUpload: true
                },
                showFileList: false,
                multiple: false
            });

            $scope.MainImageUpload.bind("success", (result) => {
                Logger.Notify(result);

                var observableObject = contextService.Model.SiteDetails;
                observableObject.set("WebsiteLogoPath", result.response.Url);

                var r = Math.floor(Math.random() * 99999) + 1;
                $timeout(() => {
                    $scope.TempWebsiteLogoPath = result.response.Url + "?k=" + r;
                    $scope.HasWebsiteLogo = true;
                });
            });

            $scope.MobileImageUpload.bind("success", (result) => {
                Logger.Notify(result);

                var observableObject = contextService.Model.SiteDetails;
                observableObject.set("MobileLogoPath", result.response.Url);

                var r = Math.floor(Math.random() * 99999) + 1;
                $timeout(() => {
                    $scope.TempMobileLogoPath = result.response.Url + "?k=" + r;
                    $scope.HasMobileLogo = true;
                });
            });

            $scope.FaviconImageUpload.bind("success", (result) => {
                console.log(result);

                var observableObject = contextService.Model.SiteDetails;
                observableObject.set("FaviconPath", result.response.Url);

                var r = Math.floor(Math.random() * 99999) + 1;
                $timeout(() => {
                    $scope.TempFaviconLogoPath = result.response.Url + "?k=" + r;
                    $scope.HasFaviconLogo = true;
                });
            });
        }

        public static SetupKendoMvvm($scope: Scopes.ISiteDetailsScope, $timeout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((webSiteSettings) => {

                    var viewElement = $("#SiteDetailsController");
                    kendo.bind(viewElement, webSiteSettings.SiteDetails);

                    $scope.WebSiteSettings = webSiteSettings;
                    $scope.SiteDetails = webSiteSettings.SiteDetails;

                    //added 500ms timeout as there are random issues. 
                    $timeout(() => {
                        this.SetupUploaders($scope, $timeout, contextService);
                    
                        var r = Math.floor(Math.random() * 99999) + 1;
                        
                        if(webSiteSettings.SiteDetails.WebsiteLogoPath && webSiteSettings.SiteDetails.WebsiteLogoPath !== null){
                            $scope.TempWebsiteLogoPath = webSiteSettings.SiteDetails.WebsiteLogoPath + "?k=" + r;
                        }
                        if(webSiteSettings.SiteDetails.MobileLogoPath && webSiteSettings.SiteDetails.MobileLogoPath !== null) {
                            $scope.TempMobileLogoPath = webSiteSettings.SiteDetails.MobileLogoPath + "?k=" + r;
                        }
                        if(webSiteSettings.SiteDetails.FaviconPath && webSiteSettings.SiteDetails.FaviconPath !== null){
                            $scope.TempFaviconLogoPath = webSiteSettings.SiteDetails.FaviconPath + "?k=" + r;
                        }

                        $scope.HasWebsiteLogo = webSiteSettings.SiteDetails.WebsiteLogoPath && webSiteSettings.SiteDetails.WebsiteLogoPath.length > 0;
                        $scope.HasMobileLogo = webSiteSettings.SiteDetails.MobileLogoPath && webSiteSettings.SiteDetails.MobileLogoPath.length > 0;
                        $scope.HasFaviconLogo = webSiteSettings.SiteDetails.FaviconPath && webSiteSettings.SiteDetails.FaviconPath.length > 0;

                    },500, true);

                    
                });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
} 