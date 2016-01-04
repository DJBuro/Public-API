module AndroWeb.Helpers
{
    "use strict";
    export class HomePageHelper
    {
        public PostCodeSites = ko.observable<Models.ISiteDetails>();

        public SitePicked = ko.observable<boolean>(false);
        public NeedToChooseStore = ko.observable<boolean>(false);

        public Errors = ko.validation.group(this, { deep: false, observable: false })
        public PostCodeSearch = ko.observable<string>("").extend({
            pattern: {
                onlyIf: () => {
                    if (!this.PostCodeSearch) {
                        //i don't exist yet
                        return true; 
                    }

                    var postCode = this.PostCodeSearch() ;
                    if (!postCode) { postCode = ""; }

                    return postCode.replace(" ", "").length > 5;
                },
                //todo: move to engb list 
                message: 'Must be a valid UK postcode',
                params: /^([A-PR-UWYZ0-9][A-HK-Y0-9][AEHMNPRTVXY0-9]?[ABEHMNPRVWXY0-9]?\s?){1,2}([0-9][ABD-HJLN-UW-Z]{2}|GIR 0AA)$/i
            }
        });

        /* Postcode lookup UI cues  */
        public FoundAStore = ko.observable<boolean>(false);
        public NoStoresDeliver = ko.observable<boolean>(false);


        public StoresThatDeliver = ko.observable<boolean>();


        constructor()
        {
            Logger.Notify("Creating homepage helper");

            this.WatchForSiteDetails();
            this.WatchForPostcodeChanges();
        }

        private WaitForInit()
        {
            viewModel.initialised.subscribe
            (
                ready =>
                {
                    if (ready) 
                    {
                        //preload all available stores. 
                        Logger.Notify("Initilised complete ... choose store");
                        viewModel.chooseStore();
                    }
                }
            );
        }
        
        private WatchForSites()
        {
            viewModel.sites.subscribe((sites) => {
                Logger.Notify("Sites have arrived");
                Logger.Notify(sites);
            });
        }

        private WatchForSiteDetails()
        {
            if (!viewModel)
            {
                Logger.Error("No viewModel");
            }
            if (!viewModel.siteDetails)
            {
                Logger.Error("site details observable is missing");
            }

            this.SitePicked(false);
            this.NeedToChooseStore(true);

            var selected = () => {
                var siteDetails = viewModel.siteDetails();
                var sections = viewModel.sections();

                Logger.Notify("Selected siteDetails");
                Logger.Notify(siteDetails);
                Logger.Notify(sections);

                return siteDetails && sections && sections.length > 0;
            };

            viewModel.siteDetails.subscribe(siteDetails => {
                Logger.Notify("Checking site details");

                if (!siteDetails)
                {
                    this.SitePicked(false);
                    this.NeedToChooseStore(true);
                    return;
                }

                this.SitePicked(selected());
                this.NeedToChooseStore(!selected());
                
            });

            viewModel.sections.subscribe(sections => {
                Logger.Notify("sections changed");
                this.SitePicked(selected());
                this.NeedToChooseStore(!selected());
            });
        }


        private WatchForPostcodeChanges(): void
        {
            this.PostCodeSearch.subscribe((postcode) => {
                Logger.Notify("Search by postcode: " + postcode);

                var errors = this.Errors();
                var valid = errors.length === 0;

                
                if (postcode.length < 6 || !valid) {
                    this.NoStoresDeliver(false);
                    this.FoundAStore(false);
                    return;
                }

                Logger.Notify("IsValid = " + valid);

                var searchPostcode = postcode.replace(' ', '');

                Logger.Notify("Search based on postcodes");
                this.PostCodeSites([]);
                acsapi.getSites
                (
                    (values) => {
                        Logger.Notify("Result for postcode search:" + values.length);
                        this.PostCodeSites(values);
                        if (values.length === 0)
                        {
                            this.NoStoresDeliver(true); 
                            this.FoundAStore(false);
                        }
                        if (values.length > 0)
                        {
                            this.FoundAStore(true);
                            this.NoStoresDeliver(false);
                        }
                    },
                    () => 
                    {
                        viewModel.chooseStore();
                    },
                    searchPostcode
                );
            });
        }

        public SelectStoreFromPostCode() {
            Logger.Notify("Helper: Selecting from postcode");

            var sites = this.PostCodeSites(); //viewModel.sites();
            var first = sites[0];

            viewModel.pageManager.showPage("PLEASEWAIT", false, undefined, false);

            viewModel.siteDetails(undefined);
            viewModel.selectedSite(first);
            guiHelper.downloadAndShowStoreMenu();
        }

        public SelectStoreFromList(store : Models.ISiteDetails) {
            Logger.Notify("Helper: Selecting from list");
            Logger.Notify("Selecting store: " + store);

            viewModel.pageManager.showPage("PLEASEWAIT", false, undefined, false);

            viewModel.siteDetails(undefined);
            viewModel.selectedSite(store);
            guiHelper.downloadAndShowStoreMenu();
        }
        
        public ClearCurrentStore()
        {
            if (AndroWeb.Helpers.CartHelper.cart().hasItems()) {
                var warningMessage = textStrings.gExitWarning;
                var clear = window.confirm(warningMessage);

                if (!clear) {
                    return;
                }
            }

            viewModel.pageManager.showPage("PLEASEWAIT", false, undefined, false);

            this.SitePicked(false);
            this.NeedToChooseStore(true);

            viewModel.selectedSite(undefined);
            viewModel.siteDetails(undefined);
            
            
            AndroWeb.Helpers.CartHelper.clearCart();

            viewHelper.showHome();

            accountHelper.helloText(" ");
        } 
    }
}


var homePageActionHelper = {
    SelectedDropDownItem: ko.observable<AndroWeb.Models.ISiteDetails>({
        name: "none"
    }),
    SelectStoreFromPostCode: (e) => {
        AndroWeb.Logger.Notify("Click on: SelectStoreFromPostCode");

        var action = $.proxy(homePageHelper.SelectStoreFromPostCode, homePageHelper);

        action();
    },
    SelectStoreFromDropdown: (e) => {
        AndroWeb.Logger.Notify("Click on: SelectStoreFromDropdown");

        var selectedItem = homePageActionHelper.SelectedDropDownItem();
        var action = $.proxy(homePageHelper.SelectStoreFromList, homePageHelper, selectedItem);
        action();
    },
    ClearCurrentStore: (e) => {
        AndroWeb.Logger.Notify("Click on: ClearCurrentStore");
        var action = $.proxy(homePageHelper.ClearCurrentStore, homePageHelper);
        action();
    }
};

var homePageHelper = new AndroWeb.Helpers.HomePageHelper();
