/// <reference path="myandromeda.marketing.ts" />
module MyAndromeda.Marketing {

    m.controller("StartController", (
        $scope,
        $timeout: ng.ITimeoutService,
        resizeService: Services.ResizeService,
        progressService: Services.ProgressService) => {

        Logger.Notify("start");

        var resizeSubscription = resizeService.ResizeObservable.subscribe((e) => {
            var appTabStrip: kendo.ui.TabStrip = $scope.appTabStrip;
            appTabStrip.resize(true);
        });

        $scope.$on('$destroy', function iVeBeenDismissed() {
            resizeSubscription.dispose();
        });

        //var element = document.getElementById("EventDrivenMarketing");

        //progressService.Create(element).Show();
    });

} 