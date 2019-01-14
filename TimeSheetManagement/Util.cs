using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TimeSheetManagement.Business
{
    public static class Util
    {
        /// <summary>
        /// This method validates the date entered against the right format allowed (dd/MM/yyyy HH:mm:ss)
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="dateTime"></param>
        /// <returns>Result (True or False)</returns>
        public static bool ValidateDate(string Date, ref DateTime dateTime)
        {
            try
            {
                dateTime = DateTime.ParseExact(Date, "dd/MM/yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method validates the every new timecard entry against the enteries 
        /// already available for the particular employee on specific date to check for Duplicacy
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeCardEntries"></param>
        /// <param name="employee"></param>
        /// <returns>Result (True or False)</returns>
        public static bool ValidateTimeSheetEntry(DateTime date, List<TimeSheetEntry> timeSheetEntries, Employee employee)
        {
            if (timeSheetEntries.Count > 0)
            {
                foreach (TimeSheetEntry entry in timeSheetEntries)
                {
                    if (entry.EmployeeID == employee.EmpId && entry.InTime.Date == date.Date)
                        return true; // returns true if entry already available
                }
            }
            return false; // returns false if no entry is available
        }

        /// <summary>
        /// This method returns the week number from the year for the date enquired based on culture
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Result(week number)</returns>
        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;
            /*  var firstDayWeek = cul.Calendar.GetWeekOfYear(
                date,
                CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);
                int year = weekNum == 52 && date.Month == 1 ? date.Year - 1 : date.Year; */

            int weekNum = cul.Calendar.GetWeekOfYear(
                date,
                CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);

            return weekNum;
        }

    }
}
