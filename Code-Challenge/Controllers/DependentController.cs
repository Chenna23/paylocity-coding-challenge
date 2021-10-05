using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class DependentController : Controller
    {
        private readonly IDependentRepository _dependentRepository;

        public DependentController(IDependentRepository dependentRepository)
        {
            _dependentRepository = dependentRepository;
        }

        [HttpGet]
        //Index action method to retrive all dependents
        public IActionResult Index()
        {
            try
            {
                IEnumerable<Dependent> dependents = _dependentRepository.GetAllDependents();
                return View(dependents);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }

        }

        [HttpGet]
        //create dependent get action
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        //create dependent post action
        public IActionResult AddDependent(Dependent dependent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Dependent newDependent = _dependentRepository.AddDependent(dependent);
                    return RedirectToAction("Details", "employee", new { id = newDependent.EmployeeId });
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Error");
                }
            }
            return View();
        }

        [HttpGet]
        //edit existing dependent action
        public IActionResult EditDependent(int id)
        {
            try
            {
                Dependent dependent = _dependentRepository?.GetDependentById(id);
                return View(dependent);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        //Method to edit/update dependent
        public IActionResult EditDependent(Dependent updatedependent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (updatedependent?.DependentId != null)
                    {
                        _dependentRepository?.UpdateDependent(updatedependent);
                        return RedirectToAction("Details", "employee", new { id = updatedependent.EmployeeId });
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Error");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        //Method to delete dependent
        public IActionResult DeleteDependent(int id)
        {
            try
            {
                //get dependent by dependent id
                Dependent dependent = _dependentRepository.GetDependentById(id);
                //check dependent exists or not
                if (dependent != null)
                {
                    //delete dependent from database
                    _dependentRepository.DeleteDependent(id);
                    return RedirectToAction("Details", "employee", new { id = dependent.EmployeeId });
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

    }
}
