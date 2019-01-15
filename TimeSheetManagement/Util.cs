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
