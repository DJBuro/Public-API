var MyAndromeda;
(function (MyAndromeda) {
    var Services;
    (function (Services) {
        var m = angular.module("MyAndromeda.Progress", []);
        var ProgressService = (function () {
            function ProgressService() {
            }
            ProgressService.prototype.Create = function ($element) {
                this.$element = $element;
                return this;
            };
            ProgressService.prototype.Show = function () {
                kendo.ui.progress($(this.$element), true);
            };
            ProgressService.prototype.ShowProgress = function ($element) {
                kendo.ui.progress($($element), true);
            };
            ProgressService.prototype.Hide = function () {
                kendo.ui.progress($(this.$element), false);
            };
            ProgressService.prototype.HideProgress = function ($element) {
                kendo.ui.progress($($element), false);
            };
            return ProgressService;
        }());
        Services.ProgressService = ProgressService;
        m.factory("progressService", function () {
            return new ProgressService();
        });
    })(Services = MyAndromeda.Services || (MyAndromeda.Services = {}));
})(MyAndromeda || (MyAndromeda = {}));
