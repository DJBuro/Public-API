module MyAndromeda.Chain.Manipulation {
    var services = angular.module("MyAndromeda.Chain.Manipulation.Services", []);


    export class ManipulateTreeService {
        public treeViewData: kendo.data.HierarchicalDataSource; 

        constructor() { }

        public Add(item) {
        } 
    }

    services.service("manipulateTreeService", ManipulateTreeService);

    export class AssignStoreToChainService {

        constructor(private $http: ng.IHttpService) {
        }


    }

    services.service("assignStoreToChainService", AssignStoreToChainService);
}

