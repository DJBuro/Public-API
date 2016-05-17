module MyAndromeda.Hubs.Services {
    
    export class MenuHubService {
        private hub: StoreHub;
        public viewModel: kendo.Observable;
        public options : Models.IMenuHubServiceOptions;

        constructor(options: Models.IMenuHubServiceOptions) {
            var internal = this;
            this.options = options;
            this.hub = StoreHub.GetInstance(options); //new StoreHub(options);
            
            this.viewModel = kendo.observable({
                siteName: "",
                menuVersion: "",
                lastUpdated: "",

                updates: []
            });
        }

        public init(): void {
            kendo.bind(this.options.id, this.viewModel);
        }
    }
} 