
module MyAndromeda.Vouchers.Services {
    export class VoucherListService {
        public static ActiveLabel = "Active";
        public static InActiveLabel = "Inactive";

        public vm: kendo.Observable;

        constructor() {
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

        public Init(): void {
            var internal = this;
            this.vm.bind("change", function (field) {
                internal.VmChanged();
            });

            kendo.bind("#filters", this.vm);

            this.VmChanged()
        }

        public ResetFilters(): void {
            var datasource = this.GetGridDatasource(),
                vm: any = this.vm;

            vm.set("searchText", "");
            vm.set("selectedActiveOption", "true");

            datasource.filter([]);

            var a: kendo.ui.Tooltip;

        }

        public VmChanged(): void {
            var vm: any = this.vm;
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
        }

        public GetDatasourceFilterItemForSearch(searchText: string): kendo.data.DataSourceFilters {
            //http://docs.telerik.com/kendo-ui/api/framework/datasource
            //search code or description
            var f: kendo.data.DataSourceFilters = {
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
        }

        public GetDatasourceFilterItemForStatus(value: string): kendo.data.DataSourceFilterItem {
            var vm: any = this.vm;

            var filterValue: boolean = value === "true" ? true : false;

            return {
                field: "IsActive",
                operator: "eq",
                value: filterValue
            };
        }

        public GetGridDatasource(): kendo.data.DataSource {
            var grid: kendo.ui.Grid,
                datasource: kendo.data.DataSource;

            grid = $("#VouchersList").data("kendoGrid");
            datasource = grid.dataSource;
            return datasource;
        }
    }
}
