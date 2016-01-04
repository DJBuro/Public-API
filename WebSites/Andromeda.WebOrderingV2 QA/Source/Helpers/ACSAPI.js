/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

var acsapi =
{
    getSites: function (callback, errorCallback, deliveryZone)
    {
        var promise = $.ajax
        (
            {
                url: viewModel.serverUrl + "/SiteList?deliveryZone=" + (deliveryZone == undefined ? '' : deliveryZone) + "&key=x",
                type: 'GET',
                timeout: 120000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false
            }
        );

        promise.done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data == null || textStatus != 'success')
                    {
                        viewHelper.showError(textStrings.errProblemGettingSites, errorCallback, exception);
                    }
                    else
                    {
                        // Start fresh
                        //viewModel.sites.removeAll();

                        // Keep hold of the list of sites we just got from the server
                        //viewModel.sites(data);

                        // Let the caller know we're finished
                        if (callback) { 
                            callback(data);
                        }
                    }
                }
                catch (exception)
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemGettingSites, errorCallback, exception);
                }
            }
        );

        promise.fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                viewHelper.showError(textStrings.errProblemGettingSites, errorCallback, errorThrown);
            }
        );

        return promise;
    },
    getSiteList: function (callback, errorCallback, deliveryZone)
    {
        var promise = this.getSites(callback, errorCallback, deliveryZone);

        // Call the SiteList web service
        promise.done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    if (data === null || textStatus !== 'success') {
                        viewHelper.showError(textStrings.errProblemGettingSites, errorCallback, exception);
                    }
                    else
                    {
                        // Start fresh
                        viewModel.sites.removeAll();

                        if (data && data.length > 0)
                        {
                            data = data.sort(function (a, b) {
                                var aName = a.name;
                                var bName = b.name;

                                if (aName < bName) { return -1; }
                                if (aName > bName) { return 1; }
                                if (aName === bName) { return 0; }
                            });
                        }

                        // Keep hold of the list of sites we just got from the server
                        viewModel.sites(data);

                        // Let the caller know we're finished
                        callback(data);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemGettingSites, errorCallback, exception);
                }
            }
        );
    },
    getSite: function (siteId, gotMenuVersion, callback, statusCheck, errorCallback)
    {
        // Show the site list drop down combo
        viewModel.sitesMode('pleaseWait');

        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + "/sites/" + siteId + "?key=x&gotMenuVersion=" + gotMenuVersion + '&statusCheck=false', // + statusCheck(),
                type: 'GET',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
                xhr:
                    function ()
                    {
                        var xhr = new window.XMLHttpRequest();

                        if (xhr.addEventListener != undefined)
                        {
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
                                        viewHelper.showError(textStrings.errProblemGettingSiteDetails, errorCallback, exception);
                                    }
                                },
                                false
                            );
                        }

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
                        viewHelper.showError(textStrings.errProblemGettingSiteDetails, errorCallback, undefined);
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
                    viewHelper.showError(textStrings.errProblemGettingSiteDetails, errorCallback, exception);
                }
            }
        )
        .fail
        (
            function ()
            {
                viewHelper.showError(textStrings.errProblemGettingSiteDetails, errorCallback, undefined);
            }
        );
    },
    getSiteDetails: function (siteId, callback, errorCallback)
    {
        // Call the SiteDetails web service
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + "/SiteDetails/" + siteId + "?key=x",
                type: 'GET',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false
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
                        viewHelper.showError(textStrings.errProblemGettingSiteDetails, errorCallback, undefined);
                    }
                    else
                    {
                        // Keep hold of the site details we just got from the server
                        viewModel.siteDetails(data);

                        // Finished - let the caller know
                        callback(data);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemGettingSiteDetails, errorCallback, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                viewHelper.showError(textStrings.errProblemGettingSiteDetails, errorCallback, errorThrown);
            }
        );
    },
    putMercuryPayment: function (siteId, callback, errorCallback)
    {
        // Initialise a Mercury payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/MercuryPayment/' + siteId + '?key=x',
                type: 'PUT',
                contentType: "application/json",
                timeout: 60000,  // 60 seconds
                accepts: "application/json",
                dataType: 'json',
                cache: false,
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
                        viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, exception);
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
                    viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, errorThrown);
            }
        );
    },
    putDataCashPayment: function (siteId, order, callback, errorCallback)
    {
        order.paymentData = order.paymentData == undefined ? {} : order.paymentData;
        order.paymentData.browserUserAgent = encodeURIComponent(navigator.userAgent);
        order.paymentData.returnUrl = window.location.origin + window.location.pathname.replace('index.html', 'Done.html');

        // Initialise a DataCash payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/DataCashPayment/' + siteId + '?key=x',
                type: 'PUT',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                data: JSON.stringify(order),
                cache: false
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
                        viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, exception);
                    }
                    else
                    {
                        // Was there an error?
                        var result = acsapi.checkForOrderError(data);

                        // Tell the caller
                        callback(result);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, errorThrown);
            }
        );
    },
    putMercanetPayment: function (siteId, order, callback, errorCallback)
    {
        // Initialise a Mercanet payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/MercanetPayment/' + siteId + '?key=x',
                type: 'PUT',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
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
                        viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, exception);
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
                    viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                viewHelper.showError(textStrings.errProblemInitialisingPayment, errorCallback, errorThrown);
            }
        );
    },
    putPayPalPayment: function (siteId, paymentDetails, callback, errorCallback)
    {
        // Initialise a PayPal payment
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/PayPalPayment/' + siteId + '?key=x',
                type: 'PUT',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
                data: JSON.stringify(paymentDetails)
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
                        errorCallback();
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
                    errorCallback();
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Got an error
                errorCallback();
            }
        );
    },
    postPayPalCallback: function (orderRef, paymentDetails, callback, errorCallback)
    {
        // PayPal payment callback
        var jqxhr = $.ajax
        (
            {
                url: viewModel.serverUrl + '/PayPalCallback/' + orderRef + '?key=x',
                type: 'POST',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
                data: '{ "result":"' + paymentDetails.result + '", "token":"' + paymentDetails.token + '", "payerId":"' + paymentDetails.payerId + '" }'
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
                        errorCallback();
                    }
                    else
                    {
                        // Was there an error?
                        var result = acsapi.checkForOrderError(data);

                        // Let the caller know we're finished
                        callback(result);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    errorCallback(result);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Get an displayable error message
                var result = { hasError: false, errorMessage: undefined, errorCode: undefined, orderNumber: undefined, isProvisional: false };
                acsapi.checkForError(jqXHR.responseText, true, result);

                // Tell the caller
                errorCallback(result);
            }
        );
    },
    checkForOrderError: function(data)
    {
        var result = { hasError: false, errorMessage: undefined, errorCode: undefined, orderNumber: undefined, isProvisional: false, data: data, paymentReset: false };

        acsapi.checkForError(data, false, result);

        if (!result.hasError)
        {
            result.isProvisional = data.isProvisional;
            result.orderNumber = data.storeOrderId;
        }

        result.paymentReset = data.paymentReset;

        return result;
    },
    putOrder: function (siteId, order, callback, errorCallback)
    {
        // Call the order web service
        var jqxhr =
        $.ajax
        (
            {
                url: viewModel.serverUrl + "/Order/" + siteId + "?key=x",
                type: 'PUT',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType : 'json',
                data: JSON.stringify(order),
                cache: false
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    // Was there an error?
                    var result = acsapi.checkForOrderError(data);

                    // Tell the caller
                    callback(result);
                }
                catch (exception)
                {
                    // Let the user know there was an error
                    viewHelper.showError(textStrings.errDefaultInternalErrorMessage, errorCallback, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Get an displayable error message
                var result = { hasError: false, errorMessage: undefined, errorCode: undefined, orderNumber: undefined, isProvisional: false, paymentReset: false };
                acsapi.checkForError(jqXHR.responseText, true, result);

                // Tell the caller
                errorCallback(result);
            }
        );
    },
    checkOrderVouchers: function (siteId, order, callback, errorCallback)
    {
        // Call the order web service
        var jqxhr =
        $.ajax
        (
            {
                url: viewModel.serverUrl + "/OrderVouchers/" + siteId + "?key=x",
                type: 'PUT',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                data: JSON.stringify(order),
                cache: false
            }
        )
        .done
        (
            function (data, textStatus, jqXHR)
            {
                try
                {
                    // Was there an error?
                    var result = { hasError: false, errorMessage: undefined, errorCode: undefined, orderNumber: undefined, isProvisional: false, data: data };
                    acsapi.checkForError(data, false, result);

                    // Tell the caller
                    callback(result);
                }
                catch (exception)
                {
                    // Let the user know there was an error
                    viewHelper.showError(textStrings.errDefaultInternalErrorMessage, errorCallback, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                // Get an displayable error message
                var result = { hasError: false, errorMessage: undefined, errorCode: undefined, orderNumber: undefined, isProvisional: false };
                acsapi.checkForError(jqXHR.responseText, true, result);

                // Tell the caller
                errorCallback(result);
            }
        );
    },
    checkForError: function (data, alwaysReturnAnError, result)
    {
        // Anything to check?
        if (typeof (data) === 'string')
        {
            try
            {
                data = JSON.parse(data);
            }
            catch (error) { }
        }

        // Is there an error code?
        if (data != undefined && data.errorCode != undefined && data.errorCode.length > 0)
        {
            // Do we need to reset the datacash payment?
            if (data.paymentReset !== undefined && data.paymentReset)
            {
                cartHelper.cart().dataCashPaymentDetails().reference = '';
            }

            result.errorCode = data.errorCode;

            // There was an error.  Get an error message that we can display
            switch (data.errorCode)
            {
                case "-1001":
                case "-1002":
                case "-1003":

                    if (data.errorMessage != undefined)
                    {
                        result.errorMessage = textStrings.cardProcessingError.replace('{ErrorMessage}', data.errorMessage);
                    }
                    else
                    {
                        result.errorMessage = textStrings.errDefaultPaymentErrorMessage + ' ' + textStrings.errAppendErrorCode + data.errorCode;
                    }
                    result.hasError = true;
                    break;

                case "-1004":

                    result.errorMessage = textStrings.errCommsProblem;
                    result.hasError = true;
                    break;

                case "-1008":
                    result.errorMessage = textStrings.errSendToStoreFailedPaymentCancelled;
                    result.hasError = true;
                    break;
                case "-1009":
                    result.errorMessage = textStrings.errSendToStoreFailedPaymentCancelFailed;
                    result.hasError = true;
                    break;

                case "1":
                case "-1": 
                case "-1000": // Internal server error
                    result.errorMessage = textStrings.errDefaultWebErrorMessage + textStrings.errAppendErrorCode + data.errorCode;
                    result.hasError = true;
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
                case "2512": // Problem sending the order to a gprs printer

                    // DON'T ROLLBACK AUTH - DON'T TRY ANOTHER SERVER
                    result.errorMessage = textStrings.errDefaultWebErrorMessage +textStrings.errAppendErrorCode + data.errorCode;
                    result.hasError = true;
                    break;

                case "2010": // Unable to deliver order to site
                case "2020": // Unable to deliver order to site
                case "2100": // Unable to deliver order to site – unspecified error
                case "2125": // Order number already exists
                case "2300": // Order Failed – Store Offline

                    // ROLLBACK AUTH - TRY ANOTHER SERVER
                    result.errorMessage = textStrings.apiProblem + data.errorCode;
                    result.hasError = true;
                    break;

                case "2150": // Unable to deliver order to site - timeout
                case "2200": // Unable to deliver order to site – Await confirmation

                    // DON'T ROLLBACK AUTH - TRY ANOTHER SERVER
                    result.errorMessage = textStrings.cantDeliverOrder + data.errorCode;
                    result.hasError = true;
                    break;

                case "2101": // Unable to parse XML/JSON order data
                case "2260": // POS system rejected the order.  Price check failed
                case "2270": // POS system rejected the order.  Invalid order data

                    // ROLLBACK AUTH - DON'T TRY ANOTHER SERVER
                    result.errorMessage = textStrings.orderRejected + data.errorCode;
                    result.hasError = true;
                    break;

                default:
                    result.errorMessage = textStrings.errDefaultWebErrorMessage +textStrings.errAppendErrorCode + data.errorCode;
                    result.hasError = true;
            };
        }

        // Do we need to reset the payment details?
        result.paymentReset = (data.errorCode !== 0 && data.errorCode !== 1 && data.errorCode != 2150);  // Reset payment details for all errors except timeout

        if (alwaysReturnAnError && (result.errorMessage == undefined || result.errorMessage.length == 0))
        {
            result.hasError = true;
            result.errorMessage = textStrings.errDefaultWebErrorMessage + textStrings.appendUnknownErrorCode;
        }
    },
    getCustomer: function (siteId, username, password, callback, errorCallback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "GET",
                url: viewModel.serverUrl + "/Customers/" + username + "?key=x&siteId=" + (siteId === undefined ? '' : siteId),
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
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
                        viewHelper.showError(textStrings.errProblemGettingAccountDetails, errorCallback, undefined);
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
                    viewHelper.showError(textStrings.errProblemGettingAccountDetails, errorCallback, exception);
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
                    viewHelper.showError(textStrings.errProblemGettingAccountDetails, errorCallback, errorThrown);
                }
            }
        );
    },
    putCustomer: function (siteId, username, password, customer, callback, errorCallback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "PUT",
                url: viewModel.serverUrl + "/Customers/" + encodeURIComponent(username) + "?key=x&siteId=" + (siteId === undefined ? '' : siteId),
                beforeSend: function (xhr)
                {
                    xhr.setRequestHeader('Authorization', 'Basic ' + acsapi.encode64(password));
                },
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
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
                        viewHelper.showError(textStrings.errProblemCreatingAccountDetails, errorCallback, undefined);
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
                    viewHelper.showError(textStrings.errProblemCreatingAccountDetails, errorCallback, exception);
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
                    viewHelper.showError(textStrings.errProblemCreatingAccountDetails, errorCallback, errorThrown);
                }
            }
        );
    },
    postCustomer: function (username, existingPassword, newPassword, customer, callback, errorCallback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "POST",
                url: viewModel.serverUrl + '/Customers/' + encodeURIComponent(username) + '?key=x' +
                    (newPassword != undefined ? '&newPassword=' + encodeURIComponent(newPassword) : ''),
                beforeSend: function (xhr)
                {
                    if (existingPassword != undefined && existingPassword.length > 0)
                    {
                        xhr.setRequestHeader('Authorization', 'Basic ' + acsapi.encode64(existingPassword));
                    }
                },
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
                data: customer != undefined ? JSON.stringify(customer) : ''
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
                        viewHelper.showError(textStrings.errProblemUpdatingAccountDetails, viewModel.storeSelected, undefined);
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
                    viewHelper.showError(textStrings.errProblemUpdatingAccountDetails, errorCallback, exception);
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

                    errorCallback(response);
                }
                else
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemUpdatingAccountDetails, errorCallback, errorThrown);
                }
            }
        );
    },
    putPasswordResetRequest: function (username, callback, errorCallback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "PUT",
                //Customers/{username}/passwordresetrequest?key={key}

                url: viewModel.serverUrl + '/Customers/' + encodeURIComponent(username) + '/passwordresetrequest?key=x',
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
                data: undefined
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
                        viewHelper.showError(textStrings.errProblemResettingPassword, viewModel.storeSelected, undefined);
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
                    viewHelper.showError(textStrings.errProblemResettingPassword, errorCallback, exception);
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

                    errorCallback(response);
                }
                else
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemResettingPassword, errorCallback, errorThrown);
                }
            }
        );
    },
    postPasswordResetRequest: function (username, passwordResetToken, newPassword, callback, errorCallback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "POST",
                // Customers/{username}/passwordresetrequest?key={key}&passwordResetToken={passwordResetToken}

                url: viewModel.serverUrl + '/Customers/' + encodeURIComponent(username) + '/passwordresetrequest?key=x&passwordResetToken=' + passwordResetToken,
                beforeSend: function (xhr)
                {
                    xhr.setRequestHeader('Authorization', 'Basic ' + acsapi.encode64(newPassword));
                },
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
                data: undefined
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
                        viewHelper.showError(textStrings.errProblemRequestingAPasswordReset, viewModel.storeSelected, undefined);
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
                    viewHelper.showError(textStrings.errProblemRequestingAPasswordReset, errorCallback, exception);
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

                    errorCallback(response);
                }
                else
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemRequestingAPasswordReset, errorCallback, errorThrown);
                }
            }
        );
    },
    getCustomerOrders: function (username, password, callback, errorCallback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "GET",
                url: viewModel.serverUrl + "/Customers/" + username + "/Orders?key=x",
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
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
                        viewHelper.showError(textStrings.errProblemGettingOrders, errorCallback, undefined);
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
                    viewHelper.showError(textStrings.errProblemGettingOrders, errorCallback, exception);
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

                    var message = response != undefined && response.message != undefined ? response.message : undefined;

                    callback(response, message);
                }
                else
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemGettingOrders, errorCallback, errorThrown);
                }
            }
        );
    },
    getCustomerOrder: function (orderId, username, password, callback, errorCallback)
    {
        // Call the Customers web service
        var jqxhr = $.ajax
        (
            {
                type: "GET",
                url: viewModel.serverUrl + "/Customers/" + username + "/Orders/" + orderId + "?key=x",
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
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
                        viewHelper.showError(textStrings.errProblemGettingOrder, errorCallback, undefined);
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
                    viewHelper.showError(textStrings.errProblemGettingOrder, errorCallback, exception);
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

                    var message = response != undefined && response.message != undefined ? response.message : undefined;

                    callback(response, message);
                }
                else
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemGettingOrder, errorCallback, errorThrown);
                }
            }
        );
    },
    putFeedback: function (siteId, feedback, callback)
    {
        // Call the Feedback web service
        var jqxhr = $.ajax
        (
            {
                type: "PUT",
                url: viewModel.serverUrl + "/Feedback" + (siteId.length === 0 ? "" : ("/" + siteId)) + "?key=x",
                timeout: 60000,  // 60 seconds
                contentType: "application/json",
                accepts: "application/json",
                dataType: 'json',
                cache: false,
                data: JSON.stringify(feedback)
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
                        viewHelper.showError(textStrings.errProblemSubmittingFeedback, errorCallback, undefined);
                    }
                    else
                    {
                        // Finished - let the caller know
                        callback(true);
                    }
                }
                catch (exception)
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemSubmittingFeedback, errorCallback, exception);
                }
            }
        )
        .fail
        (
            function (jqXHR, textStatus, errorThrown)
            {
                callback(false);
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