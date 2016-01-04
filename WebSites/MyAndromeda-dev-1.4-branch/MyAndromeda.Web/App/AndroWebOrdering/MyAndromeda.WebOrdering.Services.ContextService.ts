/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Services
{
    Angular.ServicesInitilizations.push((app) => {
        app.factory(ContextService.Name, [
            () => {
                var instnance = new ContextService();
                return instnance;
            }
        ]);
    });

    export class ContextService {
        public static Name:string  = "ContextService";

        public Model : Models.IWebOrderingSettings;
        public ModelSubject : Rx.BehaviorSubject<Models.IWebOrderingSettings>;

        public StoreSubject: Rx.BehaviorSubject<Models.IStore[]>;

        public constructor() 
        {
            this.Model = null;
            this.ModelSubject = new Rx.BehaviorSubject<Models.IWebOrderingSettings>(null);
            this.StoreSubject = new Rx.BehaviorSubject<Models.IStore[]>([]);
        }
    }
   
 } 