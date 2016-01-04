var mapHelper =
{
    map: undefined,
    storeMarker: undefined,
    markersLayer: undefined,
    urls:
    [
        "http://a.tile.openstreetmap.org/${z}/${x}/${y}.png",
        "http://b.tile.openstreetmap.org/${z}/${x}/${y}.png",
        "http://c.tile.openstreetmap.org/${z}/${x}/${y}.png"
    ],
    initialiseMap: function ()
    {
        try
        {
            mapHelper.map = new OpenLayers.Map
            (
                {
                    div: "map",
                    layers:
                    [
                        new OpenLayers.Layer.XYZ
                        (
                            "OSM (with buffer)",
                            mapHelper.urls,
                            {
                                transitionEffect: "resize",
                                buffer: 2, sphericalMercator: true,
                                attribution: ""
                            }
                        ),
                        new OpenLayers.Layer.XYZ
                        (
                            "OSM (without buffer)",
                            mapHelper.urls,
                            {
                                transitionEffect: "resize",
                                buffer: 0,
                                sphericalMercator: true,
                                attribution: ""
                            }
                        )
                    ],
                    controls:
                    [
                        new OpenLayers.Control.Navigation
                        (
                            {
                                dragPanOptions: { enableKinetic: true }
                            }
                        ),
                        new OpenLayers.Control.PanZoom(),
                        new OpenLayers.Control.Attribution()
                    ],
                    center: [0, 0],
                    zoom: 3
                }
            );

            var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
            var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection

            // allow testing of specific renderers via "?renderer=Canvas", etc
            var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
            renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;

            var wgs84 = new OpenLayers.Projection("EPSG:4326");
            mapHelper.markersLayer = new OpenLayers.Layer.Vector
            (
                "Markers",
                {
                    /*renderers: renderer*/
                    styleMap: new OpenLayers.StyleMap
                    (
                        {
                            externalGraphic: settings.mapMarker,
                            graphicOpacity: 1.0,
                            graphicWidth: 32,
                            graphicHeight: 37,
                            graphicYOffset: -37
                        }
                    ),
                    projection: wgs84
                }
            );
            window.mapped = "yes";

            mapHelper.map.addLayer(mapHelper.markersLayer);

            var longitude = 0;

            if (viewModel.siteDetails().address["long"] == undefined)
            {
                longitude = viewModel.siteDetails().address.longitude;
            }
            else
            {
                longitude = viewModel.siteDetails().address["long"];
            }

            var latitude = viewModel.siteDetails().address.lat == undefined ? viewModel.siteDetails().address.latitude : viewModel.siteDetails().address.lat;

            mapHelper.setStoreMarker(longitude, latitude);

            var position = new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection);

            mapHelper.map.setCenter(position, 17);
        }
        catch (exception) { }
    },
    setStoreMarker: function (longitude, latitude)
    {
        // Is there already a store feature on the map?
        if (typeof (mapHelper.storeMarker) == 'object')
        {
            // Remove the store feature
            mapHelper.markersLayer.removeAllFeatures();
            mapHelper.markersLayer.destroyFeatures();
            mapHelper.storeMarker = undefined;
        }

        var fromProjection = new OpenLayers.Projection("EPSG:4326"); // Transform from WGS 1984
        var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection

        var lonLat = new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection);

        // Create a new feature
        mapHelper.storeMarker =
        {
            "type": "Feature",
            "geometry":
            {
                "type": "Point",
                "coordinates": [lonLat.lon, lonLat.lat]
            }
        };

        // Add the feature to the map
        var features =
        {
            "type": "FeatureCollection",
            "features": [mapHelper.storeMarker]
        };

        var reader = new OpenLayers.Format.GeoJSON();

        var locator = reader.read(features);

        mapHelper.markersLayer.addFeatures(locator);

        // Center the map on the feature
        var latLonBounds = new OpenLayers.Bounds();

        latLonBounds.extend(new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection));
    }
}