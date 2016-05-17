module MyAndromeda.Chain.Directives {

    let directives = angular.module("MyAndromeda.Chain.Directives", []); 

    directives.directive("manageChainUsers", () => {

        return {
            name: "manageChainUsers",
            scope: {
                chainId : "=chainId" // chain-id="someScopeVar"
            },
            template: `
                <a class="btn btn-danger" href="Users/Chain/{{chainId}}/UserManagement">
                    <i class="fa fa-group"></i>
                    Manage Users <span class="badge">{{context.userCount}}</span>
                </a>
            `,
            controller: ($scope, $timeout, $http: ng.IHttpService) => {
                let context = {
                    userCount : 0 
                }; 

                let route = kendo.format("/user/chain/{0}/count", $scope.chainId); 
                let promise = $http.get(route); 

                promise.then((result: ng.IHttpPromiseCallbackArg<any>) => {
                    $timeout(() => { context.userCount = result.data.Count; });
                    
                });

                $scope.context = context; 
            }

        };

    });
}