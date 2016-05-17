module MyAndromeda.Authorizer {

    var services = angular.module("MyAndromeda.Authorizer", []);

    export class AuthorizerService 
    {
        constructor(private $http: ng.IHttpService)
        { }

        //to-do 
        public AuthorizedFor(store: IStore, permission: IPermission): ng.IHttpPromise<IAuthorizedResult>
        {
            let route = "/authorize/store/permission";
            let data = {
                Store: store,
                Permission: permission
            };
            return this.$http.post(route, data);
        }

        public AuthorizedForAny(store: IStore, permissions: IPermission[]): ng.IHttpPromise<IAuthorizedResult>
        {
            let route = "/authorize/store/anyPermission";
            let data = {
                Store: store,
                Permissions: permissions
            };
            return this.$http.post(route, data); 
        }

        public CheckStoresAuthorizedFor(stores: IStore[], permission: IPermission)
        {
            let route = "/authorize/stores/permission";
            let data = {
                Stores: stores,
                Permission: permission
            };
            return this.$http.post(route, data);
        }

        public CheckStoresAuthorizedForAny(stores: IStore[], permissions: IPermission[])
        {
            let route = "/authorize/stores/anyPermission";
            let data = {
                Stores: stores,
                Permissions: permissions
            };
            return this.$http.post(route, data);
        } 
    }

    export interface IPermission {
        Name: string;
    }

    export interface IStore {
        Id: string; 
    } 
    
    export interface IAuthorizedResults {
        [StoreId: string]: IAuthorizedResult 
    }

    export interface IAuthorizedResult {
        Permission: IPermission;
        Authorized: boolean; 
    }

    export class EnrollmentPermissions
    {
        
    }

    services.service("authorizerService", AuthorizerService);
}