using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AndroAdminDataAccess.Domain;
using AndroAdmin.Model;
using AndroAdmin.Helpers;

namespace AndroAdminTests
{
    [TestClass]
    public class OpeningTimesTests
    {
        private StoreModel TestSiteModel { get; set; }

        private void ResetTestData()
        {
            // Monday opening times
            TimeSpanBlock mondayTimeSpanBlock = new TimeSpanBlock();
            mondayTimeSpanBlock.OpenAllDay = false;
            mondayTimeSpanBlock.Day = "Monday";
            mondayTimeSpanBlock.StartTime = "08:30"; // Stored in the db using 24hour clock (i.e. no AM or PM)
            mondayTimeSpanBlock.EndTime = "10:30"; // Stored in the db using 24hour clock (i.e. no AM or PM)

            List<TimeSpanBlock> mondayOpeningTimes = new List<TimeSpanBlock>();
            mondayOpeningTimes.Add(mondayTimeSpanBlock);

            // Tuesday opening times
            TimeSpanBlock tuesdayTimeSpanBlock = new TimeSpanBlock();
            tuesdayTimeSpanBlock.Day = "Tuesday";
            tuesdayTimeSpanBlock.OpenAllDay = true;

            List<TimeSpanBlock> tuesdayOpeningTimes = new List<TimeSpanBlock>();
            tuesdayOpeningTimes.Add(tuesdayTimeSpanBlock);

            // Build a list of existing opening times
            this.TestSiteModel = new StoreModel();
            this.TestSiteModel.OpeningTimesByDay = new Dictionary<string, List<TimeSpanBlock>>();
            this.TestSiteModel.OpeningTimesByDay.Add("Monday", mondayOpeningTimes);
            this.TestSiteModel.OpeningTimesByDay.Add("Tuesday", tuesdayOpeningTimes);
        }

        [TestMethod]
        [Description("Start and end times cannot be the same")]
        [TestCategory("OpeningHours")]
        public void OpenAndCloseTimesSame()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan = new TimeSpan(12, 30, 0);
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start and end times cannot be the same", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour cannot be after end hour")]
        [TestCategory("OpeningHours")]
        public void StartHourAfterEndHour()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("01:30 PM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start hour cannot be after end hour", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start minute cannot be after end minute")]
        [TestCategory("OpeningHours")]
        public void StartMinuteAfterEndMinute()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("12:45 PM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start minute cannot be after end minute", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Site already open all day")]
        [TestCategory("OpeningHours")]
        public void AddHoursButAlreadyOpenAllDay()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("12:05 PM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Tuesday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Site already has opening hours")]
        [TestCategory("OpeningHours")]
        public void AddOpenAllDayButAlreadyHasHours()
        {
            //this.ResetTestData();

            //TimeSpan startTimeSpan = new TimeSpan(12, 05, 0);
            //TimeSpan endTimeSpan = new TimeSpan(12, 30, 0);

            //string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            //Assert.AreEqual("Site already has opening hours", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour and minutes same as existing)")]
        [TestCategory("OpeningHours")]
        public void StartHourClashesWithAnExistingOpeningTime()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("08:30 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour same as existing - different minutes)")]
        [TestCategory("OpeningHours")]
        public void StartHourClashesWithAnExistingOpeningTime2()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("08:45 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour inside existing start and end hour - no minutes)")]
        [TestCategory("OpeningHours")]
        public void StartHourClashesWithAnExistingOpeningTime3()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("09:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour & minutes inside existing start and end hour)")]
        [TestCategory("OpeningHours")]
        public void StartHourClashesWithAnExistingOpeningTime4()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("09:23 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour same as existing end hour - no minutes)")]
        [TestCategory("OpeningHours")]
        public void StartHourClashesWithAnExistingOpeningTime5()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("10:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour same as existing end hour - minutes less")]
        [TestCategory("OpeningHours")]
        public void StartHourClashesWithAnExistingOpeningTime6()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("10:15 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Start hour clashes with an existing opening time (start hour & minutes same as existing end hour & minutes")]
        [TestCategory("OpeningHours")]
        public void StartHourClashesWithAnExistingOpeningTime7()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("10:30 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:30 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Start hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour and minutes same as existing start)")]
        [TestCategory("OpeningHours")]
        public void EndHourClashesWithAnExistingOpeningTime()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("07:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("08:30 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour same as existing start - different minutes)")]
        [TestCategory("OpeningHours")]
        public void EndHourClashesWithAnExistingOpeningTime2()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("07:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("08:35 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour inside existing start and end hour - no minutes)")]
        [TestCategory("OpeningHours")]
        public void EndHourClashesWithAnExistingOpeningTime3()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("07:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("09:00 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour & minutes inside existing start and end hour)")]
        [TestCategory("OpeningHours")]
        public void EndHourClashesWithAnExistingOpeningTime4()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("07:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("09:30 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour same as existing end hour - no minutes)")]
        [TestCategory("OpeningHours")]
        public void EndHourClashesWithAnExistingOpeningTime5()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("07:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("10:00 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour same as existing end hour - minutes less")]
        [TestCategory("OpeningHours")]
        public void EndHourClashesWithAnExistingOpeningTime6()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("07:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("10:25 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("End hour clashes with an existing opening time (end hour & minutes same as existing end hour & minutes")]
        [TestCategory("OpeningHours")]
        public void EndHourClashesWithAnExistingOpeningTime7()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("07:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("10:30 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("End hour clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Existing opening time clashes with an existing opening time")]
        [TestCategory("OpeningHours")]
        public void ExistingOpeningTimeClashesWithAnExistingOpeningTime()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("08:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("11:15 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("Opening time clashes with an existing opening time", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Add opening time before")]
        [TestCategory("OpeningHours")]
        public void AddOpeningTimeBefore1()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("06:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("07:15 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Add opening time before - end hour = existing start hour")]
        [TestCategory("OpeningHours")]
        public void AddOpeningTimeBefore2()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("06:00 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("08:15 AM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Add opening time after")]
        [TestCategory("OpeningHours")]
        public void AddOpeningTimeAfter1()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("11:30 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:15 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }

        [TestMethod]
        [Description("Add opening time after - start hour = existing end hour")]
        [TestCategory("OpeningHours")]
        public void AddOpeningTimeAfter2()
        {
            this.ResetTestData();

            TimeSpan startTimeSpan;
            OpeningTimesHelper.ParseTime("10:45 AM", out startTimeSpan); // Time picker control uses 12 hour format with AM or PM

            TimeSpan endTimeSpan;
            OpeningTimesHelper.ParseTime("12:15 PM", out endTimeSpan); // Time picker control uses 12 hour format with AM or PM

            string errorMessage = OpeningTimesHelper.CheckNewOpeningTime("Monday", this.TestSiteModel, startTimeSpan, endTimeSpan);

            Assert.AreEqual("", errorMessage, "Wrong error message: " + errorMessage);
        }
    }
}
