var AndroAdminWebOrderingSites;
(function (AndroAdminWebOrderingSites) {
    (function (Services) {
        (function (Websites) {
            var SitesListService = (function () {
                function SitesListService() {
                    var internal = this;
                    this.vm = kendo.observable({
                        selectedChainId: "",                        
                        searchText: "",
                        filters: {
                            byChainIds: { value: "", active: false },                            
                            bySearchText: { value: "", active: false }
                        },
                        resetFilters: function (e) {
                            internal.ResetFilters();
                        }
                    });
                };

                SitesListService.prototype.Init = function () {
                    var internal = this;
                    this.vm.bind("change", function (field) {
                        internal.VmChanged();
                    });

                    kendo.bind("#filters", this.vm);

                    this.ResetFilters();
                    //this.VmChanged();                  
                };

                SitesListService.prototype.ResetFilters = function () {
                    var datasource = this.GetGridDatasource(), vm = this.vm;
                    vm.set("searchText", "");
                    vm.set("selectedChainId", "");                    

                    datasource.filter([]);
                };

                SitesListService.prototype.VmChanged = function () {
                    var vm = this.vm;
                    var filterValues = {
                        selectedChainId: vm.get("selectedChainId"),
                        searchText: vm.get("searchText")
                    };

                    var gridDatasource = this.GetGridDatasource();
                    var f = {
                        logic: "and",
                        filters: []
                    };

                    
                    if (filterValues.selectedChainId != null && filterValues.selectedChainId != "") {
                        f.filters.push(this.GetDatasourceFilterItemForChainIds(filterValues.selectedChainId));
                    }

                    if (filterValues.searchText != null && filterValues.searchText != "") {
                        f.filters.push(this.GetDatasourceFilterItemForSearch(filterValues.searchText));
                    }

                    console.log(f);
                    gridDatasource.filter(f);
                };

                SitesListService.prototype.GetDatasourceFilterItemForChainIds = function (selectedChainId) {
                    var f = {
                        field: "ChainId",
                        operator: "eq",
                        value: parseInt(selectedChainId)
                    };

                    return f;
                };
                 
                SitesListService.prototype.GetDatasourceFilterItemForSearch = function (searchText) {
                    var f = {
                        logic: "or",
                        filters: [
                            {
                                field: "AndromedaSiteId",
                                operator: "contains",
                                value: searchText
                            },
                            {
                                field: "Name",
                                operator: "contains",
                                value: searchText
                            },
                            {
                                field: "StoreStatus.Status",
                                operator: "contains",
                                value: searchText
                            }
                        ]
                    };

                    return f;
                };
                SitesListService.prototype.GetGridDatasource = function () {
                    var grid, datasource;

                    grid = $("#WebOrderingSitesGrid").data("kendoGrid");
                    datasource = grid.dataSource;
                    return datasource;
                };

                return SitesListService;
            }
            )();

            Websites.SitesListService = SitesListService;


        })(Services.Websites || (Services.Websites = {}));
        var Websites = Services.Websites;
    })(AndroAdminWebOrderingSites.Services || (AndroAdminWebOrderingSites.Services = {}));
    var Services = AndroAdminWebOrderingSites.Services;
})(AndroAdminWebOrderingSites || (AndroAdminWebOrderingSites = {}));
