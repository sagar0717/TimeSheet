using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TimeSheetManagement.Business;

namespace TimeSheetManagement.Test
{
    [TestFixture]
    class ManageTimeSheetTest
    {
        EmployeeBook employees;
        TimeSheet timeSheet;
        string timesheetpath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\TimeSheetManagement\bin\Debug\netcoreapp2.1\timesheet.xml"));
        string employeepath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\TimeSheetManagement\bin\Debug\netcoreapp2.1\employees.xml"));

        [OneTimeSetUp]
        public void TestInit()
        {
            timeSheet = new TimeSheet();

            if (File.Exists(timesheetpath))
                timeSheet.TimeSheetLoad(timesheetpath);

            employees = EmployeeBook.Instance();
            Employee emp = new Employee()
            {
                EmpId = 1,
                EmpName = "NewUser",
                EmpHourRate = 25
            };
            employees.Add(emp);
        }

        [Test]
        public void Should_Return_True_When_New_TimeSheet_Entry_Added_With_ValidData()
        {
            //Arrange
            int employeeId = 1;
            string inTime = "05/01/2019 09:00:00";
            string outTime = "05/01/2019 17:20:00";

            //Act
            bool expectedValue = timeSheet.InputHoursForDay(employeeId, inTime, outTime);

            //Assert
            Assert.IsTrue(expectedValue, "TimeSheet entry not added");
        }

        [Test]
        public void Should_Return_False_When_Duplicate_TimeSheet_Entry_Added_With_ValidData()
        {
            //Arrange

            timeSheet.AddEntry(new TimeSheetEntry
            {
                EmployeeID = 2,
                InTime = new DateTime(2019, 1, 05, 09, 00, 05),
                OutTime = new DateTime(2019, 1, 05, 16, 30, 00)
            });

            int employeeId = 2;
            string inTime = "05/01/2019 09:00:05";
            string outTime = "05/01/2019 16:30:00";

            //Act
            bool expectedValue = timeSheet.InputHoursForDay(employeeId, inTime, outTime);

            //Assert
            Assert.IsFalse(expectedValue, "TimeSheet entry Sucessfully Added");
        }

        [Test]
        public void Should_Return_False_When_TimeSheet_Entry_Added_With_Invalid_EmployeeId()
        {
            //Arrange
            int employeeId = 200;
            string inTime = "05/01/2019 09:00:05";
            string outTime = "05/01/2019 16:30:00";

            //Act
            bool expectedValue = timeSheet.InputHoursForDay(employeeId, inTime, outTime);

            //Assert
            Assert.IsFalse(expectedValue, "TimeSheet entry Sucessfully Added");
        }

        [Test]
        public void Should_Return_False_When_TimeSheet_Entry_Added_With_InvalidFormat_InTime()
        {
            //Arrange

            int employeeId = 1;
            string inTime = "05-01-2019 09:00:05";
            string outTime = "05/01/2019 16:30:00";

            //Act
            bool expectedValue = timeSheet.InputHoursForDay(employeeId, inTime, outTime);

            //Assert
            Assert.IsFalse(expectedValue, "TimeSheet entry Sucessfully Added");
        }

        [Test]
        public void Should_Return_False_When_TimeSheet_Entry_Added_With_InvalidFormat_OutTime()
        {
            //Arrange

            int employeeId = 1;
            string inTime = "05/01/2019 09:00:00";
            string outTime = "05-01-2019 16:30:05";

            //Act
            bool expectedValue = timeSheet.InputHoursForDay(employeeId, inTime, outTime);

            //Assert
            Assert.IsFalse(expectedValue, "TimeSheet entry Sucessfully Added");
        }

        [OneTimeTearDown]
        public void TestCleanup()
        {
            if (File.Exists(timesheetpath))
            {
                File.Delete(timesheetpath);
            }

            if (File.Exists(employeepath))
            {
                File.Delete(employeepath);
            }
        }
    }
}
