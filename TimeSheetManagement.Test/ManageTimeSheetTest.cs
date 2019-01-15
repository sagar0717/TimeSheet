using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TimeSheetManagement.Business;

namespace TimeSheetManagement.Test
{
    [TestFixture]
    public class ManageTimeSheetTest
    {
        public class InputHoursMethodTest
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

                timeSheet = null;
                employees = null;
            }
        }

        public class CalculateWageMethodTest
        {
            EmployeeBook employees;
            TimeSheet timeSheet;
            WorkWeekDetails weekDetails;
            string timesheetpath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\TimeSheetManagement\bin\Debug\netcoreapp2.1\timesheet.xml"));
            string employeepath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\TimeSheetManagement\bin\Debug\netcoreapp2.1\employees.xml"));


            [OneTimeSetUp]
            public void TestInit()
            {
                timeSheet = new TimeSheet();
                employees = EmployeeBook.Instance();
                weekDetails = new WorkWeekDetails();

                Employee emp = new Employee()
                {
                    EmpId = 1,
                    EmpName = "NewUser",
                    EmpHourRate = 25
                };
                employees.Add(emp);
            }

            [Test]
            public void Should_Return_Total_HoursWorked_Cost_ForWeek_When_Employee_EntryPresent()
            {
                //Arrange

                TimeSheetEntry firstEntry = new TimeSheetEntry()
                {
                    EmployeeID = 1,
                    InTime = new DateTime(2019, 1, 15, 09, 00, 00),
                    OutTime = new DateTime(2019, 1, 15, 16, 30, 00)
                };
                TimeSheetEntry secondEntry = new TimeSheetEntry()
                {
                    EmployeeID = 1,
                    InTime = new DateTime(2019, 1, 16, 09, 00, 00),
                    OutTime = new DateTime(2019, 1, 16, 16, 30, 00)
                };

                WorkWeekDetails expected = new WorkWeekDetails()
                {
                    WeekNumber = 3,
                    HoursWorked = 15,
                    Cost = 375
                };

                timeSheet.AddEntry(firstEntry);
                timeSheet.AddEntry(secondEntry);

                int employeeId = 1;
                int weekNumber = 3;

                

                //Act
                weekDetails = timeSheet.CalculateWeeklyWages(employeeId, weekNumber);

                //Assert
                Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(expected),
                    Newtonsoft.Json.JsonConvert.SerializeObject(weekDetails));
            }

            [Test]
            public void Should_Return_Null_When_Employee_Entry_Not_Present()
            {
                //Arrange

                int employeeId = 1;
                int weekNumber = 2;

                //Act
                weekDetails = timeSheet.CalculateWeeklyWages(employeeId, weekNumber);

                //Assert
                Assert.IsNull(weekDetails, "Entry Present for the Employee");
            }

            [Test]
            public void Should_Return_Null_When_Employee_Not_Present()
            {
                //Arrange

                int employeeId = 100;
                int weekNumber = 2;

                //Act
                weekDetails = timeSheet.CalculateWeeklyWages(employeeId, weekNumber);

                //Assert
                Assert.IsNull(weekDetails, "Employee present in the system");
            }

            [Test]
            public void Should_Return_Null_When_Dates_NotIn_SameWeek()
            {
                //Arrange

                int employeeId = 1;
                DateTime weekStartDate = new DateTime(1999, 1, 13);
                DateTime weekEndDate = new DateTime(1999, 2, 13);

                //Act
                weekDetails = timeSheet.CalculateWeeklyWages(employeeId, weekStartDate, weekEndDate);

                //Assert
                Assert.IsNull(weekDetails, "Dates present in same week");
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

                timeSheet = null;
                employees = null;
                weekDetails = null;
                EmployeeBook.ClearInstance();

            }
        }
    }
}
