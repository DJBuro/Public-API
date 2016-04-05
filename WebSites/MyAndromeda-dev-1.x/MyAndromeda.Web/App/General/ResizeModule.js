var MyAndromeda;
(function (MyAndromeda) {
    var Services;
    (function (Services) {
        var m = angular.module("MyAndromeda.Resize", []);
        var ResizeService = (function () {
            function ResizeService() {
                var r = new Rx.Subject();
                this.ResizeObservable = r.throttle(250);
                $(window).resize(function (e) {
                    var width = $(window).width();
                    var height = $(window).height();
                    r.onNext({
                        height: height,
                        width: width
                    });
                });
            }
            return ResizeService;
        }());
        Services.ResizeService = ResizeService;
        m.service("resizeService", ResizeService);
        m.run(function (resizeService) {
            resizeService.ResizeObservable.subscribe(function (e) {
                MyAndromeda.Logger.Notify(kendo.format("Resize: {0}x{1}", e.width, e.height));
            });
        });
        m.directive('ngRightClick', function ($parse) {
            return function (scope, element, attrs) {
                var fn = $parse(attrs.ngRightClick);
                element.bind('contextmenu', function (event) {
                    scope.$apply(function () {
                        event.preventDefault();
                        fn(scope, { $event: event });
                    });
                });
            };
        });
    })(Services = MyAndromeda.Services || (MyAndromeda.Services = {}));
})(MyAndromeda || (MyAndromeda = {}));
