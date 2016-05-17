var MyAndromeda;
(function (MyAndromeda) {
    var Users;
    (function (Users) {
        var Models;
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
            }());
            Models.User = User;
        })(Models = Users.Models || (Users.Models = {}));
    })(Users = MyAndromeda.Users || (MyAndromeda.Users = {}));
})(MyAndromeda || (MyAndromeda = {}));
