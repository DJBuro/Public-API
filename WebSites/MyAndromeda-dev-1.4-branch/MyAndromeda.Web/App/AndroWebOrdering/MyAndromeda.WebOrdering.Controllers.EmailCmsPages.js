var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                /* Store editor section */
                app.directive("storeEmailSection", function () {
                    WebOrdering.Logger.Notify("directive loaded");
                    return {
                        restrict: "E",
                        transclude: true,
                        template: $("#StoreEmailSection").html(),
                        //templateUrl: "#StoreEmailSection",
                        require: '^ngModel',
                        scope: {
                            ngModel: "="
                        },
                        link: function (scope, element, attrs, controller, transclude) {
                            //transclude(scope, function (clone, scope) {
                            //    element.append(clone);
                            //});
                        },
                        controller: function ($scope, $timeout, ContextService) {
                            var store = $scope.ngModel;
                            var lookup = function (store) {
                                var pages = ContextService.Model.CustomEmailTemplate.CustomTemplates[store.AndromedaSiteId];
                                var context = {
                                    Store: store,
                                    Pages: pages,
                                    Page: pages[0]
                                };
                                return context;
                            };
                            var context = lookup(store);
                            $scope.context = context;
                            WebOrdering.Logger.Notify("s: ");
                            WebOrdering.Logger.Notify(context);
                            $scope.edit = function (page) {
                                context.Page = page;
                            };
                            $scope.delete = function (page) {
                                if (!confirm("Sure you want to delete this item. There is no way of getting it back")) {
                                    return;
                                }
                                context.Pages = context.Pages.filter(function (item) {
                                    return item.Title !== page.Title;
                                });
                                if (context.Page !== null && context.Page.Title == page.Title) {
                                    context.Page = null;
                                }
                            };
                        }
                    };
                });
                app.controller("EmailCmsPagesController", function ($scope, $timeout, ContextService, WebOrderingWebApiService) {
                    var settings = ContextService.ModelSubject.where(function (e) { return e !== null; });
                    var stores = ContextService.StoreSubject.where(function (e) { return e.length > 0; });
                    var emailSettings = {
                        Pages: [
                            //region on the email to inject content into, if enabled. 
                            { Title: "Custom Content", Content: "", Enabled: false }
                        ]
                    };
                    $scope.stores = [];
                    $scope.page = null;
                    var addSectionsForStore = function (store, sections) {
                        Rx.Observable.fromArray(emailSettings.Pages).subscribe(function (page) {
                            //if its got it ... don't care. 
                            if (sections.filter(function (item) { return item.Title === page.Title; }).length > 0) {
                                return;
                            }
                            var newPage = JSON.parse(JSON.stringify(page));
                            //add any missing email sections. 
                            sections.push(newPage);
                        });
                    };
                    var both = Rx.Observable
                        .zip(settings, stores, function (settings, stores) {
                        return { settings: settings, stores: stores };
                    })
                        .subscribe(function (storesAndSettings) {
                        var settings = storesAndSettings.settings.CustomEmailTemplate;
                        for (var i = 0; i < storesAndSettings.stores.length; i++) {
                            var store = storesAndSettings.stores[i], storeId = store.AndromedaSiteId;
                            if (!settings.CustomTemplates[storeId]) {
                                settings.CustomTemplates[storeId] = [];
                            }
                            var sections = settings.CustomTemplates[storeId];
                            addSectionsForStore(store, sections);
                        }
                        //Logger.Notify("Stores in email settings: " settings.CustomTemplates.);
                    });
                    stores.subscribe(function (storeList) {
                        $scope.stores = storeList;
                    });
                    $scope.SaveChanges = function () {
                        WebOrderingWebApiService.Update();
                    };
                });
            });
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
