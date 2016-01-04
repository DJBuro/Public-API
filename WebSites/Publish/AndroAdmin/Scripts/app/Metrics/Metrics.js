var AndroAdmin;
(function (AndroAdmin) {
    (function (Services) {
        (function (Metrics) {
            var MetricsListService = (function () {
                function MetricsListService() {
                    var internal = this;
                    this.vm = kendo.observable({
                        selectedOrderStatusOptions: "0",
                        selectedErrorCodeOptions: "0",
                        searchText: "",
                        filters: {
                            byACSErrorCodes: { value: "", active: false },
                            byOrderStatus: { value: "", active: false },
                            bySearchText: { value: "", active: false }
                        },
                        resetFilters: function (e) {
                            internal.ResetFilters();
                        }
                    });
                };

                MetricsListService.prototype.Init = function () {
                    var internal = this;
                    this.vm.bind("change", function (field) {
                        internal.VmChanged();
                    });

                    kendo.bind("#filters", this.vm);

                    this.ResetFilters();
                    //this.VmChanged();                  
                };

                MetricsListService.prototype.ResetFilters = function () {
                    var datasource = this.GetGridDatasource(), vm = this.vm;
                    vm.set("searchText", "");
                    vm.set("selectedErrorCodeOptions", "");
                    vm.set("selectedOrderStatusOptions", "");

                    datasource.filter([]);

                    var filterValues;
                };

                MetricsListService.prototype.VmChanged = function () {
                    var vm = this.vm;
                    var filterValues = {
                        status: vm.get("selectedOrderStatusOptions"),
                        errorCode: vm.get("selectedErrorCodeOptions"),
                        searchText: vm.get("searchText")
                    };

                    var gridDatasource = this.GetGridDatasource();
                    var f = {
                        logic: "and",
                        filters: []
                    };

                    if (filterValues.status != null && filterValues.status != "") {
                        f.filters.push(this.GetDatasourceFilterItemForStatus(filterValues.status));
                    }

                    if (filterValues.errorCode != null && filterValues.errorCode != "") {
                        f.filters.push(this.GetDatasourceFilterItemForACSErrorCodes(filterValues.errorCode));
                    }

                    if (filterValues.searchText != null && filterValues.searchText != "") {
                        f.filters.push(this.GetDatasourceFilterItemForSearch(filterValues.searchText));
                    }

                    console.log(f);
                    gridDatasource.filter(f);
                };

                MetricsListService.prototype.GetDatasourceFilterItemForACSErrorCodes = function (errorCode) {
                    var f = {
                        field: "ACSErrorCodeNumber",
                        operator: "eq",
                        value: parseInt(errorCode)
                    };

                    return f;
                };

                MetricsListService.prototype.GetDatasourceFilterItemForStatus = function (status) {
                    var vm = this.vm;
                    return {
                        field: "Status",
                        operator: "eq",
                        value: parseInt(status)
                    };
                };

                MetricsListService.prototype.GetDatasourceFilterItemForSearch = function (searchText) {
                    var f = {
                        logic: "or",
                        filters: [
                            {
                                field: "ID",
                                operator: "contains",
                                value: searchText
                            },
                            {
                                field: "RamesesOrderNum",
                                operator: "eq",
                                value: searchText
                            },
                            {
                                field: "SiteName",
                                operator: "contains",
                                value: searchText
                            },
                             {
                                 field: "ACSErrorCode.ShortDescription",
                                 operator: "contains",
                                 value: searchText
                             },
                             {
                                 field: "OrderStatus.Description",
                                 operator: "contains",
                                 value: searchText
                             },
                             {
                                 field: "ACSOrderId",
                                 operator: "contains",
                                 value: searchText
                             },
                              {
                                  field: "ACSServer",
                                  operator: "contains",
                                  value: searchText
                              },
                               {
                                   field: "ApplicationName",
                                   operator: "contains",
                                   value: searchText
                               },
                               {
                                   field: "Customer.FirstName",
                                   operator: "contains",
                                   value: searchText
                               }
                        ]
                    };

                    return f;
                };
                MetricsListService.prototype.GetGridDatasource = function () {
                    var grid, datasource;

                    grid = $("#OrderHistory").data("kendoGrid");
                    datasource = grid.dataSource;
                    return datasource;
                };

                return MetricsListService;
            }
            )();

            Metrics.MetricsListService = MetricsListService;


        })(Services.Metrics || (Services.Metrics = {}));
        var Metrics = Services.Metrics;
    })(AndroAdmin.Services || (AndroAdmin.Services = {}));
    var Services = AndroAdmin.Services;
})(AndroAdmin || (AndroAdmin = {}));
