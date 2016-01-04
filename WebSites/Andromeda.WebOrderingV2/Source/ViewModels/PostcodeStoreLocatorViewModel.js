/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function PostcodeStoreLocatorViewModel()
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(true);
    self.isShowHomeButtonVisible = ko.observable(true);
    self.isShowMenuButtonVisible = ko.observable(true);
    self.isShowCartButtonVisible = ko.observable(true);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(false);
    self.areHeaderOptionsVisible = ko.observable(true);
    self.isHeaderLoginVisible = ko.observable(true);

    // Mobile mode
    self.title = ko.observable(textStrings.mmPostcodeCheck); // Current section name - shown in the header
    self.titleClass = ko.observable(''); // Class to use to style the section name - used for showing an icon for the section

    self.errorText = ko.observable('');

    if (viewModel.contentViewModel() != undefined && viewModel.contentViewModel().previousViewName != undefined && viewModel.contentViewModel().previousViewName.length > 0)
    {
        self.previousViewName = viewModel.contentViewModel().previousViewName;
        self.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
    }
    else
    {
        self.previousViewName = guiHelper.getCurrentViewName();
        self.previousContentViewModel = viewModel.contentViewModel();
    };

    self.onLogout = function ()
    {
    }

    self.onShown = function ()
    {
        guiHelper.finished();
    }

    self.checkPostcode = function ()
    {
        siteSelectorHelper.isBadPostcode(false);

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
                            if (settings.showCollectFromStoreListIfBadPostcode)
                            {
                                self.showAllSites(true);
                            }
                            else
                            {
                                siteSelectorHelper.isBadPostcode(true);
                            }
                        }
                        else if (viewModel.sites().length == 1)
                        {
                            // There is a single site that delivers to the customers postcode
                            viewModel.storeSelected(viewModel.sites()[0]);
                        }
                        else
                        {
                            self.showStores(false);
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
    };

    self.showAllSites = function (isSorryVisible)
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
                            viewHelper.showError
                            (
                                textStrings.noRestaurants,
                                function ()
                                {
                                    // Show the store selector page
                                    guiHelper.showView('postcodeStoreLocatorBodyView', new PostcodeStoreLocatorViewModel());
                                },
                                undefined
                            );
                        }
                        else
                        {
                            self.showStores(typeof (isSorryVisible) == 'boolean' ? isSorryVisible : false);
                        }
                    },
                    function ()
                    {
                        // Show the store selector page
                        guiHelper.showView('postcodeStoreLocatorBodyView', new PostcodeStoreLocatorViewModel());
                    },
                    undefined
                );
            }
        );
    };

    self.showStores = function (isSorryVisible)
    {
        // Mark all the offline stores for display purposes
        helper.markOfflineStores();

        siteSelectorHelper.isSorryVisible(isSorryVisible);

        // Got all the site details.  Show the site list view
        guiHelper.showView('sitesView', new SitesViewModel());

        siteSelectorHelper.siteDetails.removeAll();

        // Get the site details
        self.getAllSiteDetailsRecursive(0);
    };

    self.getAllSiteDetailsRecursive = function (index)
    {
        var site = viewModel.sites()[index];

        // Get the site details
        acsapi.getSiteDetails
        (
            site.siteId,
            function (data)
            {
                try
                {
                    // Store for later
                    siteSelectorHelper.siteDetails.push({ id: 'map' + index, site: data, mapUrl: self.generateMapLink(data) });

                    // Any more sites?
                    if (index < viewModel.sites().length - 1)
                    {
                        index++;

                        // Get the next site details
                        self.getAllSiteDetailsRecursive(index);
                    }
                    else
                    {
                        mapHelper.refreshMaps();
                    }
                }
                catch (exception)
                {
                    // Got an error
                    viewHelper.showError(textStrings.errProblemGettingSiteDetails, viewModel.chooseStore, exception);
                }
            },
            function ()
            {
                viewModel.storeSelected();
            }
        );
    };

    self.generateMapLink = function (site)
    {
        var latitude = site.address.lat == undefined ? site.address.latitude : site.address.lat;
        var longitude = site.address["long"] == undefined ? site.address.longitude : site.address["long"];

        return 'https://www.google.com/maps/place/Itsu/@' + latitude + ',' + longitude + ',17z';

 //       return 'http://maps.google.com/?q=description+' + site.name + '+%40' + latitude + ',' + longitude;
    };

    self.keyPress = function (data, event)
    {
        // Did the user press enter?
        if (event.which == 13 || event.keyCode == 13)
        {
            self.checkPostcode();
            return false;
        }

        return true;
    }
};






































