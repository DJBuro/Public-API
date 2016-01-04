var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(MenuToppingsService.Name, [
                    function () {
                        var instnance = new MenuToppingsService();
                        return instnance;
                    }
                ]);
            });

            var MenuToppingsService = (function () {
                function MenuToppingsService() {
                }
                MenuToppingsService.prototype.GetDataSource = function () {
                    var internal = this;
                    if (this.dataSource) {
                        return this.dataSource;
                    }

                    var routes = Menu.Settings.Routes.Toppings;

                    var dataSourceGroup = [
                        { field: "Name", dir: "" }
                    ];
                    this.dataSource = new kendo.data.DataSource({
                        type: "aspnetmvc-ajax",
                        batch: true,
                        pageSize: 10,
                        //group: dataSourceGroup,
                        transport: {
                            read: { url: routes.List },
                            update: { url: routes.Update }
                        },
                        schema: {
                            data: "Data",
                            total: "Total",
                            errors: "Errors",
                            model: {
                                id: "Id",
                                UpdateAllDelivery: function (confirm) {
                                    var item = this;

                                    item.ToppingVarients.forEach(function (varient) {
                                        var c = item.get("CollectionPrice"), d = item.get("DeliveryPrice"), s = item.get("DineInPrice");
                                        varient.set("DeliveryPrice", d);
                                        varient.trigger("change");
                                    });

                                    item.trigger("change");
                                },
                                UpdateAllCollection: function (confirm) {
                                    var item = this;

                                    item.ToppingVarients.forEach(function (varient) {
                                        var c = item.get("CollectionPrice");

                                        varient.set("CollectionPrice", c);

                                        varient.trigger("change");
                                    });
                                },
                                UpdateAllDineIn: function (confirm) {
                                    var item = this;

                                    item.ToppingVarients.forEach(function (varient) {
                                        var s = item.get("DineInPrice");

                                        varient.set("DineInPrice", s);

                                        varient.trigger("change");
                                    });
                                },
                                UpdateAllToppingPrices: function (confirm) {
                                    var item = this;
                                    console.log(item);

                                    item.ToppingVarients.forEach(function (varient) {
                                        var c = item.get("CollectionPrice"), d = item.get("DeliveryPrice"), s = item.get("DineInPrice");

                                        varient.set("CollectionPrice", c);
                                        varient.set("DeliveryPrice", d);
                                        varient.set("DineInPrice", s);

                                        varient.trigger("change");
                                    });

                                    item.trigger("change");
                                },
                                ColorStatus: function () {
                                    var item = this;
                                    if (this.dirty) {
                                        return "#F29A00";
                                    }
                                    return "#A4E400";
                                }
                            }
                        }
                    });

                    return this.dataSource;
                };

                MenuToppingsService.prototype.FindTopppings = function (predicate) {
                    var data = this.dataSource.data();
                    var filtered = data.filter(predicate);
                    return filtered;
                };
                MenuToppingsService.Name = "ToppingsService";
                return MenuToppingsService;
            })();
            Services.MenuToppingsService = MenuToppingsService;
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
