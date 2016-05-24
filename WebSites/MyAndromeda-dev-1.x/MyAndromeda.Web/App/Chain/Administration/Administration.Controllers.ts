module MyAndromeda.Chain.Administration.Controllers {

    var controllers = angular.module("MyAndromeda.Chain.Administration.Controllers", []);

    controllers.controller("ChainAdminController", ($scope, $stateParams,
        manipulateTreeService: Services.ManipulateTreeService) => {

        let blankModel: Models.ITreeViewChainModel = {};
        let context = {
            chainId: $stateParams.chainId,
            model: null,
            newModel: blankModel
        };


        let treeViewData = manipulateTreeService.treeViewData;
        let treeViewOptions: kendo.ui.TreeViewOptions = {
            dragstart: (e) => {
                
            },
            dragend: (e) => {
                
            }
        };
        
        $scope.chianId = context.chainId;
        $scope.context = context;
        $scope.treeViewData = treeViewData;
        $scope.addItem = () => {
            Logger.Notify("insert");
            let treeview: kendo.ui.TreeView = $scope.tree;

            treeview.insertAfter(context.newModel, $scope.tree.select());
            //manipulateTreeService.Add(context.model);
            context.newModel = {};
        };
        $scope.saveChanges = () => {
            treeViewData.sync();
        };
        $scope.cancelChanges = () => {
            treeViewData.cancelChanges();
        }
        
   
    });

}