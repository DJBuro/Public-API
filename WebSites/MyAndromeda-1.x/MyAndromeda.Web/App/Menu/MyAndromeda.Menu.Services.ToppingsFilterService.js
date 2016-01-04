var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        /// <reference path="../../Scripts/typings/rx.js/rx.d.ts" />
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(MenuToppingsFilterService.Name, [
                    Services.MenuToppingsService.Name,
                    function (menuToppingsService) {
                        var instnance = new MenuToppingsFilterService(menuToppingsService);

                        return instnance;
                    }
                ]);
            });

            var MenuToppingsFilterService = (function () {
                function MenuToppingsFilterService(menuToppingsService) {
                    var _this = this;
                    this.menuToppingsService = menuToppingsService;
                    this.dataSource = this.menuToppingsService.GetDataSource();
                    this.ResetFiltersObservable = new Rx.Subject();

                    this.model = {
                        Name: "",
                        ResetFilters: function () {
                            _this.ResetFilters();
                        }
                    };
                }
                MenuToppingsFilterService.prototype.ChangeNameFilter = function (name) {
                    this.model.Name = name;
                    this.Filter();
                };

                MenuToppingsFilterService.prototype.GetName = function () {
                    return this.model.Name;
                };

                MenuToppingsFilterService.prototype.GetNameFilter = function () {
                    var filter = {
                        field: "Name",
                        operator: "contains",
                        value: this.model.Name
                    };

                    return filter;
                };

                MenuToppingsFilterService.prototype.Filter = function () {
                    var filters = [];
                    if (this.model.Name !== "") {
                        var nameFilter = this.GetNameFilter();
                        filters.push(nameFilter);
                    }

                    this.dataSource.filter(filters);
                };

                MenuToppingsFilterService.prototype.ResetFilters = function () {
                    this.dataSource.filter([]);
                    this.ResetFiltersObservable.onNext(true);
                };
                MenuToppingsFilterService.Name = "MenuToppingsFilterService ";
                return MenuToppingsFilterService;
            })();
            Services.MenuToppingsFilterService = MenuToppingsFilterService;
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
