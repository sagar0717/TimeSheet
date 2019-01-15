using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSheetManagement.Business
{
    /// <summary>
    /// Details of an employee stored in EmployeeBook.
    /// </summary>
    public class Employee
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public double EmpHourRate { get; set; }
    }
}
