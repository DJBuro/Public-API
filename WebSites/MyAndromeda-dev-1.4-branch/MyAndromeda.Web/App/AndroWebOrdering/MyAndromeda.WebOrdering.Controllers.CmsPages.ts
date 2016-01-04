module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller("CmsPagesController",($scope, $timeout,
            ContextService: Services.ContextService,
            WebOrderingWebApiService: Services.WebOrderingWebApiService) => {

            var siteSettings: Models.ICmsPages = {
                Pages: []
            };

            ContextService.ModelSubject.where((e) => e !== null).subscribe((settings) => {
                $scope.pages = settings.Pages;
                siteSettings = settings;
            });
            
            $scope.page = null;

            $scope.add = () => {
                var checkExisting = (title : string) => {
                    var existing = siteSettings.Pages.filter((item) => item.Title === title);
                    return existing.length > 0;
                };
                var newTitle = "Sample page " + (siteSettings.Pages.length + 1);
                
                while (checkExisting(newTitle))
                {
                    newTitle += "_";
                }

                var page: Models.IPage = {
                    Title: newTitle,
                    Content: "Sample text",
                    Enabled: true
                };

                //$scope.page = page;

                siteSettings.Pages.push(page);

                var loading = $("#CmsEditors");
                kendo.ui.progress(loading, true);

                $timeout(() => {
                    var loadPage = siteSettings.Pages.filter((item) => {
                        return item.Title === newTitle;
                    });
                    $scope.page = loadPage[0];
                    kendo.ui.progress(loading, false);
                }, 500);
            };

            $scope.edit = (page: Models.IPage) => {
                $scope.page = page;
            };

            $scope.delete = (page: Models.IPage) => {
                if (!confirm("Sure you want to delete this item. There is no way of getting it back"))
                {
                    return;
                }

                siteSettings.Pages = siteSettings.Pages.filter((item) => {
                    return item.Title !== page.Title;
                });

                $scope.pages = siteSettings.Pages;

                if ($scope.page !== null && $scope.page.Title == page.Title) {
                    $scope.page = null;
                }
            };

            $scope.save = () => {
                WebOrderingWebApiService.Update();
            };
            
        });
    });
}

