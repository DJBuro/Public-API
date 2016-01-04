/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/modernizr/modernizr.d.ts" />
var AndroWeb;
(function (AndroWeb) {
    var pageManager = (function () {
        function pageManager() {
        }
        pageManager.prototype.showPreviousPage = function (addToHistory, state, mustBeDifferentPage) {
            if (addToHistory === void 0) { addToHistory = true; }
            if (state === void 0) { state = undefined; }
            if (mustBeDifferentPage === void 0) { mustBeDifferentPage = undefined; }
            var previousPagePath = this.getPreviousPage(mustBeDifferentPage);
            this.showPage(previousPagePath, addToHistory, state);
        };
        pageManager.prototype.showPage = function (path, addToHistory, state) {
            if (addToHistory === void 0) { addToHistory = true; }
            if (state === void 0) { state = undefined; }
            path = this.parseUrl(path);
            var pathParts = path.split("/");
            var rootPathPart = pathParts[0].toUpperCase();
            // Special case :)
            if (rootPathPart === 'MENU' && pathParts.length === 1) {
                // We want to go to the menu page but no menu section specified so default to the first one
                var section = viewModel.sections()[0];
                this.showPage('Menu/' + section.display.Name, true);
                return;
            }
            // Push the page into the browsers history and show it in the browser bar
            this.addToHistory(rootPathPart, pathParts, addToHistory);
            // Is a site already selected?
            if (viewModel.selectedSite() === undefined && rootPathPart !== "PLEASEWAIT" && rootPathPart !== "FEEDBACK" && rootPathPart !== "PAGES") {
                // No site selected - must select a site before we go anywhere
                this.gotoHomePage(pathParts);
            }
            else {
                switch (rootPathPart) {
                    case "STOREDETAILS":
                        this.gotoStoreDetailsPage(pathParts);
                        break;
                    case "MENU":
                        this.gotoMenuPage(pathParts);
                        break;
                    case "CART":
                        this.gotoCartPage(pathParts);
                        break;
                    case "LOGIN":
                        this.gotoLoginPage(pathParts, state);
                        break;
                    case "CHECKOUT":
                        this.gotoCheckoutPage(pathParts);
                        break;
                    case "MYPROFILE":
                        this.gotoMyProfilePage(pathParts, state);
                        break;
                    case "MYORDERS":
                        this.gotoMyOrdersPage(pathParts);
                        break;
                    case "MOBILEMENU":
                        this.gotoMobileMenuPage(pathParts);
                        break;
                    case "CUSTOMIZEMENUITEM":
                        this.gotoCustomizeMenuItemPage(pathParts, state);
                        break;
                    case "INFO":
                        this.gotoInfoPage(pathParts);
                        break;
                    case "PAGES":
                        this.gotoCmsPage(pathParts);
                        break;
                    case "PASSWORDRESETREQUEST":
                        this.gotoPasswordResetRequestPage(pathParts);
                        break;
                    case "MERCURYPAYMENT":
                        this.gotoMercuryPaymentPage(pathParts);
                        break;
                    case "DATACASHPAYMENT":
                        this.gotoDatacashPaymentPage(pathParts);
                        break;
                    case "MERCANETPAYMENT":
                        this.gotoMercanetPaymentPage(pathParts);
                        break;
                    case "SUBMITORDER":
                        this.gotoSubmitOrderPage(pathParts, state);
                        break;
                    case "ORDERRESULT":
                        this.gotoOrderResultPage(pathParts, state);
                        break;
                    case "PLEASEWAIT":
                        this.gotoPleaseWaitPage(pathParts, state);
                        break;
                    case "DELIVERYADDRESS":
                        this.gotoDeliveryAddressPage(pathParts, state);
                        break;
                    case "ADDDEAL":
                        this.gotoAddDealPage(pathParts, state);
                        break;
                    case "FEEDBACK":
                        this.gotoFeedbackPage(pathParts, state);
                        break;
                    case "HOME":
                    default:
                        this.gotoHomePage(pathParts);
                        break;
                }
            }
        };
        pageManager.prototype.gotoChooseStorePage = function (path) {
            viewModel.chooseStore();
        };
        pageManager.prototype.gotoStoreDetailsPage = function (path) {
            viewHelper.showStoreDetails();
        };
        pageManager.prototype.gotoMenuPage = function (path) {
            var menuSectionIndex = 0;
            if (path.length > 1) {
                menuSectionIndex = menuHelper.getMenuSectionIndex(path[1]);
            }
            guiHelper.showMenu(menuSectionIndex);
        };
        pageManager.prototype.gotoCartPage = function (path) {
            viewHelper.showCart();
        };
        pageManager.prototype.gotoHomePage = function (path) {
            viewHelper.showHome();
        };
        pageManager.prototype.gotoLoginPage = function (path, state) {
            viewHelper.showLogin(state);
        };
        pageManager.prototype.gotoCheckoutPage = function (path) {
            // Is the customer logged in?
            if (this.canAccessCheckout(path)) {
                // Logged in - show the checkout page
                viewHelper.showCheckout();
            }
        };
        pageManager.prototype.gotoMyProfilePage = function (path, state) {
            // Is the customer logged in?
            if (this.isLoggedIn(path)) {
                // Logged in - show the my profile page
                viewHelper.showMyProfile(state);
            }
        };
        pageManager.prototype.gotoMyOrdersPage = function (path) {
            // Is the customer logged in?
            if (this.isLoggedIn(path)) {
                // Logged in - show the my orders page
                viewHelper.showMyOrders();
            }
        };
        pageManager.prototype.gotoMobileMenuPage = function (path) {
            viewHelper.showMobileMenu();
        };
        pageManager.prototype.gotoCustomizeMenuItemPage = function (path, state) {
            if (state === undefined) {
                guiHelper.showMenu(0);
            }
            else {
                viewHelper.showToppingsPopup(state);
            }
        };
        pageManager.prototype.gotoInfoPage = function (path) {
            if (path.length > 1) {
                var infoSection = path[1];
                switch (infoSection) {
                    case "TermsOfUse":
                        viewHelper.showTermsAndConditions();
                        break;
                    case "PrivacyPolicy":
                        viewHelper.showPrivacyPolicy();
                        break;
                    case "CookiePolicy":
                        viewHelper.showCookiePolicy();
                        break;
                }
            }
        };
        pageManager.prototype.gotoCmsPage = function (path) {
            if (path.length > 1) {
                var pageName = path[1];
                viewHelper.showCmsPage(pageName);
            }
        };
        pageManager.prototype.gotoPasswordResetRequestPage = function (path) {
            viewHelper.showPasswordResetRequest();
        };
        pageManager.prototype.gotoMercuryPaymentPage = function (path) {
            if (this.canAccessCheckout(path)) {
                viewHelper.showMercuryPayment();
            }
        };
        pageManager.prototype.gotoDatacashPaymentPage = function (path) {
            if (this.canAccessCheckout(path)) {
                viewHelper.showDatacashPayment();
            }
        };
        pageManager.prototype.gotoMercanetPaymentPage = function (path) {
            if (this.canAccessCheckout(path)) {
                viewHelper.showMercanetPayment();
            }
        };
        pageManager.prototype.gotoSubmitOrderPage = function (path, state) {
            if (this.canAccessCheckout(path)) {
                viewHelper.showSubmitOrder();
            }
        };
        pageManager.prototype.gotoOrderResultPage = function (path, state) {
            // Is the customer logged in?       
            viewHelper.showOrderAccepted(state);
        };
        pageManager.prototype.gotoPleaseWaitPage = function (path, state) {
            viewHelper.showPleaseWaitPage(state);
        };
        pageManager.prototype.gotoDeliveryAddressPage = function (path, state) {
            // Is the customer logged in?
            if (this.canAccessCheckout(path)) {
                if (state === undefined) {
                    guiHelper.showMenu(0);
                }
                else {
                    viewHelper.showDeliveryAddressPopup(state);
                }
            }
        };
        pageManager.prototype.gotoAddDealPage = function (path, state) {
            guiHelper.showAddDealPopup(state);
        };
        pageManager.prototype.gotoFeedbackPage = function (path, state) {
            viewHelper.showFeedback(state);
        };
        pageManager.prototype.isLoggedIn = function (path) {
            // Is the customer logged in?
            if (!accountHelper.isLoggedIn()) {
                this.showPage("Login", true);
                return false;
            }
            return true;
        };
        pageManager.prototype.canAccessCheckout = function (path) {
            // Is the customer logged in?
            if (!accountHelper.isLoggedIn()) {
                this.showPage("Login", true);
                return false;
            }
            // Is there anything in the customers cart?
            if (cartHelper.isCartEmpty()) {
                this.showPage("Menu", true);
                return false;
            }
            return true;
        };
        pageManager.prototype.addToHistory = function (rootPathPart, path, addToHistory) {
            if (!addToHistory)
                return;
            // Remember which page we're currently showing
            viewModel.pageManager.currentPage = rootPathPart;
            if (window.history === undefined || window.history.pushState === undefined)
                return;
            var newPath = "#/";
            for (var index = 0; index < path.length; index++) {
                newPath += path[index] + "/";
            }
            var state = window.history.state;
            var pageId = 0;
            if (typeof (state) === 'number' && !isNaN(state)) {
                pageId = Number(state) + 1;
            }
            // Add the page to the browser history
            window.history.pushState(pageId, undefined, newPath);
            // Store for later
            pageManager.historyStack[pageId] = newPath;
        };
        // Do this last to ensure all functions are available
        pageManager.prototype.initialise = function () {
            var _this = this;
            if (Modernizr.history) {
                pageManager.historyStack = [];
                window.addEventListener("hashchange", function (ev) {
                    var url = window.location.href;
                    var path = url.substring(url.indexOf("#") + 1);
                    path = path.replace("&", "/");
                    _this.showPage(path, false);
                });
            }
        };
        pageManager.prototype.endsWith = function (target, endsWithText) {
            return (target.indexOf(endsWithText, target.length - 1) > -1);
        };
        pageManager.prototype.getPreviousPage = function (mustBeDifferentPage) {
            var previousUrl = "";
            if (window.history.length > 0) {
                var currentPage = mustBeDifferentPage ? pageManager.historyStack[window.history.state] : "";
                var state = window.history.state;
                var pageId = 0;
                if (typeof (state) === 'number' && !isNaN(state)) {
                    pageId = Number(state) - 1;
                }
                while (true === true) {
                    var previousHistoryState = pageManager.historyStack[pageId];
                    if (previousHistoryState === undefined) {
                        previousUrl = currentPage;
                        break;
                    }
                    if (previousHistoryState !== currentPage) {
                        previousUrl = this.parseUrl(previousHistoryState);
                        break;
                    }
                    pageId--;
                }
            }
            return previousUrl;
        };
        pageManager.prototype.parseUrl = function (url) {
            var hashIndex = url.indexOf("#/");
            if (url.slice(0, 1) === '/')
                url = url.slice(1);
            var realUrl;
            if (hashIndex > -1) {
                realUrl = url.slice(hashIndex + 2);
            }
            else {
                realUrl = url;
            }
            return realUrl;
        };
        return pageManager;
    })();
    AndroWeb.pageManager = pageManager;
})(AndroWeb || (AndroWeb = {}));
//# sourceMappingURL=pagemanager.js.map