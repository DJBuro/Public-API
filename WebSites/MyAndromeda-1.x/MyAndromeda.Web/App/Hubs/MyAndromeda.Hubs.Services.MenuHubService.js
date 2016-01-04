var MyAndromeda;
(function (MyAndromeda) {
    (function (Hubs) {
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
            })();
            Services.MenuHubService = MenuHubService;
        })(Hubs.Services || (Hubs.Services = {}));
        var Services = Hubs.Services;
    })(MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
    var Hubs = MyAndromeda.Hubs;
})(MyAndromeda || (MyAndromeda = {}));
