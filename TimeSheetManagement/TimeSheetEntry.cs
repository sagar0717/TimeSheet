using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TimeSheetManagement.Business
{
    [DataContract(Name = "TimeCard")]
    public class TimeSheetEntry
    {
        [DataMember]
        public Guid TimeSheetId { get; set; } = Guid.NewGuid();
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember(Order = 0)]
        public DateTime InTime { get; set; }
        [DataMember(Order = 1)]
        public DateTime OutTime { get; set; }


        private const double penaltyRateWeekDay = 0;
        private const double penaltyRateSaturday = 1.5;
        private const double penaltyRateSunday = 2;


        private double _penalty;

        private int _week;

        private double _numberofHoursWorked;

        private DayOfWeek _workDay;

        [DataMember]
        public int Week
        {
            get
            {
                return Util.GetWeekNumber(InTime);
            }
            private set
            {
                _week = Util.GetWeekNumber(InTime);
            }
        }

        [DataMember]
        public double NumberOfHoursWorked
        {
            get
            {
                if (CalculateDayWorkHours(InTime, OutTime) < 0 || CalculateDayWorkHours(InTime, OutTime) > 24)
                {
                    throw new ArgumentException();
                }

                return CalculateDayWorkHours(InTime, OutTime);
            }
            private set
            {
                if (CalculateDayWorkHours(InTime, OutTime) < 0 || CalculateDayWorkHours(InTime, OutTime) > 24)
                {
                    throw new ArgumentException();
                }
                _numberofHoursWorked = CalculateDayWorkHours(InTime, OutTime);
            }

        }

        [DataMember]
        public DayOfWeek WorkDay
        {
            get
            {
                return InTime.DayOfWeek;
            }
            private set
            {
                _workDay = InTime.DayOfWeek;
            }
        }

        [DataMember]
        public double Penalty
        {
            get
            {
                return CalculatePenaltyRate(WorkDay);
            }
            private set
            {
                _penalty = CalculatePenaltyRate(WorkDay);
            }
        }

        public double CalculateDayWorkHours(DateTime inTime, DateTime outTime)
        {
            TimeSpan hoursWorked = outTime - inTime;
            double roundedworkHours = Math.Round(hoursWorked.TotalHours, 2);
            return roundedworkHours;
        }

        /// <summary>
        /// This method returns the penalty rate applied based on working day i.e. weekday or weekend
        /// </summary>
        /// <param name="Day"></param>
        /// <returns>Rate for the day</returns>
        public double CalculatePenaltyRate(DayOfWeek Day)
        {
            double rate;
            if (Day == DayOfWeek.Saturday)
            {
                rate = penaltyRateSaturday;
            }
            else if (Day == DayOfWeek.Sunday)
            {
                rate = penaltyRateSunday;
            }
            else
                rate = penaltyRateWeekDay;

            return rate;
        }
    }
}
