var MyAndromeda;
(function (MyAndromeda) {
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
    })(MyAndromeda.Models || (MyAndromeda.Models = {}));
    var Models = MyAndromeda.Models;
})(MyAndromeda || (MyAndromeda = {}));
