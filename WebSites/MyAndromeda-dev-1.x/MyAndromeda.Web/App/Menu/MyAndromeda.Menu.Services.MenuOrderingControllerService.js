var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
            });
            var Positioning = (function () {
                function Positioning() {
                }
                Positioning.BEFORE = "before";
                Positioning.AFTER = "after";
                Positioning.INTO = "into";
                Positioning.REPLACE = "replace";
                return Positioning;
            }());
            var MenuOrderingControllerService = (function () {
                function MenuOrderingControllerService(options) {
                    var internal = this;
                    this.options = options;
                    this.menuService = new Services.MenuService(this.options);
                    this.publishingService = new Services.PublishingService(this.options, this.menuService);
                    this.positionWindowViewModel = kendo.observable({
                        length: function () {
                            var items = internal.menuService.menuItemService.dataSource.view();
                            var total = items.length;
                            return total;
                        },
                        onMoveToFirst: function () {
                            var item = this.get("item");
                            var firstItem = internal.menuService.menuItemService.dataSource.view()[0];
                            internal.movePlaces(firstItem, item, Positioning.BEFORE);
                            internal.updateWindowUi();
                        },
                        onMoveToLast: function () {
                            var menuItemService = internal.menuService.menuItemService;
                            var view = internal.menuService.menuItemService.dataSource.view();
                            var item = this.get("item");
                            var lastItem = view[this.get("length") - 1];
                            internal.movePlaces(lastItem, item, Positioning.AFTER);
                            internal.updateWindowUi();
                        },
                        onMoveItemDown: function () {
                            var item = this.get("item"), index = parseInt(this.get("index")), view = internal.menuService.menuItemService.dataSource.view(), vm = internal.positionWindowViewModel;
                            vm.set("index", index < view.length ? index + 1 : index);
                            internal.moveItemByTextBox();
                        },
                        onMoveItemUp: function () {
                            var item = this.get("item"), index = parseInt(this.get("index")), vm = internal.positionWindowViewModel;
                            vm.set("index", index > 1 ? index - 1 : index);
                            internal.moveItemByTextBox();
                        },
                        onMoveToPositionClick: function () {
                            var item = this.get("item"), index = this.get("index"), view = internal.menuService.menuItemService.dataSource.view(), vm = internal.positionWindowViewModel;
                            internal.moveItemByTextBox();
                        },
                        saveChanges: function () {
                            internal.closeWindow(true);
                        },
                        cancelChanges: function () {
                            internal.closeWindow(false);
                            internal.menuService.menuItemService.dataSource.cancelChanges();
                        },
                        displayTest: function () {
                            var value = this.get("item");
                            if (value === null) {
                                return "nothing";
                            }
                            return value.DisplayName();
                        },
                        item: null,
                        itemName: "-",
                        itemIndex: 0,
                        //index of the numeric input box
                        index: 0
                    });
                }
                MenuOrderingControllerService.prototype.updateWindowUi = function () {
                    var internal = this;
                    internal.normalize();
                    internal.menuService.menuItemService.dataSource.sort({ field: "WebSequence", dir: "asc" });
                    var vm = this.positionWindowViewModel;
                    var item = vm.get("item");
                    vm.set("itemName", item.DisplayName());
                    vm.set("itemIndex", item.Index());
                    vm.set("index", item.Index());
                };
                MenuOrderingControllerService.prototype.moveItemByTextBox = function () {
                    var vm = this.positionWindowViewModel, item = vm.get("item"), internal = this, view = this.menuService.menuItemService.dataSource.view();
                    var value = $("#numericIndexSelector").val();
                    if (value < 0) {
                        return;
                    }
                    if (value == vm.get("itemIndex")) {
                        return;
                    }
                    vm.set("index", parseInt(value));
                    var a = view[value - 1];
                    this.movePlaces(a, item, Positioning.INTO);
                    this.updateWindowUi();
                };
                MenuOrderingControllerService.prototype.initListView = function () {
                    var internal = this;
                    this.listView = $(this.options.ids.listViewId).kendoListView({
                        autoBind: false,
                        dataSource: this.menuService.menuItemService.dataSource,
                        template: kendo.template(this.options.listview.template),
                        editTemplate: kendo.template(this.options.listview.editTemplate)
                    }).data("kendoListView");
                };
                MenuOrderingControllerService.prototype.highlighOriginalItem = function (item) {
                    $(item)
                        .animate({ "margin-left": "10px", "opacity": 0.4 }, 100)
                        .animate({ "margin-left": "0px" }, 100)
                        .css({ "border-color": "#E7001B", "border-width": "4px" });
                };
                MenuOrderingControllerService.prototype.UnhighlightOriginalItem = function (item) {
                    $(item).css({ "border-width": "0px" });
                };
                MenuOrderingControllerService.prototype.initListViewEvents = function () {
                    var internal = this;
                    var ds = internal.menuService.menuItemService.dataSource;
                    var draggableSettings = {
                        filter: ".menu-item",
                        hint: internal.generateHint,
                        group: "listViewItems",
                        //container: "html",
                        holdToDrag: true,
                        hold: function (e) {
                            if (e.which === 2) {
                                e.preventDefault();
                            }
                            internal.highlighOriginalItem(e.currentTarget);
                        },
                        dragcancel: function (e) {
                            internal.UnhighlightOriginalItem(e.currentTarget);
                        },
                        dragstart: function (e) {
                        },
                        dragend: function (e) {
                            internal.UnhighlightOriginalItem(e.currentTarget);
                        },
                        drag: function (e) {
                        }
                    };
                    $(internal.options.ids.listViewId).kendoDraggable(draggableSettings);
                    var dropTargetAreaOptions = {
                        filter: ".menu-item",
                        group: "listViewItems",
                        hint: internal.generateHint,
                        cancel: internal.destroyDraggable,
                        dragenter: function (e) {
                            var $e = e.dropTarget;
                            //if ($e.is(".moving")) { return; }
                            $($e).css({ "border-color": "#A5E400" });
                            $($e).animate({
                                "margin-left": "40px",
                                "opacity": 0.9,
                                "border-width": 4
                            }, 200);
                        },
                        dragleave: function (e) {
                            var $e = e.dropTarget;
                            $($e).animate({
                                "margin-left": "0px",
                                "opacity": 1,
                                "border-width": 0
                            }, 200);
                        },
                        drop: function (e) {
                            var draggableThing = e.draggable;
                            var hint = draggableThing.hint;
                            var draggableDataItem = ds.getByUid(hint.data("uid")), //ds.getByUid(e.draggable.hint.data("uid")),
                            dropTargetDataItem = ds.getByUid(e.dropTarget.data("uid"));
                            internal.movePlaces(dropTargetDataItem, draggableDataItem, Positioning.BEFORE);
                            internal.normalize();
                            ds.sort({ field: "WebSequence", dir: "asc" });
                            internal.menuService.menuItemService.dataSource.sync();
                        }
                    };
                    $(internal.options.ids.listViewId).kendoDropTargetArea(dropTargetAreaOptions);
                };
                /*todo write a handler to allow the page to scroll with drag */
                MenuOrderingControllerService.prototype.movePage = function (mouseMoveEvent) {
                    var m = mouseMoveEvent;
                };
                MenuOrderingControllerService.prototype.movePlaces = function (target, moveItem, switchPosition) {
                    switchPosition || (switchPosition = Positioning.BEFORE);
                    var internal = this, ds = internal.menuService.menuItemService.dataSource, items = ds.view(), max = items.length - 1;
                    var draggedItem = { id: moveItem.id, item: moveItem, webSesquence: moveItem.get("WebSequence"), position: items.indexOf(moveItem) };
                    var droppedOnItem = { id: target.id, item: target, webSesquence: target.get("WebSequence"), position: items.indexOf(target) };
                    var switchPositions = draggedItem.position - droppedOnItem.position;
                    switchPositions *= switchPositions;
                    if (switchPositions === 1) {
                        moveItem.set("WebSequence", droppedOnItem.webSesquence);
                        target.set("WebSequence", draggedItem.webSesquence);
                    }
                    else if (droppedOnItem.position === 0) {
                        //move item to position 1
                        var moveItemPosition = droppedOnItem.webSesquence / 2;
                        moveItem.set("WebSequence", moveItemPosition);
                    }
                    else if (droppedOnItem.position === max) {
                        //move item before or after last position
                        var moveItemPosition = 0;
                        switch (switchPosition) {
                            case Positioning.BEFORE:
                                moveItemPosition = droppedOnItem.webSesquence + items[droppedOnItem.position - 1];
                                break;
                            case Positioning.INTO:
                                moveItemPosition = droppedOnItem.webSesquence * 2 + 100;
                                break;
                            case Positioning.AFTER:
                                moveItemPosition = droppedOnItem.webSesquence * 2 + 100;
                                break;
                        }
                        moveItemPosition = moveItemPosition / 2;
                        draggedItem.item.set("WebSequence", moveItemPosition);
                    }
                    else {
                        var fitBetweenItem = xIndex === 0 ? target : items[droppedOnItem.position - 1];
                        if (Positioning.INTO === switchPosition && draggedItem.position < droppedOnItem.position) {
                            fitBetweenItem = droppedOnItem.position === 0 ? droppedOnItem.item : items[droppedOnItem.position + 1];
                        }
                        //if (Positioning.INTO === switchPosition && draggedItem.position > droppedOnItem.position)
                        //{
                        //    fitBetweenItem = droppedOnItem.position === 0 ? droppedOnItem.item : items[droppedOnItem.position - 1]
                        //}
                        var xIndex = droppedOnItem.position;
                        var zPosition = droppedOnItem.webSesquence + fitBetweenItem.get("WebSequence");
                        zPosition = zPosition / 2;
                        draggedItem.item.set("WebSequence", zPosition);
                    }
                };
                MenuOrderingControllerService.prototype.normalize = function () {
                    var internal = this, ds = internal.menuService.menuItemService.dataSource, view = ds.view();
                    var start = 100;
                    var linq = Enumerable.from(view);
                    var group = linq
                        .groupBy(function (x) { return x.Name; }, function (x) { return x; }, function (key, result) {
                        return {
                            key: key,
                            items: result.toArray()
                        };
                    })
                        .toArray();
                    group.forEach(function (item) {
                        //group should end up in the same order as the first element found for each key. 
                        //ergo we can apply it sequentially. 
                        item.items.forEach(function (menuItem) {
                            menuItem.set("WebSequence", start);
                        });
                        start += 100;
                    });
                    //view.forEach((item: any, index, source) => {
                    //    item.set("WebSequence", start);
                    //    start += 100;
                    //});
                    internal.positionWindowViewModel.trigger("change");
                    ds.trigger("change");
                };
                MenuOrderingControllerService.prototype.closeWindow = function (saveChanges) {
                    var kendoWindow = $("#positionWindow").data("kendoWindow");
                    var ds = this.menuService.menuItemService.dataSource;
                    if (saveChanges) {
                        ds.sync();
                    }
                    else {
                        ds.cancelChanges();
                    }
                    kendoWindow.close();
                };
                MenuOrderingControllerService.prototype.displayWindow = function () {
                    var internal = this, wrapper = $("#positionWindowWrapper");
                    kendo.bind(wrapper, this.positionWindowViewModel);
                    $(internal.options.ids.listViewId).on("click", ".k-button-edit-position", function (e) {
                        e.preventDefault();
                        var itemId = $(this).closest(".menu-item").data("id"), item = internal.menuService.menuItemService.findById(itemId), items = internal.menuService.menuItemService.dataSource.view(), vm = internal.positionWindowViewModel;
                        var kendoWindows = $("#positionWindow").data("kendoWindow");
                        vm.set("item", item);
                        vm.set("itemName", item.DisplayName());
                        vm.set("itemIndex", item.Index());
                        vm.set("length", items.length);
                        vm.set("index", item.Index());
                        kendoWindows.open();
                        kendoWindows.center();
                    });
                };
                MenuOrderingControllerService.prototype.generateHint = function (element) {
                    var hint = element.clone();
                    hint.css({
                        "width": element.width(),
                        "height": element.height()
                    });
                    $(hint).css({ "border-color": "#600BA2", "border-width": "4px" });
                    $(element).animate({ "opacity": 0.8 });
                    //return hint;
                };
                MenuOrderingControllerService.prototype.initHubChanges = function () {
                    var internal = this, hub = MyAndromeda.Hubs.StoreHub.GetInstance(internal.options.routeParameters);
                    hub.bind(MyAndromeda.Hubs.StoreHub.MenuItemChangeEvent, function (context) {
                        console.log("i have changes :)");
                        console.log(context);
                        internal.menuService.extendMenuItemData(context.EditedItems);
                        internal.menuService.menuItemService.updateItems(context.EditedItems, true);
                        internal.menuService.menuItemService.dataSource.sort({ field: "WebSequence", dir: "asc" });
                    });
                };
                MenuOrderingControllerService.prototype.destroyDraggable = function (e) {
                    var o = this;
                    e.currentTarget.css("opacity", 1);
                    e.currentTarget.removeClass("draggable");
                    o.destroy();
                };
                MenuOrderingControllerService.prototype.init = function () {
                    Services.menuFilterController.RESETFILTER = [{ field: "DisplayCategoryId", operator: "eq", value: -19232131 }];
                    Services.menuFilterController.SORTFILTER = [{ field: "WebSequence", dir: "asc" }];
                    this.menuService.init();
                    this.menuService.menuItemService.dataSource.pageSize(1000);
                    this.initListView();
                    this.initListViewEvents();
                    this.initHubChanges();
                    this.displayWindow();
                };
                return MenuOrderingControllerService;
            }());
            Services.MenuOrderingControllerService = MenuOrderingControllerService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
