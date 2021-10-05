using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
