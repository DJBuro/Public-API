var siteSelectorHelper =
{
    isSorryVisible: ko.observable(false), // This should be in the viewmodel but can't do this at the moment becuase of binding problems

    errorText: ko.observable(''),
    siteDetails: ko.observableArray(),
    isPostcodeTextboxVisible: ko.observable(false),
    postcode: ko.observable(''),
    checkPostcode: function ()
    {
        "use strict";

        siteSelectorHelper.isBadPostcode(false);
        self.errorText('');

        if (siteSelectorHelper.postcode().length == 0)
        {
            siteSelectorHelper.isBadPostcode(true);
            self.errorText(textStrings.hSiteSelectorNoPostcode);
            return;
        }

        // Show the please wait view
        guiHelper.showPleaseWait
        (
            textStrings.gettingStoreDetails,
            undefined,
            function ()
            {
                // Clear out the store details
                viewModel.siteDetails(undefined);

                // Refresh the site list from the server
                acsapi.getSiteList
                (
                    function ()
                    {
                        // Mark all the offline stores for display purposes
                        helper.markOfflineStores();

                        if (viewModel.sites() === undefined || viewModel.sites().length == 0)
                        {
                            siteSelectorHelper.isBadPostcode(true);
                        }
                        else if (viewModel.sites().length == 1)
                        {
                            // There is a single site that delivers to the customers postcode
                            viewModel.storeSelected(viewModel.sites()[0]);
                        }
                        else
                        {
                            // There are multiple sites that deliver to the customers postcode
                            viewModel.deliverySites.removeAll();
                            viewModel.deliverySites(viewModel.sites());

                            // Show the site list view
                            guiHelper.showView('storeSelectorSitesView');
                        }
                    },
                    function()
                    {
                        viewModel.chooseStore();
                    },
                    siteSelectorHelper.postcode()
                );
            }
        );
    },
    siteSelectorPostcodeKeypress: function (data, event)
    {
        if (event.which == 13)
        {
            var postcode = $('#siteSelectorPostcodeInput').val();
            siteSelectorHelper.postcode(postcode);

            //event.preventDefault();
            setTimeout
            (
                siteSelectorHelper.checkPostcode(),
                0
            );
        }

        return true;
    },
    isBadPostcode: ko.observable(false),
    hideBadPostcode: function ()
    {
        siteSelectorHelper.isBadPostcode(false);
    }
}