var MyAndromeda;
(function (MyAndromeda) {
    (function (Users) {
        (function (Models) {
            var User = (function () {
                function User(id, userName, firstName, surName, roles) {
                    this.Id = id;
                    this.Username = userName;
                    this.FirstName = firstName;
                    this.SurName = surName;
                    if (roles) {
                        this.Roles = roles;
                    }
                }
                return User;
            })();
            Models.User = User;
        })(Users.Models || (Users.Models = {}));
        var Models = Users.Models;
    })(MyAndromeda.Users || (MyAndromeda.Users = {}));
    var Users = MyAndromeda.Users;
})(MyAndromeda || (MyAndromeda = {}));
