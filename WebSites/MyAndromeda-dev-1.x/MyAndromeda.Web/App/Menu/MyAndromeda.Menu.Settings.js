var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Settings;
        (function (Settings) {
            var Routes = (function () {
                function Routes() {
                }
                Routes.Toppings = {
                    List: "",
                    Update: ""
                };
                Routes.MenuItems = {
                    ListMenuItems: "",
                    SaveMenuItems: "",
                    SaveImageUrl: "",
                    RemoveImageUrl: ""
                };
                Routes.Ftp = {
                    DownloadMenu: "",
                    UploadMenu: "",
                    Version: "",
                    Delete: ""
                };
                Routes.Publish = "";
                return Routes;
            }());
            Settings.Routes = Routes;
            ;
            ;
            ;
        })(Settings = Menu.Settings || (Menu.Settings = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
