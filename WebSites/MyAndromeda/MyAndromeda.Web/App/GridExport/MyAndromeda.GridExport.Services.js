var MyAndromeda;
(function (MyAndromeda) {
    (function (GridExport) {
        (function (Services) {
            var KendoGridExcelExporter = (function () {
                function KendoGridExcelExporter(options) {
                    this.options = options;
                }
                KendoGridExcelExporter.prototype.getGrid = function () {
                    return $(this.options.gridSelector).data("kendoGrid");
                };

                KendoGridExcelExporter.prototype.setupExcelEvents = function () {
                    var internal = this;
                    $(this.options.downloadSelector).on("click", function (e) {
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
                    $(".k-button-show-orderType-then-payType").on("click", function (e) {
                        var grid = internal.getGrid();
                        var currentGroup = grid.dataSource.group();

                        if (currentGroup && currentGroup.length > 0) {
                            alert(KendoGridExcelExporter.alreadyGroupedItems);
                            e.preventDefault();
                            return;
                        }

                        grid.dataSource.group({
                            field: "PayType", dir: "asc", aggregates: [
                                { field: "PayType", aggregate: "count" },
                                { field: "FinalPrice", aggregate: "min" },
                                { field: "FinalPrice", aggregate: "max" },
                                { field: "FinalPrice", aggregate: "sum" },
                                { field: "FinalPrice", aggregate: "average" }
                            ]
                        });
                    });
                };

                KendoGridExcelExporter.prototype.exportExcel = function () {
                    var internal = this, grid = this.getGrid();

                    var schemaTruncated = grid.columns.map(function (column, index) {
                        return {
                            title: column.title,
                            field: column.field
                        };
                    });

                    var title = this.options.title, model = JSON.stringify(schemaTruncated), data = JSON.stringify(grid.dataSource.view());

                    //get the the pager
                    $(this.options.titleSelector).val(title);
                    $(this.options.modelSelector).val(model);
                    $(this.options.dataSelector).val(data);
                };

                KendoGridExcelExporter.prototype.init = function () {
                    this.setupExcelEvents();
                };
                KendoGridExcelExporter.notSurportedMessage = "Please remove all groups before exporting. This feature is not supported.";
                KendoGridExcelExporter.alreadyGroupedItems = "There the grid is already grouped by an item";
                return KendoGridExcelExporter;
            })();
            Services.KendoGridExcelExporter = KendoGridExcelExporter;
        })(GridExport.Services || (GridExport.Services = {}));
        var Services = GridExport.Services;
    })(MyAndromeda.GridExport || (MyAndromeda.GridExport = {}));
    var GridExport = MyAndromeda.GridExport;
})(MyAndromeda || (MyAndromeda = {}));
