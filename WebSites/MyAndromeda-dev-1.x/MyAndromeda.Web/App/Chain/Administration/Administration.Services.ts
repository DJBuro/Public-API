module MyAndromeda.Chain.Administration.Services {
    var services = angular.module("MyAndromeda.Chain.Administration.Services", []);

    export class ManipulateTreeService {
        public treeViewData: kendo.data.HierarchicalDataSource; 

        constructor(private $http: ng.IHttpService) {

            let model: kendo.data.HierarchicalDataSourceSchemaModel = {
                id: "Id",
                children: "Items",
                fields: {
                    Name: { field: "Name", type: "string" }
                }
            };
            let schema: kendo.data.HierarchicalDataSourceSchema = {
                model: model
            };
            
            var treeViewData = new kendo.data.HierarchicalDataSource({
                //data: [
                //    { Id: 1, Name: "test", ParentId: null },
                //    { Id: 2, Name: "test 2", ParentId: 1 }
                //],
                //schema: {
                //    model: {
                //        id: "Id",
                //        fields: {
                            
                //            Name: {
                //                field: "Name", type: "string"
                //            },
                //            StoreCount: { field: "StoreCount", type: "number" },
                //            ParentId: { field: "ParentId", nullable: true },
                            
                //        },
                //        children: "Items",
                //        //should be the context of the model:
                //        hasChildren: function () {
                //            //Logger.Notify("has children:");
                //            //Logger.Notify(this);
                //            return false;
                //            //let items = this.Items;
                //            //return items.length; 
                //        }
                //    }
                //},
                schema: schema,
                transport: {
                    read: (options: kendo.data.DataSourceTransportReadOptions) => {
                        let route = "/data/admin/chains";

                        let promise = this.$http.get(route);
                        promise.then((callback) => {
                            
                            options.success(callback.data);
                        });
                    },
                    update: (options: kendo.data.DataSourceTransportReadOptions) => {
                        
                    },
                    create: (options: kendo.data.DataSourceTransportReadOptions) => {
                        
                    },
                    destroy: (options: kendo.data.DataSourceTransportReadOptions) => {
                        
                    }
                },
                sort: [
                    { field: "Name", dir "asc" }
                ],
                serverSorting: false
            });

            treeViewData.read();
            this.treeViewData = treeViewData;
        }

        public Add(item: Models.ITreeViewChainModel) {
            this.treeViewData.add(item);
        } 
    }

    services.service("manipulateTreeService", ManipulateTreeService);

    export class AssignStoreToChainService {

        constructor(private $http: ng.IHttpService) {
        }

        public assignToChain(chainId: number, models: any[]) {
            let route = "/chain-administration/{0}";
            route = kendo.format(route, chainId);
            
            //this.$http.post(
        }
    }

    services.service("assignStoreToChainService", AssignStoreToChainService);
}

