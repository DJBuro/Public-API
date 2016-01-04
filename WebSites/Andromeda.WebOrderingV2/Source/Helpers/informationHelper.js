function InformationHelper ()
{
    "use strict";

    var self = this;

    self.termsOfUse = function ()
    {
        viewModel.pageManager.showPage('Info/TermsOfUse', true);
    };

    self.privacyPolicy = function ()
    {
        viewModel.pageManager.showPage('Info/PrivacyPolicy', true);
    };

    self.cookiePolicy = function ()
    {
        viewModel.pageManager.showPage('Info/CookiePolicy', true);
    };
};

var informationHelper = new InformationHelper();