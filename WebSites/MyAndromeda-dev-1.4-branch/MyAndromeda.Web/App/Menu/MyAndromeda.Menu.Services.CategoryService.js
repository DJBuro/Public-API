var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            var CategoryService = (function () {
                function CategoryService(categories) {
                    this.categories = categories;
                }
                CategoryService.prototype.findById = function (id) {
                    for (var i = 0; i < this.categories.length; i++) {
                        var c = this.categories[i];
                        if (c.Id === id) {
                            return c;
                        }
                    }
                    return null;
                };
                CategoryService.prototype.findCategoriesByParentId = function (id) {
                    var elements = $.grep(this.categories, function (category, index) {
                        return category.ParentId === id;
                    });
                    return elements;
                };
                return CategoryService;
            })();
            Services.CategoryService = CategoryService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
