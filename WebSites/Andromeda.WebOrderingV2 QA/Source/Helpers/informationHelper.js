/// <reference path="../ViewModels/ViewModel.js" />
function InformationHelper ()
{
    "use strict";

    var self = this;

    self.termsOfUse = function ()
    {
        viewModel.pageManager.showPage('Info/TermsOfUse', true, undefined, false);
    };

    self.privacyPolicy = function ()
    {
        viewModel.pageManager.showPage('Info/PrivacyPolicy', true, undefined, false);
    };

    self.cookiePolicy = function ()
    {
        viewModel.pageManager.showPage('Info/CookiePolicy', true, undefined, false);
    };
};

var informationHelper = new InformationHelper();