using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Common
{
    public class General
    {
        //Employee releated static data
        public sealed class EmployeeStaticData
        {
            public const int Employee_Deductions = 1000;
            public const int Dependent_Deductions = 500;
            public const int Employee_Gross_Wages = 2000;
            public const int Pay_Periods_Per_Year = 26;
            public const int Deductions_Discount = 10;
        }

        public sealed class RegexPatterns
        {
            //validation for first and last name
            public static bool IsStringOnlyAlphaNumeric(string inputString)
            {
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^[ a-zA-Z0-9]*$");

                return rg.IsMatch(inputString);
            }
        }
    }
}
