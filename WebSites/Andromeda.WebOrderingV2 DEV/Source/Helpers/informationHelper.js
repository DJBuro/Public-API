/// <reference path="../ViewModels/ViewModel.js" />

var informationHelper = new InformationHelper();

function InformationHelper()
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