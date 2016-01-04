using System;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Core
{
    public static class Haversine
    {
        public static bool ProximityDelivered(this PremisesDriver driver, Coordinates customerCoordinates)
        {
            var km = DistanceInKm(driver.Latitude, driver.Longitude, customerCoordinates.Latitude,
                                    customerCoordinates.Longitude);

            var returnVal = km <= 0.1;

            return returnVal;

        }

        public static bool WithinDeliveryRadius(this Store store, ref Coordinates coordinates)
        {
            var km = DistanceInKm(store.Coordinates.Latitude, store.Coordinates.Longitude, coordinates.Latitude,
                                  coordinates.Longitude);

            return km <= store.DeliveryRadius;

        }

        
        
        private static double DistanceInKm(float latitude1, float longitude1, float latitude2, float longitude2)
        {
            
            var dLat = ToRadian(latitude2 - latitude1);
            var dLon = ToRadian(longitude1 - longitude2);

            var haverSine = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(latitude1)) * Math.Cos(ToRadian(latitude2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(haverSine)));
            var distanceInKm = EarthRadius * c; //d = km

            return distanceInKm;

        }


        //(Math.PI / 180) = 0.017453292519943295
        const double Pi = 0.017453292519943295;
        const double EarthRadius = 6365; //roughly the European radius: http://en.wikipedia.org/wiki/File:EarthEllipRadii.jpg


        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”></param>
        /// <returns></returns>
        private static double ToRadian(double val)
        {
            return (Pi) * val;
        }
    }


 

}
