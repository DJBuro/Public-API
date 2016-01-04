var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(PublishingService.Name, [
                    function () {
                        return new PublishingService({
                            publishPanel: {
                                mainButtonId: "#doesnotexist",
                                publishPanelId: "#publishPanel",
                                publishUrlPath: Menu.Settings.Routes.Publish
                            }
                        }, null);
                    }
                ]);
            });

            var PublishingService = (function () {
                function PublishingService(options, menuservice) {
                    var _this = this;
                    var internal = this;
                    this.options = options;
                    this.menuService = menuservice;

                    this.model = kendo.observable({
                        publishThumbnails: true,
                        publishMenu: true,
                        publishNow: true,
                        publishLater: false,
                        publishOn: new Date(),
                        minDate: new Date(),
                        cancel: function () {
                            $(options.publishPanel.publishPanelId).kendoMobileModalView("close");
                        },
                        publishNowClick: function () {
                            internal.publishNow(true, true, new Date());
                        },
                        publishLaterClick: function () {
                            var publishThumbs = internal.model.get("publishThumbnails"), publishMenu = internal.model.get("publishMenu"), publishOn = internal.model.get("publishOn");

                            internal.publishNow(publishMenu, publishThumbs, publishOn);
                        },
                        publishThumbnailsClick: function () {
                            internal.publishNow(false, true, new Date());
                        }
                    });

                    this.model.bind("change", function (e) {
                        if (e.field !== 'publishNow')
                            return;
                        var now = _this.model.get("publishNow");

                        _this.model.set("publishLater", now === "later");
                    });

                    this.init();
                }
                PublishingService.prototype.publishNow = function (menu, thumbnails, date) {
                    var internal = this, data = {
                        menu: menu,
                        thumbnails: thumbnails,
                        dateUtc: null
                    };
                    if (this.menuService !== null && this.menuService.menuItemService !== null && this.menuService.menuItemService.anyDirtyItems()) {
                        internal.closeWindow();
                        alert("Please save the items before publish");
                        return;
                    }

                    if (date > new Date()) {
                        //toJson will change a local date time to UTC format that is accepted by the json parser in .NET
                        data.dateUtc = date.toJSON();
                    }

                    $.ajax({
                        url: internal.options.publishPanel.publishUrlPath,
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        cache: false,
                        data: JSON.stringify(data),
                        success: function (data) {
                            internal.closeWindow();
                        }
                    });
                };

                PublishingService.prototype.openWindow = function () {
                    var options = this.options.publishPanel;
                    $(options.publishPanelId).kendoMobileModalView("open");
                };
                PublishingService.prototype.closeWindow = function () {
                    var options = this.options.publishPanel;
                    $(options.publishPanelId).kendoMobileModalView("close");
                };

                PublishingService.prototype.init = function () {
                    var internal = this, options = this.options.publishPanel;

                    $(options.publishPanelId).kendoMobileModalView();

                    kendo.bind(options.publishPanelId, this.model);

                    $(options.mainButtonId).on("click", function (e) {
                        e.preventDefault();

                        internal.openWindow();
                    });
                };
                PublishingService.Name = "PublishingService";
                return PublishingService;
            })();
            Services.PublishingService = PublishingService;
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
