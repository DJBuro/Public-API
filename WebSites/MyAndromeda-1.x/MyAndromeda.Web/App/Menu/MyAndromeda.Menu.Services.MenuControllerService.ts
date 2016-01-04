/// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
module MyAndromeda.Menu.Services {

    export class MenuControllerService {
        public static MaxLengthMessage: string = "The max length for the field is {0} characters. Current count: {1}";
        public static EditingNewItemMessage: string = "You are navigating away from the current item, please save changes or press cancel to the current item.";

        private storeHub: MyAndromeda.Hubs.StoreHub;
        private listView: kendo.ui.ListView;
        private menuService: MenuService;
        private publishingService: Services.PublishingService;

        private modelDefinition: kendo.data.Model;

        public options: Models.IMenuServiceOptions;

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
                    var listView = <any>internal.listView,
                        dataSource = internal.menuService.menuItemService.dataSource,
                        item = e.item,
                        model = e.model;
                    //prevent the sync being called on the data source
                    e.preventDefault();
                    //close the list view editor
                    internal.closeCurrentEditedRow();
                    dataSource.trigger("change");
                    //d.anyDirtyItems();
                },
                edit: (e) => {
                    var editors = $(".k-editor");
                    internal.manageHtmlEditors(editors);
                }
            }).data("kendoListView");
        }

        private cleanHtmlEvent(e: kendo.ui.EditorPasteEvent) {
            e.html = $(e.html).text();
        }

        private manageHtmlEditors(query: JQuery) {
            var internal = this,
                editors = $.map(query, (element) => { return $(element).data("kendoEditor"); });

            editors.forEach((editor) => {
                editor.unbind("paste", internal.cleanHtmlEvent);
                editor.bind("paste", internal.cleanHtmlEvent);
            });
        }

        private closeCurrentEditedRow() {
            var listView = <any>this.listView;
            listView._closeEditable(true);
        }

        private initListViewEvents(): void {
            var internal = this;

            this.listView.bind("edit", function (e) {
                //physical html element
                var menuItemElement = $(e.item),
                    //data-id attribute 
                    id = menuItemElement.data("id"),
                    //unique name incase batch mode is enabled
                    uploadId = "#files" + id,
                    advancedUploadId = "#cropFiles" + id;
                //need to find the observable model
                var menuItem = internal.menuService.menuItemService.findById(id);

                internal.initEditListViewItem(uploadId, advancedUploadId, menuItem);
            });
        }

        //list view item events: 
        private initListViewItemEvents(): void {
            var listViewId = this.options.ids.listViewId,
                internal = this;

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
        }

        private initEditListViewItem(elementId: string, advancedElementId: string, menuItem: Models.IMenuItemObservable): void {
            var internal = this;
            var folder = "?folderPath=" + menuItem.Id; //folder to store the image in 
            var removeFile = Settings.Routes.MenuItems.RemoveImageUrl + folder; // this.options.actionUrls.removeImageUrl + folder;

            var uploadBox: kendo.ui.Upload = $(elementId).kendoUpload({
                async: {
                    saveUrl: Settings.Routes.MenuItems.SaveImageUrl + folder,
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

                //response will contain all of the thumb elements;
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
                        relatedItems.forEach((item: Models.IMenuItemObservable) => {
                            item.set("Thumbs", thumbData);
                        });
                    }
                }
            });

            var advancedUploadBox: kendo.ui.Upload;
            var onSelect = function (selectArgs) {
                var fileList = <any[]>selectArgs.files;
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
                    }
                    fileReader.readAsDataURL(<Blob>value.rawFile);
                });
            };

            advancedUploadBox = $(advancedElementId).kendoUpload({
                async: {
                    saveUrl: Settings.Routes.MenuItems.SaveImageUrl + folder,
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
            })

            //remove all thumbnails
            $(this.options.ids.listViewId).on("click", ".k-button-removeall-thumb", function () {
                //var itemUrl = $(this).closest(".thumb").data("fileName");

                //if(!confirm("Are you sure you want to remove all of this items thumbnails"))
                //{
                //    return false;
                //}

                var removeFrom: Models.IMenuItem[] = [menuItem];
                var relatedItems = internal.menuService.menuItemService.getRelatedItems([menuItem]);
                
                if(relatedItems.length > 0)
                {
                    var message = kendo.format("remove this image from {0} other records? You need to cancel all changes if you want to undo!", relatedItems.length);
                    var assignToSimilarItems = confirm(message);

                    if(assignToSimilarItems)
                    {
                        relatedItems.forEach((item) => {
                            removeFrom.push(item);
                        });    
                    }
                }

                removeFrom.forEach((item) => {
                    while(item.Thumbs.length > 0)
                    {
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
          })
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
            this.initListViewEvents();
            this.initListViewItemEvents();

            this.initPager();
            this.initMenuStatusView();

            this.initHubChanges();
        }
    }

} 