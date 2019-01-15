using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSheetManagement.Business
{
    class EmployeeValidator : IValidator<Employee>
    {
        private const double maxRatePerHour = 50;
        public bool Validate(Employee t)
        {
            if (t.EmpId == 0 || String.IsNullOrEmpty(t.EmpName) || t.EmpHourRate == 0)
                return true;
             return t.EmpHourRate > maxRatePerHour ? true : false;
        }
    }
}
