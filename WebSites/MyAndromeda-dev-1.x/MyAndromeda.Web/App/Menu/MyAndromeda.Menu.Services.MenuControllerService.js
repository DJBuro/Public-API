/// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
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
                    var _this = this;
                    var internal = this, dataSource = this.menuService.menuItemService.dataSource;
                    this.listView = $(this.options.ids.listViewId).kendoListView({
                        dataSource: dataSource,
                        template: kendo.template(this.options.listview.template),
                        editTemplate: kendo.template(this.options.listview.editTemplate),
                        save: function (e) {
                            console.log("save");
                            var listView = internal.listView, dataSource = internal.menuService.menuItemService.dataSource, item = e.item, model = e.model;
                            //prevent the sync being called on the data source
                            e.preventDefault();
                            //close the list view editor
                            _this.closeEditItem.onNext(model);
                            dataSource.trigger("change");
                            //d.anyDirtyItems();
                        },
                        edit: function (e) {
                            console.log("edit");
                            //physical HTML element
                            var menuItemElement = $(e.item), 
                            //data-id attribute 
                            id = menuItemElement.data("id"), 
                            //unique name encase batch mode is enabled
                            uploadId = "#files" + id, advancedUploadId = "#cropFiles" + id;
                            //need to find the observable model
                            var menuItem = internal.menuService.menuItemService.findById(id);
                            _this.openEditItem.onNext(menuItem);
                            internal.initEditListViewItem(uploadId, advancedUploadId, menuItem);
                        },
                        cancel: function (e) {
                            var model = e.model;
                            _this.closeEditItem.onNext(model);
                        }
                    }).data("kendoListView");
                    //public openEditItem: Rx.BehaviorSubject<Models.IMenuItemObservable>;
                    //public closeEditItem: Rx.BehaviorSubject<Models.IMenuItemObservable>;
                    this.openEditItem = new Rx.BehaviorSubject(null);
                    this.closeEditItem = new Rx.BehaviorSubject(null);
                    this.closeEditItem.where(function (e) { return e !== null; }).subscribe(function (e) {
                        var listView = _this.listView;
                        listView._closeEditable(true);
                    });
                    var changeHandler = function (e) {
                        var menuItem = this, webNameField = "WebName", descriptionField = "WebDescription", webSequenceField = "WebSequence";
                        console.log("change k");
                        console.log(e);
                        var relatedItems = internal.menuService.menuItemService.getRelatedItems([menuItem]);
                        console.log("related = " + relatedItems.length);
                        if (relatedItems.length === 0) {
                            //don't need to care... run away
                            return;
                        }
                        if (e.field === webNameField) {
                            var newVal = menuItem.get(webNameField);
                            relatedItems.forEach(function (item, index) {
                                item.set(webNameField, newVal);
                            });
                        }
                        if (e.field === descriptionField) {
                            var newVal = menuItem.get(descriptionField);
                            relatedItems.forEach(function (item, index) {
                                item.set(descriptionField, newVal);
                            });
                        }
                        if (e.field === webSequenceField) {
                            var newVal = menuItem.get(webSequenceField);
                            relatedItems.forEach(function (item, index) {
                                item.set(webSequenceField, newVal);
                            });
                        }
                    };
                    var altered = Rx.Observable.combineLatest(this.openEditItem, this.closeEditItem, function (v1, v2) {
                        return {
                            opened: v1,
                            closed: v2
                        };
                    });
                    altered.subscribe(function (c) {
                        console.log("opened / closed item");
                        if (c.opened === null) {
                            return;
                        }
                        if (c.closed !== null) {
                            c.closed.unbind("change", changeHandler);
                        }
                        console.log("add change handler");
                        c.opened.bind("change", changeHandler);
                    });
                };
                //list view item events: 
                MenuControllerService.prototype.initListViewItemEvents = function () {
                    var _this = this;
                    var listViewId = this.options.ids.listViewId;
                    var internal = this;
                    //watch for the edit button clicked.
                    $(listViewId).on("click", ".k-button-edit", function (e) {
                        e.preventDefault();
                        console.log("watching edits");
                        var row = $(this).closest(".menu-item");
                        //check if any editor rows are active
                        var editorRow = internal.listView.element.find(".k-edit-item");
                        if (editorRow.length == 0) {
                            //internal.listView.cancel();
                            internal.listView.edit(row);
                            return;
                        }
                        var menuItemElement = editorRow, 
                        //data-id attribute 
                        id = menuItemElement.data("id");
                        var menuItem = internal.menuService.menuItemService.findById(id);
                        internal.closeEditItem.onNext(menuItem);
                        internal.listView.edit(row);
                    });
                    $(listViewId).on("click", ".k-button-enable", function (e) {
                        e.preventDefault();
                        var row = $(this).closest(".menu-item");
                        var id = row.data("id");
                        var menuItem = internal.menuService.menuItemService.findById(id);
                        menuItem.Enable();
                    });
                    $(listViewId).on("click", ".k-button-disable", function (e) {
                        e.preventDefault();
                        var row = $(this).closest(".menu-item");
                        var id = row.data("id");
                        var menuItem = internal.menuService.menuItemService.findById(id);
                        menuItem.Disable();
                    });
                    //watch for any save event
                    $(listViewId).on("click", ".k-button-save", function (e) {
                        e.preventDefault();
                        //save changes
                        _this.listView.save();
                    });
                    //watch for any cancel event
                    $(listViewId).on("click", ".k-button-cancel", function (e) {
                        e.preventDefault();
                        _this.listView.cancel();
                    });
                };
                MenuControllerService.prototype.initEditListItemThumbnailUpload = function (thumbnailUploadElementId, folder, menuItem) {
                    var _this = this;
                    var uploadBox = $(thumbnailUploadElementId).kendoUpload({
                        async: {
                            saveUrl: Menu.Settings.Routes.MenuItems.SaveImageUrl + folder,
                            //removeUrl: removeFile,
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
                        //response will contain all of the thumb elements;
                        for (var i = 0; i < e.response.length; i++) {
                            var thumb = e.response[i];
                            thumbData.push(thumb);
                        }
                        //menu item is an observable, ergo updating the array should make it happy. 
                        menuItem.set("Thumbs", thumbData);
                        var relatedItems = _this.menuService.menuItemService.getRelatedItems([menuItem]);
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
                };
                MenuControllerService.prototype.initEditListViewAdvancedThumbnail = function (advancedThumbnailUploadElementId, folder) {
                    var advancedUploadBox;
                    var onSelect = function (selectArgs) {
                        var fileList = selectArgs.files;
                        fileList.forEach(function (value, index) {
                            var fileReader = new FileReader();
                            fileReader.onload = function (onLoadArgs) {
                                var image = $('<img />').attr("src", fileReader.result);
                                var imageHolder = advancedUploadBox.element.closest(".k-widget").find(".imageHolder");
                                imageHolder.append(image);
                            };
                            fileReader.readAsDataURL(value.rawFile);
                        });
                    };
                    advancedUploadBox = $(advancedThumbnailUploadElementId).kendoUpload({
                        async: {
                            saveUrl: Menu.Settings.Routes.MenuItems.SaveImageUrl + folder,
                            //removeUrl: removeFile,
                            autoUpload: false,
                            batch: false
                        },
                        template: $("#thumbnails-crop-template").html(),
                        select: onSelect
                    }).data("kendoUpload");
                };
                MenuControllerService.prototype.initEditListViewRemoveThumbnails = function (removeFile, menuItem) {
                    var _this = this;
                    //remove thumbnail
                    $(this.options.ids.listViewId).on("click", ".k-button-remove-thumb", function () {
                        var itemUrl = $(_this).closest(".thumb").data("fileName");
                        var call = $.ajax({
                            url: removeFile,
                            type: "POST",
                            dataType: "json",
                            data: { fileName: itemUrl }
                        });
                        call.done(function (e) {
                            _this.menuService.menuItemService.removeThumb(menuItem, itemUrl);
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
                        var relatedItems = _this.menuService.menuItemService.getRelatedItems([menuItem]);
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
                    });
                };
                MenuControllerService.prototype.initEditListViewFlocking = function (menuItem) {
                    menuItem.bind("change", function (e) { });
                };
                MenuControllerService.prototype.initEditListViewItem = function (thumbnailUploadElementId, advancedThumbnailUploadElementId, menuItem) {
                    var folder = "?folderPath=" + menuItem.Id; //folder to store the image in 
                    var removeFile = Menu.Settings.Routes.MenuItems.RemoveImageUrl + folder; // this.options.actionUrls.removeImageUrl + folder;
                    /* setup thumbnails */
                    this.initEditListItemThumbnailUpload(thumbnailUploadElementId, folder, menuItem);
                    this.initEditListViewAdvancedThumbnail(advancedThumbnailUploadElementId, folder);
                    this.initEditListViewRemoveThumbnails(removeFile, menuItem);
                    /* setup item edit */
                    this.initEditListViewFlocking(menuItem);
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
                    //set in the initialization 
                    //this.initListViewEvents();
                    this.initListViewItemEvents();
                    this.initPager();
                    this.initMenuStatusView();
                    this.initHubChanges();
                };
                MenuControllerService.MaxLengthMessage = "The max length for the field is {0} characters. Current count: {1}";
                MenuControllerService.EditingNewItemMessage = "You are navigating away from the current item, please save changes or press cancel to the current item.";
                return MenuControllerService;
            }());
            Services.MenuControllerService = MenuControllerService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
