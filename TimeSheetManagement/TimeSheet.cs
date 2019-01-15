using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TimeSheetManagement.Business
{
    public class TimeSheet
    {
        /// <summary>
        /// The in-memory "database" of timesheet.
        /// TimeSheetSave must be called if any changes are to be persisted.
        /// </summary>
        private List<TimeSheetEntry> timeSheet;

        /// <summary>
        /// The filename to store the timesheet.
        /// </summary>        
        private const string timeSheetPath = "timesheet.xml";

        DateTime dayStartTime; // Time entry for start of day.
        DateTime dayEndTime; //Time entry for end of day.

        /// <summary>
        /// Create a new, empty timesheet.
        /// </summary>
        public TimeSheet()
        {
            timeSheet = new List<TimeSheetEntry>();
        }

        public bool InputHoursForDay(int employeeId, string strTime, string endTime)
        {
            // Empty employee book
            EmployeeBook employees = EmployeeBook.Instance();

            // If employee with the searched name exists
            if (employees.AllEmployees.Where(x => x.EmpId == employeeId).Count() > 0)
            {
                Employee emp = employees.AllEmployees.Where(x => x.EmpId == employeeId).FirstOrDefault(); // Fetching the particular employee for whom timecard entry needs to be added

                if (Util.ValidateDate(strTime, ref dayStartTime) && Util.ValidateDate(endTime, ref dayEndTime)) // validating start and end time is in correct format
                {
                    bool duplicateEntry = Util.ValidateTimeSheetEntry(dayStartTime, timeSheet, emp); // validating the duplicacy of the entry already done

                    // if no duplicate entry exists then creating new entry for the day for the respective user.
                    if (!duplicateEntry)
                    {
                        AddEntry(new TimeSheetEntry()
                        {
                            EmployeeID = emp.EmpId,
                            InTime = dayStartTime,
                            OutTime = dayEndTime
                        });


                        TimeSheetSave(timeSheetPath); // Saving the values in xml file
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AddEntry(TimeSheetEntry employee)
        {
            timeSheet.Add(employee);
            return true;
        }



        /// <summary>
        /// This method calculate the weekly wages for a particular employee based on Weeknumber of the year
        /// </summary>
        /// <param name="employeeName"></param>
        /// <param name="weekNumber"></param>
        /// <returns></returns>
        public WorkWeekDetails CalculateWeeklyWages(string employeeName, int weekNumber)
        {
            double payables = 0;
            double hoursWorkedInWeek = 0;

            EmployeeBook employees = EmployeeBook.Instance();
            if (employees.AllEmployees.Where(x => x.EmpName == employeeName).Count() > 0)
            {
                Employee emp = employees.AllEmployees.Where(x => x.EmpName == employeeName).FirstOrDefault();

                foreach (TimeSheetEntry entry in timeSheet.Where(x => x.EmployeeID == emp.EmpId))
                {
                    hoursWorkedInWeek = hoursWorkedInWeek + entry.NumberOfHoursWorked;
                    payables = payables + (entry.NumberOfHoursWorked * (emp.EmpHourRate + ((entry.Penalty * emp.EmpHourRate) / 100)));
                }

                return new WorkWeekDetails()
                {
                    WeekNumber = weekNumber,
                    HoursWorked = Math.Round(hoursWorkedInWeek, 2),
                    Cost = Math.Round(payables, 2)
                };
            }

            return null;
        }

        /// <summary>
        /// This is an overloaded method which calculates weekly wages for a particular employee based on dates in the same week
        /// </summary>
        /// <param name="employeeName"></param>
        /// <param name="weekStartDate"></param>
        /// <param name="weekEndDate"></param>
        /// <returns></returns>
        public WorkWeekDetails CalculateWeeklyWages(string employeeName, DateTime weekStartDate, DateTime weekEndDate)
        {
            double payables = 0;
            double hoursWorkedInWeek = 0;
            EmployeeBook employees = EmployeeBook.Instance();


            if (employees.AllEmployees.Where(x => x.EmpName == employeeName).Count() > 0)
            {
                Employee emp = employees.AllEmployees.Where(x => x.EmpName == employeeName).FirstOrDefault();
                int StartWeek = Util.GetWeekNumber(weekStartDate);
                int EndWeek = Util.GetWeekNumber(weekEndDate);

                if (StartWeek == EndWeek)
                {

                    foreach (TimeSheetEntry entry in timeSheet.Where(x => x.EmployeeID == emp.EmpId))
                    {
                        hoursWorkedInWeek = hoursWorkedInWeek + entry.NumberOfHoursWorked;
                        payables = payables + (entry.NumberOfHoursWorked * (emp.EmpHourRate + ((entry.Penalty * emp.EmpHourRate) / 100)));
                    }

                    return new WorkWeekDetails()
                    {
                        WeekNumber = StartWeek,
                        HoursWorked = Math.Round(hoursWorkedInWeek, 2),
                        Cost = Math.Round(payables, 2)
                    };
                }
            }
            return null;
        }


        public void TimeSheetLoad(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<TimeSheetEntry>));
                timeSheet = serializer.ReadObject(stream) as List<TimeSheetEntry>;
            }
        }

        public void TimeSheetSave(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<TimeSheetEntry>));
                serializer.WriteObject(stream, timeSheet);
            }
        }

    }
}
