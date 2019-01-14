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
                EmpId = 1,
                EmpName = "user1",
                EmpHourRate = 40
            };
            employees.Add(emp);
            employees.Save(employeepath);
            
        }

        [Test]
        public void Should_Return_True_When_NewEmployee_Added_ValidData()
        {
            //Arrange
            Employee emp = new Employee()
            {
                EmpId = 2,
                EmpName = "user2",
                EmpHourRate = 40
            };


            //Act
            bool expectedValue = employees.Add(emp);

            //Assert
            Assert.IsTrue(expectedValue, "Employee Already Exists");
        }

    }
}
