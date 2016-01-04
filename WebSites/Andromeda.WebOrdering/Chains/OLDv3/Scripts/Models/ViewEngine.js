/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function viewEngine()
{
    var self = this;

    self.viewModels = {};
    self.popupCount = 0;
    self.currentStateIndex = 0;
    self.currentViewModel = undefined;

    self.initialise = function ()
    {
        // Determine based on the current window size whether we're in mobile mode
        self.refreshIsMobileMode();

        // We'll need to know when the window is resized
        $(window).resize(self.resize);

        window.History.Adapter.bind
        (
            window,
            'statechange',
            function ()
            {
                // Note: We are using statechange instead of popstate
                var toState = History.getState(); // Note: We are using History.getState() instead of event.state

                // Do we have a view model for this history item?
                var toViewModel = self.viewModels[toState.data.viewName];

                // Get the state we're going from
                var fromState = History.savedStates[self.currentStateIndex];
                var fromViewModel = undefined;
                if (fromState != undefined)
                {
                    fromViewModel = self.viewModels[fromState.data.viewName];
                }

                var action = '';
                if (toState != undefined)
                {
                    if (toState.data.stateIndex < self.currentStateIndex)
                    {
                        // Back button pressed
                        action = 'back';
                    }
                    else if (toState.data.stateIndex > self.currentStateIndex)
                    {
                        // Forward button pressed
                        action = 'forward';
                    }
                }

                // Did the user press back or forward?
                if (action.length > 0)
                {
                    // We've moved from the previous state
                    self.currentStateIndex = toState.data.stateIndex;

                    // Is there a view model for where we're coming from
                    if (fromViewModel != undefined && fromViewModel.onNavigateCallback != null)
                    {
                        // Tell the view model we're going from that the back button was pressed
                        fromViewModel.onNavigateCallback(action + 'From');
                    }

                    // Is there a view model for where we're going to
                    if (toViewModel.onNavigateCallback != null)
                    {
                        // Tell the view model we're going to that the back button was pressed
                        toViewModel.onNavigateCallback(action + 'To');
                    }
                }
                else
                {
                    // A new view/popup has been displayed
                    self.currentStateIndex = self.currentStateIndex + 1;
                }
            }
        );
    };

    self.showView = function (viewName, viewModel, viewContainerName, includeInNavHistory)
    {
        // Get the view html from inside the specified element 
        var viewHtml = { viewHtml: $('#' + viewName).html() }; // We're using an object so we can pass by ref

        // The DOM element that contains the view 
        var viewElement = document.getElementById('viewContainer');

        // Remove any existing knockout bindings
        ko.cleanNode(viewElement);

        // Let the browser process any changes
        setTimeout
        (
            function ()
            {
                // Let the view model do any initialisation
                if (typeof (viewModel.onBeforeViewShown) == 'function')
                {
                    viewModel.onBeforeViewShown(viewHtml);
                }

                // Keep track of the currently visible view
                guiHelper.viewName(viewName);

                // Get the container element that the view needs to be in
                var viewContainer = $(viewContainerName == undefined ? '#viewContainer' : '#' + viewContainerName);

                // Make sure the view container is visible
                viewContainer.css('display', 'block');

                // Insert the view html in to the view box
                viewContainer.html(viewHtml.viewHtml);

                // Bind the view to the data model
                ko.applyBindings(viewModel, viewElement);

                // Reset the window scroll position
                $(window).scrollTop(0);

                // Keep hold of the view in case the user clicks back or forward
                self.viewModels[viewName] = viewModel;
                self.currentViewModel = viewModel;

                if (includeInNavHistory)
                {
                    // Add the view change to the nav history        
                    History.pushState
                    (
                        {
                            stateIndex: self.currentStateIndex,
                            viewName: includeInNavHistory ? viewName : undefined
                        },
                        undefined,
                        '?page=' + viewName
                    );
                }

                // Let the gui update
                setTimeout
                (
                    function ()
                    {
                        // Do we need to tell the view model that the view has been shown?
                        if (typeof (viewModel.onAfterViewShown) == 'function')
                        {
                            viewModel.onAfterViewShown();
                        }
                    },
                    0
                );
            },
            0
        );
    };

    self.showPopup = function (viewName, viewModel)
    {
        self.popupCount = self.popupCount + 1;
        var zOrder = 8000 + Number(self.popupCount);

        // Get the view html from inside the specified element 
        var viewHtml = $('#' + viewName).html();

        var html =
            '<div id="popup' + self.popupCount + '" class="popupOverlay" style="z-index: ' + zOrder  + ';">' +
                '<div>' +
                    viewHtml +
                '</div>' +
            '</div>';

        $('#popupContainer').append(html);

        if (viewModel != undefined)
        {
            // Keep hold of the view in case the user clicks back or forward
            self.viewModels[viewName] = viewModel;
            self.currentViewModel = viewModel;

            // Add the view change to the nav history        
            History.pushState
            (
                {
                    stateIndex: self.currentStateIndex,
                    viewName: viewName
                },
                undefined,
                '?page=' + viewName
            );

            // The DOM element that contains the view 
            var viewElement = document.getElementById('popup' + self.popupCount);

            // Bind the view to the data model
            ko.applyBindings(viewModel, viewElement);
        }
    }

    self.hidePopup = function ()
    {
        $('#popup' + self.popupCount).remove();

        self.popupCount = self.popupCount - 1;
    }

    self.resize = function ()
    {
        self.refreshIsMobileMode();
    }

    self.refreshIsMobileMode = function ()
    {
        var newPageWidth = $(window).width();

        if (Number(newPageWidth) < template_mobileWidth)
        {
            // Mobile mode
            var fromIsMobileMode = app.isMobileMode();
            app.isMobileMode(true);

            // Are we in PC/Tablet mode?
            if (!fromIsMobileMode)
            {
                // We're transitioning from PC/Tablet mode to mobile mode
                // Do we need to tell the current view model that we've transitioned?
                if (self.currentViewModel != undefined &&
                    typeof (self.currentViewModel.onTransitionToMode) == 'function')
                {
                    self.currentViewModel.onTransitionToMode('mobile');
                }
            }
        }
        else
        {
            // PC/Tablet mode
            var fromIsMobileMode = app.isMobileMode();
            app.isMobileMode(false);

            // Are we in PC/Tablet mode?
            if (fromIsMobileMode)
            {
                // We're transitioning from mobile mode to PC/Tablet mode
                // Do we need to tell the current view model that we've transitioned?
                if (self.currentViewModel != undefined &&
                    typeof (self.currentViewModel.onTransitionToMode) == 'function')
                {
                    self.currentViewModel.onTransitionToMode('tabletPc');
                }
            }
        }
    };
};