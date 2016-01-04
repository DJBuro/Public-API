/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(StatusController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    StatusController.OnLoad($scope, $timeout, webOrderingWebApiService);
                }
            ]);
    });

    export class StatusController {
        public static Name: string = "StatusController";

        public static OnLoad(
            $scope: Scopes.IStatusControllerScope,
            $timout: ng.ITimeoutService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {

            $scope.SaveChanges = () => {
                webOrderingWebApiService.Update();
            };

            $scope.PublishChanges = () => {
                webOrderingWebApiService.Publish();
            };

            $scope.PreviewChanges = () => {
                webOrderingWebApiService.Preview();
            };

            webOrderingWebApiService.IsSaving.subscribe(e=> {
                $timout(() => {
                    $scope.Saving = e;
                });
            });
            webOrderingWebApiService.IsPublishPreviewBusy.subscribe(e=> {
                $timout(() => {
                    $scope.PublishPreviewBusy = e;
                });
                
            });
            webOrderingWebApiService.IsPublishLiveBusy.subscribe(e=> {
                $timout(() => {
                    $scope.PublishLiveBusy = e;
                });
                
            });

            //webOrderingWebApiService.IsPreviewReady.subscribe((busy) => {
            //    $scope.PreviewWindow.refresh({
            //        //content: {
            //        //    url: webOrderingWebApiService.Context.Model.SiteDetails.DomainName + "?q" + Math.random() 
            //        //}
            //    });
            //    $scope.PreviewWindow.open();
            //    $scope.PreviewWindow.center();
            //});

            //webOrderingWebApiService.IsWebOrderingBusy.subscribe((busy) => {
            //    $timout(() => {
            //        if (busy) {
            //            if (!$scope.Modal) { return; }
            //            var m = <any>$scope.Modal;
            //            m.open();
            //        }
            //        else {
            //            if (!$scope.Modal) { return; }
            //            var m = <any>$scope.Modal;
            //            m.close();
            //        }
            //        $scope.IsBusy = busy;
            //    });
            //});
        }
    }
} 