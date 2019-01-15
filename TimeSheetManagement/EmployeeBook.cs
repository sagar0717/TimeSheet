using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TimeSheetManagement.Business
{
    /// <summary>
    /// An Employee Book is a collection of Employees.
    /// This class represents the core domain logic for adding and searching the employees.
    /// </summary>
    public class EmployeeBook
    {

        /// <summary>
        /// The filename to store the employees details.
        /// </summary>
        private const string employeePath = "employees.xml";

        /// <summary>
        /// The in-memory "database" of employees.
        /// Save must be called if any changes are to be persisted.
        /// </summary>
        private List<Employee> employees;


        private static EmployeeBook _instance;
        private EmployeeBook()
        {
            employees = new List<Employee>();
            if (File.Exists(employeePath))
                Load(employeePath);
        }

        public static EmployeeBook Instance()
        {

            if (_instance == null)
            {
                _instance = new EmployeeBook();
            }

            return _instance;
        }

        public static void ClearInstance()
        {
            _instance = null;
        }





        /// <summary>
        /// Add an Employee to the Employee Book.
        /// Maximum Hourly Rate allowed for any employee is 50 as per problem statement
        /// An asumption is made that this is only the case for base rate, 
        /// after the penalty is applied this can increase above 50 
        /// </summary>
        /// <param name="employee">The new Employee</param>
        public bool Add(Employee employee)
        {
            EmployeeValidator validator = new EmployeeValidator();
            if (employees.Where(x => x.EmpId == employee.EmpId).Count() == 0)
            {
                if (!validator.Validate(employee))
                {
                    employees.Add(employee);
                    return true;
                }

            }
            return false;
        }


        /// <summary>
        /// The collection of all employees previously Added (or Loaded) into this Employee Book.
        /// </summary>
        public IEnumerable<Employee> AllEmployees
        {
            get
            {
                return employees;
            }
        }

        /// <summary>
        /// Retrieve all employee whose name contains the supplied search string.
        /// The comparison is case insensitive.
        /// </summary>
        /// <param name="search">The substring to search.</param>
        /// <returns></returns>
        public IEnumerable<Employee> SearchByName(string search)
        {
            string pattern = search.ToLower();
            return from c in employees
                   where c.EmpName.ToLower().Contains(pattern)
                   select c;
        }

        /// <summary>
        /// Retrieve an employee based on employeeId.
        /// </summary>
        /// <param name="id">The substring to search.</param>
        /// <returns></returns>
        public Employee GetEmployeeById(int id)
        {
            return employees.Where(x => x.EmpId == id).FirstOrDefault();
        }

        /// <summary>
        /// Save the Employee book, in XML format, to the supplied filename.
        /// If a file already exists, it is silently overwritten.
        /// </summary>
        /// <param name="path">The filename to save the XML data.</param>
        public void Save(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
                serializer.Serialize(stream, employees);
            }
        }

        /// <summary>
        /// Load an employee book from an XML file previously saved using the Save method.
        /// An exception will be thrown if the file does not exist.
        /// </summary>
        /// <param name="path">The filename to read previously saved Employee Book XML data from.</param>
        /// <exception cref="System.IO.FileNotFoundException">If the path does not exist.</exception>
        public void Load(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
                employees = serializer.Deserialize(stream) as List<Employee>;
            }
        }
    }
}
