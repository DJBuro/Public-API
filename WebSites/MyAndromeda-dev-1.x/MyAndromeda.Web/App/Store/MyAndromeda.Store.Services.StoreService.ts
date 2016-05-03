module MyAndromeda.Store.Services {



    //obsolete  
    export class storeService implements Models.IStoreService {
        public routes: Models.IStoreServiceRoutes
        constructor(routes: Models.IStoreServiceRoutes) {
            this.routes = routes;
        }

        public get(id: number, callback: { (stores: Models.IStore[]): void })
        public get(externameId: string, callback: { (stores: Models.IStore[]): void })
        public get(id: any, callback: { (stores: Models.IStore[]): void }) {
            var internal = this;
            var route = {
                type: "POST",
                dataType: "json",
                success: function (data) {
                    callback(data);
                }
            };
            if (typeof (id) === "string") {
                $.ajax($.extend({}, route, {
                    url: internal.routes.getByExternalId
                }));
            }
            if (typeof (id) === "number") {
                $.ajax($.extend({}, route, {
                    url: internal.routes.getById
                }));
            }
        }

        public getStores(chainId: number, callback: { (stores: Models.IStore[]): void }) {
            var internal = this;
        }
    }
    
}