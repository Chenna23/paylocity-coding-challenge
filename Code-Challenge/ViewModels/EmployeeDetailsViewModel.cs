using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public Employee Employee { get; set; }

        public List<Dependent> Dependents { get; set; }

        public Dependent Dependent { get; set; }

        public string PageTitle { get; set; }

        public string FullName { get; set; }
    }
}
