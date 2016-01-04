/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(PickThemeController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    WebOrdering.Services.WebOrderingThemeWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService, webOrderingThemeWebApiService) {
                        PickThemeController.OnLoad($scope, $timeout, webOrderingWebApiService, webOrderingThemeWebApiService);
                        PickThemeController.SetupScope($scope);
                        PickThemeController.SetupSelection($scope, webOrderingWebApiService);
                        PickThemeController.SetupCurrentSelection($scope, $timeout, contextService);
                    }
                ]);
            });
            var PickThemeController = (function () {
                function PickThemeController() {
                }
                PickThemeController.OnLoad = function ($scope, $timout, webOrderingWebApiService, webOrderingThemeWebApiService) {
                    var isThemesBusySubscription = webOrderingThemeWebApiService.IsBusy.subscribe(function (value) {
                        $timout(function () {
                            $scope.IsThemesBusy = value;
                        });
                    });
                    var isDataBusySubscription = webOrderingWebApiService.IsLoading.subscribe(function (value) {
                        $timout(function () {
                            $scope.IsDataBusy = value;
                        });
                    });
                    $scope.ListViewTemplate = $("#ListViewTemplate").html();
                    $scope.DataSource = webOrderingThemeWebApiService.GetThemeDataSource();
                    $scope.HasPreviewTheme = false;
                    $scope.HasCurrentTheme = false;
                    $scope.SearchTemplates = function () {
                        webOrderingThemeWebApiService.SearchText($scope.SearchText);
                    };
                    $scope.$on('$destroy', function () {
                        isThemesBusySubscription.dispose();
                    });
                };
                PickThemeController.SetupCurrentSelection = function ($scope, $timout, contextService) {
                    var modelSubscription = contextService.ModelSubject.where(function (value) {
                        return value !== null;
                    }).subscribe(function (settings) {
                        var s = settings;
                        s.bind("change", function () {
                            $timout(function () {
                                //console.log("set current theme settings");
                                console.log(settings.ThemeSettings);
                                $scope.CurrentTheme = settings.ThemeSettings;
                                $scope.HasCurrentTheme = true;
                            });
                        });
                        console.log(settings.ThemeSettings);
                        $scope.CurrentTheme = settings.ThemeSettings;
                        $scope.HasCurrentTheme = true;
                    });
                    $scope.$on("$destroy", function () {
                        modelSubscription.dispose();
                    });
                };
                PickThemeController.SetupSelection = function ($scope, webOrderingWebApiService) {
                    $scope.SelectTemplate = function (id) {
                        var dataSource = $scope.DataSource;
                        var previewItem = dataSource.data().find(function (item) {
                            return item.Id === id;
                        });
                        $scope.HasPreviewTheme = true;
                        $scope.SelectedTheme = previewItem;
                    };
                    $scope.SelectPreviewTheme = function (theme) {
                        console.log(theme);
                        webOrderingWebApiService.UpdateThemeSettings(theme);
                    };
                };
                PickThemeController.SetupScope = function ($scope) {
                    $scope.$on('$destroy', function () { });
                };
                PickThemeController.Name = "PickThemeController";
                PickThemeController.Route = "/";
                return PickThemeController;
            })();
            Controllers.PickThemeController = PickThemeController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
