var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
            });
            var MenuItemService = (function () {
                function MenuItemService(dataSource) {
                    this.dataSource = dataSource;
                    this.menuStatus = new Services.MenuStatus(dataSource);
                }
                //find elements based on the id
                MenuItemService.prototype.findById = function (id) {
                    return this.dataSource.get(id);
                };
                //find elements based on the name
                MenuItemService.prototype.getRelatedItems = function (menuItems) {
                    var internal = this;
                    var data = internal.dataSource.data();
                    //var filtered = data.filter((value: Models.IMenuItemObservable) => {
                    //    var left = menuItems.filter((item) => {
                    //        if (item.Id === value.Id) { return false; }
                    //        if (item.Name === value.Name) { return true; }
                    //    });
                    //    return 
                    //});
                    var series = Enumerable.from(menuItems).selectMany(function (x) {
                        return internal.dataSource.data().filter(function (value, index, array) {
                            return value.Name == x.Name && value.Id !== x.Id;
                        });
                    }).toArray();
                    return series;
                };
                MenuItemService.prototype.removeThumb = function (menuItem, filename) {
                    var thumbs = menuItem.get("Thumbs");
                    var thumb = thumbs.find(function (item, index, array) {
                        return item.FileName === filename;
                    });
                    thumbs.remove(thumb);
                };
                MenuItemService.prototype.updateItems = function (menuItems, updateDataSource) {
                    var internal = this;
                    for (var i = 0; i < menuItems.length; i++) {
                        var item = menuItems[i];
                        var dataSourceItem = internal.findById(item.Id), dataItem = dataSourceItem;
                        if (!dataSourceItem) {
                            continue;
                        }
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
                };
                MenuItemService.prototype.anyDirtyItems = function () {
                    var internal = this;
                    return this.dataSource.hasChanges();
                };
                return MenuItemService;
            })();
            Services.MenuItemService = MenuItemService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
