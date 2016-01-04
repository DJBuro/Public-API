/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Services;
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
                    this.StoreSubject = new Rx.BehaviorSubject([]);
                }
                ContextService.Name = "ContextService";
                return ContextService;
            })();
            Services.ContextService = ContextService;
        })(Services = WebOrdering.Services || (WebOrdering.Services = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
