var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        /// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
        (function (Services) {
            var MenuControllerService = (function () {
                function MenuControllerService(options) {
                    this.options = options;
                    this.menuService = new Services.MenuService(options);
                    this.publishingService = new Services.PublishingService(options, this.menuService);
                }
                MenuControllerService.prototype.initPager = function () {
                    $(this.options.ids.pagerId).kendoPager({
                        dataSource: this.menuService.menuItemService.dataSource
                    });
                };

                MenuControllerService.prototype.initListView = function () {
                    var internal = this, dataSource = this.menuService.menuItemService.dataSource;
                    this.listView = $(this.options.ids.listViewId).kendoListView({
                        dataSource: dataSource,
                        template: kendo.template(this.options.listview.template),
                        editTemplate: kendo.template(this.options.listview.editTemplate),
                        save: function (e) {
                            var listView = internal.listView, dataSource = internal.menuService.menuItemService.dataSource, item = e.item, model = e.model;

                            //prevent the sync being called on the data source
                            e.preventDefault();

                            //close the list view editor
                            internal.closeCurrentEditedRow();
                            dataSource.trigger("change");
                            //d.anyDirtyItems();
                        },
                        edit: function (e) {
                            var editors = $(".k-editor");
                            internal.manageHtmlEditors(editors);
                        }
                    }).data("kendoListView");
                };

                MenuControllerService.prototype.cleanHtmlEvent = function (e) {
                    e.html = $(e.html).text();
                };

                MenuControllerService.prototype.manageHtmlEditors = function (query) {
                    var internal = this, editors = $.map(query, function (element) {
                        return $(element).data("kendoEditor");
                    });

                    editors.forEach(function (editor) {
                        editor.unbind("paste", internal.cleanHtmlEvent);
                        editor.bind("paste", internal.cleanHtmlEvent);
                    });
                };

                MenuControllerService.prototype.closeCurrentEditedRow = function () {
                    var listView = this.listView;
                    listView._closeEditable(true);
                };

                MenuControllerService.prototype.initListViewEvents = function () {
                    var internal = this;

                    this.listView.bind("edit", function (e) {
                        //physical html element
                        var menuItemElement = $(e.item), id = menuItemElement.data("id"), uploadId = "#files" + id, advancedUploadId = "#cropFiles" + id;

                        //need to find the observable model
                        var menuItem = internal.menuService.menuItemService.findById(id);

                        internal.initEditListViewItem(uploadId, advancedUploadId, menuItem);
                    });
                };

                //list view item events:
                MenuControllerService.prototype.initListViewItemEvents = function () {
                    var listViewId = this.options.ids.listViewId, internal = this;

                    //watch for the edit button clicked.
                    $(listViewId).on("click", ".k-button-edit", function (e) {
                        e.preventDefault();
                        var row = $(this).closest(".menu-item");

                        //check if any editor rows are active
                        var editorRow = internal.listView.element.find(".k-edit-item");

                        if (editorRow.length == 0) {
                            //internal.listView.cancel();
                            internal.listView.edit(row);
                            return;
                        }

                        internal.closeCurrentEditedRow();
                        internal.listView.edit(row);
                    });

                    //watch for any save event
                    $(listViewId).on("click", ".k-button-save", function (e) {
                        e.preventDefault();

                        //save changes
                        internal.listView.save();
                    });

                    //watch for any cancel event
                    $(listViewId).on("click", ".k-button-cancel", function (e) {
                        e.preventDefault();
                        internal.listView.cancel();
                    });
                };

                MenuControllerService.prototype.initEditListViewItem = function (elementId, advancedElementId, menuItem) {
                    var internal = this;
                    var folder = "?folderPath=" + menuItem.Id;
                    var removeFile = Menu.Settings.Routes.MenuItems.RemoveImageUrl + folder;

                    var uploadBox = $(elementId).kendoUpload({
                        async: {
                            saveUrl: Menu.Settings.Routes.MenuItems.SaveImageUrl + folder,
                            removeUrl: removeFile,
                            autoUpload: true,
                            batch: false
                        },
                        showFileList: false
                    }).data("kendoUpload");

                    //when the service comes back and says its complete + succeeded
                    uploadBox.bind("success", function (e) {
                        //effectively the JSON response.
                        var response = e.response;
                        var thumbData = new kendo.data.ObservableArray([]);

                        for (var i = 0; i < e.response.length; i++) {
                            var thumb = e.response[i];
                            thumbData.push(thumb);
                        }

                        //menu item is an observable, ergo updating the array should make it happy.
                        menuItem.set("Thumbs", thumbData);

                        var relatedItems = internal.menuService.menuItemService.getRelatedItems([menuItem]);
                        if (relatedItems.length > 0) {
                            var message = kendo.format("Add this image to {0} other records?", relatedItems.length);
                            var assignToSimilarItems = confirm(message);

                            if (assignToSimilarItems) {
                                relatedItems.forEach(function (item) {
                                    item.set("Thumbs", thumbData);
                                });
                            }
                        }
                    });

                    var advancedUploadBox;
                    var onSelect = function (selectArgs) {
                        var fileList = selectArgs.files;
                        var fileList2 = advancedUploadBox.element.find("input");

                        fileList.forEach(function (value, index) {
                            var fileReader = new FileReader();

                            //console.log("foreach");
                            fileReader.onload = function (onLoadArgs) {
                                //console.log("onload");
                                //console.log(fileReader.result);
                                var image = $('<img />').attr("src", fileReader.result);
                                var imageHolder = advancedUploadBox.element.closest(".k-widget").find(".imageHolder");
                                imageHolder.append(image);
                            };
                            fileReader.readAsDataURL(value.rawFile);
                        });
                    };

                    advancedUploadBox = $(advancedElementId).kendoUpload({
                        async: {
                            saveUrl: Menu.Settings.Routes.MenuItems.SaveImageUrl + folder,
                            removeUrl: removeFile,
                            autoUpload: false,
                            batch: false
                        },
                        template: $("#thumbnails-crop-template").html(),
                        select: onSelect
                    }).data("kendoUpload");

                    //remove thumbnail
                    $(this.options.ids.listViewId).on("click", ".k-button-remove-thumb", function () {
                        var itemUrl = $(this).closest(".thumb").data("fileName");

                        var call = $.ajax({
                            url: removeFile,
                            type: "POST",
                            dataType: "json",
                            data: { fileName: itemUrl }
                        });

                        call.done(function (e) {
                            internal.menuService.menuItemService.removeThumb(menuItem, itemUrl);
                        });

                        call.fail(function (e) {
                            alert("There was an error processing the request. Please try again");
                        });
                    });

                    //remove all thumbnails
                    $(this.options.ids.listViewId).on("click", ".k-button-removeall-thumb", function () {
                        //var itemUrl = $(this).closest(".thumb").data("fileName");
                        //if(!confirm("Are you sure you want to remove all of this items thumbnails"))
                        //{
                        //    return false;
                        //}
                        var removeFrom = [menuItem];
                        var relatedItems = internal.menuService.menuItemService.getRelatedItems([menuItem]);

                        if (relatedItems.length > 0) {
                            var message = kendo.format("remove this image from {0} other records? You need to cancel all changes if you want to undo!", relatedItems.length);
                            var assignToSimilarItems = confirm(message);

                            if (assignToSimilarItems) {
                                relatedItems.forEach(function (item) {
                                    removeFrom.push(item);
                                });
                            }
                        }

                        removeFrom.forEach(function (item) {
                            while (item.Thumbs.length > 0) {
                                item.Thumbs.pop();
                            }
                        });
                        //menuItem.Thumbs.forEach((thumb: Models.IMenuItemThumb) => {
                        //    if (thumb.Url) {
                        //        var call = $.ajax({
                        //            url: removeFile,
                        //            type: "POST",
                        //            dataType: "json",
                        //            data: { fileName: thumb.FileName }
                        //        });
                        //        call.done(function (e) {
                        //            internal.menuService.menuItemService.removeThumb(menuItem, thumb.FileName);
                        //        });
                        //        call.fail(function (e) {
                        //            alert("There was an error processing the request. Please try again");
                        //        });
                        //    }
                        //})
                    });
                };

                MenuControllerService.prototype.initHubChanges = function () {
                    var internal = this, hub = MyAndromeda.Hubs.StoreHub.GetInstance(this.options.routeParameters);

                    hub.bind(MyAndromeda.Hubs.StoreHub.MenuItemChangeEvent, function (context) {
                        console.log("i have changes :)");

                        //console.log(context);
                        internal.menuService.extendMenuItemData(context.EditedItems);

                        var updateDataSource = context.Section === "Sequence";
                        internal.menuService.menuItemService.updateItems(context.EditedItems, updateDataSource);

                        if (!$.isEmptyObject(Services.menuFilterController.SORTFILTER)) {
                            var currentSort = internal.menuService.menuItemService.dataSource.sort();
                            if (currentSort != Services.menuFilterController.SORTFILTER) {
                                internal.menuService.menuItemService.dataSource.sort(Services.menuFilterController.SORTFILTER);
                            }
                        }
                    });
                };

                MenuControllerService.prototype.setEditorDefaultBehaviour = function () {
                    var editor = kendo.ui.Editor, tools = editor.defaultTools;

                    tools["insertLineBreak"].options.shift = true;
                    tools["insertParagraph"].options.shift = true;
                };

                MenuControllerService.prototype.initMenuStatusView = function () {
                    var id = this.options.ids.statusViewId, status = this.menuService.menuItemService.menuStatus;
                    if (id) {
                        kendo.bind($(id), status.observable);
                    }
                };

                MenuControllerService.prototype.init = function () {
                    this.setEditorDefaultBehaviour();
                    this.menuService.init();

                    //setup ui elements & events
                    this.initListView();
                    this.initListViewEvents();
                    this.initListViewItemEvents();

                    this.initPager();
                    this.initMenuStatusView();

                    this.initHubChanges();
                };
                MenuControllerService.MaxLengthMessage = "The max length for the field is {0} characters. Current count: {1}";
                MenuControllerService.EditingNewItemMessage = "You are navigating away from the current item, please save changes or press cancel to the current item.";
                return MenuControllerService;
            })();
            Services.MenuControllerService = MenuControllerService;
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
