var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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

                    webOrderingWebApiService.IsPreviewReady.subscribe(function (busy) {
                        $scope.PreviewWindow.refresh({});
                        $scope.PreviewWindow.open();
                        $scope.PreviewWindow.center();
                    });

                    webOrderingWebApiService.IsWebOrderingBusy.subscribe(function (busy) {
                        $timout(function () {
                            if (busy) {
                                if (!$scope.Modal) {
                                    return;
                                }
                                var m = $scope.Modal;
                                m.open();
                            } else {
                                if (!$scope.Modal) {
                                    return;
                                }
                                var m = $scope.Modal;
                                m.close();
                            }
                            $scope.IsBusy = busy;
                        });
                    });
                };
                StatusController.Name = "StatusController";
                return StatusController;
            })();
            Controllers.StatusController = StatusController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
