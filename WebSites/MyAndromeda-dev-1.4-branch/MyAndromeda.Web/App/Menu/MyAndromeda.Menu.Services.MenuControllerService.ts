/// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
module MyAndromeda.Menu.Services {

    export class MenuControllerService {
        public static MaxLengthMessage: string =
            "The max length for the field is {0} characters. Current count: {1}";
        public static EditingNewItemMessage: string =
            "You are navigating away from the current item, please save changes or press cancel to the current item.";

        private storeHub: MyAndromeda.Hubs.StoreHub;
        private listView: kendo.ui.ListView;
        private menuService: MenuService;
        private publishingService: Services.PublishingService;

        private modelDefinition: kendo.data.Model;

        public options: Models.IMenuServiceOptions;

        public openEditItem: Rx.BehaviorSubject<Models.IMenuItemObservable>; 
        public closeEditItem: Rx.BehaviorSubject<Models.IMenuItemObservable>;

        constructor(options: Models.IMenuServiceOptions) {
            this.options = options;
            this.menuService = new MenuService(options);
            this.publishingService = new Services.PublishingService(options, this.menuService);
        }

        private initPager(): void {
            $(this.options.ids.pagerId).kendoPager({
                dataSource: this.menuService.menuItemService.dataSource
            });
        }

        private initListView(): void {
            var internal = this,
                dataSource = this.menuService.menuItemService.dataSource;

            this.listView = $(this.options.ids.listViewId).kendoListView({
                dataSource: dataSource,
                template: kendo.template(this.options.listview.template),
                editTemplate: kendo.template(this.options.listview.editTemplate),
                save: (e) => {
                    console.log("save");
                    var listView = <any>internal.listView,
                        dataSource = internal.menuService.menuItemService.dataSource,
                        item = e.item,
                        model: any = e.model;

                    //prevent the sync being called on the data source
                    e.preventDefault();

                    //close the list view editor
                    this.closeEditItem.onNext(model);

                    dataSource.trigger("change");
                    //d.anyDirtyItems();
                },
                edit: (e) => {
                    console.log("edit");
                    //physical HTML element
                    var menuItemElement = $(e.item),
                        //data-id attribute 
                        id = menuItemElement.data("id"),
                        //unique name encase batch mode is enabled
                        uploadId = "#files" + id,
                        advancedUploadId = "#cropFiles" + id;
                    //need to find the observable model
                    var menuItem = internal.menuService.menuItemService.findById(id);

                    this.openEditItem.onNext(menuItem);
                    internal.initEditListViewItem(uploadId, advancedUploadId, menuItem);
                },
                cancel: (e) => {
                    var model: any = e.model;

                    this.closeEditItem.onNext(model);
                }
            }).data("kendoListView");

            //public openEditItem: Rx.BehaviorSubject<Models.IMenuItemObservable>;
            //public closeEditItem: Rx.BehaviorSubject<Models.IMenuItemObservable>;
            this.openEditItem = new Rx.BehaviorSubject<Models.IMenuItemObservable>(null);
            this.closeEditItem = new Rx.BehaviorSubject<Models.IMenuItemObservable>(null);

            this.closeEditItem.where(e=> e !== null).subscribe(e=> {
                var listView = <any>this.listView;
                listView._closeEditable(true);
            });

            var changeHandler = function(e) {
                var menuItem: Models.IMenuItemObservable = this,
                    webNameField = "WebName",
                    descriptionField = "WebDescription",
                    webSequenceField = "WebSequence"; 

                console.log("change k");
                console.log(e); 

                var relatedItems = internal.menuService.menuItemService.getRelatedItems([menuItem]);

                console.log("related = " + relatedItems.length);
                if (relatedItems.length === 0) { 
                    //don't need to care... run away
                    return;
                }

                if (e.field === webNameField)
                {
                    var newVal = menuItem.get(webNameField);

                    relatedItems.forEach((item, index) => {
                        item.set(webNameField, newVal);
                    });
                }
                if (e.field === descriptionField)
                {
                    var newVal = menuItem.get(descriptionField);

                    relatedItems.forEach((item, index) => {
                        item.set(descriptionField, newVal);
                    });
                }
                if (e.field === webSequenceField)
                {
                    var newVal = menuItem.get(webSequenceField); 

                    relatedItems.forEach((item, index) => {
                        item.set(webSequenceField, newVal);
                    });
                }

            };
                
            var altered = Rx.Observable.combineLatest(this.openEditItem, this.closeEditItem, (v1: Models.IMenuItemObservable, v2: Models.IMenuItemObservable) => {
                return {
                    opened: v1,
                    closed: v2
                };
            });

            altered.subscribe(c => {
                console.log("opened / closed item")
                if (c.opened === null) { return; }

                if (c.closed !== null) {
                    c.closed.unbind("change", changeHandler);
                }

                console.log("add change handler");
                c.opened.bind("change", changeHandler);
            });
        }


