using EmployeeManagement.Common;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Controllers
{
    //Employee Controller
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDependentRepository _dependentRepository;


        //Employee controller constructor
        public EmployeeController(IEmployeeRepository employeeRepository, IDependentRepository dependentRepository)
        {
            _employeeRepository = employeeRepository;
            _dependentRepository = dependentRepository;
        }

        //Get All employees List
        [HttpGet]
        public IActionResult EmployeeList()
        {
            IEnumerable<Employee> employees = _employeeRepository.GetAllEmployees();
            EmployeesListViewModel employeesListViewModel = new EmployeesListViewModel()
            {
                Employees = employees
            };
            return View(employeesListViewModel);
        }

        [HttpGet]
        public IActionResult Search(string SearchTerm)
        {
            IEnumerable<Employee> employees = _employeeRepository.Search(SearchTerm);
            EmployeesListViewModel employeesListViewModel = new EmployeesListViewModel()
            {
                Employees = employees
            };
            return View("EmployeeList", employeesListViewModel);
        }

        //return create an employee view
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Add an employee by employee object
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                //check valid firstname
                if (!string.IsNullOrWhiteSpace(employee?.FirstName) && !General.RegexPatterns.IsStringOnlyAlphaNumeric(employee?.FirstName?.Trim()))
                {
                    ViewBag.ErrorMessage = $"Please enter valid Employee FirstName = {employee.FirstName} Accepts only AlphaNumeric";
                    return View();
                }

                //check valid lastname
                if (!string.IsNullOrWhiteSpace(employee?.LastName) && !General.RegexPatterns.IsStringOnlyAlphaNumeric(employee?.LastName?.Trim()))
                {
                    ViewBag.ErrorMessage = $"Please enter valid Employee LastName = {employee.LastName} Accepts only AlphaNumeric";
                    return View();
                }

                //check employee already exists
                if (IsEmployeeExists(employee))
                {
                    ViewBag.ErrorMessage = $"Employee with FirstName = {employee.FirstName} and LastName = {employee.LastName} already exists, please create another employee";
                    return View();
                }
                //add employee
                Employee newEmployee = _employeeRepository.Add(employee);
                return RedirectToAction("Details", new { id = newEmployee.EmployeeId });
            }
            return View();
        }

        //Get Employee Details by employee id 
        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                Employee employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    ViewBag.ErrorMessage = $"Employee with Id = {id} not found.";
                    return View("EmployeeNotFound", id);
                }

                List<Dependent> dependents = _dependentRepository.GetDependentsByEmployeeId(id);
                EmployeeDetailsViewModel employeDetailsViewModel = new EmployeeDetailsViewModel()
                {
                    Employee = employee,
                    Dependents = dependents?.OrderBy(x => x.RelationToEmployee)?.ToList(),
                    PageTitle = "Employee Details",
                    FullName = employee.FirstName + " " + employee.LastName
                };
                return View(employeDetailsViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }

        }

        //Delete employee by Id
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public IActionResult Delete(int id)
        {
            try
            {
                Employee currentEmployee = _employeeRepository.GetEmployeeById(id);
                if (currentEmployee != null)
                {
                    //get all the dependents for the employee id
                    List<Dependent> dependents = _dependentRepository.GetDependentsByEmployeeId(id);
                    if (dependents.Any())
                    {
                        //loop through and delete all the dependents before deleting the employee
                        foreach (var dependent in dependents)
                        {
                            //delete dependent
                            _dependentRepository?.DeleteDependent(dependent.DependentId);
                        }
                    }
                    //delete employee
                    _employeeRepository?.Delete(currentEmployee.EmployeeId);
                }
                return RedirectToAction("EmployeeList");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        //Pay preview method based on employee id
        [HttpGet]
        public IActionResult PayPreview(int id)
        {
            try
            {
                //Get employee by Id
                Employee employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    ViewBag.ErrorMessage = $"Employee with Id = {id} not found.";
                    return View("EmployeeNotFound", id);
                }

                //Get list of dependents by employee id
                List<Dependent> dependents = _dependentRepository.GetDependentsByEmployeeId(id);

                //Calculate employee and dependent deductions
                CalculateDeductions employeeDeductions = new CalculateDeductions
                {
                    Employee = employee,
                    Dependents = dependents?.OrderBy(x => x.RelationToEmployee)?.ToList(),
                    DependentCount = dependents.Any() ? dependents.Count() : 0,
                    YearlyGrossWages = General.EmployeeStaticData.Employee_Gross_Wages
                    * General.EmployeeStaticData.Pay_Periods_Per_Year,
                    TotalDeductions = Math.Round((EmployeeDeductions(employee) + DependentDeductions(dependents)) / General.EmployeeStaticData.Pay_Periods_Per_Year, 2),
                    EmployeeDeduction = Math.Round(EmployeeDeductions(employee) / General.EmployeeStaticData.Pay_Periods_Per_Year, 2),
                    DependentsDeduction = Math.Round(DependentDeductions(dependents) / General.EmployeeStaticData.Pay_Periods_Per_Year, 2),
                    YearlyTotalDeductions = EmployeeDeductions(employee) + DependentDeductions(dependents),
                    YearlyEmployeeDeduction = EmployeeDeductions(employee),
                    YearlyDependentsDeduction = DependentDeductions(dependents)
                };
                employeeDeductions.YearlyNetWages = employeeDeductions.YearlyGrossWages - employeeDeductions.YearlyTotalDeductions;
                employeeDeductions.GrossWages = General.EmployeeStaticData.Employee_Gross_Wages;
                employeeDeductions.NetWages = employeeDeductions.GrossWages - employeeDeductions.TotalDeductions;
                employeeDeductions.YearlyNetWages = Math.Round(employeeDeductions.YearlyNetWages, 2);
                employeeDeductions.NetWages = Math.Round(employeeDeductions.NetWages, 2);

                return View(employeeDeductions);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        //Method to check Name start's with A to apply discount
        private bool IsDiscountAvailable(string firstName)
        {
            //check first name is not null/empty and name starts with 'A'
            if (!string.IsNullOrWhiteSpace(firstName) && firstName.Trim().ToUpper().StartsWith('A'))
                return true;
            return false;
        }

        //Method to retrive employee deductions
        private double EmployeeDeductions(Employee employee)
        {
            //check employee is not null
            if (employee != null && IsDiscountAvailable(employee.FirstName))
            {
                double discount = General.EmployeeStaticData.Employee_Deductions / General.EmployeeStaticData.Deductions_Discount;
                Math.Round(discount, 2);
                return General.EmployeeStaticData.Employee_Deductions - discount;
            }
            return General.EmployeeStaticData.Employee_Deductions;
        }

        //Method to retrive all dependent deductions
        private double DependentDeductions(List<Dependent> dependents)
        {
            //check any dependent's available or not
            if (dependents.Any())
            {
                //get total dependent deductions before discount
                double totalDependentDeductions = dependents.Count() * General.EmployeeStaticData.Dependent_Deductions;
                foreach (var dependent in dependents)
                {
                    //check name start with 'A' or not
                    if (IsDiscountAvailable(dependent.FirstName))
                    {
                        double discount = General.EmployeeStaticData.Dependent_Deductions / General.EmployeeStaticData.Deductions_Discount;
                        Math.Round(discount, 2);
                        totalDependentDeductions -= discount;
                    }
                }
                return Math.Round(totalDependentDeductions, 2);
            }
            return 0;
        }

        //check employee already exists in repository
        private bool IsEmployeeExists(Employee employee)
        {
            try
            {
                //get all employees
                IEnumerable<Employee> employees = _employeeRepository.GetAllEmployees();
                if (employees.Any())
                {
                    //check employee exists or not
                    return employees.Any(x => x.FirstName.Trim().ToUpper() == employee.FirstName.Trim().ToUpper() &&
                        x.LastName.Trim().ToUpper() == employee.LastName.Trim().ToUpper());
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
