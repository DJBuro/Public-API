﻿module MyAndromeda.Menu.Services {
    Menu.Angular.ServicesInitilizations.push((app) => {
        app.factory(PublishingService.Name, [
            () => {
                return new PublishingService(
                    {
                        publishPanel: {
                            mainButtonId: "#doesnotexist",
                            publishPanelId: "#publishPanel",
                            publishUrlPath: Settings.Routes.Publish
                        }
                    }, null);
            }
        ]);
    });

    export class PublishingService {
        public static Name: string = "PublishingService"

        private model: kendo.data.ObservableObject;
        public options: Models.IMenuPublishOptions;        
        private menuService: MenuService;


        constructor(options: Models.IMenuPublishOptions, menuservice: MenuService) {
            var internal = this;
            this.options = options;
            this.menuService = menuservice;

            this.model = kendo.observable({
                publishThumbnails: true,
                publishMenu: true,
                publishNow: "now",
                publishLater: false,
                publishOn: new Date(),
                minDate: new Date(),
                cancel: function () {
                    $(options.publishPanel.publishPanelId).kendoMobileModalView("close")
                },
                publishLaterClick: function () {
                    var publishThumbs: boolean = internal.model.get("publishThumbnails"),
                        publishMenu: boolean = internal.model.get("publishMenu"),
                        publishNowSelection: string = internal.model.get("publishNow"),
                        publishOn = internal.model.get("publishOn"),
                        publishNow = publishNowSelection.toLocaleLowerCase() === "now";

                    Logger.Notify("publish now" + publishNowSelection);

                    internal.publishNow(publishNow, publishMenu, publishThumbs, publishOn);
                }
            });

            this.model.bind("change", (e) => {
                if (e.field !== 'publishNow') return;
                var now = this.model.get("publishNow");

                this.model.set("publishLater", now === "later");
            });

            this.init();
        }

        public publishNow(now: boolean, menu: boolean, thumbnails: boolean, date: Date): void {
            var internal = this,
                data = {
                    menu: menu,
                    now: now,
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
        }

        public openWindow(): void {
            var options = this.options.publishPanel;
            $(options.publishPanelId).kendoMobileModalView("open");
        }
        public closeWindow(): void {
            var options = this.options.publishPanel;
            $(options.publishPanelId).kendoMobileModalView("close");
        }

        public init(): void {
            var internal = this,
                options = this.options.publishPanel;

            $(options.publishPanelId).kendoMobileModalView();

            kendo.bind(options.publishPanelId, this.model);

            $(options.mainButtonId).on("click", (e) => {
                e.preventDefault();

                internal.openWindow();
            });
        }
    }
} 