        //list view item events: 
        private initListViewItemEvents(): void {
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
                var menuItem: any = internal.menuService.menuItemService.findById(id);


                internal.closeEditItem.onNext(menuItem);

                internal.listView.edit(row);
            });
            $(listViewId).on("click", ".k-button-enable", function(e) {
                e.preventDefault();
                var row = $(this).closest(".menu-item");
                var id = row.data("id");
                var menuItem = internal.menuService.menuItemService.findById(id);

                menuItem.Enable();
            });
            $(listViewId).on("click", ".k-button-disable",function(e) {
                e.preventDefault();
                
                var row = $(this).closest(".menu-item");
                var id = row.data("id");
                var menuItem = internal.menuService.menuItemService.findById(id);

                menuItem.Disable();
            });

            //watch for any save event
            $(listViewId).on("click", ".k-button-save", (e) => {
                e.preventDefault();
                //save changes
                this.listView.save();
            });
            //watch for any cancel event
            $(listViewId).on("click", ".k-button-cancel", (e) => {
                e.preventDefault();
                this.listView.cancel();
            });
        }

        private initEditListItemThumbnailUpload(thumbnailUploadElementId: string, folder: string, menuItem: Models.IMenuItemObservable)
        {
            
            var uploadBox: kendo.ui.Upload = $(thumbnailUploadElementId).kendoUpload({
                async: {
                    saveUrl: Settings.Routes.MenuItems.SaveImageUrl + folder,
                    //removeUrl: removeFile,
                    autoUpload: true,
                    batch: false
                },
                showFileList: false
            }).data("kendoUpload");

            //when the service comes back and says its complete + succeeded 
            uploadBox.bind("success", (e) => {
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

                var relatedItems = this.menuService.menuItemService.getRelatedItems([menuItem]);

                if (relatedItems.length > 0) {
                    var message = kendo.format("Add this image to {0} other records?", relatedItems.length);
                    var assignToSimilarItems = confirm(message);

                    if (assignToSimilarItems) {
                        relatedItems.forEach((item: Models.IMenuItemObservable) => {
                            item.set("Thumbs", thumbData);
                        });
                    }
                }
            });
        }

        private initEditListViewAdvancedThumbnail(advancedThumbnailUploadElementId: string, folder:string) : void
        {
            var advancedUploadBox: kendo.ui.Upload;
            var onSelect = function (selectArgs) {
                var fileList = <any[]>selectArgs.files;

                fileList.forEach(function (value, index) {
                    var fileReader = new FileReader();

                    fileReader.onload = function (onLoadArgs) {
                        var image = $('<img />').attr("src", fileReader.result);
                        var imageHolder = advancedUploadBox.element.closest(".k-widget").find(".imageHolder");
                        imageHolder.append(image);
                    }
                    fileReader.readAsDataURL(<Blob>value.rawFile);
                });
            };

            advancedUploadBox = $(advancedThumbnailUploadElementId).kendoUpload({
                async: {
                    saveUrl: Settings.Routes.MenuItems.SaveImageUrl + folder,
                    //removeUrl: removeFile,
                    autoUpload: false,
                    batch: false
                },
                template: $("#thumbnails-crop-template").html(),
                select: onSelect
            }).data("kendoUpload");
        }

        private initEditListViewRemoveThumbnails(removeFile: string, menuItem: Models.IMenuItemObservable): void {
            //remove thumbnail
            $(this.options.ids.listViewId).on("click", ".k-button-remove-thumb", () => {
                var itemUrl = $(this).closest(".thumb").data("fileName");

                var call = $.ajax({
                    url: removeFile,
                    type: "POST",
                    dataType: "json",
                    data: { fileName: itemUrl }
                });

                call.done((e) => {
                    this.menuService.menuItemService.removeThumb(menuItem, itemUrl);
                });

                call.fail((e) => {
                    alert("There was an error processing the request. Please try again");
                });
            });

            //remove all thumbnails
            $(this.options.ids.listViewId).on("click", ".k-button-removeall-thumb", () => {
                //var itemUrl = $(this).closest(".thumb").data("fileName");

                //if(!confirm("Are you sure you want to remove all of this items thumbnails"))
                //{
                //    return false;
                //}

                var removeFrom: Models.IMenuItem[] = [menuItem];
                var relatedItems = this.menuService.menuItemService.getRelatedItems([menuItem]);

                if (relatedItems.length > 0) {
                    var message = kendo.format("remove this image from {0} other records? You need to cancel all changes if you want to undo!", relatedItems.length);
                    var assignToSimilarItems = confirm(message);

                    if (assignToSimilarItems) {
                        relatedItems.forEach((item) => {
                            removeFrom.push(item);
                        });
                    }
                }

                removeFrom.forEach((item) => {
                    while (item.Thumbs.length > 0) {
                        item.Thumbs.pop();
                    }
                });
            });
        }

        private initEditListViewFlocking( menuItem: Models.IMenuItemObservable)
        {
            menuItem.bind("change", (e) => { });
        }

        private initEditListViewItem(thumbnailUploadElementId: string, advancedThumbnailUploadElementId: string, menuItem: Models.IMenuItemObservable): void {
            var folder = "?folderPath=" + menuItem.Id; //folder to store the image in 
            var removeFile = Settings.Routes.MenuItems.RemoveImageUrl + folder; // this.options.actionUrls.removeImageUrl + folder;

            /* setup thumbnails */
            this.initEditListItemThumbnailUpload(thumbnailUploadElementId, folder, menuItem);
            this.initEditListViewAdvancedThumbnail(advancedThumbnailUploadElementId, folder);
            this.initEditListViewRemoveThumbnails(removeFile, menuItem);

            /* setup item edit */
            this.initEditListViewFlocking(menuItem);
        }

        private initHubChanges(): void {
            var internal = this,
                hub = MyAndromeda.Hubs.StoreHub.GetInstance(this.options.routeParameters);

            hub.bind(MyAndromeda.Hubs.StoreHub.MenuItemChangeEvent, (context) => {
                console.log("i have changes :)");
                //console.log(context);

                internal.menuService.extendMenuItemData(context.EditedItems);

                var updateDataSource = context.Section === "Sequence";
                internal.menuService.menuItemService.updateItems(context.EditedItems, updateDataSource);



                if (!$.isEmptyObject(menuFilterController.SORTFILTER)) {
                    var currentSort = internal.menuService.menuItemService.dataSource.sort();
                    if (currentSort != menuFilterController.SORTFILTER) {
                        internal.menuService.menuItemService.dataSource.sort(menuFilterController.SORTFILTER);
                    }
                }
            });

        }

        private setEditorDefaultBehaviour(): void {
            var editor = <any>kendo.ui.Editor,
                tools = editor.defaultTools;


            tools["insertLineBreak"].options.shift = true;
            tools["insertParagraph"].options.shift = true;
        }

        private initMenuStatusView(): void {
            var id = this.options.ids.statusViewId,
                status = this.menuService.menuItemService.menuStatus;
            if (id) {
                kendo.bind($(id), status.observable);
            }
        }

        public init(): void {
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
        }
    }

} 