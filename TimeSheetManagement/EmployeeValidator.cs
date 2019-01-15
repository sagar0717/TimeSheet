using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSheetManagement.Business
{
    /// <summary>
    /// A class to validate the attributes for Employee Class before adding to the employee list
    /// </summary>
    class EmployeeValidator : IValidator<Employee>
    {
        private const double maxRatePerHour = 50; // maximu rate allowed per hour
        public bool Validate(Employee t)
        {
            if (t.EmpId == 0 || String.IsNullOrEmpty(t.EmpName) || t.EmpHourRate == 0) // if any of the connsditions matches and return true else false
                return true;
             return t.EmpHourRate > maxRatePerHour ? true : false;
        }
    }
}
