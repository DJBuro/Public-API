var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("CmsPagesController", function ($scope, $timeout, ContextService, WebOrderingWebApiService) {
                    var siteSettings = {
                        Pages: []
                    };
                    ContextService.ModelSubject.where(function (e) { return e !== null; }).subscribe(function (settings) {
                        $scope.pages = settings.Pages;
                        siteSettings = settings;
                    });
                    $scope.page = null;
                    $scope.add = function () {
                        var checkExisting = function (title) {
                            var existing = siteSettings.Pages.filter(function (item) { return item.Title === title; });
                            return existing.length > 0;
                        };
                        var newTitle = "Sample page " + (siteSettings.Pages.length + 1);
                        while (checkExisting(newTitle)) {
                            newTitle += "_";
                        }
                        var page = {
                            Title: newTitle,
                            Content: "Sample text",
                            Enabled: true
                        };
                        //$scope.page = page;
                        siteSettings.Pages.push(page);
                        var loading = $("#CmsEditors");
                        kendo.ui.progress(loading, true);
                        $timeout(function () {
                            var loadPage = siteSettings.Pages.filter(function (item) {
                                return item.Title === newTitle;
                            });
                            $scope.page = loadPage[0];
                            kendo.ui.progress(loading, false);
                        }, 500);
                    };
                    $scope.edit = function (page) {
                        $scope.page = page;
                    };
                    $scope.delete = function (page) {
                        if (!confirm("Sure you want to delete this item. There is no way of getting it back")) {
                            return;
                        }
                        siteSettings.Pages = siteSettings.Pages.filter(function (item) {
                            return item.Title !== page.Title;
                        });
                        $scope.pages = siteSettings.Pages;
                        if ($scope.page !== null && $scope.page.Title == page.Title) {
                            $scope.page = null;
                        }
                    };
                    $scope.save = function () {
                        WebOrderingWebApiService.Update();
                    };
                });
            });
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
