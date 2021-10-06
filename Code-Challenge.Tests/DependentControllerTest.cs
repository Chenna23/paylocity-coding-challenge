using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Code_Challenge.Tests
{
    public class DependentControllerTest
    {
        private readonly Mock<IDependentRepository> _dependentRepository;
        private readonly DependentController _dependentController;
        private readonly List<Dependent> _dependentList;

        //Dependent controller constructor
        public DependentControllerTest()
        {
            //Arrange
            _dependentList = new List<Dependent>()
            {
                new Dependent() {EmployeeId = 1, DependentId = 1, RelationToEmployee = RelationToEmployee.Spouse, FirstName = "Andy", LastName = "Jaffer"},
                new Dependent() {EmployeeId = 1, DependentId = 2, RelationToEmployee = RelationToEmployee.Children, FirstName = "Rose", LastName = "Bird"},
            };

            _dependentRepository = new Mock<IDependentRepository>();
            _dependentController = new DependentController(_dependentRepository.Object);
        }

        [Fact]
        //Get all dependents
        public void Get_All_Dependents()
        {
            //Arrange
            _dependentRepository.Setup(er => er.GetAllDependents()).Returns(_dependentList);

            //Act
            var result = _dependentController.Index() as ViewResult;
            var dependentsList = (IEnumerable<Dependent>)result.ViewData.Model;
            var dependents = dependentsList.ToList();

            //Assert
            Assert.Equal("Andy", dependents[0].FirstName);
            Assert.Equal("Jaffer", dependents[0].LastName);
            Assert.Equal(2, dependents.Count());
        }

        [Fact]
        public void Create_Dependent_Index_View()
        {
            //Act
            var result = _dependentController.Create() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        //create dependent request
        public void Create_Dependent_Request()
        {
            //Arrange
            Dependent _request = new Dependent
            {
                DependentId = 1,
                FirstName = "Andy",
                LastName = "Jaffer"
            };
            _dependentRepository.Setup(er => er.AddDependent(It.IsAny<Dependent>())).Returns(_dependentList[0]);

            //Act
            var result = _dependentController.AddDependent(_request) as RedirectToActionResult;

            //Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.Equal("Details", result.ActionName);
            Assert.Single(result.RouteValues.Values.ToList());
        }

        [Fact]
        public void Edit_Dependent_Should_Return_Current_Dependent()
        {
            //Arrange 
            Dependent _request = new Dependent
            {
                DependentId = 1,
                FirstName = "Andy",
                LastName = "Jaffer"
            };
            _dependentRepository.Setup(er => er.GetDependentById(It.IsAny<int>())).Returns(_dependentList[0]);

            //Act        
            var result = _dependentController.EditDependent(1) as ViewResult;
            var dependent = (Dependent)result.ViewData.Model;

            //Assert
            Assert.Equal("Andy", dependent.FirstName);
            Assert.Equal("Jaffer", dependent.LastName);
            Assert.Equal(1, dependent.DependentId);
        }

        [Fact]
        //edit dependent post request
        public void Edit_Dependent_Should_Update_Current_Dependent()
        {
            //Arrange
            Dependent _request = new Dependent
            {
                DependentId = 1,
                FirstName = "Andy",
                LastName = "Jaffer"
            };
            _dependentRepository.Setup(er => er.UpdateDependent(It.IsAny<Dependent>())).Returns(_dependentList[0]);

            //Act
            var result = _dependentController.EditDependent(_request) as RedirectToActionResult;

            //Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.Equal("Details", result.ActionName);
            Assert.Single(result.RouteValues.Values.ToList());
        }

        [Fact]
        public void Delete_Dependent_Invalid_Request()
        {
            //Arrange
            Dependent _request = null;
            _dependentRepository.Setup(er => er.GetDependentById(It.IsAny<int>())).Returns(_request);

            //Act
            var result = _dependentController.DeleteDependent(99) as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_Dependent_Valid_Request()
        {
            //Arrange
            Dependent _request = new Dependent
            {
                DependentId = 1,
                FirstName = "Andy",
                LastName = "Jaffer"
            };
            _dependentRepository.Setup(er => er.GetDependentById(It.IsAny<int>())).Returns(_request);
            _dependentRepository.Setup(er => er.DeleteDependent(It.IsAny<int>())).Returns(_request);

            //Act
            var result = _dependentController.DeleteDependent(1) as RedirectToActionResult;

            //Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.Equal("Details", result.ActionName);
        }
    }
}
