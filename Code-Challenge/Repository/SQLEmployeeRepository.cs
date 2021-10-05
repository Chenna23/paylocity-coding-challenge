using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;
        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context?.Employees;
        }

        public Employee GetEmployeeById(int Id)
        {
            return context?.Employees?.Find(Id);
        }

        public Employee Add(Employee employee)
        {
            context?.Employees?.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = context?.Employees?.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = context?.Employees?.FirstOrDefault(e => e.EmployeeId == employeeChanges.EmployeeId);

            if (employee != null)
            {
                employee.FirstName = employeeChanges.FirstName;
                employee.LastName = employeeChanges.LastName;
                context.SaveChanges();
                return employee;
            }
            return null;
        }

        public IEnumerable<Employee> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return context?.Employees;
            }
            return context?.Employees?.Where(e => e.FirstName.ToUpper().Contains(searchTerm.Trim().ToUpper()) || e.LastName.ToUpper().Contains(searchTerm.Trim().ToUpper()))?.ToList();
        }
    }
}
