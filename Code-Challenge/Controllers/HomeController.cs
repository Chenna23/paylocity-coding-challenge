using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    //Home Controller
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}
