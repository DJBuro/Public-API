/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/modernizr/modernizr.d.ts" />

module AndroWeb.ViewModels
{
    export class BaseViewModel
    {
        isShowStoreDetailsButtonVisible: KnockoutObservable<boolean>;
        isShowHomeButtonVisible: KnockoutObservable<boolean>;
        isShowMenuButtonVisible: KnockoutObservable<boolean>;
        isShowCartButtonVisible: KnockoutObservable<boolean>;

        isHeaderVisible: KnockoutObservable<boolean>;
        isPostcodeSelectorVisible: KnockoutObservable<boolean>;
        areHeaderOptionsVisible: KnockoutObservable<boolean>;
        isHeaderLoginVisible: KnockoutObservable<boolean>;

        constructor()
        {
            this.isShowStoreDetailsButtonVisible = ko.observable(true);
            this.isShowHomeButtonVisible = ko.observable(true);
            this.isShowMenuButtonVisible = ko.observable(true);
            this.isShowCartButtonVisible = ko.observable(true);

            this.isHeaderVisible = ko.observable(true);
            this.isPostcodeSelectorVisible = ko.observable(false);
            this.areHeaderOptionsVisible = ko.observable(true);
            this.isHeaderLoginVisible = ko.observable(true);
        }
    }
} 