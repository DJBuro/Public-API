module MyAndromeda.Services
{
    var m = angular.module("MyAndromeda.Resize", []);

    export class ResizeService
    {
        public ResizeObservable: Rx.Observable<{ height: number, width: number}>;

        public constructor()
        {
            var r = new Rx.Subject<{ height: number, width: number }>();
            this.ResizeObservable = r.throttle(250);

            $(window).resize((e) => {
                var width = $(window).width();
                var height = $(window).height();

                r.onNext({
                    height: height,
                    width : width 
                });
            });
        }
    }

    m.service("resizeService", ResizeService);
    m.run((resizeService: ResizeService) => {
        resizeService.ResizeObservable.subscribe((e) => {
            Logger.Notify(kendo.format("Resize: {0}x{1}", e.width, e.height));
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
}
