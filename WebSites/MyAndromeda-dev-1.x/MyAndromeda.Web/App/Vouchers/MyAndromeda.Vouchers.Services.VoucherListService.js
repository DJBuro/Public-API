var MyAndromeda;
(function (MyAndromeda) {
    var Vouchers;
    (function (Vouchers) {
        var Services;
        (function (Services) {
            var VoucherListService = (function () {
                function VoucherListService() {
                    var internal = this;
                    this.vm = kendo.observable({
                        activeOptions: [
                            { name: VoucherListService.ActiveLabel, value: "true" },
                            { name: VoucherListService.InActiveLabel, value: "false" }
                        ],
                        selectedActiveOption: "true",
                        searchText: "",
                        filters: {
                            byNameDescription: { value: "", active: false },
                            byStatus: { value: "", active: false }
                        },
                        resetFilters: function (e) {
                            //e.preventDefault();
                            internal.ResetFilters();
                        }
                    });
                }
                VoucherListService.prototype.Init = function () {
                    var internal = this;
                    this.vm.bind("change", function (field) {
                        internal.VmChanged();
                    });
                    kendo.bind("#filters", this.vm);
                    this.VmChanged();
                };
                VoucherListService.prototype.ResetFilters = function () {
                    var datasource = this.GetGridDatasource(), vm = this.vm;
                    vm.set("searchText", "");
                    vm.set("selectedActiveOption", "true");
                    datasource.filter([]);
                    var a;
                };
                VoucherListService.prototype.VmChanged = function () {
                    var vm = this.vm;
                    var a = {
                        text: vm.get("searchText"),
                        option: vm.get("selectedActiveOption")
                    };
                    var gridDatasource = this.GetGridDatasource();
                    //[{ field="VoucherCode", operator="eq", value="monday"}, Object { field="IsActive", operator="eq", value=true}]
                    //your not working .. why?
                    var f = {
                        logic: "and",
                        filters: [
                            this.GetDatasourceFilterItemForSearch(a.text),
                            this.GetDatasourceFilterItemForStatus(a.option)
                        ]
                    };
                    console.log(f);
                    gridDatasource.filter(f);
                };
                VoucherListService.prototype.GetDatasourceFilterItemForSearch = function (searchText) {
                    //http://docs.telerik.com/kendo-ui/api/framework/datasource
                    //search code or description
                    var f = {
                        logic: "or",
                        filters: [
                            {
                                field: "VoucherCode",
                                operator: "contains",
                                value: searchText
                            },
                            {
                                field: "Description",
                                operator: "contains",
                                value: searchText
                            }
                        ]
                    };
                    return f;
                };
                VoucherListService.prototype.GetDatasourceFilterItemForStatus = function (value) {
                    var vm = this.vm;
                    var filterValue = value === "true" ? true : false;
                    return {
                        field: "IsActive",
                        operator: "eq",
                        value: filterValue
                    };
                };
                VoucherListService.prototype.GetGridDatasource = function () {
                    var grid, datasource;
                    grid = $("#VouchersList").data("kendoGrid");
                    datasource = grid.dataSource;
                    return datasource;
                };
                VoucherListService.ActiveLabel = "Active";
                VoucherListService.InActiveLabel = "Inactive";
                return VoucherListService;
            }());
            Services.VoucherListService = VoucherListService;
        })(Services = Vouchers.Services || (Vouchers.Services = {}));
    })(Vouchers = MyAndromeda.Vouchers || (MyAndromeda.Vouchers = {}));
})(MyAndromeda || (MyAndromeda = {}));
