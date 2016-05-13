 module MyAndromeda.Menu.Services 
 {
    export class menuFilterController {
        private static ITEMNAME = "ItemName";
        private static DISPLAYCATEGORY = "SelectedDisplayCategory";
        private static CATEGORY1 = "SelectedCategory1";
        private static CATEGORY2 = "SelectedCategory2";
        private static SHOWGETSTARTEDMESSAGE = "ShowGetStartedMessage";
        private static FILTERED = "Filtered";
        private static FILTEREDBY = "FilteredBy";
        private static SORTED = "Sorted";
        private static SORTEDBY = "SortedBy";

        public static RESETFILTER = [];
        public static SORTFILTER = [];

        private displayCategoryDataSource: kendo.data.DataSource;
        private category1DataSource: kendo.data.DataSource;
        private category2DataSource: kendo.data.DataSource;

        private itemSearchBox: JQuery;
        private displayCategoryCombo: kendo.ui.ComboBox;
        private category1Combo: kendo.ui.ComboBox;
        private category2Combo: kendo.ui.ComboBox;

        private displayCategoryService: Services.CategoryService;
        private category1Service: Services.CategoryService;
        private category2Service: Services.CategoryService;

        public viewModel: kendo.data.ObservableObject;

        constructor(public filterIds: Models.IFilterids, public target: kendo.data.DataSource) {
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
        private InitDisplayCategoryDataSource(): void {
            var source = <any>this.displayCategoryService.categories;
            this.displayCategoryDataSource = new kendo.data.DataSource({
                data: source
            });
            
            this.viewModel.set("DisplayCategory", this.displayCategoryDataSource);
            this.viewModel.set("SelectedDisplayCategory", null);
        }

        //setup category 1 data
        private InitCategory1DataSource(): void {
            var source = <any>this.category1Service.categories;
            this.category1DataSource = new kendo.data.DataSource({
                data: source
            });

            this.viewModel.set("Category1", this.category1DataSource);
        }

        //setup category 2 data
        private InitCategory2DataSource(): void {
            var source = <any>this.category2Service.categories;
            this.category2DataSource = new kendo.data.DataSource({
                data: source
            });

            this.viewModel.set("Category2", this.category2DataSource);
        }

        //setup all local data sources
        private InitDataSources(): void {
            console.log("setting category data sources");
            this.InitDisplayCategoryDataSource();
            this.InitCategory1DataSource();
            this.InitCategory2DataSource();
        }

        private InitSearchBox(): void {
            var internal = this;
            this.itemSearchBox = $(this.filterIds.itemNameId);
        }

        private BindViewModel(): void {
            kendo.bind(this.filterIds.toolBarId, this.viewModel);
        }

        private InitDisplayCategoryAndEvents(): void {
            var internal = this;
            this.displayCategoryCombo = $(this.filterIds.displayCategoryId).data("kendoComboBox");

            this.displayCategoryCombo.bind("select", function (e) {
                internal.viewModel.set(menuFilterController.SHOWGETSTARTEDMESSAGE, false);
            });
        }

        private InitFields(): void {
            this.BindViewModel();
            this.InitSearchBox();
            this.InitDisplayCategoryAndEvents();
            this.category1Combo = $(this.filterIds.category1Id).data("kendoComboBox");
            this.category2Combo = $(this.filterIds.category2Id).data("kendoComboBox");
        }

        private buildFilterForMainList(): kendo.data.DataSourceFilterItem[] {
            var filters: kendo.data.DataSourceFilterItem[] = [];

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
        }

        

        private initFilterChanges(): void {
            var internal = this;
            $(this.filterIds.toolBarId).on("keyup change", function (e) {
                //if(internal.itemSearchBox.context.activeElement.value){
                //}

                var filters = internal.buildFilterForMainList();

                internal.target.filter(filters);
                internal.target.sort(menuFilterController.SORTFILTER);
            });
        }

        private initDisplayFilterAndSortMessageCue(): void {
            var internal = this;
            internal.target.bind("change", function (e) {
                var dsFilter = internal.target.filter(),
                    dsSort = internal.target.sort(),
                    vm = internal.viewModel;

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
        }

        private initFilterReset(): void {
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
        }

        public init(displayCategoryService: Services.CategoryService, 
            category1Service: Services.CategoryService, 
            category2Service: Services.CategoryService): void
         {
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
        }
    }
 }