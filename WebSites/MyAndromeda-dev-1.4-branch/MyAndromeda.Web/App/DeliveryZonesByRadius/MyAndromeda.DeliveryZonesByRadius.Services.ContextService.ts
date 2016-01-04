/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
module MyAndromeda.DeliveryZonesByRadius.Services {
    Angular.ServicesInitilizations.push((app) => {
        app.factory(ContextService.Name, [
            () => {
                var instnance = new ContextService();
                return instnance;
            }
        ]);
    });

    export class ContextService {
        public static Name: string = "ContextService";

        public Model: Models.IDeliveryZoneNameViewModelSettings;
        public ModelSubject: Rx.Subject<Models.IDeliveryZoneNameViewModelSettings>;
        public PostcodeModels: Rx.Subject<Models.IPostCodeSectorsViewModel[]>;
        public constructor() {
            this.Model = null;
            this.ModelSubject = new Rx.Subject<Models.IDeliveryZoneNameViewModelSettings>();
            this.PostcodeModels = new Rx.Subject<Models.IPostCodeSectorsViewModel[]>();
        }
    }

}  