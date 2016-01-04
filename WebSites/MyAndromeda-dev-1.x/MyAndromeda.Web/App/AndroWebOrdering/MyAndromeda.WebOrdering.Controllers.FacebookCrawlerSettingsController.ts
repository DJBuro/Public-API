/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(FacebookCrawlerSettingsController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    FacebookCrawlerSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    FacebookCrawlerSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                }
            ]);
    });

    export class FacebookCrawlerSettingsController {
        public static Name: string = "FacebookCrawlerSettingsController";

        public static OnLoad(
            $scope: Scopes.IFacebookCrawlerSettingsScope,
            $timeout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {

            $scope.SaveChanges = () => {
                if (contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath == null || contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath == "") {
                    $("#FacebookCrawlerLogo").attr("required", "required");
                    $("#FacebookCrawlerLogo").attr("title", "Please select an image");
                }
                else {                   
                    if ($scope.FacebookCrawlerSettingsValidator.validate()) {
                        webOrderingWebApiService.Update();
                    }
                }            
            };

            $scope.HasFacebookProfileLogo = false;
            $scope.TempFaceboolProfileLogoPath = "";
        }

        private static SetupUploaders($scope: Scopes.IFacebookCrawlerSettingsScope, $timeout: ng.ITimeoutService, contextService: Services.ContextService) {

        var facebookImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadFacebookLogo/{2}", Settings.AndromedaSiteId, Settings.WebSiteId, "FacebookCrawler");

        $scope.FacebookProfileLogoUpload.setOptions({ async: { saveUrl: facebookImageUploadRoute, autoUpload: true }, showFileList: false, multiple: false });
        $scope.FacebookProfileLogoUpload.bind("success", (result) => {
            console.log(result);
            var observableObject = contextService.Model.FacebookCrawlerSettings;
            observableObject.set("FacebookProfileLogoPath", result.response.Url);

            var r = Math.floor(Math.random() * 99999) + 1;
            $timeout(() => {
                $scope.TempFaceboolProfileLogoPath = result.response.Url + "?k=" + r;
                contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath = result.response.Url;
                $("#FacebookCrawlerLogo").removeAttr("required");
                $("#FacebookCrawlerLogo").removeAttr("title");
                $scope.HasFacebookProfileLogo = true;
            });
        });
        }

        public static SetupKendoMvvm($scope: Scopes.IFacebookCrawlerSettingsScope, $timeout: ng.ITimeoutService, contextService: Services.ContextService): void {
            var settingsSubscription = contextService.ModelSubject
            .where((e) => { return e !== null; })
            .subscribe((webSiteSettings) => {

                var viewElement = $("#FacebookCrawlerSettingsController");
                kendo.bind(viewElement, webSiteSettings.FacebookCrawlerSettings);

                //added 500ms timeout as there are random issues. 
                $timeout(() => {
                    this.SetupUploaders($scope, $timeout, contextService);

                    var r = Math.floor(Math.random() * 99999) + 1;
                    $scope.TempFaceboolProfileLogoPath = webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath + "?k=" + r;

                    $scope.HasFacebookProfileLogo = webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath && webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath.length > 0;
                }, 500, true);
            });

            $scope.$on('$destroy', () => {
            settingsSubscription.dispose();
        });
        }
    }
} 