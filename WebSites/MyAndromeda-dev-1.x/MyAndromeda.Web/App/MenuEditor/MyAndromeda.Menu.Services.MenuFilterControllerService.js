var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            var menuFilterController = (function () {
                function menuFilterController(filterIds, target) {
                    this.filterIds = filterIds;
                    this.target = target;
                    this.viewModel = kendo.observable({
                        ItemName: "",
                        SelectedDisplayCategory: "",
                        SelectedCategory1: "",
                        SelectedCategory2: "",
                        ShowGetStartedMessage: true,
                        Filtered: false,
                        FilteredBy: kendo.observable({
                            Name: "",
                            DisplayCategory: "",
                            Category1: "",
                            Category2: ""
                        }),
                        Sorted: false,
                        SortedBy: ""
                    });
                }
                //setup display category data
                menuFilterController.prototype.InitDisplayCategoryDataSource = function () {
                    var source = this.displayCategoryService.categories;
                    this.displayCategoryDataSource = new kendo.data.DataSource({
                        data: source
                    });
                    this.viewModel.set("DisplayCategory", this.displayCategoryDataSource);
                    this.viewModel.set("SelectedDisplayCategory", null);
                };
                //setup category 1 data
                menuFilterController.prototype.InitCategory1DataSource = function () {
                    var source = this.category1Service.categories;
                    this.category1DataSource = new kendo.data.DataSource({
                        data: source
                    });
                    this.viewModel.set("Category1", this.category1DataSource);
                };
                //setup category 2 data
                menuFilterController.prototype.InitCategory2DataSource = function () {
                    var source = this.category2Service.categories;
                    this.category2DataSource = new kendo.data.DataSource({
                        data: source
                    });
                    this.viewModel.set("Category2", this.category2DataSource);
                };
                //setup all local data sources
                menuFilterController.prototype.InitDataSources = function () {
                    console.log("setting category data sources");
                    this.InitDisplayCategoryDataSource();
                    this.InitCategory1DataSource();
                    this.InitCategory2DataSource();
                };
                menuFilterController.prototype.InitSearchBox = function () {
                    var internal = this;
                    this.itemSearchBox = $(this.filterIds.itemNameId);
                };
                menuFilterController.prototype.BindViewModel = function () {
                    kendo.bind(this.filterIds.toolBarId, this.viewModel);
                };
                menuFilterController.prototype.InitDisplayCategoryAndEvents = function () {
                    var internal = this;
                    this.displayCategoryCombo = $(this.filterIds.displayCategoryId).data("kendoComboBox");
                    this.displayCategoryCombo.bind("select", function (e) {
                        internal.viewModel.set(menuFilterController.SHOWGETSTARTEDMESSAGE, false);
                    });
                };
                menuFilterController.prototype.InitFields = function () {
                    this.BindViewModel();
                    this.InitSearchBox();
                    this.InitDisplayCategoryAndEvents();
                    this.category1Combo = $(this.filterIds.category1Id).data("kendoComboBox");
                    this.category2Combo = $(this.filterIds.category2Id).data("kendoComboBox");
                };
                menuFilterController.prototype.buildFilterForMainList = function () {
                    var filters = [];
                    var values = {
                        itemName: this.viewModel.get(menuFilterController.ITEMNAME).trim(),
                        displayCategoryId: this.displayCategoryCombo.value(),
                        category1Id: this.category1Combo.value(),
                        category2Id: this.category2Combo.value()
                    };
                    if (values.itemName) {
                        var nameFilters = {
                            logic: "or",
                            filters: [
                                { field: "Name", operator: "contains", value: values.itemName },
                                { field: "WebName", operator: "contains", value: values.itemName },
                                { field: "WebDescription", operator: "contains", value: values.itemName }
                            ]
                        };
                        filters.push(nameFilters);
                    }
                    //value needs to be the same as the query item. 
                    if (typeof (values.displayCategoryId) !== "undefined" && values.displayCategoryId !== "") {
                        filters.push({ field: "DisplayCategoryId", operator: "eq", value: parseInt(values.displayCategoryId) });
                    }
                    var category1Selected = typeof (values.category1Id) !== "undefined" && values.category1Id !== "";
                    var category2Selected = typeof (values.category2Id) !== "undefined" && values.category2Id !== "";
                    if (category1Selected) {
                        filters.push({ field: "CategoryId1", operator: "eq", value: parseInt(values.category1Id) });
                    }
                    if (category2Selected) {
                        filters.push({ field: "CategoryId2", operator: "eq", value: parseInt(values.category2Id) });
                    }
                    return filters;
                };
                menuFilterController.prototype.initFilterChanges = function () {
                    var internal = this;
                    $(this.filterIds.toolBarId).on("keyup change", function (e) {
                        //if(internal.itemSearchBox.context.activeElement.value){
                        //}
                        var filters = internal.buildFilterForMainList();
                        internal.target.filter(filters);
                        internal.target.sort(menuFilterController.SORTFILTER);
                    });
                };
                menuFilterController.prototype.initDisplayFilterAndSortMessageCue = function () {
                    var internal = this;
                    internal.target.bind("change", function (e) {
                        var dsFilter = internal.target.filter(), dsSort = internal.target.sort(), vm = internal.viewModel;
                        vm.set(menuFilterController.FILTERED, dsFilter && dsFilter.filters && dsFilter.filters.length > 0 && internal.target.data().length > 0);
                        var filteredBy = vm.get(menuFilterController.FILTEREDBY);
                        filteredBy.set("Name", internal.itemSearchBox.val());
                        filteredBy.set("DisplayCategory", internal.displayCategoryCombo.text());
                        filteredBy.set("Category1", internal.category1Combo.text());
                        filteredBy.set("Category2", internal.category2Combo.text());
                        var sorting = dsSort.length > 0;
                        vm.set(menuFilterController.SORTED, sorting);
                        vm.set(menuFilterController.SORTEDBY, sorting ? dsSort[0].field : "");
                    });
                };
                menuFilterController.prototype.initFilterReset = function () {
                    var internal = this;
                    $(this.filterIds.resetId).on("click", function (e) {
                        e.preventDefault();
                        internal.viewModel.set(menuFilterController.DISPLAYCATEGORY, "");
                        internal.viewModel.set("SelectedDisplayCategory", null);
                        internal.viewModel.set(menuFilterController.CATEGORY1, "");
                        internal.viewModel.set(menuFilterController.CATEGORY2, "");
                        internal.viewModel.set(menuFilterController.ITEMNAME, "");
                        internal.viewModel.set(menuFilterController.SHOWGETSTARTEDMESSAGE, true);
                        internal.target.filter(menuFilterController.RESETFILTER);
                    });
                };
                menuFilterController.prototype.init = function (displayCategoryService, category1Service, category2Service) {
                    this.displayCategoryService = displayCategoryService;
                    this.category1Service = category1Service;
                    this.category2Service = category2Service;
                    //create data sources from the data 
                    this.InitDataSources();
                    //create ui controls to manage the filters 
                    this.InitFields();
                    this.initFilterChanges();
                    this.initFilterReset();
                    this.initDisplayFilterAndSortMessageCue();
                };
                menuFilterController.ITEMNAME = "ItemName";
                menuFilterController.DISPLAYCATEGORY = "SelectedDisplayCategory";
                menuFilterController.CATEGORY1 = "SelectedCategory1";
                menuFilterController.CATEGORY2 = "SelectedCategory2";
                menuFilterController.SHOWGETSTARTEDMESSAGE = "ShowGetStartedMessage";
                menuFilterController.FILTERED = "Filtered";
                menuFilterController.FILTEREDBY = "FilteredBy";
                menuFilterController.SORTED = "Sorted";
                menuFilterController.SORTEDBY = "SortedBy";
                menuFilterController.RESETFILTER = [];
                menuFilterController.SORTFILTER = [];
                return menuFilterController;
            }());
            Services.menuFilterController = menuFilterController;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
