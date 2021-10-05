using EmployeeManagement.Common;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() {EmployeeId = 1, FirstName = "Mary", LastName = "Hendry"},
                new Employee() {EmployeeId = 2, FirstName = "Amy", LastName = "Bard"},
                new Employee() {EmployeeId = 3, FirstName = "Sam", LastName = "Aaron"},
                new Employee() {EmployeeId = 4, FirstName = "Andy", LastName = "Rose" }
            };
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployeeById(int Id)
        {
            return _employeeList?.FirstOrDefault(e => e.EmployeeId == Id);
        }

        public Employee Add(Employee employee)
        {
            if (!_employeeList.Any())
            {
                employee.EmployeeId = 1;
            }
            else
            {
                employee.EmployeeId = _employeeList.Max(employee => employee.EmployeeId) + 1;
            }

            var result = !General.RegexPatterns.IsStringOnlyAlphaNumeric(employee?.FirstName?.Trim()) ||
                !General.RegexPatterns.IsStringOnlyAlphaNumeric(employee?.LastName?.Trim());
            if (!result)
            {
                _employeeList.Add(employee);
                return employee;
            }
            return null;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.EmployeeId == id);
            if (employee != null)
            {
                _employeeList?.Remove(employee);
            }
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList?.FirstOrDefault(e => e.EmployeeId == employeeChanges.EmployeeId);
            if (employee != null)
            {
                employee.FirstName = employeeChanges.FirstName;
                employee.LastName = employeeChanges.LastName;
            }
            return employee;
        }

        public IEnumerable<Employee> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _employeeList;
            }
            return _employeeList?.Where(e => e.FirstName.ToUpper().Contains(searchTerm.ToUpper()) || e.LastName.ToUpper().Contains(searchTerm.ToUpper()))?.ToList();
        }
    }
}
