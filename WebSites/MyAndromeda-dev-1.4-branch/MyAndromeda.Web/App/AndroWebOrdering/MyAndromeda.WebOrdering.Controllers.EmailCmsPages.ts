
module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        /* Store editor section */
        app.directive("storeEmailSection",() => {
            Logger.Notify("directive loaded")
            return {
                restrict: "E",
                transclude: true,
                template: $("#StoreEmailSection").html(),// "<div class='hi {{context.Store.Name}}'><ng-transclude></ng-transclude></div>",
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
                controller: ($scope, $timeout, ContextService: Services.ContextService) => {
                    var store: Models.IStore = $scope.ngModel;

                    var lookup = (store: Models.IStore) => {
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

                    Logger.Notify("s: ");
                    Logger.Notify(context);

                    $scope.edit = (page: Models.IPage) => {
                        context.Page = page;
                    };

                    $scope.delete = (page: Models.IPage) => {
                        if (!confirm("Sure you want to delete this item. There is no way of getting it back")) {
                            return;
                        }

                        context.Pages = context.Pages.filter((item) => {
                            return item.Title !== page.Title;
                        });

                        if (context.Page !== null && context.Page.Title == page.Title) {
                            context.Page = null;
                        }
                    };
                }
            };
        });

        app.controller("EmailCmsPagesController",(
            $scope, $timeout,
            ContextService: Services.ContextService,
            WebOrderingWebApiService: Services.WebOrderingWebApiService) => {

            var settings = ContextService.ModelSubject.where(e=> e !== null);
            var stores = ContextService.StoreSubject.where(e=> e.length > 0);

            var emailSettings: Models.ICmsPages = {
                Pages: [
                    //region on the email to inject content into, if enabled. 
                    { Title: "Custom Content", Content: "", Enabled: false }
                ]
            };

            $scope.stores = [];
            $scope.page = null;


            var addSectionsForStore = (store: Models.IStore, sections: Models.IPage[]) => {
                Rx.Observable.fromArray(emailSettings.Pages).subscribe((page) => {
                    //if its got it ... don't care. 
                    if (sections.filter((item) => item.Title === page.Title).length > 0) {
                        return;
                    }

                    var newPage = JSON.parse(JSON.stringify(page));
                    //add any missing email sections. 
                    sections.push(newPage);
                });
            };

            var both = Rx.Observable
                .zip(settings, stores,(settings, stores) => {
                    return { settings: settings, stores: stores }
                })
                .subscribe((storesAndSettings) => {

                var settings = storesAndSettings.settings.CustomEmailTemplate;
                for (var i = 0; i < storesAndSettings.stores.length; i++) {
                    var store = storesAndSettings.stores[i],
                        storeId = store.AndromedaSiteId;

                    if (!settings.CustomTemplates[storeId]) {
                        settings.CustomTemplates[storeId] = [];
                    }

                    var sections = settings.CustomTemplates[storeId];
                    addSectionsForStore(store, sections);
                }

                //Logger.Notify("Stores in email settings: " settings.CustomTemplates.);
            });

            stores.subscribe((storeList) => {
                $scope.stores = storeList;
            });

            $scope.SaveChanges = () => {
                WebOrderingWebApiService.Update();
            };

        });

    });

}

