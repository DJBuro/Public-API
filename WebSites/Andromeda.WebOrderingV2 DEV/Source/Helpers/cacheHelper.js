// Cache management
var cacheHelper =
{
    loadCachedMenuForSiteId: function (siteId)
    {
        var menu = undefined;

        try
        {
            // Show the please wait view
            guiHelper.showPleaseWait
            (
                textStrings.loadingMenu,
                undefined,
                function ()
                {
                    // Get the menu index
                    var menuIndex = amplify.store('menuIndex');

                    // Did we find the menu index?
                    if (menuIndex != undefined)
                    {
                        // Find the menu index for this site
                        var menuIndexItem = undefined;
                        for (var index = 0; index < menuIndex.length; index++)
                        {
                            menuIndexItem = menuIndex[index];

                            // Do we already have the menu for this site and version in the local cache?
                            if (menuIndexItem.siteId === siteId &&
                                menuIndexItem.menuVersion === viewModel.selectedSite().menuVersion)
                            {
                                // We have the correct version of the menu in the cache for this site

                                // Get the menu out of the cache
                                menu = amplify.store('m_' + siteId);
                            }
                        }
                    }
                }
            );
        }
        catch (exception) { } // Ignore exceptions 

        return menu;
    },
    cacheMenu: function (menu)
    {
        var success = false;

        // Get the menu index
        var menuIndex = amplify.store('menuIndex');

        // If there is no menu index create a new empty one
        if (menuIndex === undefined)
        {
            menuIndex = [];
        }

        // Find the menu index for this site
        var menuIndexItem = undefined;
        for (var index = 0; index < menuIndex.length; index++)
        {
            var checkMenuIndexItem = menuIndex[index];

            if (checkMenuIndexItem.siteId === viewModel.selectedSite().siteId)
            {
                menuIndexItem = checkMenuIndexItem;
                break;
            }
        }

        // Have we got a menu for this site?
        if (menuIndexItem === undefined)
        {
            // No menu for the site
            menuIndexItem =
            {
                siteId: viewModel.selectedSite().siteId,
                lastUsed: new Date(),
                menuVersion: viewModel.selectedSite().menuVersion
            };

            // Add the menu to the index
            menuIndex.push(menuIndexItem);
        }
        else
        {
            // Update the last time the menu was accessed
            menuIndexItem.menuVersion = viewModel.selectedSite().menuVersion;
            menuIndexItem.lastUsed = new Date();
        }

        // Add/update the menu
        if (cacheHelper.cacheObject('m_' + viewModel.selectedSite().siteId, menu, menuIndex))
        {
            // Update the index
            if (cacheHelper.cacheObject('menuIndex', menuIndex, menuIndex))
            {
                success = true;
            }
        }

        return success;
    },
    cacheObject: function (keyName, object, menuIndex)
    {
        var keepTrying = true;
        var success = false;

        // If we run out of storage, keep deleting old menus until there is space
        while (keepTrying)
        {
            // Lets not get stuck in an infinite loop...
            keepTrying = false;

            try
            {
                // Update the menu index
                amplify.store(keyName, object);

                success = true;
            }
            catch (exception)
            {
                // Is the cache full?
                if (exception == 'amplify.store quota exceeded')
                {
                    // Try and clear out the oldest menu/s
                    if (cacheHelper.removeOldestMenuFromCache(menuIndex))
                    {
                        // We've successfully removed a menu.  There might be space now...
                        keepTrying = true;
                    }
                }
            }
        }

        return success;
    },
    removeOldestMenuFromCache: function (menuIndex)
    {
        // Find the oldest menu
        var oldestMenuIndex = undefined;
        for (var index = 0; index < menuIndex.length; index++)
        {
            var checkMenuIndexItem = menuIndex[index];

            if (oldestMenuIndex === undefined || checkMenuIndexItem.lastUsed < oldestMenuIndex.lastUsed)
            {
                oldestMenuIndex = checkMenuIndexItem;
            }
        }

        // Have we got a menu for this site?
        if (oldestMenuIndex != undefined)
        {
            // Remove the cached menu
            amplify.store('m_' + oldestMenuIndex.siteId, null);

            // Remove the menu from the index
            menuIndex.splice(oldestMenuIndex, 1);

            // Save the index
            amplify.store('menuIndex', menuIndex);

            // We've made some space
            return true;
        }
        else
        {
            // Nothing left to delete?
            return false;
        }
    }
};