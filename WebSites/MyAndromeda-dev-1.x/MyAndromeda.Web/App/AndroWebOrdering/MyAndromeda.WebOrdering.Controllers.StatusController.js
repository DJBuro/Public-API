/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(StatusController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        StatusController.OnLoad($scope, $timeout, webOrderingWebApiService);
                    }
                ]);
            });
            var StatusController = (function () {
                function StatusController() {
                }
                StatusController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                    $scope.PublishChanges = function () {
                        webOrderingWebApiService.Publish();
                    };
                    $scope.PreviewChanges = function () {
                        webOrderingWebApiService.Preview();
                    };
                    webOrderingWebApiService.IsSaving.subscribe(function (e) {
                        $timout(function () {
                            $scope.Saving = e;
                        });
                    });
                    webOrderingWebApiService.IsPublishPreviewBusy.subscribe(function (e) {
                        $timout(function () {
                            $scope.PublishPreviewBusy = e;
                        });
                    });
                    webOrderingWebApiService.IsPublishLiveBusy.subscribe(function (e) {
                        $timout(function () {
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
                };
                StatusController.Name = "StatusController";
                return StatusController;
            })();
            Controllers.StatusController = StatusController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
