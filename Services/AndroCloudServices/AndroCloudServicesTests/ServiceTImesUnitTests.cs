using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AndroCloudDataAccess;
using AndroCloudHelper;
using AndroCloudDataAccess.Domain;
using AndroCloudDataAccessEntityFramework.Model;
using AndroCloudDataAccessEntityFramework.DataAccess;

namespace AndroCloudServicesTests
{
    [TestClass]
    public class ServiceTimesUnitTests
    {
        [TestMethod]
        [Description("Site occasion times")]
        public void SiteOccasions_TestMethod()
        {
            // Test data
            ICollection<SiteOccasionTime> siteOccasionTimes = new List<SiteOccasionTime>();

            // Open all day, Monday, Delivery & Collection
            SiteOccasionTime openAllDay = new SiteOccasionTime()
            {
                AndromedaSiteId = 1423,
                EndTimezone = null,
                EndUtc = new DateTime(2016, 3, 3, 12, 0, 0),
                IsAllDay = true,
                Occasions = "Delivery,Collection",
                RecurrenceException = "",
                RecurrenceRule = "FREQ=WEEKLY;BYDAY=MO",
                StartTimezone = null,
                StartUtc = new DateTime(2016, 3, 3, 12, 0, 0),
                Title = "Open all day, Monday, Delivery & Collection"
            };
            siteOccasionTimes.Add(openAllDay);
            // Open all day, Wednesday, Collection
            SiteOccasionTime openAllDayCollection = new SiteOccasionTime()
            {
                AndromedaSiteId = 1423,
                EndTimezone = null,
                EndUtc = new DateTime(2016, 3, 3, 12, 0, 0),
                IsAllDay = true,
                Occasions = "Collection",
                RecurrenceException = "",
                RecurrenceRule = "FREQ=WEEKLY;BYDAY=WE",
                StartTimezone = null,
                StartUtc = new DateTime(2016, 3, 3, 12, 0, 0),
                Title = "Open all day, Wednesday, Collection"
            };
            siteOccasionTimes.Add(openAllDayCollection);
            // Open lunchtime - Tuesday, Thursday, Delivery & Collection
            SiteOccasionTime openLunchtime = new SiteOccasionTime()
            {
                AndromedaSiteId = 1423,
                EndTimezone = null,
                EndUtc = new DateTime(2016, 3, 3, 15, 0, 0),
                IsAllDay = false,
                Occasions = "Delivery,Collection",
                RecurrenceException = "",
                RecurrenceRule = "FREQ=WEEKLY;BYDAY=TU,FR",
                StartTimezone = null,
                StartUtc = new DateTime(2016, 3, 3, 11, 0, 0),
                Title = "Open lunchtime - Tuesday, Thursday, Delivery & Collection"
            };
            siteOccasionTimes.Add(openLunchtime);
            // Open evening - Thursday, Delivery & Collection
            SiteOccasionTime eveningCollection = new SiteOccasionTime()
            {
                AndromedaSiteId = 1423,
                EndTimezone = null,
                EndUtc = new DateTime(2016, 3, 4, 1, 0, 0),
                IsAllDay = false,
                Occasions = "Collection",
                RecurrenceException = "",
                RecurrenceRule = "FREQ=WEEKLY;BYDAY=TH,FR",
                StartTimezone = null,
                StartUtc = new DateTime(2016, 3, 3, 17, 0, 0),
                Title = "Open evening - Thursday, Delivery & Collection"
            };
            siteOccasionTimes.Add(eveningCollection);
            // Open evening - Friday, Delivery & Collection
            SiteOccasionTime eveningDelivery  = new SiteOccasionTime()
            {
                AndromedaSiteId = 1423,
                EndTimezone = null,
                EndUtc = new DateTime(2016, 3, 4, 1, 30, 0),
                IsAllDay = false,
                Occasions = "Delivery",
                RecurrenceException = "",
                RecurrenceRule = "FREQ=WEEKLY;BYDAY=TH",
                StartTimezone = null,
                StartUtc = new DateTime(2016, 3, 3, 17, 30, 0),
                Title = "Open evening - Friday, Delivery & Collection"
            };
            siteOccasionTimes.Add(eveningDelivery);

            TimeSpanBlock3 lunchTime = new TimeSpanBlock3()
            {
                StartTime = "11:00",
                DurationMinutes = 240
            };

            TimeSpanBlock3 evening = new TimeSpanBlock3()
            {
                StartTime = "17:00",
                DurationMinutes = 480
            };

            TimeSpanBlock3 eveningLater = new TimeSpanBlock3()
            {
                StartTime = "17:30",
                DurationMinutes = 480
            };

            // Extract service times from the test data
            SiteDetailsDataAccess siteDetailsDataAccess = new SiteDetailsDataAccess();
            List<ServiceTimes> siteServiceTimes = siteDetailsDataAccess.ExtractOccasionTimes(siteOccasionTimes);

            // Check the results
            Assert.IsNotNull(siteServiceTimes);
            Assert.AreEqual(3, siteServiceTimes.Count); // Delivery, collection and dine in

            bool deliveryFound = false;
            bool collectionFound = false;
            bool dineInFound = false;

            foreach (ServiceTimes siteServices in siteServiceTimes)
            {
                if (siteServices.Occasion.ToLower() == "delivery")
                {
                    deliveryFound = true;
                    TestServiceTimesDay(siteServices.Monday, true, false, null);
                    TestServiceTimesDay(siteServices.Tuesday, false, false, new List<TimeSpanBlock3>() { lunchTime });
                    TestServiceTimesDay(siteServices.Wednesday, false, true, null);
                    TestServiceTimesDay(siteServices.Thursday, false, false, new List<TimeSpanBlock3>() { eveningLater });
                    TestServiceTimesDay(siteServices.Friday, false, false, new List<TimeSpanBlock3>() { lunchTime });
                    TestServiceTimesDay(siteServices.Saturday, false, true, null);
                    TestServiceTimesDay(siteServices.Sunday, false, true, null);
                }
                if (siteServices.Occasion.ToLower() == "collection")
                {
                    collectionFound = true;
                    TestServiceTimesDay(siteServices.Monday, true, false, null);
                    TestServiceTimesDay(siteServices.Tuesday, false, false, new List<TimeSpanBlock3>() { lunchTime });
                    TestServiceTimesDay(siteServices.Wednesday, true, false, null);
                    TestServiceTimesDay(siteServices.Thursday, false, false, new List<TimeSpanBlock3>() { evening });
                    TestServiceTimesDay(siteServices.Friday, false, false, new List<TimeSpanBlock3>() { lunchTime, evening });
                    TestServiceTimesDay(siteServices.Saturday, false, true, null);
                    TestServiceTimesDay(siteServices.Sunday, false, true, null);
                }
                if (siteServices.Occasion.ToLower() == "dinein")
                {
                    dineInFound = true;
                    TestServiceTimesDay(siteServices.Monday, false, true, null);
                    TestServiceTimesDay(siteServices.Tuesday, false, true, null);
                    TestServiceTimesDay(siteServices.Wednesday, false, true, null);
                    TestServiceTimesDay(siteServices.Thursday, false, true, null);
                    TestServiceTimesDay(siteServices.Friday, false, true, null);
                    TestServiceTimesDay(siteServices.Saturday, false, true, null);
                    TestServiceTimesDay(siteServices.Sunday, false, true, null);
                }
            }

            Assert.IsTrue(deliveryFound);
            Assert.IsTrue(collectionFound);
            Assert.IsTrue(dineInFound);
        }

        private static void TestServiceTimesDay(
            DayServiceTimes checkDayServiceTimes, 
            bool isOpenAllDay, 
            bool isClosedAllDay, 
            List<TimeSpanBlock3> expectedOpeningTimes)
        {
            Assert.IsNotNull(checkDayServiceTimes);
            Assert.AreEqual(isOpenAllDay, checkDayServiceTimes.IsOpenAllDay);
            Assert.AreEqual(isClosedAllDay, checkDayServiceTimes.IsClosedAllDay);

            if (!isOpenAllDay && !isClosedAllDay)
            {
                int foundCount = 0;
                foreach (TimeSpanBlock3 expectedTimeSpanBlock in expectedOpeningTimes)
                {
                    foreach(TimeSpanBlock3 timeSpanBlock3 in checkDayServiceTimes.Times)
                    {
                        if (timeSpanBlock3.StartTime == expectedTimeSpanBlock.StartTime &&
                            timeSpanBlock3.DurationMinutes == expectedTimeSpanBlock.DurationMinutes)
                        {
                            foundCount++;
                            break;
                        }
                    }
                }

                Assert.AreEqual(expectedOpeningTimes.Count, foundCount);
            }
        }
    }
}
