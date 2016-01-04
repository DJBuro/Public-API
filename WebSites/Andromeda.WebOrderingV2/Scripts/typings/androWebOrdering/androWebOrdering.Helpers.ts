declare module AndroWeb.Helpers {
    //export interface IViewModel {
    //    siteDetails: KnockoutObservable<AndroWeb.Models.ISiteDetails>;
    //    selectedSite: KnockoutObservable<any>;
    //    chooseStore(siteID?: string, storeName?: string);
    //    onAddItemToCart(state: any, addToHistory: boolean);
    //    sections: KnockoutObservableArray<any>;
    //    pageManager: AndroWeb.pageManager;
    //    serverUrl: string;
    //}

    export interface IAccountHelper {
        helloText: KnockoutObservable<string>;
        isLoggedIn: KnockoutObservable<boolean>;

        customerDetails: Models.ICustomer;
    }

    export interface IAddressHelper {
        validatePostcode: (postCode: string) => boolean;
    }

    export interface IGuiHelper {
        downloadAndShowStoreMenu: (lockToOrderType: string, callback: Function) => void;


        //showStoreDetails: () => void;
        showMenu: (index: number) => void;
        //showCart: () => void;
        //showHome: () => void;

        //showLogin: (state?: any) => void;
        //showCheckout: () => void;
        //showMyProfile: () => void;
        //showMyOrders: () => void;

        //showMobileMenu: () => void;

        //showTermsAndConditions: () => void;
        //showPrivacyPolicy: () => void;
        //showCookiePolicy: () => void;

        //showPasswordResetRequest: () => void;

        /* payment screens */
        //showMercuryPayment: () => void;
        //showDatacashPayment: () => void;
        //showMercanetPayment: () => void;
        //showSubmitOrder: () => void;
        showAddDealPopup(state: any);

        //showOrderAccepted: (state: any) => void;
        //showPleaseWaitPage: (state: any) => void;

        showPleaseWait(message: string, progress: string, callback: (feedback)=> void): void;
    }

    export interface IViewHelper {
        showStoreDetails();
        showCart();
        showHome();
        showLogin(viewModel: any);
        showCheckout();
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
    }


    export interface IQueryStringHelper {
        menuSection: any;
    }

    export interface IToppingsPopupHelper {
        showPopupUI: (state: any) => void;
    }

    export interface ICustomerHelper {

    }

    export interface IPopupHelper {
        showToppingsPopup: (state: any) => void;
    }

    export interface IMenuHelper {
        getMenuSectionIndex: (menuSection: string) => number;

        menuItemLookup: Array<Models.IMenuItem>;
        menuDataThumbnails: KnockoutObservable<Models.IMyAndromedaThumbnailObject>;
    }

    export interface ICheckoutHelper {
        checkoutDetails: KnockoutObservable<Models.ICheckoutDetails>;

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
    }

    export interface ICartHelper {
        cart: KnockoutObservable<Models.ICartObservable>;
        clearCart: () => void;
        refreshCart: () => void;

        isCartEmpty(): boolean;
    }

    export interface IHelper {
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
    }

    

    export interface IACSAPI {
        putFeedback(siteId: string, feedback: any, callback: any): void;
    }
}
