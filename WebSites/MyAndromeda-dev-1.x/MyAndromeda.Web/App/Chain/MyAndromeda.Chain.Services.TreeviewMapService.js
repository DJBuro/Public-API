/// <reference path="../../Scripts/typings/linqjs/linq.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Chain;
    (function (Chain) {
        var Services;
        (function (Services) {
            var TreeviewMapService = (function () {
                function TreeviewMapService(kendoTreeView) {
                    var internal = this;
                    this.viewModel = kendo.observable({
                        stores: []
                    });
                    var bindingElement = $("#mapData");
                    kendo.bind(bindingElement, this.viewModel);
                    this.viewModel.bind("change", function () {
                        internal.AddMarkers();
                    });
                    this.kendoMap = $("#map").data("kendoMap");
                }
                TreeviewMapService.prototype.AddMarkers = function () {
                    var map = this.kendoMap, markers = map.markers;
                    map.markers.clear();
                    var stores = this.viewModel.get("stores");
                    var viableStores = Enumerable.from(stores).where(function (e) { return e.latitude !== null && e.longitude !== null; });
                    //zoom out
                    if (viableStores.count() === 0) {
                        map.zoom(1);
                        map.center([0, 0]);
                        return;
                    }
                    var centeredPosition = function () {
                        //console.log(viableStores.toArray());
                        var avgLat = viableStores.select(function (e) { return parseFloat(e.latitude); }).average();
                        var avgLong = viableStores.select(function (e) { return parseFloat(e.longitude); }).average();
                        var c = [avgLat, avgLong];
                        return c;
                    };
                    viableStores.forEach(function (store, index) {
                        if (store.longitude && store.latitude) {
                            var location = [store.latitude, store.longitude];
                            var addition = {
                                shape: "pin",
                                store: store,
                                tooltip: {
                                    animation: {
                                        close: {
                                            effects: "fade:out"
                                        }
                                    },
                                    autoHide: true,
                                    position: "right",
                                    showOn: "mouseenter",
                                    template: $("#map-tooltip-template").html()
                                },
                                location: location
                            };
                            map.markers.add(addition);
                        }
                    });
                    var centerdLocation = centeredPosition();
                    map.center(centeredPosition());
                    map.zoom(3);
                };
                return TreeviewMapService;
            }());
            var TreeviewChainService = (function () {
                function TreeviewChainService(data) {
                    var internal = this;
                    this.data = data;
                    this.rootDataSource = new kendo.data.HierarchicalDataSource({
                        transport: {
                            read: function (options) {
                                options.success(internal.data);
                            }
                        },
                        schema: {
                            model: {
                                id: "id",
                                children: "items"
                            }
                        }
                    });
                    this.rootDataSource.read();
                    var treeVm = kendo.observable({
                        chains: this.rootDataSource
                    });
                    var treeviewElement = $("#TreeviewChains").kendoTreeView({
                        template: kendo.template($('#StoreNode').html()),
                        dataSource: this.rootDataSource,
                        loadOnDemand: false,
                        dataTextField: "name"
                    });
                    this.kendoTreeView = treeviewElement.data("kendoTreeView");
                    treeviewElement.on("click", ".k-button-show-stores", function (e) {
                        e.preventDefault();
                        var uid = $(this).closest(".k-item").data("uid");
                        var element = internal.rootDataSource.getByUid(uid);
                        internal.mapService.viewModel.set("stores", element.stores);
                    });
                    kendo.bind($("#treeviewdata"), treeVm);
                    this.mapService = new TreeviewMapService(this.kendoTreeView);
                }
                return TreeviewChainService;
            }());
            Services.TreeviewChainService = TreeviewChainService;
        })(Services = Chain.Services || (Chain.Services = {}));
    })(Chain = MyAndromeda.Chain || (MyAndromeda.Chain = {}));
})(MyAndromeda || (MyAndromeda = {}));
