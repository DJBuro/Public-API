/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

var acsApi =
{
    getSiteList: function (callback)
    {
        // Call the SiteList web service
        var jqxhr = $.ajax(viewModel.serverUrl + "/SiteList?key=x")
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        callback(false, undefined);
                        return;
                    }
                    else
                    {
                        // Let the caller know we're finished
                        callback(true, data);
                        return;
                    }
                }
                catch (exception)
                {
                    // Got an error
                    callback(false, undefined);
                    return;
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                callback(false, undefined);
                return;
            }
        );
    },
    getSiteDetails: function (siteId, callback)
    {
        // Call the SiteDetails web service
        var jqxhr = $.ajax(viewModel.serverUrl + "/SiteDetails/" + siteId + "?key=x")
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        callback(false, undefined);
                        return;
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(true, data);
                        return;
                    }
                }
                catch (exception)
                {
                    // Got an error
                    callback(false, undefined);
                    return;
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                callback(false, undefined);
                return;
            }
        );
    },
    getMenu: function (siteId, callback, progressCallback)
    {
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + "/menu/" + siteId + "?key=x",
                xhr:
                    function ()
                    {
                        var xhr = new window.XMLHttpRequest();

                        xhr.addEventListener
                        (
                            'progress',
                            function (event)
                            {
                                try
                                {
                                    if (event.lengthComputable)
                                    {
                                        var percentageComplete = (event.loaded / event.total) * 100;
                                        var progress = parseInt(percentageComplete) + '%';
                                        progressCallback(progress);
                                    }
                                }
                                catch (exception)
                                {
                                    // Got an error
                                    callback(false, undefined);
                                    return;
                                }
                            },
                            false
                        );

                        return xhr;
                    }
            }
        )
        .always
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (textStatus === 'error')
                    {
                        // Got an error
                        callback(false, undefined);
                        return;
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(true, data);
                        return;
                    }
                }
                catch (exception)
                {
                    // Got an error
                    callback(false, undefined);
                    return;
                }
            }
        )
        .fail
        (
            function ()
            {
                callback(false, undefined);
                return;
            }
        );
    },
    putMercuryPayment: function (siteId, callback)
    {
        // Initialise a Mercury payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/MercuryPayment/' + siteId + '?key=x',
                type: 'PUT',
                contentType: "application/json",
                data: '{ "amount": "' + app.cart.cart().totalPrice() + '", "returnUrl": "' + window.location.href + '/Done.html"}'
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                    }
                    else
                    {
                        // Keep hold of the mercury payment id
                        app.cart.cart().mercuryPaymentId(data.paymentId)

                        // Let the caller know we're finished
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, errorThrown);
            }
        );
    },
    putDataCashPayment: function (siteId, callback)
    {
        // Initialise a DataCash payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/DataCashPayment/' + siteId + '?key=x',
                type: 'PUT',
                contentType: "application/json",
                data: '{ "amount": "' + app.cart.cart().totalPrice() + '", "returnUrl": "' + window.location.href + '/Done.html"}'
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                    }
                    else
                    {
                        // Keep hold of the DataCash payment id
                        app.cart.cart().dataCashPaymentDetails(data)

                        // Let the caller know we're finished
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, errorThrown);
            }
        );
    },
    //putMercanetPayment: function (siteId, callback)
    //{
    //    // Initialise a Mercury payment
    //    var jqxhr = $.ajax
    //    (
    //        {
    //            url: viewModel.serverUrl + '/MercanetPayment/' + siteId + '?key=x',
    //            type: 'PUT',
    //            contentType: "application/json",
    //            data: '{ "amount": "' + app.cart.cart().totalPrice() + '"}'
    //        }
    //    )
    //    .done
    //    (
    //        function (data, textStatus, jqXHR)
    //        {
    //            try
    //            {
    //                if (data == null || textStatus != 'success')
    //                {
    //                    guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
    //                }
    //                else
    //                {
    //                    // Keep hold of the mercury payment id
    //                    app.cart.cart().mercuryPaymentId(data.paymentId)

    //                    // Let the caller know we're finished
    //                    callback(data);
    //                }
    //            }
    //            catch (exception)
    //            {
    //                // Got an error
    //                guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
    //            }
    //        }
    //    )
    //    .fail
    //    (
    //        function (jqXHR, textStatus, errorThrown)
    //        {
    //            // Got an error
    //            guiHelper.showError(text_problemInitialisingPayment, guiHelper.showCheckoutAfterError, errorThrown);
    //        }
    //    );
    //},
    putOrder: function (siteId, order)
    {
        // Call the order web service
        var jqxhr =
        $.ajax
        (
            {
                url: viewModel.serverUrl + "/Order/" + siteId + "?key=x",
                type: 'PUT',
                contentType: "application/json",
                data: JSON.stringify(order)
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    // Was there an error?
                    var errorMessage = apiHelper.checkForError(data, false);
                    if (errorMessage.length > 0)
                    {
                        // Let the user know there was an error
                        guiHelper.showError(errorMessage, undefined, guiHelper.showCheckoutAfterError, errorThrown);
                    }
                    else
                    {
                        // Show the order accepted view
                        app.viewEngine.showView('orderAcceptedView');
                    }
                }
                catch (exception)
                {
                    // Let the user know there was an error
                    guiHelper.showError(guiHelper.defaultInternalErrorMessage, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Get an displayable error message
                var errorMessage = apiHelper.checkForError(jqXHR.responseText, true);

                // Let the user know there was an error
                guiHelper.showError(errorMessage, guiHelper.showCheckoutAfterError, errorThrown);
            }
        );
    },
    checkForError: function (data, alwaysReturnAnError)
    {
        var errorMessage = "";

        // Anything to check?
        if (typeof (data) === 'string')
        {
            // Convert the JSON to an object
            var error = undefined;

            try
            {
                error = JSON.parse(data);
            }
            catch (error) { }

            // Is there an error code?
            if (error != undefined && error.errorCode != undefined)
            {
                // There was an error.  Get an error message that we can display
                switch (error.errorCode)
                {
                    case "-1000":
                    case "-1001":
                    case "-1002":

                        if (error.errorMessage != undefined)
                        {
                            errorMessage = text_cardProcessingError.replace('{ErrorMessage}', error.errorMessage);
                        }
                        else
                        {
                            errorMessage = guiHelper.defaultPaymentErrorMessage + text_appendErrorCode + error.errorCode;
                        }
                        break;

                    case "-1004":

                        errorMessage = text_commsProblem;
                        break;

                    case "1":
                    case "-1": //Internal server error
                        errorMessage = guiHelper.defaultWebErrorMessage + text_appendErrorCode + error.errorCode;
                        break;

                    case "1000": // Missing partnerId parameter in request
                    case "1001": // Missing longitude parameter in request
                    case "1002": // Missing latitude parameter in request
                    case "1003": // Missing maxDistance parameter in request
                    case "1004": // No partner found for the specified partner id
                    case "1005": // No group found for the specified group id
                    case "1006": // Missing siteId parameter in request
                    case "1007": // No site found for the specified siteId
                    case "1008": // There is no menu for the specified site id
                    case "1014": // Missing order parameter in request
                    case "1015": // The format of the order id is not valid
                    case "1016": // No order found for the specified siteId
                    case "1017": // PartnerId is not authorized to access this siteId
                    case "1019": // Missing order resource in request
                    case "1028": // Missing applicationId parameter in request
                    case "2501": // Order Wanted Time in order data is invalid format
                    case "2502": // Order Payments element is missing from the order data

                        // DON'T ROLLBACK AUTH - DON'T TRY ANOTHER SERVER
                        errorMessage = guiHelper.defaultWebErrorMessage + text_appendErrorCode + error.errorCode;
                        break;

                    case "2010": // Unable to deliver order to site
                    case "2020": // Unable to deliver order to site
                    case "2100": // Unable to deliver order to site – unspecified error
                    case "2125": // Order number already exists
                    case "2300": // Order Failed – Store Offline

                        // ROLLBACK AUTH - TRY ANOTHER SERVER
                        errorMessage = text_apiProblem + error.errorCode;
                        break;

                    case "2150": // Unable to deliver order to site - timeout
                    case "2200": // Unable to deliver order to site – Await confirmation

                        // DON'T ROLLBACK AUTH - TRY ANOTHER SERVER
                        errorMessage = text_cantDeliverOrder + error.errorCode;
                        break;

                    case "2101": // Unable to parse XML/JSON order data
                    case "2260": // POS system rejected the order.  Price check failed
                    case "2270": // POS system rejected the order.  Invalid order data

                        // ROLLBACK AUTH - DON'T TRY ANOTHER SERVER
                        errorMessage = text_orderRejected + error.errorCode;
                        break;

                    default:
                        errorMessage = guiHelper.defaultWebErrorMessage + text_appendErrorCode + error.errorCode;
                };
            }
            else
            {
                errorMessage = guiHelper.defaultWebErrorMessage;
            }
        }

        if (alwaysReturnAnError && errorMessage.length == 0)
        {
            errorMessage = guiHelper.defaultWebErrorMessage + text_appendUnknownErrorCode;
        }

        return errorMessage;
    },
    getDeliveryZones: function (siteId, callback)
    {
        // Call the DeliveryZones web service
        var jqxhr = $.ajax(viewModel.serverUrl + "/DeliveryZones/" + siteId + "?key=x")
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        callback(false, undefined);
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(true, data);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    callback(false, undefined);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                callback(false, undefined);
            }
        );
    }
};