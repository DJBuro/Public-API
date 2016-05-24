/// <reference path="managechainuserslink.directive.ts" />
module MyAndromeda.Chain.Directives {

    let directives = angular.module("MyAndromeda.Chain.Directives"); 

    directives.directive("manageChainHierachyLink", () => {

        return {
            name: "manageChainHierachyLink",
            scope: {
                chainId : "=chainId" // chain-id="someScopeVar"
            },
            template: `
                <a class="btn btn-default" style="width:100px" ui-sref="chain-admin.edit({chainId: chainId})">
                    <i class="fa fa-code-fork"></i>
                    Edit Chain
                </a>
            `,
            controller: ($scope, $timeout, $http: ng.IHttpService) => {
                let context = {
                };

                $scope.context = context; 
            }

        };

    });
}