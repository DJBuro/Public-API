 module MyAndromeda.Menu.Services 
 {
    Angular.ServicesInitilizations.push((app)=> {
        
    });

    export class MenuItemService {
        public dataSource: kendo.data.DataSource;
        public hierarchicalDataSource: kendo.data.HierarchicalDataSource;
        public menuStatus: Services.MenuStatus;

        constructor(dataSource: kendo.data.DataSource) {
            this.dataSource = dataSource;
            this.menuStatus = new Services.MenuStatus(dataSource);
        }

        //find elements based on the id
        public findById(id: number): Models.IMenuItemObservable {
            return <any>this.dataSource.get(id);
        }

        //find elements based on the name
        public getRelatedItems(menuItems: Models.IMenuItemObservable[]): Models.IMenuItemObservable[] {
            var internal = this;
            var data = internal.dataSource.data();
            //var filtered = data.filter((value: Models.IMenuItemObservable) => {
            //    var left = menuItems.filter((item) => {
            //        if (item.Id === value.Id) { return false; }
            //        if (item.Name === value.Name) { return true; }
            //    });
            //    return 
            //});

            var series = Enumerable.from(menuItems).selectMany((x) => {
                return internal.dataSource.data().filter((value : Models.IMenuItem, index, array) => {
                    return value.Name == x.Name && value.Id !== x.Id;
                });
            }).toArray();

            return series;
        }

        public removeThumb(menuItem: Models.IMenuItemObservable, filename: string): void {
            var thumbs = <kendo.data.ObservableArray>menuItem.get("Thumbs");
            var thumb = thumbs.find(function (item: Models.IMenuItemThumb, index, array) {
                return item.FileName === filename;
            });
            thumbs.remove(thumb);
        }

        public updateItems(menuItems: Models.IMenuItem[], updateDataSource: boolean): void {
            var internal = this;
            for (var i = 0; i < menuItems.length; i++) {
                var item = menuItems[i];
                var dataSourceItem = internal.findById(item.Id),
                    dataItem = <any>dataSourceItem;

                if (!dataSourceItem) { continue; }

                var acceptParams = {
                    Name: item.Name,
                    WebDescription: item.WebDescription,
                    WebName: item.WebName,
                    WebSequence: item.WebSequence,
                    Prices: item.Prices
                }; 

                dataItem.accept(acceptParams);

                if (!updateDataSource) { 
                    dataItem.trigger("change");
                }
            }

            if (updateDataSource) {
                this.dataSource.trigger("change");
            }
            //this.dataSource.trigger("change");
        }

        public anyDirtyItems(): boolean {
            var internal = this;

            return this.dataSource.hasChanges();
        }
    }
 }