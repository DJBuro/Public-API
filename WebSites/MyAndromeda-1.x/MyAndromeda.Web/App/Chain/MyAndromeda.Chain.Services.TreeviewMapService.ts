/// <reference path="../../Scripts/typings/linqjs/linq.d.ts" />
/// <reference path="../../Scripts/typings/jquery/jquery.d.ts" />

module MyAndromeda.Chain.Services 
{
    class TreeviewMapService {
        private kendoTreeView: kendo.ui.TreeView;
        private kendoMap: kendo.dataviz.ui.Map;
        public viewModel: kendo.data.ObservableObject;

        constructor(kendoTreeView: kendo.ui.TreeView) {
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

        private AddMarkers(): void {
            var map = <any>this.kendoMap,
                markers = <kendo.dataviz.ui.MapMarker[]>map.markers;
            map.markers.clear();

            var stores = <Array<Store.Models.IStoreLowerCase>>this.viewModel.get("stores");

            var viableStores = Enumerable.from(stores).where(e=> e.latitude !== null && e.longitude !== null && e.longitude.length > 0 && e.latitude.length > 0);
            //zoom out
            if (viableStores.count() === 0) {
                map.zoom(1);
                map.center([0, 0]);

                return;
            }

            var centeredPosition = function () {
                //console.log(viableStores.toArray());
                var avgLat = viableStores.select(e=> parseFloat(e.latitude)).average();
                var avgLong = viableStores.select(e=> parseFloat(e.longitude)).average();
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
        }
    }

    export class TreeviewChainService {
        private data: any;
        private rootDataSource: kendo.data.HierarchicalDataSource;
        private kendoTreeView: kendo.ui.TreeView;
        private mapService: TreeviewMapService

        constructor(data: any) {

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

            this.kendoTreeView = <kendo.ui.TreeView>treeviewElement.data("kendoTreeView");

            treeviewElement.on("click", ".k-button-show-stores", function (e) {
                e.preventDefault();
                var uid = $(this).closest(".k-item").data("uid");
                var element = <any>internal.rootDataSource.getByUid(uid);

                internal.mapService.viewModel.set("stores", element.stores);
            });

            kendo.bind($("#treeviewdata"), treeVm);
            this.mapService = new TreeviewMapService(this.kendoTreeView);
        }
    }
} 