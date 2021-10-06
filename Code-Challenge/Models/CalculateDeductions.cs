using System.Collections.Generic;

namespace EmployeeManagement.Models
{
    //Calculate employee deductions class
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
