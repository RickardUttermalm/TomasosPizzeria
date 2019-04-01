using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeriaTest.Models;
using TomasosPizzeriaTest.IdentityData;
using Microsoft.AspNetCore.Authorization;
using TomasosPizzeriaTest.ViewModels;

namespace TomasosPizzeriaTest.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly TomasosContext _context;
        public HomeController(TomasosContext context)
        {
            _context = context;
        }

        public IActionResult StartPage()
        {          
            return View();
        }

        
        public IActionResult Meny()
        {
            var model = new MenuViewModel(_context);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
