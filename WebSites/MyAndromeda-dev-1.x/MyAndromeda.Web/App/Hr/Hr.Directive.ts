module MyAndromeda.Hr.Directives
{
    var app = angular.module("MyAndromeda.Start.Directives", []);

    app.directive("employeePic", () => {

        return {
            name: "employeePic",
            templateUrl: "employee-pic.html",
            restrict: "EA",
            scope: {
                employee: '='
            },
            controller: ($scope) => {
                var dataItem: Models.IEmployee = $scope.employee;

                $scope.profilePicture = (img) => {
                    var profilePicture = "/content/profile-picture.jpg";
                    if (img) {
                        profilePicture = img;
                    }
                    return {
                        'background-image': 'url(' + profilePicture + ')'
                    }
                };

                $scope.dataItem = dataItem;
            }
        };
    });
}