declare module AndroWeb.Helpers 
{
    export interface IAccountHelper 
    {
        helloText: KnockoutObservable<string>;
        isLoggedIn: KnockoutObservable<boolean>;
        customerDetails: Models.ICustomer;
        registerError: KnockoutObservable<any>;
        loggedIn(customerDetails: any, userName: string, emailAddress: string, password: string, isFacebookRegister: boolean): void;
        facebookLogin(response: any, loginCallback: any, isCheckoutLogin: boolean): void;
        login(emailAddress, password, loginCallback): void;
        loginDetails: any;
    }

    export interface IAddressHelper 
    {
        validatePostcode: (postCode: string) => boolean;
    }

    export interface IGuiHelper 
    {
        downloadAndShowStoreMenu: () => void;
        showMenu: (index: number) => void;
        showAddDealPopup(state: any): void;
        showPleaseWait(message: string, progress: string, callback: (feedback) => void) : void;
        getCurrentViewName(): string;
        showView(previousViewName: string, previousContentViewModel: any) : void; 
        cartActions(cartActionsCheckout: any): void;
        cartActionsCheckout: boolean;
        canChangeOrderType: KnockoutObservable<boolean>;
        isMobileMenuVisible: KnockoutObservable<boolean>;
    }

    export interface IViewHelper 
    {
        showStoreDetails();
        showCart();
        showHome();
        showLogin(viewModel: any);
        showCheckout(state?: any);
        showMyProfile(state: any);
        showMyOrders();
        showMobileMenu();
        showTermsAndConditions();
        showPrivacyPolicy();
        showCookiePolicy();
        showPasswordResetRequest();
        showMercuryPayment();
        showDatacashPayment();
        showMercanetPayment();
        showOrderAccepted(state: any);
        showPleaseWaitPage(state: any);
        showSubmitOrder();
        showToppingsPopup(state: any);
        showDeliveryAddressPopup(state: any);
        showFeedback(state: any);
        showCmsPage(pageName: string);
    }

    export interface IQueryStringHelper 
    {
        menuSection: any;
        voucherCode: string;
        tableNumber: string;
    }

    export interface IToppingsPopupHelper 
    {
        showPopupUI: (state: any) => void;
        returnToCart: boolean;
        showPopup: (cartItem: any, dummy1: boolean, dummy2: boolean) => void;
    }

    export interface ICustomerHelper 
    {
        bindableCustomer: KnockoutObservable<any>;
    }

    export interface IPopupHelper 
    {
        showToppingsPopup: (state: any) => void;
    }

    export interface IMenuHelper 
    {
        getMenuSectionIndex: (menuSection: string) => number;

        menuItemLookup: Array<AndroWeb.Models.IMenuItemWrapper>;
        menuItemWrapperLookup: Array<AndroWeb.Models.IMenuItemWrapper>;

        menuDataThumbnails: KnockoutObservable<Models.IMyAndromedaThumbnailObject>;
        toppingLookup: Array<any>;

        dealLookup: Array<AndroWeb.Models.DealWrapper>;

        getItemPrice: (menuItem: any) => number;
        getCartItemDisplayName: (self: any) => string;
        getToppingPrice: (topping: any) => number;
        getCartDisplayToppings: (toppings: any) => any;
        rebuildCategory2List: (itemWrapper: any, selectedCategory1: any, selectedCategory2: any) => any;

        refreshDealsAvailabilty: (cart) => void;
        fullOrderDiscountDeal: any;
        isDealItemEnabledForCart: KnockoutObservable<boolean>;
        calculateDealLinePrice: (cartDealLine: any) => number;
        isItemEnabledForCart: KnockoutObservable<boolean>;
        calculateItemPrice: (menuItem: any, quantity: number, dummy: any) => number;
        isItemAvailable: (itemWrapper: any) => boolean;
        calculateToppingPrice: (topping: any) => number;

        updatePrices: () => void;
    }

    export interface ICheckoutHelper 
    {
        checkoutDetails: AndroWeb.Models.ICheckoutDetails;

        telephoneNumber: KnockoutObservable<string>;
        clearCheckout: () => void;
        initialiseCheckout: () => void;
        isAddressMissing: () => boolean;

        refreshTimes: () => void;
        addDeliveryHourSlots: (hour, startMinute, endMinute, slotSize) => void;
        addDeliverySlot: (hour, minute, slotSize) => void;

        formatMinute: (minute: number) => string;
        formatHour: (hour: number) => string;

        visibleCheckoutSection: KnockoutObservable<number>;
        times: KnockoutObservableArray<string>;
        paymentProvider: KnockoutObservable<string>;

        placeOrder: () => void;
        showPaymentPicker: () => void;
        payAtDoor: () => void;
        payNow: () => void;

        prePaymentOnlineCheck: (callback: () => void) => void;

        mercuryPayment: () => void;
        datacashPayment: () => void;
        mercanetPayment: () => void;
        payPalPayment: () => void;

        sendOrderToStore: () => void;
        sendOrderToStoreCallback: (result) => void;
        generateOrderJson: (vouchers) => void;

        voucherError: KnockoutObservable<string>;

        recheckVouchers: () => void;

        checkoutViewModel: any;

        addVoucherCode(): void;
        checkVoucherCode(voucherCode: any): void;
        refreshTimeslotsAndCheckSelected(time: any): void;
    }

    export interface ICartHelper 
    {
        cart: KnockoutObservable<AndroWeb.Models.Cart>;
        clearCart: () => void;
        refreshCart: (cart: any, applyCardCharge: any) => void;

        isCartEmpty(): boolean;
        checkout(): void;
    }

    export interface IHelper 
    {
        markOfflineStores: () => void;
        newOrder: () => void;
        clearOrder: () => void;

        formatUTCDate: (date: Date) => string;
        formatUTCSlot: (slot: string) => string;

        formatPrice: (price: number) => string;
        currencySymbol: string;
        useCommaDecimalPoint: boolean;
        curencyAfter: boolean;
        use24hourClock: boolean;

        findById: (cat1Id: any, category1: any) => any;
        findByMenuId: (id: any, toppings: any) => any;
    }

    export interface IACSAPI 
    {
        putFeedback(siteId: string, feedback: any, callback: any): void;
    }

    export interface IMyOrdersHelper
    {
        repeatOrder();
    }

    export interface IDealPopupHelper
    {
        returnToCart: boolean;
        showDealPopup(dummy1: string, dummy2: boolean, dummy3: any): void;
    }
}
