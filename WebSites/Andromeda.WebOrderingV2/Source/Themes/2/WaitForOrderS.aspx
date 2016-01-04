<!DOCTYPE html>
<!--
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
-->
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <title id="title"></title>
    <meta http-equiv="cache-control" content="max-age=0" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="pragma" content="no-cache" />

<%--    <link rel="stylesheet" type="text/css" href="Source/StyleSheets/WebOrdering.css" />
    <link rel="stylesheet" type="text/css" href="Source/StyleSheets/WebOrderingMobile.css" />
    <link rel="stylesheet" type="text/css" href="Source/StyleSheets/WebOrderingPCandTablet.css" />
    <link rel="stylesheet" type="text/css" href="Source/StyleSheets/WebOrderingPC.css" />
    <link rel="stylesheet" type="text/css" href="Source/StyleSheets/WebOrderingTablet.css" />--%>

    <link rel="stylesheet" type="text/css" href="Content/Styles.css" />
    <link rel="stylesheet" type="text/css" href="Custom/Skin.css" />
    <link rel="stylesheet" type="text/css" href="Custom/Custom.css" />

    <script type="text/javascript">

        // Translation strings
        var textStrings = undefined;

        // Settings
        var settings = undefined;

    </script>

    <script src="Content/Scripts.js" type="text/javascript"></script>
    
    <%--<script src="Source/ResourceStrings/en-GB.js" type="text/javascript"></script>
    <script src="Source/ResourceStrings/fr-FR.js" type="text/javascript"></script>

    <script src="Source/Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="Source/Scripts/jreject.js" type="text/javascript"></script>
    <script src="Source/Scripts/amplify.min.js" type="text/javascript"></script>
    <script src="Source/Scripts/jquery.history.js" type="text/javascript"></script>
    <script src="Source/Scripts/knockout-3.0.0.js" type="text/javascript"></script>
    <script src="Source/Scripts/modernizr.min.js" type="text/javascript"></script>
    <script src="Source/Scripts/OpenLayers.js" type="text/javascript"></script>

    <script src="Source/Helpers/Settings.js" type="text/javascript"></script>
    <script src="Source/Helpers/ACSAPI.js" type="text/javascript"></script>

    <script src="Source/Helpers/customerHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/addressHelper.js" type="text/javascript"></script>

    <script src="Source/Helpers/accountHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/cacheHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/cartHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/checkoutHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/checkoutMenuHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/cookieHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/dealHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/dealPopupHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/deliveryZoneHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/guiHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/helper.js" type="text/javascript"></script>
    <script src="Source/Helpers/imageHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/informationHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/mapHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/menuHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/myOrdersHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/openingTimesHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/toppingsPopupHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/postcodeCheckHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/queryStringHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/siteSelectorHelper.js" type="text/javascript"></script>
    <script src="Source/Helpers/tandcHelper.js" type="text/javascript"></script>

    <script src="Source/ViewModels/Checkout/CheckoutViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/Checkout/DataCashPaymentViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/Checkout/MercanetPaymentViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/Checkout/PayPalPaymentViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/Checkout/MercuryPaymentViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/Checkout/PaymentViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/CartViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/DefaultViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/HeaderViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/HomeViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/InformationViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/RegisterFullViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/LoginViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/MenuViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/MobileMenuViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/MyProfileViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/MyOrdersViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/PleaseWaitViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/PasswordResetRequestViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/PasswordResetViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/PostcodeStoreLocatorViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/RepeatOrderViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/SitesViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/ToppingsViewModel.js" type="text/javascript"></script>
    <script src="Source/ViewModels/viewModel.js" type="text/javascript"></script>--%>

    <script src="Custom/Custom.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready
        (
            function ()
            {
                try
                {
                    // Parse the query string
                    queryStringHelper.initialise();

                    // Get the order reference that we previously saved in the cookie
                    var orderRef = cookieHelper.getCookie('or');

                    var paymentDetails =
                        {
                            result: queryStringHelper.queryString["result"],
                            token: queryStringHelper.queryString["token"],
                            payerId: queryStringHelper.queryString["PayerID"]
                        };


                    // Do we have an order reference to work with?
                    if (orderRef == undefined || orderRef.length == 0)
                    {
                        // Return to the parent website
                        viewModel.returnToHostWebsite();
                        return;
                    }

                    // Let the user know we're doing stuff
                    $('#waitForOrderStatus').html(textStrings.wfopPleaseWait);
                    $('#waitForOrderLink').html(textStrings.hReturnToParentWebsiteButton);
                
                    acsapi.postPayPalCallback
                    (
                        orderRef,
                        paymentDetails,
                        function(result)
                        {
                            if (result.hasError)
                            {
                                // Show an error message
                                $('#waitForOrderStatus').html(textStrings.wfopACSFailed + ' ' + result.errorCode);
                            }
                            else
                            {
                                var orderStatusmessage = '';
                                if (result.isProvisional)
                                {
                                    // Show order pending message
                                    orderStatusMessage = textStrings.wfopOrderPending;
                                    orderStatusMessage = orderStatusMessage.replace('{orderNumber}', result.orderNumber);
                                }
                                else
                                {
                                    // Show order accepted messages
                                    orderStatusMessage = textStrings.wfopOrderAccepted;
                                    orderStatusMessage = orderStatusMessage.replace('{orderNumber}', result.orderNumber);
                                }

                                $('#waitForOrderStatus').html(orderStatusMessage);
                            }

                            // Show the return to parent website button
                            $('#waitForOrderButton').css('display', 'block');
                            $('#waitImage').css('display', 'none');

                            // Clear out the order reference
                            cookieHelper.setCookie('or', '');
                        },
                        function (result)
                        {
                            // Failure
                            $('#waitForOrderStatus').html(textStrings.wfopACSFailed + ' ' + result.errorCode);

                            // Show the return to parent website button
                            $('#waitForOrderButton').css('display', 'block');
                            $('#waitImage').css('display', 'none');

                            // Clear out the order reference
                            cookieHelper.setCookie('or', '');
                        }
                    );
                }
                catch (exception)
                {
                    // There was a problem processing the order
                    $('#waitForOrderStatus').html(textStrings.wfopServerFailed);

                    // Show the return to parent website button
                    $('#waitImage').css('display', 'none');
                    $('#waitForOrderButton').css('display', 'block');

                    // Clear out the order reference
                    cookieHelper.setCookie('or', '');

                    console.error(exception);
                }
            }
        );
    </script>

    <script type="text/javascript" id="resourceStrings"></script>

</head>
<body>

    <header>
        <div id="pageHeader">
            <div id="pageHeaderInner">
                <div id="logo"></div>
                <div id="headerRight">

                    <div id="socialMedia"></div>
                    <div id="storeDetailsAddressWrapper"></div>

                </div>

            </div>    
        </div>
    </header>

    <div id="viewContainer" data-bind="template: { name: viewName() }">
        <div id="waitForOrderContent" class="content">
            <div class="sitesBox">
                <h1 id="waitForOrderStatus"></h1>
                <div id="waitImage" style="width:35px; margin: 0 auto;">
                    <div style="margin: 0 auto;"><img src="Content/Images/loader.gif" /></div>
                </div>
                <div id="waitForOrderButton" class="button" style="display:none;">
                    <a id="waitForOrderLink" href="javascript:viewModel.returnToHostWebsite();"></a>
                </div>
            </div>
        </div>
    </div>

</body>
</html>
