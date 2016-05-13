module MyAndromeda.Components {
    var findStoreComponet = angular.module("MyAndromeda.Components.FindStore", ["MyAndromeda.Store.Services", "MyAndromeda.Components.StoreAdminControls"]);


    findStoreComponet.controller("FindStoreController", (
        $scope,
        $timeout,
        storeService: MyAndromeda.Store.Services.StoreService) => {
        var searchSubject = new Rx.Subject<string>();

        var context = {
            Store: "",
            Stores: [],
            NoItems: false
        };
         
        let search = (text: string) => {
            Logger.Notify("i have new text: " + text);
            searchSubject.onNext(text);            
        };

        searchSubject.debounce(300).distinctUntilChanged(e => e).subscribe((searchString) => {
            Logger.Notify("debounce :" + searchString);

            if (searchString.trim().length === 0) {
                $timeout(() => {
                    context.Stores = [];
                    context.NoItems = false; // dont care about empty text field
                });

                return;
            }

            let storesPromise = storeService.Find(searchString);

            storesPromise.then((results) => {
                $timeout(() => {
                    context.Stores = results.data;
                    context.NoItems = results.data.length === 0;
                });
                Logger.Notify("results");
                Logger.Notify(results.data);
            });
        });

        $scope.context = context;
        $scope.searchStore = search;
    });

    findStoreComponet.directive("findStore", () => {

        return {
            name: "findStore",
            controller: "FindStoreController",
            template: `
                <div class='panel panel-default'>
                    <div class='panel-body'>
                        <div class="form-group has-feedback">
                            <div class="input-group">
                              <span class="input-group-addon">
                                <i class='fa fa-search'></i>
                              </span>
                              <input type="text" class="form-control" 
                                    placeholder="type the name of a store"
                                    ng-model="context.Store" 
                                    ng-change="searchStore(context.Store)" 
                                    aria-describedby="search success">
                            </div>
                           
                        </div>
                        <div ng-show="context.NoItems">There are no stores with this name</div>
                        <div ng-repeat="store in context.Stores"> 
                            <div class="row">
                                <div class="col-sm-3">
                                    {{store.Name}}
                                </div>
                                <div class="col-sm-9">
                                    <store-admin-group store="store"></store-admin-group>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`
        };

    });

}