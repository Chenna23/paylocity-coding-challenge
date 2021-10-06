using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.ViewModels
{
    //Employees View Model
    public class EmployeesListViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }

        public string SearchTerm { get; set; }
    }
}
