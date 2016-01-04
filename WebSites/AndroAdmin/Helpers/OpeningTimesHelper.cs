using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdmin.Model;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.Helpers
{
    public class OpeningTimesHelper
    {
        public const string IsOpenAllDay = "IsOpenAllDay";
        public const string IsClosedAllDay = "IsClosedAllDay";
        public const string IsOpenAtSpecficTimes = "IsOpenAtSpecficTimes";

        public static bool ParseTime(string timeText, out TimeSpan timeSpan)
        {
            bool success = false;
            timeSpan = TimeSpan.Zero;

            // Times come in the following format: "HH:MM AM"
            string hoursText = timeText.Substring(0, 2);
            string minutesText = timeText.Substring(3, 2);
            string amPmText = timeText.Substring(6, 2);

            int hours = 0;
            int minutes = 0;

            // Are the hours numeric?
            if (int.TryParse(hoursText, out hours))
            {
                // Are the minutes numeric?
                if (int.TryParse(minutesText, out minutes))
                {
                    // Is the time after 12 noon?
                    if (amPmText.Equals("PM", StringComparison.InvariantCulture))
                    {
                        if (hours != 12)
                        {
                            // We are storing times as 24hour in the database
                            hours += 12;
                        }
                    }

                    timeSpan = new TimeSpan(hours, minutes, 0);
                    success = true;
                }
            }

            return success;
        }

        /// <summary>
        /// Checks that a new opening time does not conflict with existing opening times
        /// </summary>
        /// <param name="day">The day of the week e.g. Monday</param>
        /// <param name="siteModel">The existing opening hours from the db</param>
        /// <param name="startTimeSpan">The new opening time HH:MM AM/PM</param>
        /// <param name="endTimeSpan">The new closing time HH:MM AM/PM</param>
        /// <returns></returns>
        public static string CheckNewOpeningTime(string day, StoreModel siteModel, TimeSpan startTimeSpan, TimeSpan endTimeSpan)
        {
            string errorMessage = "";

            // Make sure the open and close times are not the same
            if (startTimeSpan.Hours == endTimeSpan.Hours &&
                startTimeSpan.Minutes == endTimeSpan.Minutes)
            {
                errorMessage = "Start and end times cannot be the same";
            }

            // Make sure the opening hour is before the closing hour
            if (errorMessage.Length == 0)
            {
                if (startTimeSpan.Hours > endTimeSpan.Hours)
                {
                    errorMessage = "Start hour cannot be after end hour";
                }
            }

            // If the open and close hours are the same check that the open minutes is before the close minutes
            if (errorMessage.Length == 0)
            {
                if (startTimeSpan.Hours == endTimeSpan.Hours &&
                    startTimeSpan.Minutes > endTimeSpan.Minutes)
                {
                    errorMessage = "Start minute cannot be after end minute";
                }
            }

            // Check to see if the new open close block overlaps any existing blocks
            if (errorMessage.Length == 0)
            {
                // Any existing blocks?
                if (siteModel.OpeningTimesByDay != null)
                {
                    List<TimeSpanBlock> dayOpeningTimes = null;

                    // Are there any opening times for the day?
                    if (siteModel.OpeningTimesByDay.TryGetValue(day, out dayOpeningTimes))
                    {
                        // Check the new opening time against each existing opening time
                        foreach (TimeSpanBlock openingTime in dayOpeningTimes)
                        {
                            // Is the store already open all day?
                            if (openingTime.OpenAllDay)
                            {
                                // The site is currently set to open all day.  We're effectively changing it to be open between specific
                                // times so there is no possibility of a clash
                                break;
                            }
                            else
                            {
                                TimeSpan existingStartTimeSpan;
                                if (TimeSpan.TryParse(openingTime.StartTime, out existingStartTimeSpan))
                                {
                                    TimeSpan existingEndTimeSpan;
                                    if (TimeSpan.TryParse(openingTime.EndTime, out existingEndTimeSpan))
                                    {
                                        // Is the start time inside another opening time?
                                        if ((startTimeSpan.Hours > existingStartTimeSpan.Hours ||
                                            (startTimeSpan.Hours == existingStartTimeSpan.Hours &&
                                            startTimeSpan.Minutes >= existingStartTimeSpan.Minutes)) &&
                                            (startTimeSpan.Hours < existingEndTimeSpan.Hours ||
                                            (startTimeSpan.Hours == existingEndTimeSpan.Hours &&
                                            startTimeSpan.Minutes <= existingEndTimeSpan.Minutes)))
                                        {
                                            errorMessage = "Start hour clashes with an existing opening time";
                                            break;
                                        }
                                        // Is the end hour inside another opening time?
                                        else if ((endTimeSpan.Hours > existingStartTimeSpan.Hours ||
                                            (endTimeSpan.Hours == existingStartTimeSpan.Hours &&
                                            endTimeSpan.Minutes >= existingStartTimeSpan.Minutes)) &&
                                            (endTimeSpan.Hours < existingEndTimeSpan.Hours ||
                                            (endTimeSpan.Hours == existingEndTimeSpan.Hours &&
                                            endTimeSpan.Minutes <= existingEndTimeSpan.Minutes)))
                                        {
                                            errorMessage = "End hour clashes with an existing opening time";
                                            break;
                                        }
                                        // Is the existing opening time entirely inside the new opening hour?
                                        else if ((startTimeSpan.Hours < existingStartTimeSpan.Hours ||
                                            (startTimeSpan.Hours == existingStartTimeSpan.Hours &&
                                            startTimeSpan.Minutes < existingStartTimeSpan.Minutes)) &&
                                            (endTimeSpan.Hours > existingStartTimeSpan.Hours ||
                                            (endTimeSpan.Hours == existingStartTimeSpan.Hours &&
                                            endTimeSpan.Minutes > existingStartTimeSpan.Minutes)))
                                        {
                                            errorMessage = "Opening time clashes with an existing opening time";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return errorMessage;
        }
    }
}