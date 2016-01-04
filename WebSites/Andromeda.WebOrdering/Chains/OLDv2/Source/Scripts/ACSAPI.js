/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

var acsapi =
{
    getSiteList: function (callback, deliveryZone)
    {
        // Call the SiteList web service
        var jqxhr = $.ajax(viewModel.serverUrl + "/SiteList?deliveryZone=" + (deliveryZone == undefined ? '' : deliveryZone) + "&key=x")
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.chooseStore, exception);
                    }
                    else
                    {
                        // Keep hold of the list of sites we just got from the server
                        viewModel.sites(data);

                        // Let the caller know we're finished
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.chooseStore, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.chooseStore, errorThrown);
            }
        );
    },
    getSite: function (siteId, gotMenuVersion, callback)
    {
        // Show the site list drop down combo
        viewModel.sitesMode('pleaseWait');

        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + "/sites/" + siteId + "?key=x&gotMenuVersion=" + gotMenuVersion,
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
                                        viewModel.pleaseWaitProgress(progress);
                                        setTimeout(function () { }, 0);
                                    }
                                }
                                catch (exception)
                                {
                                    // Got an error
                                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
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
                        guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(data);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function ()
            {
                guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
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
                        guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Keep hold of the site details we just got from the server
                        viewModel.siteDetails(data);

                        // Finished - let the caller know
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, errorThrown);
            }
        );
    },
    getMenu: function (siteId, callback)
    {
        // Show the site list drop down combo
        viewModel.sitesMode('pleaseWait');

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
                                        viewModel.pleaseWaitProgress(progress);
                                        setTimeout(function () { }, 0);
                                    }
                                }
                                catch (exception)
                                {
                                    // Got an error
                                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
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
                        guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(data);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function ()
            {
                guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
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
                data: '{ "amount": "' + cartHelper.cart().totalPrice() + '", "returnUrl": "' + window.location.href + '/Done.html"}'
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
                        guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                    }
                    else
                    {
                        // Keep hold of the mercury payment id
                        cartHelper.cart().mercuryPaymentId(data.paymentId)

                        // Let the caller know we're finished
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, errorThrown);
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
                data: '{ "amount": "' + cartHelper.cart().totalPrice() + '", "returnUrl": "' + window.location.href + '/Done.html"}'
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
                        guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                    }
                    else
                    {
                        // Keep hold of the DataCash payment id
                        cartHelper.cart().dataCashPaymentDetails(data)

                        // Let the caller know we're finished
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, errorThrown);
            }
        );
    },
    putMercanetPayment: function (siteId, order, callback)
    {
        // Initialise a Mercanet payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/MercanetPayment/' + siteId + '?key=x',
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
                    if (data == null || textStatus != 'success')
                    {
                        guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                    }
                    else
                    {
                        // Let the caller know we're finished
                        callback(data);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(textStrings.problemInitialisingPayment, guiHelper.showCheckoutAfterError, errorThrown);
            }
        );
    },
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
                    var errorMessage = acsapi.checkForError(data, false);
                    if (errorMessage.length > 0)
                    {
                        // Let the user know there was an error
                        guiHelper.showError(errorMessage, undefined, guiHelper.showCheckoutAfterError, errorThrown);
                    }
                    else
                    {
                        // Make sure we switch the mobile menu back for the next page
                        checkoutMenuHelper.showMenu('menu', true);

                        // Hide the mobile checkout menu
                        checkoutMenuHelper.hideMenu();

                        // Show the order accepted view
                        guiHelper.showView('orderAcceptedView');
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
                var errorMessage = acsapi.checkForError(jqXHR.responseText, true);

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
                            errorMessage = textStrings.cardProcessingError.replace('{ErrorMessage}', error.errorMessage);
                        }
                        else
                        {
                            errorMessage = guiHelper.defaultPaymentErrorMessage + textStrings.appendErrorCode + error.errorCode;
                        }
                        break;

                    case "-1004":

                        errorMessage = textStrings.commsProblem;
                        break;

                    case "1":
                    case "-1": //Internal server error
                        errorMessage = guiHelper.defaultWebErrorMessage + textStrings.appendErrorCode + error.errorCode;
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
                        errorMessage = guiHelper.defaultWebErrorMessage + textStrings.appendErrorCode + error.errorCode;
                        break;

                    case "2010": // Unable to deliver order to site
                    case "2020": // Unable to deliver order to site
                    case "2100": // Unable to deliver order to site – unspecified error
                    case "2125": // Order number already exists
                    case "2300": // Order Failed – Store Offline

                        // ROLLBACK AUTH - TRY ANOTHER SERVER
                        errorMessage = textStrings.apiProblem + error.errorCode;
                        break;

                    case "2150": // Unable to deliver order to site - timeout
                    case "2200": // Unable to deliver order to site – Await confirmation

                        // DON'T ROLLBACK AUTH - TRY ANOTHER SERVER
                        errorMessage = textStrings.cantDeliverOrder + error.errorCode;
                        break;

                    case "2101": // Unable to parse XML/JSON order data
                    case "2260": // POS system rejected the order.  Price check failed
                    case "2270": // POS system rejected the order.  Invalid order data

                        // ROLLBACK AUTH - DON'T TRY ANOTHER SERVER
                        errorMessage = textStrings.orderRejected + error.errorCode;
                        break;

                    default:
                        errorMessage = guiHelper.defaultWebErrorMessage + textStrings.appendErrorCode + error.errorCode;
                };
            }
            else
            {
                errorMessage = guiHelper.defaultWebErrorMessage;
            }
        }

        if (alwaysReturnAnError && errorMessage.length == 0)
        {
            errorMessage = guiHelper.defaultWebErrorMessage + textStrings.appendUnknownErrorCode;
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
                        guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Keep hold of the delivery zones we just got from the server
                        if (data != undefined)
                        {
                            for (var i = 0; i < data.length; i++)
                            {
                                data[i] = data[i].replace(/\s+/g, '');
                            }
                        }

                        // Keep hold of the delivery zones we just got from the server
                        deliveryZoneHelper.deliveryZones(data);

                        // Finished - let the caller know
                        callback();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, errorThrown);
            }
        );
    },
    getCustomer: function (username, password, callback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "GET",
                url: viewModel.serverUrl + "/Customers/" + username + "?key=x",
                beforeSend: function (xhr)
                {
                    xhr.setRequestHeader('Authorization', 'Basic ' + acsapi.encode64(password));
                }
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
                        guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(data);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                if (jqXHR != undefined &&
                    jqXHR.responseText != undefined)
                {
                    var response = undefined;

                    try
                    {
                        response = JSON.parse(jqXHR.responseText);
                    } catch (e) { }

                    callback(response);
                }
                else
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, errorThrown);
                }
            }
        );
    },
    putCustomer: function (username, password, customer, callback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "PUT",
                url: viewModel.serverUrl + "/Customers/" + username + "?key=x",
                beforeSend: function (xhr)
                {
                    xhr.setRequestHeader('Authorization', 'Basic ' + acsapi.encode64(password));
                },
                contentType: "application/json",
                data: JSON.stringify(customer)
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (textStatus != 'success')
                    {
                        guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(undefined);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                if (jqXHR != undefined &&
                    jqXHR.responseText != undefined)
                {
                    var response = undefined;

                    if (jqXHR.responseText.length > 0)
                    {
                        var response = undefined;

                        try
                        {
                            response = JSON.parse(jqXHR.responseText);
                        } catch (e) { }
                    }

                    callback(response);
                }
                else
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, errorThrown);
                }
            }
        );
    },
    postCustomer: function (username, password, customer, callback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "POST",
                url: viewModel.serverUrl + "/Customers/" + username + "?key=x",
                beforeSend: function (xhr)
                {
                    xhr.setRequestHeader('Authorization', 'Basic ' + acsapi.encode64(password));
                },
                contentType: "application/json",
                data: JSON.stringify(customer)
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (textStatus != 'success')
                    {
                        guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, undefined);
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(undefined);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                if (jqXHR != undefined &&
                    jqXHR.responseText != undefined)
                {
                    var response = undefined;

                    if (jqXHR.responseText.length > 0)
                    {
                        var response = undefined;

                        try
                        {
                            response = JSON.parse(jqXHR.responseText);
                        } catch (e) { }
                    }

                    callback(response);
                }
                else
                {
                    // Got an error
                    guiHelper.showError(textStrings.problemGettingSiteDetails, viewModel.storeSelected, errorThrown);
                }
            }
        );
    },
    encode64: function (data)
    {
        // http://kevin.vanzonneveld.net
        // +   original by: Tyler Akins (http://rumkin.com)
        // +   improved by: Bayron Guevara
        // +   improved by: Thunder.m
        // +   improved by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +   bugfixed by: Pellentesque Malesuada
        // +   improved by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +   improved by: Rafał Kukawski (http://kukawski.pl)
        // *     example 1: base64_encode('Kevin van Zonneveld');
        // *     returns 1: 'S2V2aW4gdmFuIFpvbm5ldmVsZA=='
        // mozilla has this native
        // - but breaks in 2.0.0.12!
        //if (typeof this.window['btoa'] === 'function') {
        //    return btoa(data);
        //}
        var b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        var o1, o2, o3, h1, h2, h3, h4, bits, i = 0,
          ac = 0,
          enc = "",
          tmp_arr = [];

        if (!data)
        {
            return data;
        }

        do
        { // pack three octets into four hexets
            o1 = data.charCodeAt(i++);
            o2 = data.charCodeAt(i++);
            o3 = data.charCodeAt(i++);

            bits = o1 << 16 | o2 << 8 | o3;

            h1 = bits >> 18 & 0x3f;
            h2 = bits >> 12 & 0x3f;
            h3 = bits >> 6 & 0x3f;
            h4 = bits & 0x3f;

            // use hexets to index into b64, and append result to encoded string
            tmp_arr[ac++] = b64.charAt(h1) + b64.charAt(h2) + b64.charAt(h3) + b64.charAt(h4);
        } while (i < data.length);

        enc = tmp_arr.join('');

        var r = data.length % 3;

        return (r ? enc.slice(0, r - 3) : enc) + '==='.slice(r || 3);
    }
};