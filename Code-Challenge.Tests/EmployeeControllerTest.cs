using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Code_Challenge.Tests
{
    public class EmployeeControllerTest
    {
        private readonly Mock<IEmployeeRepository> _employeeRepository;
        private readonly Mock<IDependentRepository> _dependentRepository;
        private readonly EmployeeController _employeeController;
        private readonly DependentController _dependentController;

        private const int Employee_Deductions = 1000;
        private const int Dependent_Deductions = 500;
        private const int Employee_Gross_Wages = 2000;
        private const int Pay_Periods_Per_Year = 26;
        private const int Deductions_Discount = 10;

        //Employee controller constructor
        public EmployeeControllerTest()
        {
            _employeeRepository = new Mock<IEmployeeRepository>();
            _dependentRepository = new Mock<IDependentRepository>();
            _employeeController = new EmployeeController(_employeeRepository.Object, _dependentRepository.Object);
            _dependentController = new DependentController(_dependentRepository.Object);
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
        public void Create_InvalidData_Return_BadRequest()
        {
            //Arrange 
            Employee _request = new Employee
            {
                FirstName = "Andy@",
                LastName = "Cooper!"
            };

            //Act        
            //var result = _employeeRepository.Add(_request);

            //Assert
            //Assert.Null(result);
        }

        [Fact]
        public void GetEmployeebyId()
        {

        }
    }
}
