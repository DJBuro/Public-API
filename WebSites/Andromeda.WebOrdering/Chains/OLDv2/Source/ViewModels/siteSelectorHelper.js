var siteSelectorHelper =
{
    isPostcodeTextboxVisible: ko.observable(false),
    postcode: ko.observable(''),
    checkPostcode: function ()
    {
        siteSelectorHelper.isBadPostcode(false);

        if (siteSelectorHelper.postcode().length == 0)
        {
            siteSelectorHelper.isBadPostcode(true);
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

                            // Show the site list view
                            guiHelper.showView('siteSelectorView');
                        }
                        else if (viewModel.sites().length == 1)
                        {
                            // There is a single site that delivers to the customers postcode
                            viewModel.storeSelected(viewModel.sites()[0]);
                        }
                        else
                        {
                            // There are multiple sites that deliver to the customers postcode
                            viewModel.deliverySites(viewModel.sites());

                            // Show the site list view
                            guiHelper.showView('storeSelectorSitesView');
                        }
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
    },
    showAllSites: function ()
    {
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
                        if (viewModel.sites() === undefined || viewModel.sites().length == 0)
                        {
                            guiHelper.showError
                            (
                                textStrings.noRestaurants,
                                function ()
                                {
                                    // Show the site list view
                                    guiHelper.showView('siteSelectorView');
                                },
                                undefined
                            );
                        }
                        else
                        {
                            // Mark all the offline stores for display purposes
                            helper.markOfflineStores();

                            // Show the site list view
                            guiHelper.showView('sitesView');
                        }
                    },
                    undefined
                );
            }
        );
    }
}