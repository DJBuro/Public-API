/// <reference path="../../Scripts/typings/rx.js/rx.d.ts" />
module MyAndromeda.Menu.Services {
    Angular.ServicesInitilizations.push((app) => {
        app.factory(MenuToppingsFilterService.Name, [
            MenuToppingsService.Name,
            (menuToppingsService) => {
                var instnance = new MenuToppingsFilterService(menuToppingsService);
                
                return instnance;
            }
        ]);
    });

    export class MenuToppingsFilterService {
        public static Name: string = "MenuToppingsFilterService ";

        private menuToppingsService : MenuToppingsService;
        private dataSource: kendo.data.DataSource;
        
        private model: Models.IMenuToppingsFilters;
        
        public ResetFiltersObservable : Rx.Subject<boolean>;
        
        constructor(menuToppingsService : MenuToppingsService)
        {
            this.menuToppingsService = menuToppingsService;
            this.dataSource = this.menuToppingsService.GetDataSource();
            this.ResetFiltersObservable = new Rx.Subject<boolean>();
            
            this.model = {
                Name : "",
                ResetFilters : () => { this.ResetFilters(); }
            };
        }

        public ChangeNameFilter(name) : void 
        {
            this.model.Name = name;
            this.Filter();
        }

        public GetName(): string {
            return this.model.Name;
        }

        private GetNameFilter() : kendo.data.DataSourceFilterItem
        {
            var filter: kendo.data.DataSourceFilterItem = {
                field : "Name",
                operator : "contains",
                value: this.model.Name
            };

            return filter;
        }
        
        public Filter(): void 
        {
            var filters = [];
            if(this.model.Name !== ""){
                var nameFilter = this.GetNameFilter();
                filters.push(nameFilter);
            }
            
            this.dataSource.filter(filters);    
        }

        public ResetFilters(): void {
            this.dataSource.filter([]);
            this.ResetFiltersObservable.onNext(true);
        }

    }


} 