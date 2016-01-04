using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAndromeda.Web.Models;
using MyAndromedaDataAccess.Domain;

namespace MyAndromeda.Tests.Helpers
{
    [TestClass]
    public class OpeningTimesHelperTest
    {
        private SiteModel TestSiteModel { get; set; }

        private void ResetTestData()
        {
            // Monday opening times
            TimeSpanBlock mondayTimeSpanBlock = new TimeSpanBlock();
            mondayTimeSpanBlock.OpenAllDay = false;
            mondayTimeSpanBlock.Day = "Monday";
            mondayTimeSpanBlock.StartTime = "08:30";
            mondayTimeSpanBlock.EndTime = "10:30";

            List<TimeSpanBlock> mondayOpeningTimes = new List<TimeSpanBlock>();
            mondayOpeningTimes.Add(mondayTimeSpanBlock);

            // Tuesday opening times
            TimeSpanBlock tuesdayTimeSpanBlock = new TimeSpanBlock();
            tuesdayTimeSpanBlock.Day = "Tuesday";
            tuesdayTimeSpanBlock.OpenAllDay = true;

            List<TimeSpanBlock> tuesdayOpeningTimes = new List<TimeSpanBlock>();
            tuesdayOpeningTimes.Add(tuesdayTimeSpanBlock);

            // Build a list of existing opening times
            this.TestSiteModel = new SiteModel();
            this.TestSiteModel.OpeningTimesByDay = new Dictionary<string, List<TimeSpanBlock>>();
            this.TestSiteModel.OpeningTimesByDay.Add("Monday", mondayOpeningTimes);
            this.TestSiteModel.OpeningTimesByDay.Add("Tuesday", tuesdayOpeningTimes);
        }

        [TestMethod]
        [Description("Start and end times cannot be the same")]
        public void OpenAndCloseTimesSame()
        {
            this.ResetTestData();

//            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 12;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 30;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start and end times cannot be the same", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour cannot be after end hour")]
        public void StartHourAfterEndHour()
        {
            this.ResetTestData();

 //           //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 13;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 30;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start hour cannot be after end hour", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start minute cannot be after end minute")]
        public void StartMinuteAfterEndMinute()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 12;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 45;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start minute cannot be after end minute", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Site already open all day")]
        public void AddHoursButAlreadyOpenAllDay()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 12;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 5;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Tuesday", this.TestSiteModel);

            Assert.AreEqual("Site already open all day", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Site already has opening hours")]
        public void AddOpenAllDayButAlreadyHasHours()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAllDay;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Site already has opening hours", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour and minutes same as existing)")]
        public void StartHourClashesWithAnExistingOpeningTime()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 8;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 30;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour same as existing - different minutes)")]
        public void StartHourClashesWithAnExistingOpeningTime2()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 8;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 45;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour inside existing start and end hour - no minutes)")]
        public void StartHourClashesWithAnExistingOpeningTime3()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 9;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour & minutes inside existing start and end hour)")]
        public void StartHourClashesWithAnExistingOpeningTime4()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 9;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 23;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour same as existing end hour - no minutes)")]
        public void StartHourClashesWithAnExistingOpeningTime5()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 10;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour same as existing end hour - minutes less")]
        public void StartHourClashesWithAnExistingOpeningTime6()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 10;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 15;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour & minutes same as existing end hour & minutes")]
        public void StartHourClashesWithAnExistingOpeningTime7()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 10;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 30;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour and minutes same as existing start)")]
        public void EndHourClashesWithAnExistingOpeningTime()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 7;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 8;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour same as existing start - different minutes)")]
        public void EndHourClashesWithAnExistingOpeningTime2()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 7;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 8;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 35;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour inside existing start and end hour - no minutes)")]
        public void EndHourClashesWithAnExistingOpeningTime3()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 7;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 9;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 0;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour & minutes inside existing start and end hour)")]
        public void EndHourClashesWithAnExistingOpeningTime4()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 7;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 9;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour same as existing end hour - no minutes)")]
        public void EndHourClashesWithAnExistingOpeningTime5()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 7;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 10;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 0;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour same as existing end hour - minutes less")]
        public void EndHourClashesWithAnExistingOpeningTime6()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 7;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 10;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 25;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour & minutes same as existing end hour & minutes")]
        public void EndHourClashesWithAnExistingOpeningTime7()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 7;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 10;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 30;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Existing opening time clashes with an existing opening time")]
        public void ExistingOpeningTimeClashesWithAnExistingOpeningTime()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 8;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 11;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 15;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("Opening time clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Add opening time before")]
        public void AddOpeningTimeBefore1()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 6;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 7;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 15;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Add opening time before - end hour = existing start hour")]
        public void AddOpeningTimeBefore2()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 6;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 0;
            // this.TestSiteModel.AddOpeningEndTimeHour = 8;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 15;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Add opening time after")]
        public void AddOpeningTimeAfter1()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 11;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 30;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 15;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Add opening time after - start hour = existing end hour")]
        public void AddOpeningTimeAfter2()
        {
            this.ResetTestData();

            //this.TestSiteModel.StoreHoursMode = MyAndromeda.Helper.OpeningTimesHelper.IsOpenAtSpecficTimes;
            // this.TestSiteModel.AddOpeningStartTimeHour = 10;
            // this.TestSiteModel.AddOpeningStartTimeMinute = 45;
            // this.TestSiteModel.AddOpeningEndTimeHour = 12;
            // this.TestSiteModel.AddOpeningEndTimeMinute = 15;

            string errorMessage = ""; //OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }
    }
}
