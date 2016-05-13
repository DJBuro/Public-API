/// <reference path="myandromeda.marketing.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Marketing;
    (function (Marketing) {
        Marketing.m.controller("StartController", function ($scope, $timeout, resizeService, progressService) {
            MyAndromeda.Logger.Notify("start");
            var resizeSubscription = resizeService.ResizeObservable.subscribe(function (e) {
                var appTabStrip = $scope.appTabStrip;
                appTabStrip.resize(true);
            });
            $scope.$on('$destroy', function iVeBeenDismissed() {
                resizeSubscription.dispose();
            });
            //var element = document.getElementById("EventDrivenMarketing");
            //progressService.Create(element).Show();
        });
    })(Marketing = MyAndromeda.Marketing || (MyAndromeda.Marketing = {}));
})(MyAndromeda || (MyAndromeda = {}));
