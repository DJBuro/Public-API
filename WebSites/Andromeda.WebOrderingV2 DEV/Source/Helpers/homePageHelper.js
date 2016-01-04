var AndroWeb;
(function (AndroWeb) {
    var Helpers;
    (function (Helpers) {
        "use strict";
        var HomePageHelper = (function () {
            function HomePageHelper() {
                var _this = this;
                this.PostCodeSites = ko.observable();
                this.SitePicked = ko.observable(false);
                this.NeedToChooseStore = ko.observable(false);
                this.Errors = ko.validation.group(this, { deep: false, observable: false });
                this.PostCodeSearch = ko.observable("").extend({
                    pattern: {
                        onlyIf: function () {
                            if (!_this.PostCodeSearch) {
                                //i don't exist yet
                                return true;
                            }
                            var postCode = _this.PostCodeSearch();
                            if (!postCode) {
                                postCode = "";
                            }
                            return postCode.replace(" ", "").length > 5;
                        },
                        //todo: move to engb list 
                        message: 'Must be a valid UK postcode',
                        params: /^([A-PR-UWYZ0-9][A-HK-Y0-9][AEHMNPRTVXY0-9]?[ABEHMNPRVWXY0-9]?\s?){1,2}([0-9][ABD-HJLN-UW-Z]{2}|GIR 0AA)$/i
                    }
                });
                /* Postcode lookup UI cues  */
                this.FoundAStore = ko.observable(false);
                this.NoStoresDeliver = ko.observable(false);
                this.StoresThatDeliver = ko.observable();
                AndroWeb.Logger.Notify("Creating homepage helper");
                //     this.WaitForInit(); // Don't do it this way as the entry point might be the password reset page not the home page
                this.WatchForSiteDetails();
                this.WatchForPostcodeChanges();
                //this.WatchForSelectedStore(); 
            }
            HomePageHelper.prototype.WaitForInit = function () {
                viewModel.initialised.subscribe(function (ready) {
                    if (ready) {
                        //preload all available stores. 
                        AndroWeb.Logger.Notify("Initilised complete ... choose store");
                        viewModel.chooseStore();
                    }
                });
            };
            HomePageHelper.prototype.WatchForSites = function () {
                viewModel.sites.subscribe(function (sites) {
                    AndroWeb.Logger.Notify("Sites have arrived");
                    AndroWeb.Logger.Notify(sites);
                });
            };
            //private WatchForSelectedStore()
            //{
            //    homePageActionHelper.SelectedDropDownItem.subscribe(change => {
            //        Logger.Notify("Store changed");
            //        Logger.Notify(change);
            //    });
            //}
            HomePageHelper.prototype.WatchForSiteDetails = function () {
                var _this = this;
                if (!viewModel) {
                    AndroWeb.Logger.Error("No viewModel");
                }
                if (!viewModel.siteDetails) {
                    AndroWeb.Logger.Error("site details observable is missing");
                }
                this.SitePicked(false);
                this.NeedToChooseStore(true);
                var selected = function () {
                    var siteDetails = viewModel.siteDetails();
                    var sections = viewModel.sections();
                    AndroWeb.Logger.Notify("Selected siteDetails");
                    AndroWeb.Logger.Notify(siteDetails);
                    AndroWeb.Logger.Notify(sections);
                    return siteDetails && sections && sections.length > 0;
                };
                viewModel.siteDetails.subscribe(function (siteDetails) {
                    AndroWeb.Logger.Notify("Checking site details");
                    if (!siteDetails) {
                        _this.SitePicked(false);
                        _this.NeedToChooseStore(true);
                        return;
                    }
                    _this.SitePicked(selected());
                    _this.NeedToChooseStore(!selected());
                });
                viewModel.sections.subscribe(function (sections) {
                    AndroWeb.Logger.Notify("sections changed");
                    _this.SitePicked(selected());
                    _this.NeedToChooseStore(!selected());
                });
            };
            HomePageHelper.prototype.WatchForPostcodeChanges = function () {
                var _this = this;
                this.PostCodeSearch.subscribe(function (postcode) {
                    AndroWeb.Logger.Notify("Search by postcode: " + postcode);
                    var errors = _this.Errors();
                    var valid = errors.length === 0;
                    if (postcode.length < 6 || !valid) {
                        _this.NoStoresDeliver(false);
                        _this.FoundAStore(false);
                        return;
                    }
                    AndroWeb.Logger.Notify("IsValid = " + valid);
                    var searchPostcode = postcode.replace(' ', '');
                    //addressHelper.validatePostcode(searchPostcode)
                    AndroWeb.Logger.Notify("Search based on postcodes");
                    _this.PostCodeSites([]);
                    acsapi.getSites(function (values) {
                        AndroWeb.Logger.Notify("Result for postcode search:" + values.length);
                        _this.PostCodeSites(values);
                        if (values.length === 0) {
                            _this.NoStoresDeliver(true);
                            _this.FoundAStore(false);
                        }
                        if (values.length > 0) {
                            _this.FoundAStore(true);
                            _this.NoStoresDeliver(false);
                        }
                    }, function () {
                        viewModel.chooseStore();
                    }, searchPostcode);
                });
            };
            HomePageHelper.prototype.SelectStoreFromPostCode = function () {
                AndroWeb.Logger.Notify("Helper: Selecting from postcode");
                var sites = this.PostCodeSites(); //viewModel.sites();
                var first = sites[0];
                viewModel.pageManager.showPage("PLEASEWAIT", false);
                viewModel.siteDetails(undefined);
                viewModel.selectedSite(first);
                guiHelper.downloadAndShowStoreMenu(undefined, viewModel.pageManager.showPage);
            };
            HomePageHelper.prototype.SelectStoreFromList = function (store) {
                AndroWeb.Logger.Notify("Helper: Selecting from list");
                AndroWeb.Logger.Notify("Selecting store: " + store);
                viewModel.pageManager.showPage("PLEASEWAIT", false);
                viewModel.siteDetails(undefined);
                viewModel.selectedSite(store);
                guiHelper.downloadAndShowStoreMenu(undefined, viewModel.pageManager.showPage);
            };
            HomePageHelper.prototype.ClearCurrentStore = function () {
                if (cartHelper.cart().hasItems()) {
                    var warningMessage = textStrings.gExitWarning;
                    var clear = window.confirm(warningMessage);
                    if (!clear) {
                        return;
                    }
                }
                this.SitePicked(false);
                this.NeedToChooseStore(true);
                viewModel.selectedSite(undefined);
                viewModel.siteDetails(undefined);
                cartHelper.clearCart();
                viewHelper.showHome();
                accountHelper.helloText(" ");
            };
            return HomePageHelper;
        })();
        Helpers.HomePageHelper = HomePageHelper;
    })(Helpers = AndroWeb.Helpers || (AndroWeb.Helpers = {}));
})(AndroWeb || (AndroWeb = {}));
var homePageActionHelper = {
    SelectedDropDownItem: ko.observable({
        name: "none"
    }),
    SelectStoreFromPostCode: function (e) {
        AndroWeb.Logger.Notify("Click on: SelectStoreFromPostCode");
        var action = $.proxy(homePageHelper.SelectStoreFromPostCode, homePageHelper);
        action();
    },
    SelectStoreFromDropdown: function (e) {
        AndroWeb.Logger.Notify("Click on: SelectStoreFromDropdown");
        var selectedItem = homePageActionHelper.SelectedDropDownItem();
        var action = $.proxy(homePageHelper.SelectStoreFromList, homePageHelper, selectedItem);
        action();
    },
    ClearCurrentStore: function (e) {
        AndroWeb.Logger.Notify("Click on: ClearCurrentStore");
        var action = $.proxy(homePageHelper.ClearCurrentStore, homePageHelper);
        action();
    }
};
var homePageHelper = new AndroWeb.Helpers.HomePageHelper();
//# sourceMappingURL=homePageHelper.js.map