/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
module AndroWeb.Models
{
    export class DealWrapper
    {
        public templateName: string = 'deal-template';
        public deal: any;
        public isEnabled: KnockoutObservable<boolean> = ko.observable(true);
        public isAvailableToday: KnockoutObservable<boolean> = ko.observable(true);
        public dealLineWrappers: any;
        public minimumOrderValue: number = 0;
        public isNotAvailableForRestOfDay: KnockoutObservable<boolean> = ko.observable(false);
        public availabilityText: KnockoutObservable<string> = ko.observable('');
        public removedTimeSlot: KnockoutObservable<boolean> = ko.observable(false);
        public extension;
        public thumbnailHeight: number = 0;
        public thumbnailWidth: number = 0;
        public thumbnail;
        public image;
        public overlayImage;
        public dealWrapper;
        public notes: KnockoutObservable<string> = ko.observable('');;
    }
};































