var viewHelper =
{  
    showError: function (message, returnCallback, exception)
    {
        if (viewModel.telemetry != undefined)
        {
            viewModel.telemetry.sendTelemetryData
            (
                'Error',
                '',
                '',
                '{ "message"="' + message + '"' + (exception != undefined && exception.message != undefined ? ',"exception"="' + exception.message + '"}' : '}')
            );
        }

        // Set the error details
        viewModel.error.message(message);

        // Return button
        viewModel.error.showReturn(returnCallback != undefined);
        viewModel.error.returnCallback = returnCallback;

        // Show the error view
        guiHelper.showView('errorView', new ErrorViewModel());

        if (exception != undefined && exception.message != undefined)
        {
    //        console.error(exception.message);
        }
    },
    showStoreDetails: function ()
    {
        guiHelper.showView('storeDetailsView', new StoreDetailsViewModel());
        viewModel.resetOrderType();

        // Make the main menu visible
        guiHelper.areHeaderOptionsVisible(true);

        // Let knockout do its thing
        setTimeout
        (
            function ()
            {
                // Initialise the map
                guiHelper.map = mapHelper.initialiseMap('map', viewModel.siteDetails());

                guiHelper.finished();
            },
            0
        );
    },
    showHome: function ()
    {
        guiHelper.showView('homeView', new HomeViewModel());
    },
    showCart: function ()
    {
        $(window).scrollTop(0);
        guiHelper.cartActions(guiHelper.cartActionsMenu);
        guiHelper.isCartLocked(false);
        guiHelper.showView('cartView', new CartViewModel());
    },
    showPostcodeCheck: function ()
    {
        guiHelper.showView('postcodeCheckView', new PostcodeCheckViewModel());
    },
    showLogin: function(viewModel)
    {
        guiHelper.showView('loginView', viewModel === undefined ? new LoginViewModel(false) : viewModel);
    },
    showMyProfile: function (state)
    {
        guiHelper.showView('myProfileView', new MyProfileViewModel(state));
    },
    showMyOrders: function ()
    {
        guiHelper.showView('myOrdersView', new MyOrdersViewModel());
    },
    showMobileMenu: function ()
    {
        guiHelper.showView('mobileMenuView', new MobileMenuViewModel());
    },
    showTermsAndConditions: function ()
    {
        guiHelper.showView('informationView', new InformationViewModel(textStrings.infTermsOfUse), true);
    },
    showPrivacyPolicy: function ()
    {
        guiHelper.showView('informationView', new InformationViewModel(textStrings.infPrivacyPolicy), true);
    },
    showCookiePolicy: function ()
    {
        guiHelper.showView('informationView', new InformationViewModel(textStrings.infCookiePolicy), true);
    },
    showCmsPage: function (name) {
        var pages, page;
        (pages = settings.pages) || (pages = []);

        var pageFilter = pages.filter(function (p) {
            return p.Title === name;
        });
        if (pageFilter.length === 0)
        {
            viewHelper.showError("There are no pages: " + name, function () { viewHelper.showHome(); });
            return;
        }
        page = pageFilter[0];
        var viewModel = new InformationViewModel(page.Content);
        guiHelper.showView('cmsPageView', viewModel, true);
    },
    showPasswordResetRequest: function ()
    {
        guiHelper.showView('passwordResetRequestView', new PasswordResetRequestViewModel());
    },
    showCheckout: function ()
    {
        // Make sure the delivery address popup is not visible
        popupHelper.isBackgroundVisible(false);
        popupHelper.isPopupVisible(false);

        if (checkoutHelper.checkoutViewModel === undefined)
        {
            checkoutHelper.checkoutViewModel = new CheckoutViewModel();
        }

        guiHelper.showView('checkoutView', checkoutHelper.checkoutViewModel);
    },
    showMercuryPayment: function ()
    {
        guiHelper.showView('mercuryPaymentView', new MercuryPaymentViewModel());
    },
    showDatacashPayment: function ()
    {
        guiHelper.showView('dataCashPaymentView', new DataCashPaymentViewModel());
    },
    showMercanetPayment: function ()
    {
        guiHelper.showView('mercanetPaymentView', new MercanetPaymentViewModel());
    },
    showOrderAccepted: function (orderAcceptedViewModel)
    {
        // Show the order accepted view
        guiHelper.showView('orderAcceptedView', orderAcceptedViewModel);
    },
    showPleaseWaitPage: function (viewModel)
    {
        guiHelper.showView('pleaseWaitView', viewModel === undefined ? new PleaseWaitViewModel() : viewModel, undefined);
    },
    showToppingsPopup: function (state)
    {
        toppingsPopupHelper.showPopupUI(state);
    },
    showDeliveryAddressPopup: function (state)
    {
        popupHelper.showPopupUI(state);
    },
    showFeedback: function (state)
    {
        guiHelper.showView('feedbackView', state === undefined ? new AndroWeb.ViewModels.FeedbackViewModel() : state);
    }
}
