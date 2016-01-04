/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../models/customer.ts" />

module AndroWeb.ViewModels
{
    export class PleaseWaitViewModel extends BaseViewModel
    {
        // Mobile mode
        public title: KnockoutObservable<string> = ko.observable(textStrings.mmMyOrder); // Current section name - shown in the header
        public titleClass: KnockoutObservable<string> = ko.observable('mobileSectionCart'); // Class to use to style the section name - used for showing an icon for the section

        private onShownCallback: any;

        public previousViewName: string;
        public previousContentViewModel: any;

        constructor(onShownCallback?)
        {
            super();

            this.onShownCallback = onShownCallback;

            this.isShowStoreDetailsButtonVisible = ko.observable(false);
            this.isShowHomeButtonVisible = ko.observable(false);
            this.isShowMenuButtonVisible = ko.observable(false);
            this.isShowCartButtonVisible = ko.observable(false);

            this.isHeaderVisible = ko.observable(false);
            this.isPostcodeSelectorVisible = ko.observable(false);
            this.areHeaderOptionsVisible = ko.observable(false);
            this.isHeaderLoginVisible = ko.observable(false);

  //          this.cannotBePrevious = true;

            customerHelper.bindableCustomer(new AndroWeb.Models.Customer());
            accountHelper.registerError(undefined);

            if (viewModel.contentViewModel() != undefined && viewModel.contentViewModel().previousViewName != undefined && viewModel.contentViewModel().previousViewName.length > 0)
            {
                this.previousViewName = viewModel.contentViewModel().previousViewName;
                this.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
            }
            else
            {
                this.previousViewName = guiHelper.getCurrentViewName();
                this.previousContentViewModel = viewModel.contentViewModel();
            }
        }

        //self.isShowStoreDetailsButtonVisible = ko.observable(false);
        //self.isShowHomeButtonVisible = ko.observable(false);
        //self.isShowMenuButtonVisible = ko.observable(false);
        //self.isShowCartButtonVisible = ko.observable(false);

        //self.isHeaderVisible = ko.observable(false);
        //self.isPostcodeSelectorVisible = ko.observable(false);
        //self.areHeaderOptionsVisible = ko.observable(false);
        //self.isHeaderLoginVisible = ko.observable(false);

        //// Mobile mode
        //self.title = ko.observable(textStrings.mmMyOrder); // Current section name - shown in the header
        //self.titleClass = ko.observable('mobileSectionCart'); // Class to use to style the section name - used for showing an icon for the section

        //self.cannotBePrevious = true;

        //self.onShownCallback = onShownCallback;

        //if (viewModel.contentViewModel() != undefined && viewModel.contentViewModel().previousViewName != undefined && viewModel.contentViewModel().previousViewName.length > 0)
        //{
        //    self.previousViewName = viewModel.contentViewModel().previousViewName;
        //    self.previousContentViewModel = viewModel.contentViewModel().previousContentViewModel;
        //}
        //else
        //{
        //    self.previousViewName = guiHelper.getCurrentViewName();
        //    self.previousContentViewModel = viewModel.contentViewModel();
        //}

        public onLogout ()
        {
            viewModel.chooseStore();
        }

        public onShown ()
        {
            if (this.onShownCallback !== undefined) this.onShownCallback();
        }

        public back ()
        {
            guiHelper.showView(this.previousViewName, this.previousContentViewModel);
        }
    }
};






































