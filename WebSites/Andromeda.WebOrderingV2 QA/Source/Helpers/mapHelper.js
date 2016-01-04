var mapHelper =
{
    urls:
    [
        "http://a.tile.openstreetmap.org/${z}/${x}/${y}.png",
        "http://b.tile.openstreetmap.org/${z}/${x}/${y}.png",
        "http://c.tile.openstreetmap.org/${z}/${x}/${y}.png"
    ],
    initialiseMap: function (div, site)
    {
        try
        {
            //mapHelper.map = new OpenLayers.Map
            var map = new OpenLayers.Map
            (
                {
                    div: div, //"map",
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
            var markersLayer = new OpenLayers.Layer.Vector
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

            map.addLayer(markersLayer);

            var longitude = 0;

            if (site.address["long"] == undefined)
            {
                longitude = site.address.longitude;
            }
            else
            {
                longitude = site.address["long"];
            }

            var latitude = site.address.lat == undefined ? site.address.latitude : site.address.lat;

            mapHelper.setStoreMarker(map, markersLayer, longitude, latitude);

            var position = new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection);

            map.setCenter(position, 17);

            // Disable panning
            for (var i = 0; i < map.controls.length; i++)
            {
                if (map.controls[i].displayClass == "olControlNavigation")
                {
                    map.controls[i].deactivate();
                }
            }

            return map;
        }
        catch (exception) { }
    },
    setStoreMarker: function (map, markersLayer, longitude, latitude)
    {
        // Is there already a store feature on the map?
        //if (typeof (mapHelper.storeMarker) == 'object')
        //{
        //    // Remove the store feature
        //    markersLayer.removeAllFeatures();
        //    markersLayer.destroyFeatures();
        //    storeMarker = undefined;
        //}

        var fromProjection = new OpenLayers.Projection("EPSG:4326"); // Transform from WGS 1984
        var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection

        var lonLat = new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection);

        // Create a new feature
        var storeMarker =
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
            "features": [storeMarker]
        };

        var reader = new OpenLayers.Format.GeoJSON();

        var locator = reader.read(features);

        markersLayer.addFeatures(locator);

        // Center the map on the feature
        var latLonBounds = new OpenLayers.Bounds();

        latLonBounds.extend(new OpenLayers.LonLat(longitude, latitude).transform(fromProjection, toProjection));
    },
    refreshMaps: function()
    {
        for (var index = 0; index < siteSelectorHelper.siteDetails().length; index++)
        {
            var siteDetails = siteSelectorHelper.siteDetails()[index];

            mapHelper.initialiseMap('map' + index, siteDetails.site);
        }
    }
}