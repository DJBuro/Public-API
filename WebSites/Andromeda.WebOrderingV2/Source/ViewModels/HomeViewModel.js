/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function HomeViewModel()
{
    "use strict";

    var self = this;

    self.isShowStoreDetailsButtonVisible = ko.observable(false);
    self.isShowHomeButtonVisible = ko.observable(true);
    self.isShowMenuButtonVisible = ko.observable(false);
    self.isShowCartButtonVisible = ko.observable(false);

    self.isHeaderVisible = ko.observable(true);
    self.isPostcodeSelectorVisible = ko.observable(false);
    self.areHeaderOptionsVisible = ko.observable(true);
    self.isHeaderLoginVisible = ko.observable(true);

    // Mobile mode
    self.title = ko.observable(textStrings.mmMyOrder); // Current section name - shown in the header
    self.titleClass = ko.observable('mobileSectionCart'); // Class to use to style the section name - used for showing an icon for the section

    self.homePageCarousels = ko.observableArray();


    self.generateMenuItemCarouselItem = function (itemDetails)
    {
        var item = undefined;

        var menuItemWrapper = menuHelper.menuItemLookup[itemDetails.id];

        if (menuItemWrapper != undefined)
        {
            var src = undefined;
            
            if (itemDetails.url != undefined)
            {
                src = itemDetails.url;
            }
            else if (menuItemWrapper.thumbnail != undefined)
            {
                src = menuHelper.menuDataThumbnails().Server.Endpoint + menuItemWrapper.thumbnail.Src;
            }

            item = 
                {
                    src: src,
                    templateName: 'homeCarouselMenuItemCard',
                    name: menuItemWrapper.name,
                    description: menuItemWrapper.menuItem.Desc,
                    price: helper.formatPrice(viewModel.orderType() == 'collection' ? menuItemWrapper.menuItem.ColPrice : menuItemWrapper.menuItem.DelPrice),
                    className: itemDetails.className,
                    id: itemDetails.id
                };
        }

        return item;
    }

    self.generateDealCarouselItem = function (itemDetails)
    {
        var item = undefined;

        var deal = menuHelper.dealLookup[itemDetails.id];

        if (deal != undefined && deal.deal.DealName.length > 0)
        {
            item =
                {
                    src: itemDetails.url,
                    templateName: 'homeCarouselDealCard',
                    isOverlayVisible: false,
                    name: deal.deal.DealName,
                    description: deal.deal.Description,
                    price: deal.deal.DeliveryAmount != undefined && deal.deal.DeliveryAmount > 0 ? helper.formatPrice(deal.deal.DeliveryAmount) : '',
                    className: itemDetails.className,
                    id: itemDetails.id
                };
        }

        return item;
    }

    self.generateImageCarouselItem = function (itemDetails)
    {
        var item =
        {
            src: itemDetails.url,
            templateName: 'homeCarouselImageCard',
            name: '',
            description: '',
            price: '',
            className: itemDetails.className,
            id: itemDetails.id
        };

        return item;
    }

    self.generateHtmlCarouselItem = function (itemDetails)
    {
        var item =
        {
            src: '',
            templateName: 'homeCarouselHtmlCard',
            name: '',
            description: itemDetails.html,
            price: '',
            className: itemDetails.className,
            id: itemDetails.id
        };

        return item;
    }

    self.onLogout = function ()
    {
    }

    self.onShown = function ()
    {
        setTimeout
        (
            function ()
            {
                for (var carouselIndex = 0; carouselIndex < self.homePageCarousels().length; carouselIndex++)
                {
                    var carousel = self.homePageCarousels()[carouselIndex];
                    if (carousel.items().length > 0)
                    {
                        carousel.slider = new $JssorSlider$(carousel.elementId, carousel.options);
                    }
                }

                setTimeout
                (
                    function ()
                    {
                        self.scaleSlider();

                        $(window).resize(self.scaleSlider);

                        guiHelper.finished();

                        viewModel.resetOrderType();

 //                       viewModel.chooseStore();

                        // Make the main menu visible
 //                       guiHelper.areHeaderOptionsVisible(true);
                    }, 0
                );
            }, 0
        );
    }

    self.onClosed = function()
    {
        $(window).off('resize', self.scaleSlider);
    }

    self.showStoreDetails = function()
    {
        viewModel.pageManager.showPage('StoreDetails', true);
    }

    self.showMenu = function()
    {
        viewModel.pageManager.showPage('Menu', true);
    }

    self.scaleSlider = function()
    {
        for (var carouselIndex = 0; carouselIndex < self.homePageCarousels().length; carouselIndex++)
        {
            var carousel = self.homePageCarousels()[carouselIndex];

            var parentWidth = $('#' + carousel.elementId).parent().width();
            if (parentWidth)
            {
                carousel.slider.$SetScaleWidth(parentWidth);
            }
        }
    }

    /* Build the carousels */
    if (settings.HomePageCarousels != undefined)
    {
        for (var carouselDetailsIndex = 0; carouselDetailsIndex < settings.HomePageCarousels.length; carouselDetailsIndex++)
        {
            var carouselDetails = settings.HomePageCarousels[carouselDetailsIndex];
            var carousel = { elementId: carouselDetails.elementId, options: carouselDetails.options, items: ko.observableArray(), slider: undefined };

            for (var itemDetailsIndex = 0; itemDetailsIndex < carouselDetails.items.length; itemDetailsIndex++)
            {
                var itemDetails = carouselDetails.items[itemDetailsIndex];
                var item = undefined;

                switch (itemDetails.type)
                {
                    case 'MenuItem':
                        item = self.generateMenuItemCarouselItem(itemDetails);
                        break
                    case 'Deal':
                        item = self.generateDealCarouselItem(itemDetails);
                        break;
                    default:
                        item = self.generateImageCarouselItem(itemDetails);
                        break;
                }

                if (item != undefined)
                {
                    carousel.items.push(item);
                }
            }

            self.homePageCarousels.push(carousel);
        }
    }
};







































