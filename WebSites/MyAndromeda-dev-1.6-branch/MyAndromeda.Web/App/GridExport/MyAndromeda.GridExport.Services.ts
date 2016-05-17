module MyAndromeda.GridExport.Services
{
    export class KendoGridExcelExporter
    {
        public static notSurportedMessage: string = "Please remove all groups before exporting. This feature is not supported.";
        public static alreadyGroupedItems: string = "There the grid is already grouped by an item";

        public options: IkendoGridExcelExporterOptions;

        constructor(options: IkendoGridExcelExporterOptions)
        {
            this.options = options;
            
        }

        private getGrid(): kendo.ui.Grid {
            return $(this.options.gridSelector).data("kendoGrid");
        }

        private setupExcelEvents(): void {
            var internal = this;
            $(this.options.downloadSelector).on("click", (e) => {
                var grid = internal.getGrid();
                var currentGroup = grid.dataSource.group();
                if (currentGroup && currentGroup.length > 0) {
                    alert(KendoGridExcelExporter.notSurportedMessage);
                    e.preventDefault();
                    return;
                }

                internal.exportExcel();
            });

            //ignore this.
            $(".k-button-show-orderType-then-payType").on("click", (e) => {
                var grid = internal.getGrid();
                var currentGroup = grid.dataSource.group();

                if (currentGroup && currentGroup.length > 0) {
                    alert(KendoGridExcelExporter.alreadyGroupedItems);
                    e.preventDefault();
                    return;
                }

                grid.dataSource.group({
                    field: "PayType", dir: "asc", aggregates: [
                        { field:"PayType", aggregate:"count" },
                        { field:"FinalPrice", aggregate: "min" },
                        { field:"FinalPrice", aggregate:"max" },
                        { field:"FinalPrice", aggregate:"sum" },
                        { field:"FinalPrice", aggregate:"average" }
                    ]
                });
            });
        }

        public exportExcel(): void {
            var internal = this,
                grid = this.getGrid();

            var schemaTruncated = grid.columns.map((column, index) => {
                return {
                    title: column.title,
                    field: column.field  
                };
            });

            var title = this.options.title,
                model = JSON.stringify(schemaTruncated),
                data = JSON.stringify(grid.dataSource.view());
            
            //get the the pager 


            $(this.options.titleSelector).val(title);
            $(this.options.modelSelector).val(model);
            $(this.options.dataSelector).val(data);
        }

        public init(): void {
            this.setupExcelEvents();
        }
    }

    export interface IkendoGridExcelExporterOptions
    {
        title: string;
        gridSelector: string;
        downloadSelector: string;
        titleSelector: string;
        modelSelector: string;
        dataSelector: string;
    }
} 