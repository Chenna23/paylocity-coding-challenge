using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class CalculateDeductions
    {
        public Employee Employee { get; set; }

        public List<Dependent> Dependents { get; set; }

		public int DependentCount { get; set; }

		public double GrossWages { get; set; }

		public double TotalDeductions { get; set; }

		public double EmployeeDeduction { get; set; }

		public double DependentsDeduction { get; set; }

		public double YearlyTotalDeductions { get; set; }

		public double YearlyEmployeeDeduction { get; set; }

		public double YearlyDependentsDeduction { get; set; }

		public double NetWages { get; set; }

		public double YearlyGrossWages { get; set; }

		public double YearlyNetWages { get; set; }

		public CalculateDeductions()
		{
			Dependents = new List<Dependent>();
		}

	}
}
