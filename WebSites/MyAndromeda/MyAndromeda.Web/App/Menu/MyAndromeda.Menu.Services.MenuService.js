var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(MenuService.Name, [
                    function () {
                        //var instance = new MenuService();
                        //return instance;
                    }
                ]);
            });

            var MenuService = (function () {
                function MenuService(options) {
                    this.enableDetailMenuEditing = false;
                    this.options = options;
                }
                MenuService.prototype.setupFilterHelper = function () {
                    this.menuFilterHelper = new Services.menuFilterController(this.options.filterIds, this.menuItemService.dataSource);
                };

                MenuService.prototype.CreateCategoryService = function (category1, category2, displayCategories) {
                    this.displayCategoriesService = new Services.CategoryService(displayCategories);
                    this.category1Service = new Services.CategoryService(category1);
                    this.category2Service = new Services.CategoryService(category2);

                    //pass along the three services to the filter helper
                    this.menuFilterHelper.init(this.displayCategoriesService, this.category1Service, this.category2Service);
                };

                MenuService.prototype.extendMenuItemData = function (data) {
                    var internal = this;
                    for (var i = 0; i < data.length; i++) {
                        var menuItem = data[i];

                        //look up names
                        var category1 = this.category1Service.findById(menuItem.CategoryId1);
                        var category2 = this.category2Service.findById(menuItem.CategoryId2);
                        menuItem.isNew = function () {
                            return false;
                        };
                        menuItem.Category1Name = category1 ? category1.Name : "";
                        menuItem.Category2Name = category2 ? category2.Name : "";
                        menuItem.DisplayName = function () {
                            var self = this, webName = self.get("WebName"), name = self.get("Name");
                            if (webName && webName.length > 0) {
                                return webName;
                            } else {
                                return name;
                            }
                        };
                        menuItem.CanEditNameAndDescription = function () {
                            return internal.enableDetailMenuEditing;
                        };
                        menuItem.ShowCantEditMessage = function () {
                            return !this.CanEditNameAndDescription();
                        };
                        menuItem.WebNameCount = function () {
                            var name = this.get("WebName");
                            var length = 0;
                            if (name && name.length && name.length > 0) {
                                length = name.length;
                            }

                            return kendo.format("{0}/{1} characters used", length, 255);
                        };
                        menuItem.WebDescriptionCount = function () {
                            var description = this.get("WebDescription");
                            var length = 0;
                            if (description && description.length && description.length > 0) {
                                length = description.length;
                            }

                            return kendo.format("{0}/{1} characters used", length, 255);
                        };
                        menuItem.Index = function () {
                            return internal.menuItemService.dataSource.view().indexOf(this) + 1;
                        };
                        menuItem.ColorStatus = function () {
                            if (this.dirty) {
                                return "#F29A00";
                            }
                            return "#A4E400";
                        };
                        menuItem.ClearWebName = function () {
                            var self = this;
                            self.set("WebName", "");
                        };
                        menuItem.ClearDescription = function () {
                            var self = this;
                            self.set("WebDescription", "");
                        };

                        if (menuItem.Thumbs == null) {
                            menuItem.Thumbs = new kendo.data.ObservableArray([]);
                        }
                    }
                };

                MenuService.prototype.initAppDataSource = function () {
                    var internal = this;

                    var t = {};

                    var dataSource = new kendo.data.DataSource({
                        batch: true,
                        filter: Services.menuFilterController.RESETFILTER,
                        sort: Services.menuFilterController.SORTFILTER,
                        transport: {
                            read: function (options) {
                                var op = $.ajax({
                                    url: Menu.Settings.Routes.MenuItems.ListMenuItems,
                                    dataType: "json",
                                    type: "POST"
                                });
                                op.done(function (result) {
                                    //create the category data sources
                                    var c1 = result.Categories1, c2 = result.Categories2, display = result.DisplayCategories;

                                    internal.enableDetailMenuEditing = result.DetailEditable;
                                    internal.CreateCategoryService(c1, c2, display);

                                    //extend the JSON items
                                    internal.extendMenuItemData(result.MenuItems);

                                    //get the data source to continue is daily, more usual life
                                    options.success(result);
                                });
                                op.fail(function (result) {
                                    options.error(result);
                                });
                            },
                            update: function (options) {
                                var data = options.data, menuItem = null, similarItems = null;

                                kendo.ui.progress($("body"), true);

                                var jData = JSON.stringify(data);
                                var promise = $.ajax({
                                    url: Menu.Settings.Routes.MenuItems.SaveMenuItems,
                                    data: jData,
                                    type: "POST",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8"
                                });

                                promise.done(function (result, textStatus, jqXHR) {
                                    options.success(result, textStatus, jqXHR);

                                    //update additional items ?
                                    //many items should have been updated with options.success but some reason isn't ... manual update.
                                    //internal.menuItemService.updateItems(result, false);
                                    kendo.ui.progress($("body"), false);
                                });

                                promise.fail(function (jqXHR, textStatus, errorThrown) {
                                    //options.error(data, textStatus, jqXHR);
                                    alert("There was a an error saving your request. Please try again");

                                    kendo.ui.progress($("body"), false);
                                });
                            }
                        },
                        schema: {
                            data: function (data) {
                                return data.MenuItems;
                            },
                            total: function (data) {
                                return data.MenuItems.length;
                            },
                            errors: "Errors",
                            model: {
                                id: "Id",
                                fields: {
                                    WebName: {
                                        validation: {}
                                    },
                                    WebDescription: {
                                        validation: {
                                            maxLength: function (context) {
                                                var $i = $(context);
                                                if ($i.is("[name=WebDescription]") || $i.is("[name=WebName]")) {
                                                    var currentLenght = $i.val().length;
                                                    $i.attr("data-maxlength-msg", kendo.format(Services.MenuControllerService.MaxLengthMessage, 255, currentLenght));
                                                    return $i.val().length <= 255;
                                                }
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        serverPaging: false,
                        serverFiltering: false,
                        serverSorting: false,
                        pageSize: 10
                    });

                    this.menuItemService = new Services.menuItemService(dataSource);
                };

                MenuService.prototype.init = function () {
                    //setup own data source
                    this.initAppDataSource();

                    //setup filters
                    this.setupFilterHelper();
                };
                MenuService.Name = "MenuService";
                return MenuService;
            })();
            Services.MenuService = MenuService;
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
