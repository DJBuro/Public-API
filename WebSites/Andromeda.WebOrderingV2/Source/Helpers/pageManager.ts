/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/modernizr/modernizr.d.ts" />

module AndroWeb {
    export class pageManager {
        private static historyStack: string[];
        public currentPage: string;
         
        public showPreviousPage(addToHistory: boolean = true, state: any = undefined, mustBeDifferentPage: any = undefined) 
        {
            var previousPagePath: string = this.getPreviousPage(mustBeDifferentPage);
            this.showPage(previousPagePath, addToHistory, state);
        }

        public showPage(path: string, addToHistory: boolean = true, state: any = undefined) 
        {
            path = this.parseUrl(path);

            var pathParts: string[] = path.split("/");
            var rootPathPart = pathParts[0].toUpperCase();

            // Special case :)
            if (rootPathPart === 'MENU' && pathParts.length === 1) 
            {
                // We want to go to the menu page but no menu section specfied so default to the first one
                var section = viewModel.sections()[0];
                this.showPage('Menu/' + section.display.Name, true);
                return;
            }

            // Push the page into the browsers history and show it in the browser bar
            this.addToHistory(rootPathPart, pathParts, addToHistory);

            // Is a site already selected?
            if (viewModel.selectedSite() === undefined &&
                rootPathPart !== "PLEASEWAIT" &&
                rootPathPart !== "FEEDBACK")
            {
                // No site selected - must select a site before we go anywhere
                this.gotoHomePage(pathParts);
            }
            else 
            {
                // Show the view
                switch (rootPathPart) 
                {
                    case "STOREDETAILS": this.gotoStoreDetailsPage(pathParts); break;
                    case "MENU": this.gotoMenuPage(pathParts); break;
                    case "CART": this.gotoCartPage(pathParts); break;
                    case "LOGIN": this.gotoLoginPage(pathParts, state); break;
                    case "CHECKOUT": this.gotoCheckoutPage(pathParts); break;
                    case "MYPROFILE": this.gotoMyProfilePage(pathParts, state); break;
                    case "MYORDERS": this.gotoMyOrdersPage(pathParts); break;
                    case "MOBILEMENU": this.gotoMobileMenuPage(pathParts); break;
                    case "CUSTOMIZEMENUITEM": this.gotoCustomizeMenuItemPage(pathParts, state); break;
                    case "INFO": this.gotoInfoPage(pathParts); break;
                    case "PASSWORDRESETREQUEST": this.gotoPasswordResetRequestPage(pathParts); break;
                    case "MERCURYPAYMENT": this.gotoMercuryPaymentPage(pathParts); break;
                    case "DATACASHPAYMENT": this.gotoDatacashPaymentPage(pathParts); break;
                    case "MERCANETPAYMENT": this.gotoMercanetPaymentPage(pathParts); break;
                    case "SUBMITORDER": this.gotoSubmitOrderPage(pathParts, state); break;
                    case "ORDERRESULT": this.gotoOrderResultPage(pathParts, state); break;
                    case "PLEASEWAIT": this.gotoPleaseWaitPage(pathParts, state); break;
                    case "DELIVERYADDRESS": this.gotoDeliveryAddressPage(pathParts, state); break;
                    case "ADDDEAL": this.gotoAddDealPage(pathParts, state); break;
                    case "FEEDBACK": this.gotoFeedbackPage(pathParts, state); break;

                    case "HOME":
                    default: this.gotoHomePage(pathParts); break;
                }
            }
        }

        private gotoChooseStorePage(path: string[]) 
        {
            viewModel.chooseStore();
        }

        private gotoStoreDetailsPage(path: string[]) 
        {
            viewHelper.showStoreDetails();
        }

        private gotoMenuPage(path: string[]) 
        {
            var menuSectionIndex: number = 0;

            if (path.length > 1) 
            {
                menuSectionIndex = menuHelper.getMenuSectionIndex(path[1]);
            }

            guiHelper.showMenu(menuSectionIndex);
        }

        private gotoCartPage(path: string[]) 
        {
            viewHelper.showCart();
        }

        private gotoHomePage(path: string[]) 
        {
            viewHelper.showHome();
        }

        private gotoLoginPage(path: string[], state?: any) 
        {
            viewHelper.showLogin(state);
        }

        private gotoCheckoutPage(path: string[]) 
        {
            // Is the customer logged in?
            if (this.canAccessCheckout(path)) 
            {
                // Logged in - show the checkout page
                viewHelper.showCheckout();
            }
        }

        private gotoMyProfilePage(path: string[], state:any) 
        {
            // Is the customer logged in?
            if (this.isLoggedIn(path)) 
            {
                // Logged in - show the my profile page
                viewHelper.showMyProfile(state);
            }
        }

        private gotoMyOrdersPage(path: string[]) 
        {
            // Is the customer logged in?
            if (this.isLoggedIn(path)) 
            {
                // Logged in - show the my orders page
                viewHelper.showMyOrders();
            }
        }

