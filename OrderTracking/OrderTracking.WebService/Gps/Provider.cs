using System;
using System.Collections.Generic;
using OrderTracking.Dao;
using OrderTracking.Dao.Domain;
using OrderTracking.WebService.AndroGeoCode;
using OrderTracking.WebService.Tracking;
using Account = OrderTracking.Dao.Domain.Account;
using Coordinates = OrderTracking.Dao.Domain.Coordinates;
using Log = OrderTracking.WebService.Tracking.Logging;

namespace OrderTracking.WebService.Gps
{
    public static class GpsProvider
    {
        public static List<Tracker> GetTrackers(this Account account)
        {
            var trackers = new List<Tracker>();

            Reconcile.TrackerRecon.ReconTrackers(ref account);

            foreach (Tracker tracker in account.Store.Trackers)
            {
                trackers.Add(tracker);
            }

            return trackers;
        }

        public static Tracker Get(this Tracker tracker)
        {
            Reconcile.TrackerRecon.ReconTracker(ref tracker);

            return tracker;
        }

        public static Tracker GetTracker(this Account account, string trackerName)
        {
            var outTrackers = GetTrackers(account);
            var tracker = new Tracker();

            foreach (var outTracker in outTrackers)
            {
                if (!outTracker.Name.Equals(trackerName)) continue;

                tracker = outTracker;
                Reconcile.TrackerRecon.ReconTracker(ref tracker);

                break;
            }

            return tracker;
        }


        public static Results Geocode(this CustomerDetails customerDetails,  Store store,  Results results, ref Coordinates coordinates, string environment)
        {
            return GoogleGeoCode(customerDetails, results, environment, ref coordinates, store.ExternalStoreId);
        }

        /// <summary>
        /// Used for OrderTracking Admin and AndroDB
        /// </summary>
        /// <param name="storeDetails"></param>
        /// <param name="results"></param>
        /// <param name="coordinates"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static Results Geocode(this StoreDetails storeDetails, Results results, ref Coordinates coordinates, string environment)
        {
            return GoogleGeoCode(storeDetails, results, environment, ref coordinates, storeDetails.ExternalStoreId);
        }

        /// <summary>
        /// Todo: implement this as async in future.
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="eventArgs"></param>
        public static void CoorResults(object eh, EventArgs eventArgs)
        {
            var blahx = eventArgs;

            var xzyz = blahx.ToString();

        }

        public static Results GoogleGeoCode<TAddress>(TAddress address, Results results, string environment, ref Coordinates coordinates, string ExternalStoreId) where TAddress : IGeoCoordinates, IGeoAddress
        {
            //TODO:implement this as async in future??.
            //var test = new AndroGeoCode.GeoCode();
            //test.GeoCodeAddressCompleted += CoorResults;
            //test.GeoCodeAddressAsync(ExternalStoreId, address.HouseNumber, address.BuildingName, address.RoadName, address.TownCity, address.PostCode, address.Country);

            var geoCode = new GeoCode();
            var geoCoords = geoCode.GeoCodeAddress(ExternalStoreId, address.HouseNumber, address.BuildingName, address.RoadName, address.TownCity, address.PostCode, address.Country);

            coordinates.Latitude = geoCoords.Latitude;
            coordinates.Longitude = geoCoords.Longitude;

            return results;
        }
    }
}