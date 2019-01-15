using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TimeSheetManagement.Business;

namespace TimeSheetManagement.Test
{
    [TestFixture]
    class ManageEmployeeTest
    {
        EmployeeBook employees;
        string employeepath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\TimeSheetManagement\bin\Debug\netcoreapp2.1\employees.xml"));

        [OneTimeSetUp]
        public void TestInit()
        {
            employees = EmployeeBook.Instance();
            Employee emp = new Employee()
            {
                EmpId = 100,
                EmpName = "userAlreadyPresent",
                EmpHourRate = 25
            };
            employees.Add(emp);
            employees.Save(employeepath);

        }

        [Test]
        public void Should_Return_True_When_New_Employee_Added_ValidData()
        {
            //Arrange
            Employee emp = new Employee
            {
                EmpId = 1,
                EmpName = "Newuser",
                EmpHourRate = 40
            };

            //Act
            bool expectedValue = employees.Add(emp);

            //Assert
            Assert.IsTrue(expectedValue, "Employee with EmpId already exists");
        }

        [Test]
        public void Should_Return_False_When_Employee_Added_Already_exist()
        {
            //Arrange
            Employee emp = new Employee()
            {
                EmpId = 100,
                EmpName = "userAlreadyPresent",
                EmpHourRate = 25
            };

            //Act
            bool expectedValue = employees.Add(emp);

            //Assert
            Assert.IsFalse(expectedValue, "Employee does not exist");
        }

        [Test]
        public void Should_Return_False_When_EmployeeAdded_With_EmpId_Blank()
        {
            //Arrange
            Employee emp = new Employee()
            {
                EmpName = "NewUser",
                EmpHourRate = 25
            };

            //Act
            bool expectedValue = employees.Add(emp);

            //Assert
            Assert.IsFalse(expectedValue, "Employee added with Valid Data");
        }

        [Test]
        public void Should_Return_False_When_EmployeeAdded_With_EmpName_Blank()
        {
            //Arrange
            Employee emp = new Employee()
            {
                EmpId = 1,
                EmpHourRate = 25
            };

            //Act
            bool expectedValue = employees.Add(emp);

            //Assert
            Assert.IsFalse(expectedValue, "Employee added with Valid Data");
        }

        [Test]
        public void Should_Return_False_When_EmployeeAdded_HourlyRate_Greater_Than_50()
        {
            //Arrange
            Employee emp = new Employee()
            {
                EmpId = 1,
                EmpName = "NewUser",
                EmpHourRate = 100
            };

            //Act
            bool expectedValue = employees.Add(emp);

            //Assert
            Assert.IsFalse(expectedValue, "Hourly Rate within allowed value");
        }

        [Test]
        public void Should_Return_False_When_EmployeeAdded_HourlyRate_Equals_0()
        {
            //Arrange
            Employee emp = new Employee()
            {
                EmpId = 1,
                EmpName = "NewUser",
                EmpHourRate = 0
            };

            //Act
            bool expectedValue = employees.Add(emp);

            //Assert
            Assert.IsFalse(expectedValue, "Hourly Rate within allowed value");
        }

        [OneTimeTearDown]
        public void TestCleanup()
        {
            if (File.Exists(employeepath))
            {
                File.Delete(employeepath);
            }
        }

    }
}
