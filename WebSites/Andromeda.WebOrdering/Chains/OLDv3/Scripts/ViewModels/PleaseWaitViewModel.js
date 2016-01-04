/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function pleaseWaitViewModel(pleaseWaitMessage, pleaseWaitProgress)
{
    var self = this;

    self.pleaseWaitMessage = ko.observable(pleaseWaitMessage);
    self.pleaseWaitProgress = ko.observable(pleaseWaitProgress); 
}