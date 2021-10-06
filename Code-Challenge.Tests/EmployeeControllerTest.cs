using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Code_Challenge.Tests
{
    public class EmployeeControllerTest
    {
        private readonly List<Employee> _employeeList;
        private readonly List<Dependent> _dependentList;
        private readonly Mock<IEmployeeRepository> _employeeRepository;
        private readonly Mock<IDependentRepository> _dependentRepository;
        private readonly EmployeeController _employeeController;

        //Employee controller constructor
        public EmployeeControllerTest()
        {
            //Arrange
            _employeeList = new List<Employee>()
            {
                new Employee() { EmployeeId =1, FirstName = "Mary", LastName = "Hendry"},
                new Employee() { EmployeeId =2, FirstName = "Amy", LastName = "Bard"},
                new Employee() { EmployeeId =3, FirstName = "Sam", LastName = "Aaron"},
                new Employee() { EmployeeId =4, FirstName = "Andy", LastName = "Rose" }
            };

            _dependentList = new List<Dependent>()
            {
                new Dependent() {EmployeeId = 1, DependentId = 1, RelationToEmployee = RelationToEmployee.Spouse, FirstName = "Andy", LastName = "Jaffer"},
                new Dependent() {EmployeeId = 1, DependentId = 2, RelationToEmployee = RelationToEmployee.Children, FirstName = "Rose", LastName = "Bird"},
            };

            _employeeRepository = new Mock<IEmployeeRepository>();
            _dependentRepository = new Mock<IDependentRepository>();
            _employeeController = new EmployeeController(_employeeRepository.Object, _dependentRepository.Object);
        }

        [Fact]
        public void Get_AllEmployees_Request()
        {
            //Arrange
            _employeeRepository.Setup(er => er.GetAllEmployees()).Returns(_employeeList);

            //Act
            var result = _employeeController.EmployeeList() as ViewResult;
            var employeesList = (EmployeesListViewModel)result.ViewData.Model;
            var employees = employeesList.Employees.ToList();

            //Assert
            Assert.Equal("Mary", employees[0].FirstName);
            Assert.Equal(4, employees.Count());
        }

        [Fact]
        public void Search_Employee_From_ListofEmployees_When_Search_Is_Empty()
        {
            //Arrange
            _employeeRepository.Setup(er => er.Search(It.IsAny<string>())).Returns(_employeeList);

            //Act
            var result = _employeeController.Search("") as ViewResult;
            var employeesList = (EmployeesListViewModel)result.ViewData.Model;
            var employees = employeesList.Employees.ToList();

            //Assert
            Assert.Equal("Mary", employees[0].FirstName);
            Assert.Equal(4, employees.Count());
        }

        [Fact]
        public void Search_Employee_From_ListofEmployees_When_Search_Is_Not_Empty()
        {
            //Arrange
            List<Employee> _employeeList = new List<Employee>()
            {
                new Employee() { FirstName = "Sam", LastName = "Aaron"}
            };
            _employeeRepository.Setup(er => er.Search(It.IsAny<string>())).Returns(_employeeList);

            //Act
            var searchTerm = "Sam";
            var result = _employeeController.Search(searchTerm) as ViewResult;
            var employeesList = (EmployeesListViewModel)result.ViewData.Model;
            var employees = employeesList.Employees.ToList();

            //Assert
            Assert.Single(employees);
        }

        [Fact]
        public void Create_Employee_Index_View()
        {
            //Act
            var result = _employeeController.Create() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_Employee_Request()
        {
            //Arrange
            Employee _request = new Employee
            {
                FirstName = "Andy",
                LastName = "Cooper"
            };
            _employeeRepository.Setup(er => er.Add(It.IsAny<Employee>())).Returns(_request);

            //Act
            var result = _employeeController.Create(_request) as RedirectToActionResult;

            //Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.Equal("Details", result.ActionName);
        }

        [Fact]
        public void Create_Employee_With_Invalid_FirstName()
        {
            //Arrange 
            Employee _request = new Employee
            {
                FirstName = "Andy@",
                LastName = "Cooper"
            };
            var expected = $"Please enter valid Employee FirstName = {_request.FirstName} Accepts only AlphaNumeric";
            _employeeRepository.Setup(er => er.Add(It.IsAny<Employee>())).Returns(_request);

            //Act
            var result = _employeeController.Create(_request) as ViewResult;
            var actual = (string)result.ViewData["ErrorMessage"];

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_Employee_With_Invalid_LastName()
        {
            //Arrange 
            Employee _request = new Employee
            {
                FirstName = "Andy",
                LastName = "Cooper!"
            };
            var expected = $"Please enter valid Employee LastName = {_request.LastName} Accepts only AlphaNumeric";
            _employeeRepository.Setup(er => er.Add(It.IsAny<Employee>())).Returns(_request);

            //Act
            var result = _employeeController.Create(_request) as ViewResult;
            var actual = (string)result.ViewData["ErrorMessage"];

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_Employee_That_Already_Exists_In_Employees()
        {
            //Arrange 
            Employee _request = new Employee
            {
                FirstName = "Andy",
                LastName = "Rose"
            };
            var expected = $"Employee with FirstName = {_request.FirstName} and LastName = {_request.LastName} already exists, please create another employee";
            _employeeRepository.Setup(er => er.GetAllEmployees()).Returns(_employeeList);

            //Act
            var result = _employeeController.Create(_request) as ViewResult;
            var actual = (string)result.ViewData["ErrorMessage"];

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get_Employee_By_Id_Invalid_Employee()
        {
            //Arrange 
            Employee _request = null;
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_request);
            var expected = $"Employee with Id = 99 not found.";

            //Act
            var result = _employeeController.Details(99) as ViewResult;
            var actual = (string)result.ViewData["ErrorMessage"];

            //Assert
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void Get_All_Dependents_By_Employee_By_Id()
        {
            //Arrange
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_employeeList[0]);
            _dependentRepository.Setup(er => er.GetDependentsByEmployeeId(1)).Returns(_dependentList);

            //Act
            var result = _employeeController.Details(1) as ViewResult;
            var dependentsList = (EmployeeDetailsViewModel)result.ViewData.Model;

            //Assert
            Assert.Equal("Andy", dependentsList.Dependents[0].FirstName);
            Assert.Equal("Jaffer", dependentsList.Dependents[0].LastName);
            Assert.Equal(RelationToEmployee.Spouse, dependentsList.Dependents[0].RelationToEmployee);
            Assert.Equal(2, dependentsList.Dependents.Count());
        }

        [Fact]
        public void Get_Employee_Details_By_Id()
        {
            //Arrange
            var expected = "Employee Details";
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_employeeList[0]);
            _dependentRepository.Setup(er => er.GetDependentsByEmployeeId(1)).Returns(_dependentList);

            //Act
            var result = _employeeController.Details(1) as ViewResult;
            var employeeDetails = (EmployeeDetailsViewModel)result.ViewData.Model;

            //Assert
            Assert.Equal("Mary Hendry", employeeDetails.FullName);
            Assert.Equal("Mary", employeeDetails.Employee.FirstName);
            Assert.Equal("Hendry", employeeDetails.Employee.LastName);
            Assert.Equal("Andy", employeeDetails.Dependents[0].FirstName);
            Assert.Equal("Jaffer", employeeDetails.Dependents[0].LastName);
            Assert.Equal(RelationToEmployee.Spouse, employeeDetails.Dependents[0].RelationToEmployee);
            Assert.Equal(2, employeeDetails.Dependents.Count());
            Assert.Equal(expected, employeeDetails.PageTitle);
        }

        [Fact]
        public void Delete_Employee_By_Id_Invalid_Request()
        {
            //Arrange
            Employee _request = null;
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_request);

            //Act
            var result = _employeeController.Delete(99) as RedirectToActionResult;

            //Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.Equal("EmployeeList", result.ActionName);
        }

        [Fact]
        public void Delete_Employee_By_Id_Valid_Request()
        {
            //Arrange
            Employee _request = new Employee
            {
                EmployeeId = 1,
                FirstName = "Andy",
                LastName = "Cooper"
            };
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_request);
            _dependentRepository.Setup(er => er.GetDependentsByEmployeeId(_request.EmployeeId)).Returns(_dependentList);
            _employeeRepository.Setup(er => er.Delete(It.IsAny<int>())).Returns(_request);

            //Act
            var result = _employeeController.Delete(1) as RedirectToActionResult;

            //Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.Equal("EmployeeList", result.ActionName);
        }

        [Fact]
        public void Employee_PayPreview_Invalid_Employee()
        {
            //Arrange 
            Employee _request = null;
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_request);
            var expected = $"Employee with Id = 99 not found.";

            //Act
            var result = _employeeController.PayPreview(99) as ViewResult;
            var actual = (string)result.ViewData["ErrorMessage"];

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Employee_PayPreview_Valid_Employee()
        {
            //Arrange 
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_employeeList[0]);
            _dependentRepository.Setup(er => er.GetDependentsByEmployeeId(1)).Returns(_dependentList);

            //Act
            var result = _employeeController.PayPreview(1) as ViewResult;
            var employeeDeductions = (CalculateDeductions)result.ViewData.Model;

            //Assert
            Assert.Equal("Mary", employeeDeductions.Employee.FirstName);
            Assert.Equal("Hendry", employeeDeductions.Employee.LastName);
            Assert.Equal(2, employeeDeductions.DependentCount);
            Assert.Equal("Andy", employeeDeductions.Dependents[0].FirstName);
            Assert.Equal("Jaffer", employeeDeductions.Dependents[0].LastName);
            Assert.Equal(RelationToEmployee.Spouse, employeeDeductions.Dependents[0].RelationToEmployee);
            Assert.Equal(1000, employeeDeductions.YearlyEmployeeDeduction);
            Assert.Equal(950, employeeDeductions.YearlyDependentsDeduction);
        }

        [Fact]
        public void PayPreview_Check_Employee_Has_Discount()
        {
            //Arrange 
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_employeeList[1]);
            _dependentRepository.Setup(er => er.GetDependentsByEmployeeId(1)).Returns(_dependentList);

            //Act
            var result = _employeeController.PayPreview(1) as ViewResult;
            var employeeDeductions = (CalculateDeductions)result.ViewData.Model;

            //Assert
            Assert.Equal("Amy", employeeDeductions.Employee.FirstName);
            Assert.Equal("Bard", employeeDeductions.Employee.LastName);
            Assert.Equal(900, employeeDeductions.YearlyEmployeeDeduction);
            Assert.Equal(52000, employeeDeductions.YearlyGrossWages);
        }

        [Fact]
        public void PayPreview_Check_Employee_Has_NoDiscount()
        {
            //Arrange 
            _employeeRepository.Setup(er => er.GetEmployeeById(It.IsAny<int>())).Returns(_employeeList[0]);
            _dependentRepository.Setup(er => er.GetDependentsByEmployeeId(1)).Returns(_dependentList);

            //Act
            var result = _employeeController.PayPreview(1) as ViewResult;
            var employeeDeductions = (CalculateDeductions)result.ViewData.Model;

            //Assert
            Assert.Equal("Mary", employeeDeductions.Employee.FirstName);
            Assert.Equal("Hendry", employeeDeductions.Employee.LastName);
            Assert.Equal(1000, employeeDeductions.YearlyEmployeeDeduction);
            Assert.Equal(52000, employeeDeductions.YearlyGrossWages);
        }
    }
}
