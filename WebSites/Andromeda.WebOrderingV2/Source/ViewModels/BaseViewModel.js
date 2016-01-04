/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/modernizr/modernizr.d.ts" />
var AndroWeb;
(function (AndroWeb) {
    var ViewModels;
    (function (ViewModels) {
        var BaseViewModel = (function () {
            function BaseViewModel() {
                this.isShowStoreDetailsButtonVisible = ko.observable(true);
                this.isShowHomeButtonVisible = ko.observable(true);
                this.isShowMenuButtonVisible = ko.observable(true);
                this.isShowCartButtonVisible = ko.observable(true);
                this.isHeaderVisible = ko.observable(true);
                this.isPostcodeSelectorVisible = ko.observable(false);
                this.areHeaderOptionsVisible = ko.observable(true);
                this.isHeaderLoginVisible = ko.observable(true);
            }
            return BaseViewModel;
        })();
        ViewModels.BaseViewModel = BaseViewModel;
    })(ViewModels = AndroWeb.ViewModels || (AndroWeb.ViewModels = {}));
})(AndroWeb || (AndroWeb = {}));
//# sourceMappingURL=BaseViewModel.js.map