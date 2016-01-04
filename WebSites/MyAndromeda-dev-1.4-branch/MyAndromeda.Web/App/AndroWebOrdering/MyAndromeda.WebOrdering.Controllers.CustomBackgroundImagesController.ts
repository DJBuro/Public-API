/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(CustomBackgroundImagesController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    CustomBackgroundImagesController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    CustomBackgroundImagesController.SetupKendoMvvm($scope, $timeout, contextService);
                }
            ]);
    });

    export class CustomBackgroundImagesController {
        public static Name: string = "CustomBackgroundImagesController";

        public static OnLoad(
            $scope: Scopes.ICustomThemeSettingsScope,
            $timeout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {
            $scope.SaveChanges = () => {
                console.log("save");
                webOrderingWebApiService.Update();
            };

            $scope.DeleteCustomBackgroundImage = (imageType) => {
                var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/DeleteBackgroundImage/{2}", Settings.AndromedaSiteId, Settings.WebSiteId, imageType);

                var deleteImage = $.ajax({
                    url: webSiteImageUploadRoute,
                    type: "POST",
                    contentType: 'application/json',
                    dataType: "json"
                });

                $timeout(() => {
                    if (imageType == 'desktop') {
                        $scope.HasDesktopBackgroundImage = false;
                        $scope.TempDesktopBackgroundImagePath = contextService.Model.CustomThemeSettings.DesktopBackgroundImagePath = null;
                    }
                    else {
                        $scope.HasMobileBackgroundImage = false;
                        $scope.TempMobileBackgroundImagePath = contextService.Model.CustomThemeSettings.MobileBackgroundImagePath = null;
                    }
                    $scope.SaveChanges();
                });
               
            }
            $scope.HasDesktopBackgroundImage = false;
            $scope.HasMobileBackgroundImage = false;
            $scope.TempDesktopBackgroundImagePath = "";
            $scope.TempMobileBackgroundImagePath = "";
        }

        public static SetupKendoMvvm($scope: Scopes.ICustomThemeSettingsScope, $timeout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
                .where((e) => { return e !== null; })
                .subscribe((websiteSettings) => {
                    var customThemeSettings = websiteSettings.CustomThemeSettings;
                    var viewElement = $("#CustomBackgroundImagesController");
                    kendo.bind(viewElement, websiteSettings.CustomThemeSettings);
                    $timeout(() => {
                        var r = Math.floor(Math.random() * 99999) + 1;
                        $scope.TempDesktopBackgroundImagePath = customThemeSettings.DesktopBackgroundImagePath + "?k=" + r;
                        $scope.TempMobileBackgroundImagePath = customThemeSettings.MobileBackgroundImagePath + "?k=" + r;

                        $scope.HasDesktopBackgroundImage = customThemeSettings.DesktopBackgroundImagePath && customThemeSettings.DesktopBackgroundImagePath.length > 0;
                        $scope.HasMobileBackgroundImage = customThemeSettings.MobileBackgroundImagePath && customThemeSettings.MobileBackgroundImagePath.length > 0;
                    });
                });

            var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadBackgroundImage/{2}", Settings.AndromedaSiteId, Settings.WebSiteId, "desktop");
            var desktopUpload = <kendo.ui.Upload>$("#DesktopBackgroundUpload").kendoUpload({
                async: {
                    saveUrl: webSiteImageUploadRoute,
                    autoUpload: true,
                    batch: false
                },
                showFileList: false
            }).data("kendoUpload");

            desktopUpload.bind("success", (result) => {
                console.log(result);
                var observableObject = contextService.Model.CustomThemeSettings;

                observableObject.set("DesktopBackgroundImagePath", result.response.Url);

                var r = Math.floor(Math.random() * 99999) + 1;
                $timeout(() => {
                    $scope.TempDesktopBackgroundImagePath = result.response.Url + "?k=" + r;
                    $scope.HasDesktopBackgroundImage = true;

                });
            });

            var mobileImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadBackgroundImage/{2}", Settings.AndromedaSiteId, Settings.WebSiteId, "mobile");
            var mobileUpload = <kendo.ui.Upload>$("#MobileBackgroundUpload").kendoUpload({
                async: {
                    saveUrl: mobileImageUploadRoute,
                    autoUpload: true,
                    batch: false
                },
                showFileList: false
            }).data("kendoUpload");

            mobileUpload.bind("success", (result) => {
                console.log(result);
                var observableObject = contextService.Model.CustomThemeSettings;
                observableObject.set("MobileBackgroundImagePath", result.response.Url);

                var r = Math.floor(Math.random() * 99999) + 1;
                $timeout(() => {
                    $scope.TempMobileBackgroundImagePath = result.response.Url + "?k=" + r;
                    $scope.HasMobileBackgroundImage = true;

                });
            });

            $scope.$on('$destroy', () => {
                settingsSubscription.dispose();
            });
        }
    }
}    