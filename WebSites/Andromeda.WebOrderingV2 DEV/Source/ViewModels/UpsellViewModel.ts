/*
    Copyright © 2014 Andromeda Trading Limited.  All rights reserved.  
    THIS FILE AND ITS CONTENTS ARE PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    ANY USE OF THE CONTENTS OF THIS FILE OR ANY PART OF THIS FILE WITHOUT EXPLICIT PERMISSION FROM ANDROMEDA TRADING LIMITED IS PROHIBITED. 
*/
/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/modernizr/modernizr.d.ts" />
/// <reference path="baseviewmodel.ts" />

module AndroWeb.ViewModels
{
    export class UpsellViewModel extends BaseViewModel
    {
        // The menu sections to upsell
        static upsellMenuSections: KnockoutObservableArray<any> = ko.observableArray();

        // Not sure if this is needed = possibly for the popup that this view model is in
        popupViewModel: KnockoutObservable<any>;

        constructor(upsellMenuSections: number[])
        {
            super();

            // Clear out the upsell menu sections
            AndroWeb.ViewModels.UpsellViewModel.upsellMenuSections.removeAll();

            for (var upsellIndex: number = 0; upsellIndex < upsellMenuSections.length; upsellIndex++)
            {
                var upsellMenuSectionId = upsellMenuSections[upsellIndex];

                // Get the menu section name
                var menuSectionName: string = "";
                var menuSectionIndex: number = 0;
                for (menuSectionIndex = 0; menuSectionIndex < viewModel.menu.Display.length; menuSectionIndex++)
                {
                    var menuSection = viewModel.menu.Display[menuSectionIndex];
                    if (menuSection.Id == upsellMenuSectionId)
                    {
                        menuSectionName = textStrings.checkUpsellPrompt.replace("{menusection}", menuSection.Name);
                        break;
                    }
                }

                if (menuSectionName.length > 0)
                {
                    // Add this menu section name to the upsell list
                    AndroWeb.ViewModels.UpsellViewModel.upsellMenuSections.push
                    (
                        {
                            text: menuSectionName,
                            index: menuSectionIndex
                        }
                    );
                }
            }
        }

        public static noThanksClicked = () =>
        {       
            // Skip upsell and go to the checkout
            popupHelper.hidePopup();
            viewModel.pageManager.showPage("Checkout", true);
        }

        public static upsell = (menuSection) =>
        {
            // Go to the menu section that we have upsold
            popupHelper.hidePopup();
            guiHelper.showMenu(menuSection.index);
        }
    }
} 