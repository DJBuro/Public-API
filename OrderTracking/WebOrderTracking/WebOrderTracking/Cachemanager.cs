using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Configuration;
using System.Xml.XPath;

namespace Andromeda.WebOrderTracking
{
    internal static class CacheManager
    {
        private static Order OTTestOrder { get; set; }
        private static int OTTestOrderLocationIndex { get; set; }
        private static Dictionary<int,KeyValuePair<float,float>> OTTestOrderLocation { get; set; }

        // The last time an external client called any of the web services
        private static DateTime LastExternalCallDateTime { get; set; }

        // For background tasks:
        // 1) Poll the GPSGate server for tracker locations & update the cache 
        // 2) Remove old entries from the caches
        private static Timer BackgroundTimer { get; set; }
        
        // We use this to sync threads so that only one thread accesses the cache at a time
        // If the cache is modified by one thread while another thread is reading from it, bad things can happen
        internal static object CacheLock { get; set; }

        // All trackers cache
        internal static Dictionary<string, Tracker> TrackersByTrackerName { get; set; }
        
        // Client orders cache
        internal static Dictionary<string, CachedClientCustomers> CustomersByClientKey { get; set; }

        static CacheManager()
        {
            // Create indexed data caches
            CacheManager.TrackersByTrackerName = new Dictionary<string,Tracker>();
            CacheManager.CustomersByClientKey = new Dictionary<string, CachedClientCustomers>();
            
            CacheManager.CacheLock = new object();

            CacheManager.OTTestOrderLocationIndex = 0;
            CacheManager.OTTestOrderLocation = new Dictionary<int,KeyValuePair<float,float>>();
            CacheManager.OTTestOrderLocation.Add(0, new KeyValuePair<float, float>(51.361f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(1, new KeyValuePair<float, float>(51.362f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(2, new KeyValuePair<float, float>(51.363f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(3, new KeyValuePair<float, float>(51.364f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(4, new KeyValuePair<float, float>(51.365f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(5, new KeyValuePair<float, float>(51.366f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(6, new KeyValuePair<float, float>(51.367f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(7, new KeyValuePair<float, float>(51.368f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(8, new KeyValuePair<float, float>(51.369f, -0.15022f));
            CacheManager.OTTestOrderLocation.Add(9, new KeyValuePair<float, float>(51.370f, -0.15022f));
        }

        internal static Order GetOTTestOrder()
        {
            if (CacheManager.OTTestOrder == null)
            {
                CacheManager.OTTestOrder = new Order(
                    -1,
                    DateTime.Now,
                    null,
                    null,
                    51.36037d,
                    -0.15022d,
                    51.36476d,
                    -0.15690d,
                    "",
                    1,
                    "OTTESTTRACKER");
            }
            else
            {
                if (CacheManager.OTTestOrder.Status == 1)
                {
                    CacheManager.OTTestOrder.Status = 2;
                }
                else if (CacheManager.OTTestOrder.Status == 2)
                {
                    CacheManager.OTTestOrder.Status = 3;
                }
                else if (CacheManager.OTTestOrder.Status == 3)
                {
                    CacheManager.OTTestOrder.Status = 4;
                    CacheManager.OTTestOrder.OutForDeliveryDateTime = DateTime.Now;
                }
                else if (CacheManager.OTTestOrder.Status == 4)
                {
                    CacheManager.OTTestOrder.Status = 5;
                    CacheManager.OTTestOrder.CompletedDateTime = DateTime.Now;
                }
                else if (CacheManager.OTTestOrder.Status == 5)
                {
                    CacheManager.OTTestOrder.Status = 1;
                    CacheManager.OTTestOrder.OrderTakenDateTime = DateTime.Now;
                    CacheManager.OTTestOrder.OutForDeliveryDateTime = null;
                    CacheManager.OTTestOrder.CompletedDateTime = null;
                }
            }

            return CacheManager.OTTestOrder;
        }

        internal static void GetOTTestOrderLocation(out float latitude, out float longitude)
        {
            KeyValuePair<float, float> location = CacheManager.OTTestOrderLocation[CacheManager.OTTestOrderLocationIndex];

            latitude = location.Key;
            longitude = location.Value;

            if (CacheManager.OTTestOrderLocationIndex < CacheManager.OTTestOrderLocation.Count - 1)
            {
                CacheManager.OTTestOrderLocationIndex++;
            }
            else
            {
                CacheManager.OTTestOrderLocationIndex = 0;
            }
        }

        internal static Tracker GetTrackerFromCache(string clientKey, string customerCredentials, string trackerName)
        {
            Tracker tracker = null;
            bool pollRequired = false;

            // Is the tracker in the cache?
            pollRequired = CacheManager.GetTracker(clientKey, customerCredentials, trackerName, out tracker);
            
            // Is polling running?
            if (CacheManager.BackgroundTimer == null)
            {
                pollRequired = true;
            }
            
            // Do we need to poll now?
            if (pollRequired)
            {
                // Do an immediate poll so the caller has the results now
                CacheManager.DoPoll(null);
            }
                        
            // Start polling
            CacheManager.StartPolling();

            return tracker;
        }
        
        private static bool GetTracker(string clientKey, string customerCredentials, string trackerName, out Tracker tracker)
        {
            bool pollRequired = false;
            tracker = null;

            CacheManager.TrackersByTrackerName.TryGetValue(trackerName, out tracker);

            //lock (CacheManager.CacheLock)
            //{
            //    CachedClientCustomers cachedClientCustomers = null;

            //    // Try and get the clients customers
            //    if (!CacheManager.CustomersByClientKey.TryGetValue(clientKey, out cachedClientCustomers))
            //    {
            //        // Unknown client
            //        cachedClientCustomers = new CachedClientCustomers();
            //        CacheManager.CustomersByClientKey.Add(clientKey, cachedClientCustomers);
            //    }

            //    // Try and get the customer details
            //    CachedCustomer cachedCustomer = null;
            //    if (!cachedClientCustomers.CustomersByCustomerCredentials.TryGetValue(customerCredentials, out cachedCustomer))
            //    {
            //        // The customer is unknown
            //        cachedCustomer = new CachedCustomer();
            //        cachedClientCustomers.CustomersByCustomerCredentials.Add(customerCredentials, cachedCustomer);
            //    }

            //    CachedOrder cachedOrder = null;
            //    if (!cachedCustomer.OrdersByOrderId.TryGetValue(trackerName, out cachedOrder))
            //    {
            //        tracker = new Tracker(trackerName, null, null);
            //        CachedOrder
            //        // Add the new tracker to the cache
            //        cachedCustomer.Add(trackerName, tracker);
            //        CacheManager.TrackersByTrackerName.Add(trackerName, tracker);

            //        // We'll need to get location data for the cache asap
            //        pollRequired = true;
            //    }
            //}

            return pollRequired;
        }

        private static void StartPolling()
        {
            // Get the timer interval
            string backgroundTimerInvervalMillisecondsText = ConfigurationManager.AppSettings["BackgroundTimerInvervalMilliseconds"];
            int backgroundTimerInvervalMilliseconds = 5000;
            Int32.TryParse(backgroundTimerInvervalMillisecondsText, out backgroundTimerInvervalMilliseconds);
                
            // Have we started the timer?
            if (CacheManager.BackgroundTimer == null)
            {
                // When the background timer fires call this method
                TimerCallback timerCallback = CacheManager.DoPoll;

                // We need to start the timer - it'll only fire once
                CacheManager.BackgroundTimer = new Timer(timerCallback, null, backgroundTimerInvervalMilliseconds, 0);
            }
            else
            {
                // We need to start the timer - it'll only fire once
                CacheManager.BackgroundTimer.Change(backgroundTimerInvervalMilliseconds, 0);
            }
        }
        
        private static void DoPoll(Object stateInfo)
        {
            try
            {
                //If LastExternalCallDateTime < 10 minutes
                TimeSpan timeSinceLastExternalCall = DateTime.Now - CacheManager.LastExternalCallDateTime;
                string gpsGatePollingIdleTimeMinutesText = ConfigurationManager.AppSettings["GPSGatePollingIdleTimeMinutes"]; 
                int gpsGatePollingIdleTimeMinutes = 0;
                
                if (Int32.TryParse(gpsGatePollingIdleTimeMinutesText, out gpsGatePollingIdleTimeMinutes))
                {            
                    if (timeSinceLastExternalCall.TotalMinutes > gpsGatePollingIdleTimeMinutes)
                    {
                        // Don't poll now
                        CacheManager.StartPolling();
                    }
                }

                // For all Cached orders
                //  If LastPolledDateTime > 10 mins ago remove from cache
                //  If CompletedDateTime > 10 mins ago remove from cache
            
                string url = ConfigurationManager.AppSettings["GPSGateTrackerServicesUrl"] + "/Trackers"; 
            
                string responseXml = HttpHelper.HttpGet(url);

                // Make sure other threads don't access the index while it's being updated
                lock (CacheManager.TrackersByTrackerName)
                {               
                    using (StringReader stream = new StringReader(responseXml))
                    {
                        XPathDocument document = new XPathDocument(stream);
                        XPathNavigator navigator = document.CreateNavigator();
                        XPathNodeIterator orderNodes = navigator.Select("/Trackers/Tracker");

                        float latitude = 0;
                        float longitude = 0;

                        while (orderNodes.MoveNext())
                        {
                            // Get the tracker details out of the xml
                            string name = orderNodes.Current.GetAttribute("name", "");
                            string latText = orderNodes.Current.GetAttribute("lat", "");
                            string lonText = orderNodes.Current.GetAttribute("lon", "");
                            
                            // Is the latitude a valid float?
                            latitude = 0;
                            if (float.TryParse(latText, out latitude))
                            {
                                // Is the longitude a valid float?
                                longitude = 0;
                                if (float.TryParse(lonText, out longitude))
                                {
                                    Tracker tracker = null;
                                
                                    // Is the tracker already in the cache?
                                    if (CacheManager.TrackersByTrackerName.TryGetValue(name, out tracker))
                                    {
                                        // Update the tracker location
                                        tracker.Latitude = latitude;
                                        tracker.Longitude = longitude;
                                        tracker.LastUpdated = DateTime.Now;
                                    }
                                    else
                                    {
                                        // Add the tracker to the cache
                                        CacheManager.TrackersByTrackerName.Add(name, new Tracker(name, latitude, longitude));
                                    }
                                }
                            }
                        }
                        
                        latitude = 0;
                        longitude = 0;

                        CacheManager.GetOTTestOrderLocation(out latitude, out longitude);

                        Tracker testTracker = null;

                        // Is the tracker already in the cache?
                        if (CacheManager.TrackersByTrackerName.TryGetValue("Test Driver", out testTracker))
                        {
                            // Update the tracker location
                            testTracker.Latitude = latitude;
                            testTracker.Longitude = longitude;
                            testTracker.LastUpdated = DateTime.Now;
                        }
                        else
                        {
                            // Add the tracker to the cache
                            CacheManager.TrackersByTrackerName.Add("Test Driver", new Tracker("Test Driver", latitude, longitude));
                        }
                    }        

                    // Cleanup the cache
                    // Clear out any lat/lons older than X mins
                    int expireTrackerLocationsAfterMinutes = Int32.Parse(ConfigurationManager.AppSettings["ExpireTrackerLocationsAfterMinutes"]);
                    DateTime cutOffDateTime = DateTime.Now.AddMinutes(expireTrackerLocationsAfterMinutes * -1);

                    foreach (Tracker tracker in CacheManager.TrackersByTrackerName.Values)
                    {
                        if (tracker.LastUpdated.Ticks < cutOffDateTime.Ticks)
                        {
                            tracker.Latitude = null;
                            tracker.Longitude = null;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                DataAccess.LogEvent(exception, "");
            }

            CacheManager.StartPolling();
        }
        
        internal static void CheckIfPollRequired()
        {
            TimeSpan timeSinceLastExternalCall = DateTime.Now - CacheManager.LastExternalCallDateTime;
            string gpsGatePollingIdleTimeMinutesText = ConfigurationManager.AppSettings["GPSGatePollingIdleTimeMinutes"];
            int gpsGatePollingIdleTimeMinutes = 0;
            
            if (Int32.TryParse(gpsGatePollingIdleTimeMinutesText, out gpsGatePollingIdleTimeMinutes))
            {            
                if (timeSinceLastExternalCall.TotalMinutes > gpsGatePollingIdleTimeMinutes)
                {
                    CacheManager.DoPoll(null);
                }
            }
            
            CacheManager.LastExternalCallDateTime = DateTime.Now;
        }
    }
}
