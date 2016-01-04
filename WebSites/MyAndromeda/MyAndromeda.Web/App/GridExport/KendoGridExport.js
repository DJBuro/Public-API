var MyAndromeda;
(function (MyAndromeda) {
    (function (GridExport) {
        (function (Services) {
            var kendoGridExcelExporter = (function () {
                function kendoGridExcelExporter(options) {
                    this.options = options;
                }
                kendoGridExcelExporter.prototype.getGrid = function () {
                    return $(this.options.gridSelector).data("kendoGrid");
                };

                kendoGridExcelExporter.prototype.setupExcelEvents = function () {
                    var internal = this;
                    $(this.options.downloadSelector).on("click", function (e) {
                        var grid = internal.getGrid();
                        var currentGroup = grid.dataSource.group();
                        if (currentGroup && currentGroup.length > 0) {
                            alert(kendoGridExcelExporter.notSurportedMessage);
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
                            alert(kendoGridExcelExporter.alreadyGroupedItems);
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

                kendoGridExcelExporter.prototype.exportExcel = function () {
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

                kendoGridExcelExporter.prototype.init = function () {
                    this.setupExcelEvents();
                };
                kendoGridExcelExporter.notSurportedMessage = "Please remove all groups before exporting. This feature is not supported.";
                kendoGridExcelExporter.alreadyGroupedItems = "There the grid is already grouped by an item";
                return kendoGridExcelExporter;
            })();
            Services.kendoGridExcelExporter = kendoGridExcelExporter;
        })(GridExport.Services || (GridExport.Services = {}));
        var Services = GridExport.Services;
    })(MyAndromeda.GridExport || (MyAndromeda.GridExport = {}));
    var GridExport = MyAndromeda.GridExport;
})(MyAndromeda || (MyAndromeda = {}));
//# sourceMappingURL=KendoGridExport.js.map
