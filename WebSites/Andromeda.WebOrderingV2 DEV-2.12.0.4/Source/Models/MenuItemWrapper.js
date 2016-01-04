/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/

function MenuItemWrapper(menuItem, itemName)
{
    "use strict";

    var self = this;

    this.templateName = 'menuItem-template';
    this.name = itemName;
    this.menuItem = menuItem;
    this.displayOrder = menuItem.DispOrder;
    this.description = menuHelper.fixName(menuItem.Desc == undefined ? menuItem.Description : menuItem.Desc);
    this.category1s = ko.observableArray();
    this.category2s = ko.observableArray();
    this.menuItems = ko.observableArray();
    this.selectedCategory1 = ko.observable();
    this.selectedCategory2 = ko.observable();
    this.price = ko.observable(helper.formatPrice(menuHelper.getItemPrice(menuItem)));
    this.quantity = ko.observable(1);
    this.thumbnail = undefined;
    this.thumbnailWidth = 0;
    this.thumbnailHeight = 0;
    this.image = undefined;
    this.overlayImage = undefined;
    this.isNotAvailableForRestOfDay = ko.observable(false);
    this.availabilityText = ko.observable('');
    this.isEnabled = ko.observable(true);
    this.isTemporarilyDisabled = false;
    this.notAvailableText = ko.observable('');
};































