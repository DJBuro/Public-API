var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
            });

            var menuItemService = (function () {
                function menuItemService(dataSource) {
                    this.dataSource = dataSource;
                    this.menuStatus = new Services.MenuStatus(dataSource);
                }
                //find elements based on the id
                menuItemService.prototype.findById = function (id) {
                    return this.dataSource.get(id);
                };

                //find elements based on the name
                menuItemService.prototype.getRelatedItems = function (menuItems) {
                    var internal = this;
                    var series = Enumerable.from(menuItems).selectMany(function (x) {
                        return internal.dataSource.data().filter(function (value, index, array) {
                            return value.Name == x.Name && value.Id !== x.Id;
                        });
                    }).toArray();

                    return series;
                };

                menuItemService.prototype.removeThumb = function (menuItem, filename) {
                    var thumbs = menuItem.get("Thumbs");
                    var thumb = thumbs.find(function (item, index, array) {
                        return item.FileName === filename;
                    });
                    thumbs.remove(thumb);
                };

                menuItemService.prototype.updateItems = function (menuItems, updateDataSource) {
                    var internal = this;
                    for (var i = 0; i < menuItems.length; i++) {
                        var item = menuItems[i];
                        var dataSourceItem = internal.findById(item.Id), dataItem = dataSourceItem;
                        if (!dataSourceItem) {
                            continue;
                        }

                        //dataSourceItem.dirty = false;
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

                menuItemService.prototype.anyDirtyItems = function () {
                    var internal = this;

                    return this.dataSource.hasChanges();
                };
                return menuItemService;
            })();
            Services.menuItemService = menuItemService;
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
