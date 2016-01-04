var AndroAdminChain;
(function (AndroAdminChain) {
    (function (Services) {
        (function (ChainStores) {
            var ChainsListService = (function () {
                function ChainsListService() {
                    var internal = this;
                    this.vm = kendo.observable({
                        searchText: "",
                        filters: {
                            bySearchText: { value: "", active: false }
                        },
                        resetFilters: function (e) {
                            internal.ResetFilters();
                        }
                    });
                };

                ChainsListService.prototype.Init = function () {
                    var internal = this;
                    this.vm.bind("change", function (field) {
                        internal.VmChanged();
                    });

                    kendo.bind("#filters", this.vm);

                    this.ResetFilters();
                };

                ChainsListService.prototype.ResetFilters = function () {
                    var datasource = this.GetGridDatasource(), vm = this.vm;
                    vm.set("searchText", "");
                    datasource.filter([]);

                    var filterValues;
                };

                ChainsListService.prototype.VmChanged = function () {
                    var vm = this.vm;
                    var filterValues = {
                        searchText: vm.get("searchText")
                    };

                    var gridDatasource = this.GetGridDatasource();
                    var f = {
                        logic: "or",
                        filters: []
                    };

                    if (filterValues.searchText != null && filterValues.searchText != "") {
                        f.filters.push(this.GetDatasourceFilterItemForSearch(filterValues.searchText));
                    }

                    console.log(f);
                    gridDatasource.filter(f);
                };

                ChainsListService.prototype.GetDatasourceFilterItemForSearch = function (searchText) {
                    var f = {
                        logic: "or",
                        filters: [
                            {
                                field: "AndromedaSiteId",
                                operator: "contains",
                                value: searchText
                            }
                            ,
                            {
                                field: "Name",
                                operator: "contains",
                                value: searchText
                            }
                            ,
                            {
                                field: "StoreStatus.Status",
                                operator: "contains",
                                value: searchText
                            }
                        ]
                    };

                    return f;
                };

                ChainsListService.prototype.GetGridDatasource = function () {
                    var grid, datasource;

                    grid = $("#ChainStoresGrid").data("kendoGrid");
                    datasource = grid.dataSource;
                    return datasource;
                };

                return ChainsListService;
            }
            )();

            ChainStores.ChainsListService = ChainsListService;


        })(Services.ChainStores || (Services.ChainStores = {}));
        var ChainStores = Services.ChainStores;

    })(AndroAdminChain.Services || (AndroAdminChain.Services = {}));
    var Services = AndroAdminChain.Services;
})(AndroAdminChain || (AndroAdminChain = {}));
