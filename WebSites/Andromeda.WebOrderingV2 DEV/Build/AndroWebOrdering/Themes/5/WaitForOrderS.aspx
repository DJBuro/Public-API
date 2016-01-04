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

                  //  console.error(exception);
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
