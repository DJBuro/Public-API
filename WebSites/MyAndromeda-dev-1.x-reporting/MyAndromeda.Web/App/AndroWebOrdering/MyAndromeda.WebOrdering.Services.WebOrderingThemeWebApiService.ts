/// <reference path="../../scripts/typings/rx/rx.d.ts" />
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Services {
    Angular.ServicesInitilizations.push((app) => {
        app.factory(WebOrderingThemeWebApiService.Name, [
            () => {
                Logger.Notify("new WebOrderingThemeWebApiService");
                var instnance = new WebOrderingThemeWebApiService();
                return instnance;
            }
        ]);
    });

    export class WebOrderingThemeWebApiService {
        public static Name: string = "WebOrderingThemeWebApiService";

        public IsBusy: Rx.BehaviorSubject<boolean>;
        public Search: Rx.Subject<string>;

        private dataSource: kendo.data.DataSource;

        public constructor() {
            this.IsBusy = new Rx.BehaviorSubject<boolean>(false);
            this.Search = new Rx.Subject<string>();

            if (!Settings.AndromedaSiteId) { throw "Settings.AndromedaSiteId is undefined"; }
            
            //throttle input for 1 second. ie search will resume after the user stops typing.
            this.Search
                .throttle(1000)
                .subscribe((value) => this.SearcInternal(value));
        }

        public GetThemeDataSource(): kendo.data.DataSource {
            var route = kendo.format('/api/AndroWebOrderingTheme/{0}/List', Settings.AndromedaSiteId);
            this.dataSource = new kendo.data.DataSource({
                transport: {
                    read: route
                }
            });

            this.dataSource.bind("requestStart", () => { this.IsBusy.onNext(true); });
            this.dataSource.bind("requestEnd", () => { this.IsBusy.onNext(false); });

            return this.dataSource;
        }

        public SearchText(value: string): void {
            this.Search.onNext(value);
        }

        private SearcInternal(value: string): void {
            value || (value = "");
            value = value.trim();

            if (value.length === 0) {
                this.dataSource.filter([]);
                return;
            }

            var op = "contains";
            var filterFileName: kendo.data.DataSourceFilterItem = {
                field: "FileName",
                operator: op,
                value: value
            };
            var filterInterName: kendo.data.DataSourceFilterItem = {
                field: "InternalName",
                operator: op,
                value: value
            };

            var filterGroup: kendo.data.DataSourceFilters = {
                filters: [filterFileName, filterInterName],
                logic: "or"
            }

            this.dataSource.filter([
                filterGroup
            ]);
        }
    }

} 