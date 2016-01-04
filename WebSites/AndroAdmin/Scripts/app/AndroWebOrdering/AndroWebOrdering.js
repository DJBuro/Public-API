var AndroWebOrdering;
(function (AndroWebOrdering) {
    (function (Services) {
        (function (Websites) {
            var WebsitesListService = (function () {
                function WebsitesListService() {
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

                WebsitesListService.prototype.Init = function () {
                    var internal = this;
                    this.vm.bind("change", function (field) {
                        internal.VmChanged();
                    });

                    kendo.bind("#filters", this.vm);

                    this.ResetFilters();
                };

                WebsitesListService.prototype.ResetFilters = function () {
                    var datasource = this.GetGridDatasource(), vm = this.vm;
                    vm.set("searchText", "");
                    datasource.filter([]);

                    var filterValues;
                };

                WebsitesListService.prototype.VmChanged = function () {
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

                WebsitesListService.prototype.GetDatasourceFilterItemForSearch = function (searchText) {
                    var f = {
                        logic: "or",
                        filters: [
                            {
                                field: "Name",
                                operator: "contains",
                                value: searchText
                            }
                            ,
                            {
                                field: "URL",
                                operator: "contains",
                                value: searchText
                            }
                            ,
                            {
                                field: "StoresCount",
                                operator: "contains",
                                value: searchText
                            }
                            ,
                            {
                                field: "Status",
                                operator: "contains",
                                value: searchText
                            }
                            ,
                            {
                                field: "EnvironmentName",
                                operator: "contains",
                                value: searchText
                            }
                        ]
                    };

                    return f;
                };

                WebsitesListService.prototype.GetGridDatasource = function () {
                    var grid, datasource;

                    grid = $("#AndroWebOrderingGrid").data("kendoGrid");
                    datasource = grid.dataSource;
                    return datasource;
                };

                return WebsitesListService;
            }
            )();

            Websites.WebsitesListService = WebsitesListService;


        })(Services.Websites || (Services.Websites = {}));
        var Websites = Services.Websites;

    })(AndroWebOrdering.Services || (AndroWebOrdering.Services = {}));
    var Services = AndroWebOrdering.Services;
})(AndroWebOrdering || (AndroWebOrdering = {}));
