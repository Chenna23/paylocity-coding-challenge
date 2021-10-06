using EmployeeManagement.Common;
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
                Dependent dependent = _dependentRepository.GetDependentById(id);
                if (dependent == null)
                {
                    ViewBag.ErrorMessage = $"Dependent with Id = {id} not found.";
                    return View("DependentNotFound", id);
                }
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
                //check valid firstname
                if (!string.IsNullOrWhiteSpace(updatedependent?.FirstName) && !General.RegexPatterns.IsStringOnlyAlphaNumeric(updatedependent?.FirstName?.Trim()))
                {
                    ViewBag.ErrorMessage = $"Please enter valid Dependent FirstName = {updatedependent.FirstName} Accepts only AlphaNumeric";
                    return View(updatedependent);
                }

                //check valid lastname
                if (!string.IsNullOrWhiteSpace(updatedependent?.LastName) && !General.RegexPatterns.IsStringOnlyAlphaNumeric(updatedependent?.LastName?.Trim()))
                {
                    ViewBag.ErrorMessage = $"Please enter valid Dependent LastName = {updatedependent.LastName} Accepts only AlphaNumeric";
                    return View(updatedependent);
                }

                try
                {
                    if (updatedependent?.DependentId != null)
                    {
                        _dependentRepository.UpdateDependent(updatedependent);
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