        private gotoMobileMenuPage(path: string[]) 
        {
            viewHelper.showMobileMenu();
        }

        private gotoCustomizeMenuItemPage(path: string[], state: any) 
        {
            if (state === undefined) 
            {
                guiHelper.showMenu(0);
            }
            else 
            {
                viewHelper.showToppingsPopup(state);
            }
        }

        private gotoInfoPage(path: string[]) 
        {
            if (path.length > 1) 
            {
                var infoSection: string = path[1];

                switch (infoSection) 
                {
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
        }

        private gotoPasswordResetRequestPage(path: string[]) 
        {
            viewHelper.showPasswordResetRequest();
        }

        private gotoMercuryPaymentPage(path: string[]) 
        {
            if (this.canAccessCheckout(path)) 
            {
                viewHelper.showMercuryPayment();
            }
        }

        private gotoDatacashPaymentPage(path: string[]) 
        {
            if (this.canAccessCheckout(path)) 
            {
                viewHelper.showDatacashPayment();
            }
        }

        private gotoMercanetPaymentPage(path: string[]) 
        {

            if (this.canAccessCheckout(path)) 
            {
                viewHelper.showMercanetPayment();
            }
        }

        private gotoSubmitOrderPage(path: string[], state: any) 
        {
            if (this.canAccessCheckout(path)) 
            {
                viewHelper.showSubmitOrder();
            }
        }

        private gotoOrderResultPage(path: string[], state: any) 
        {
            // Is the customer logged in?       
            viewHelper.showOrderAccepted(state);
        }

        private gotoPleaseWaitPage(path: string[], state: any) 
        {
            viewHelper.showPleaseWaitPage(state);
        }

        private gotoDeliveryAddressPage(path: string[], state: any) 
        {
            // Is the customer logged in?
            if (this.canAccessCheckout(path)) 
            {
                if (state === undefined) 
                {
                    guiHelper.showMenu(0);
                }
                else 
                {
                    viewHelper.showDeliveryAddressPopup(state);
                }
            }
        }

        private gotoAddDealPage(path: string[], state: any)
        {
            guiHelper.showAddDealPopup(state);
        }

        private gotoFeedbackPage(path: string[], state: any) 
        {
            viewHelper.showFeedback(state);
        }

        private isLoggedIn(path: string[]) 
        {
            // Is the customer logged in?
            if (!accountHelper.isLoggedIn()) 
            {
                this.showPage("Login", true);
                //this.gotoLoginPage(path);
                return false;
            }

            return true;
        }

        private canAccessCheckout(path: string[]) 
        {
            // Is the customer logged in?
            if (!accountHelper.isLoggedIn()) 
            {
                this.showPage("Login", true);
                //this.gotoLoginPage(path);
                return false;
            }

            // Is there anything in the customers cart?
            if (cartHelper.isCartEmpty()) 
            {
                this.showPage("Menu", true);
                //this.gotoMenuPage(path);
                return false;
            }

            return true;
        }

        private addToHistory(rootPathPart: string, path: string[], addToHistory: boolean) 
        {
            if (!addToHistory) return;

            // Remember which page we're currently showing
            viewModel.pageManager.currentPage = rootPathPart;

            var newPath: string = "#/";

            for (var index = 0; index < path.length; index++) 
            {
                newPath += path[index] + "/";
            }

            var state = window.history.state;
            var pageId = 0;
            if (typeof (state) === 'number' && !isNaN(state)) 
            {
                pageId = Number(state) + 1;
            }

            // Add the page to the browser history
            window.history.pushState(pageId, undefined, newPath);

            // Store for later
            pageManager.historyStack[pageId] = newPath;
        }

        // Do this last to ensure all functions are available
        public initialise() 
        {
            if (Modernizr.history) 
            {
                pageManager.historyStack = [];

                window.addEventListener
                    (
                    "hashchange",
                    (ev: PopStateEvent): void=> 
                    {
                        var url = window.location.href;
                        var path: string = url.substring(url.indexOf("#") + 1);
                        path = path.replace("&", "/");

                        this.showPage(path, false);
                    }
                    );
            }
        }

        private endsWith(target: string, endsWithText: string): boolean 
        {
            return (target.indexOf(endsWithText, target.length - 1) > -1);
        }

        private getPreviousPage(mustBeDifferentPage): string 
        {
            var previousUrl = "";

            if (window.history.length > 0) 
            {
                var currentPage = mustBeDifferentPage ? pageManager.historyStack[window.history.state] : "";

                var state = window.history.state;
                var pageId = 0;
                if (typeof (state) === 'number' && !isNaN(state)) 
                {
                    pageId = Number(state) - 1;
                }

                while (true === true) 
                {
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
        }

        private parseUrl(url: string): string 
        {
            var hashIndex: number = url.indexOf("#/");

            if (url.slice(0, 1) === '/') url = url.slice(1);

            var realUrl: string;
            if (hashIndex > -1) 
            {
                realUrl = url.slice(hashIndex + 2);
            }
            else 
            {
                realUrl = url;
            }

            return realUrl;
        }
    }
}