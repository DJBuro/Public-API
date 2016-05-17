var MyAndromeda;
(function (MyAndromeda) {
    var Hubs;
    (function (Hubs) {
        var Services;
        (function (Services) {
            var MenuHubService = (function () {
                function MenuHubService(options) {
                    var internal = this;
                    this.options = options;
                    this.hub = Hubs.StoreHub.GetInstance(options); //new StoreHub(options);
                    this.viewModel = kendo.observable({
                        siteName: "",
                        menuVersion: "",
                        lastUpdated: "",
                        updates: []
                    });
                }
                MenuHubService.prototype.init = function () {
                    kendo.bind(this.options.id, this.viewModel);
                };
                return MenuHubService;
            }());
            Services.MenuHubService = MenuHubService;
        })(Services = Hubs.Services || (Hubs.Services = {}));
    })(Hubs = MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
})(MyAndromeda || (MyAndromeda = {}));
