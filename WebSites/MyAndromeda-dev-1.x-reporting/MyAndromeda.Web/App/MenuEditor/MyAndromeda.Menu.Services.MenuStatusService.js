var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            var MenuStatus = (function () {
                function MenuStatus(dataSource) {
                    this.dataSource = dataSource;
                    this.observable = kendo.observable({
                        Message: MenuStatus.EditedMessage,
                        HasChanges: false,
                        CanPublish: false,
                        SaveChanges: function () {
                            dataSource.sync();
                        },
                        CancelChanges: function () {
                            if (confirm("All changes will be lost, continue?")) {
                                dataSource.cancelChanges();
                            }
                        }
                    });
                    this.intitDataSource();
                    this.initPageEvents();
                }
                MenuStatus.prototype.intitDataSource = function () {
                    var internal = this;
                    this.dataSource.bind("change", function () {
                        internal.observable.set(MenuStatus.HasChangesField, internal.dataSource.hasChanges());
                        internal.observable.set(MenuStatus.CanPublishField, !internal.dataSource.hasChanges());
                    });
                };
                MenuStatus.prototype.initPageEvents = function () {
                    var _this = this;
                    $(window).on("beforeunload", function (e) {
                        if (_this.dataSource.hasChanges()) {
                            //return MenuStatus.LeaveMesssage;
                            e.preventDefault();
                        }
                    });
                };
                MenuStatus.Name = "MenuStatusService";
                MenuStatus.LeaveMesssage = "There are unsaved changes to the menu. Are you sure you want to leave?";
                MenuStatus.EditedMessage = "There are unsaved changes to the menu. Don't forget to save changes as you go along.";
                MenuStatus.HasChangesField = "HasChanges";
                MenuStatus.CanPublishField = "CanPublish";
                MenuStatus.MessageField = "Message";
                return MenuStatus;
            }());
            Services.MenuStatus = MenuStatus;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
