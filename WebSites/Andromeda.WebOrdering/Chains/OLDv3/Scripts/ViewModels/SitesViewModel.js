/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function sitesViewModel()
{
    var self = this;
    self.isMobileMenuVisible = false;

    self.sites = undefined;
    self.showPleaseWait = ko.observable(true);
    self.pleaseWaitMessage = ko.observable(text_gettingStoreDetails);
    self.pleaseWaitProgress = ko.observable('');

    self.onAfterViewShown = function ()
    {
        // Get a list of sites
        self.getSiteList();
    }

    // Gets a list of stores from the server. If there is more than one store then prompts the user to select one.
    // Then gets the store details from the server and if required downloads the menu
    self.getSiteList = function ()
    {
        try
        {
            // Refresh the site list from the server
            acsApi.getSiteList
            (
                function (success, sites)
                {
                    if (!success)
                    {
                        app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, undefined);
                        return;
                    }
                    
                    // Keep the sites for later
                    app.sites(sites);

                    // Mark all the offline stores for display purposes
                    self.markOfflineStores();

                    if (app.sites() === undefined || app.sites().length === 0)
                    {
                        // No stores!!
                        app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, undefined);
                        return;
                    }
                    else if (app.sites().length == 1)
                    {
                        // Auto select the only site
                        self.storeSelected(app.sites()[0]);
                    }
                    else
                    {
                        // Hide the please wait
                        self.showPleaseWait(false);
                    }
                }
            );
        }
        catch (exception)
        {
            // Got an error
            app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, exception);
        }
    }
    self.storeSelected = function (store)
    {
        // Gets the site details (address etc...) and then downloads the menu
        try
        {
            // Create a new site
            app.site(new site());

            // Remember which site the user selected
            app.site().selectedSite(store);

            // Show the please wait
            self.showPleaseWait(true);

            // Get the site details
            setTimeout(self.getSiteDetails, 0);
        }
        catch (exception)
        {
            // Got an error
            app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, exception);
        }
    };
    self.getSiteDetails = function()
    {
        try
        {
            // Is the store taking orders?
            //if (location.host == 'localhost')
            //{
                app.site().isTakingOrders(true);
            //}
            //else
            //{
            //    app.site().isTakingOrders(app.site().selectedSite().isOpen);
            //}

            // Get the site details
            acsApi.getSiteDetails
            (
                app.site().selectedSite().siteId,
                function (success, data)
                {
                    if (!success)
                    {
                        app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, undefined);
                        return;
                    }

                    // Keep hold of the site details we just got from the server
                    app.site().siteDetails(data);

                    // Get the delivery zones
                    acsApi.getDeliveryZones
                    (
                        app.site().selectedSite().siteId,
                        function (success, data)
                        {
                            if (!success)
                            {
                                app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, undefined);
                                return;
                            }

                            // Keep hold of the delivery zones we just got from the server
                            app.site().deliveryZones(data);

                            self.pleaseWaitMessage(text_loadingMenu);

                            // Attempt to load menu from the cache
                            setTimeout(self.getSiteFromCache, 0);
                        }
                    );
                }
            );
        }
        catch (exception)
        {
            // Got an error
            app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, exception);
        }
    };
    self.getSiteFromCache = function()
    {
        try
        {
            // Get the menu from the cache
            var menu = app.cache.loadCachedMenuForSiteId(app.site().selectedSite().siteId)

            if (menu != undefined)
            {
                // Rendering the menu can take a couple of seconds on a mobile
                self.pleaseWaitMessage(text_loadingMenu);

                // Ensure the UI is updated
                setTimeout(function () { self.renderSiteMenu(menu) }, 0);
            }
            else
            {
                // Show the please wait view
                self.pleaseWaitMessage(text_downloadingStoreMenu);
                self.pleaseWaitProgress('0%');

                // Ensure the UI is updated
                setTimeout(self.getSiteMenu, 0);
            }
        }
        catch (exception)
        {
            // Got an error
            app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, exception);
        }
    };
    self.getSiteMenu = function ()
    {
        // Get the site menu from the server
        acsApi.getMenu
        (
            app.site().selectedSite().siteId,
            function (success, menu)
            {
                try
                {
                    if (!success)
                    {
                        app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, undefined);
                        return;
                    }

                    // Rendering the menu can take a couple of seconds on a mobile
                    self.pleaseWaitMessage(text_savingMenuToDevice);
                    self.pleaseWaitProgress('');

                    // Allow JavaScript to process events (kind of like doEvents)
                    setTimeout
                    (
                        function ()
                        {
                            try
                            {
                                // Cache the menu locally.  If this fails just carry on anyway
                                app.cache.cacheMenu(menu);

                                // Rendering the menu can take a couple of seconds on a mobile
                                self.pleaseWaitMessage(text_loadingMenu);
                                self.pleaseWaitProgress('');

                                setTimeout(function () { self.renderSiteMenu(menu) }, 0);
                            }

                            catch (exception)
                            {
                                // Got an error
                                app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, exception);
                            }
                        },
                        0
                    );
                }
                catch (exception)
                {
                    // Got an error
                    app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, exception);
                }
            },
            function (progress)
            {
                self.pleaseWaitProgress(progress);
                setTimeout(function () { }, 0);
            }
        );
    };
    self.renderSiteMenu = function (newMenu)
    {
        try
        {
            self.pleaseWaitMessage(text_renderingMenu);
            self.pleaseWaitProgress('');

            setTimeout
            (
                function ()
                {
                    try
                    {
                        // We've got a new menu for the site
                        app.site().menu = new menu();

                        // Render the menu 
                        app.site().menu.render
                        (
                            newMenu,
                            function ()
                            {
                                // Show the home page
                                app.viewEngine.showView('homeView', new homeViewModel(), undefined, true);
                            },
                            function (progress)
                            {
                                self.pleaseWaitProgress(progress);
                                setTimeout(function () { }, 0);
                            }
                        );
                    }
                    catch (exception)
                    {
                        // Got an error
                        app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, exception);
                    }
                },
                0
            );
        }
        catch (exception)
        {
            // Got an error
            app.showError(text_problemGettingSiteDetails, undefined, self.errorRetry, exception);
        }
    }
    self.markOfflineStores = function ()
    {
        for (var index = 0; index < app.sites().length; index++)
        {
            var site = app.sites()[index];

            var displayName = site.name;

            if (!site.isOpen)
            {
                displayName += text_appendOffline;
            }

            site.displayText = ko.observable(displayName);
        }
    };
    self.errorRetry = function ()
    {
        // Show the sites view
        app.viewEngine.showView('sitesView', self, undefined, false);
    }
}