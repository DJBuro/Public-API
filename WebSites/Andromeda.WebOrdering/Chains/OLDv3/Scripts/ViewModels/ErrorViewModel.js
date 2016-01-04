/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function errorViewModel(message, returnCallback, retryCallback, exception)
{
    var self = this;

    self.message = message; // The error message to display
    self.returnCallback = returnCallback; // A function to call if the ok button is clicked on the error page
    self.retryCallback = retryCallback; // A function to call if the retry button is clicked on the error page
    self.showReturn = (self.returnCallback != undefined);
    self.showRetry = (self.retryCallback != undefined);
    self.exception = exception;

    self.errorReturn = function ()
    {
        self.returnCallback();
    };

    self.errorRetry = function ()
    {
        self.retryCallback();
    };
}