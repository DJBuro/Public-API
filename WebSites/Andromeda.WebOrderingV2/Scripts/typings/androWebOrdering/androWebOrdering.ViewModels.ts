declare module AndroWeb.ViewModels {
    export interface IViewModel {
        pageManager: pageManager;
        sites: KnockoutObservableArray<Models.ISiteDetails>;
        siteDetails: KnockoutObservable<Models.ISiteDetails>;
        sections: KnockoutObservableArray<any>;
        selectedSite: KnockoutObservable<Models.ISiteDetails>;
        gotSiteList: () => void;
        chooseStore: () => void;

        initialised: KnockoutObservable<boolean>;
        initialise: () => void;

        serverUrl: string;
    }

} 