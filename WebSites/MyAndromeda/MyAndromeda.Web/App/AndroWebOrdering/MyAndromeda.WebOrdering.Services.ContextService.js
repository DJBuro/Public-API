var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
                app.factory(ContextService.Name, [
                    function () {
                        var instnance = new ContextService();
                        return instnance;
                    }
                ]);
            });

            var ContextService = (function () {
                function ContextService() {
                    this.Model = null;
                    this.ModelSubject = new Rx.BehaviorSubject(null);
                }
                ContextService.Name = "ContextService";
                return ContextService;
            })();
            Services.ContextService = ContextService;
        })(WebOrdering.Services || (WebOrdering.Services = {}));
        var Services = WebOrdering.Services;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
