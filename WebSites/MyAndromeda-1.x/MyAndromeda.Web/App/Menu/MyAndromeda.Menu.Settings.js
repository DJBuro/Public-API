var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
            })();
            Settings.Routes = Routes;

            ;

            ;

            ;
        })(Menu.Settings || (Menu.Settings = {}));
        var Settings = Menu.Settings;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
