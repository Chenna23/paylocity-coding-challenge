using System.Collections.Generic;

namespace EmployeeManagement.Models
{
    //Dependent Repository
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeById(int Id);
        Employee Add(Employee employee);
        Employee Update(Employee employeeChanges);
        Employee Delete(int id);
        IEnumerable<Employee> Search(string searchTerm);
    }
}
