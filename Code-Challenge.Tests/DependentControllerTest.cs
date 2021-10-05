using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Code_Challenge.Tests
{
    public class DependentControllerTest
    {
        private readonly Mock<IDependentRepository> _dependentRepository;
        private readonly DependentController _dependentController;

        //Dependent controller constructor
        public DependentControllerTest()
        {
            _dependentRepository = new Mock<IDependentRepository>();
            _dependentController = new DependentController(_dependentRepository.Object);
        }

        [Fact]
        //create dependent request
        public void Create_Dependent_Request()
        {
            //Arrange
            Dependent _request = new Dependent
            {
                FirstName = "Andy",
                LastName = "Cooper"
            };
            _dependentRepository.Setup(er => er.AddDependent(It.IsAny<Dependent>())).Returns(_request);

            //Act
            var result = _dependentController.AddDependent(_request) as RedirectToActionResult;

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
        public void GetDependentbyId()
        {

        }
    }
}
