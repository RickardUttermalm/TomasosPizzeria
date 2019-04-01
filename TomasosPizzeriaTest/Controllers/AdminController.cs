using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeriaTest.IdentityData;
using TomasosPizzeriaTest.Models;
using TomasosPizzeriaTest.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;

namespace TomasosPizzeriaTest.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TomasosContext _tomasos;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TomasosContext tomasos)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tomasos = tomasos;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddMenuitem()
        {
            var model = new AddItemViewModel(_tomasos);

            return View(model);
        }


        [HttpPost]
        public IActionResult AddMenuitem(AddItemViewModel newitem)
        {
            if (ModelState.IsValid)
            {
                var newMatratt = new Matratt()
                {
                    MatrattNamn = newitem.MatrattNamn,
                    MatrattTyp = newitem.MatrattTyp,
                    Pris = newitem.Pris,
                    Beskrivning = newitem.Beskrivning
                };
                _tomasos.Add(newMatratt);
                foreach (var item in newitem.ValdaIdn)
                {
                    _tomasos.Add(
                        new MatrattProdukt()
                        {
                            MatrattId = newMatratt.MatrattId,
                            ProduktId = item
                        });
                }
                _tomasos.SaveChanges();
            }
            else
            {
                return View();
            }


            return RedirectToAction("StartPage", "Home");
        }

        [HttpPost]
        public IActionResult CreateNewProduct(Produkt newProdukt)
        {
            if (ModelState.IsValid)
            {
                _tomasos.Produkt.Add(newProdukt);
                _tomasos.SaveChanges();
            }
            else
            {
                return View();
            }


            return RedirectToAction("AddMenuitem", new AddItemViewModel(_tomasos));
        }

        public IActionResult Update(int id)
        {
            var model = new UpdateViewModel(_tomasos, id);

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {

                var old = _tomasos.Matratt.SingleOrDefault(m => m.MatrattId == model.CurrentRatt.MatrattId);
                old.MatrattNamn = model.CurrentRatt.MatrattNamn;
                old.Pris = model.CurrentRatt.Pris;
                old.Beskrivning = model.CurrentRatt.Beskrivning;
                _tomasos.SaveChanges();
            }
            else
            {
                return View(model.CurrentRatt.MatrattId);
            }


            return RedirectToAction("Update", model.CurrentRatt.MatrattId);
        }

        [HttpPost]
        public IActionResult RemoveIngredient(UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var matrattprod = _tomasos.MatrattProdukt.SingleOrDefault(p => p.MatrattId == model.CurrentRatt.MatrattId && p.ProduktId == model.ToDeleteProdId);

                _tomasos.MatrattProdukt.Remove(matrattprod);

                _tomasos.SaveChanges();
            }
            else
            {
                return View(model.CurrentRatt.MatrattId);
            }

            return RedirectToAction("Update", new RouteValueDictionary(
            new { controller = "Admin", action = "Update", id = model.CurrentRatt.MatrattId }));
        }

        [HttpPost]
        public IActionResult AddIngredientToMenu(UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mattratt = _tomasos.Matratt.SingleOrDefault(m => m.MatrattId == model.CurrentRatt.MatrattId);
                var newprodukt = _tomasos.Produkt.SingleOrDefault(p => p.ProduktId == model.ToBeAddedProdId);

                _tomasos.MatrattProdukt.Add(new MatrattProdukt() {MatrattId = mattratt.MatrattId, ProduktId = newprodukt.ProduktId });

                _tomasos.SaveChanges();
            }
            else
            {
                return View(model.CurrentRatt.MatrattId);
            }

            return RedirectToAction("Update", new RouteValueDictionary(
            new { controller = "Admin", action = "Update", id = model.CurrentRatt.MatrattId }));
        }

        public IActionResult ManageRoles()
        {
            var model = new ManageRolesViewModel(_tomasos);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoles(ManageRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var knud = _tomasos.Kund.SingleOrDefault(k => k.KundId == model.SelectedUserID);
                var user = await _userManager.FindByIdAsync(knud.IdentityId);

                await _userManager.RemoveFromRoleAsync(user, "Regular");
                await _userManager.AddToRoleAsync(user, "Premium");
            }
            else
            {
                return RedirectToAction("ManageRoles");
            }

            return RedirectToAction("ManageRoles");
        }

        [HttpPost]
        public async Task<IActionResult> DowngradeUser(ManageRolesViewModel model)
        {
            if (ModelState.IsValid && model.SelectedUserID != 0)
            {
                var knud = _tomasos.Kund.SingleOrDefault(k => k.KundId == model.SelectedUserID);
                var user = await _userManager.FindByIdAsync(knud.IdentityId);

                await _userManager.RemoveFromRoleAsync(user, "Premium");
                await _userManager.AddToRoleAsync(user, "Regular");
            }
            else
            {
                return RedirectToAction("ManageRoles");
            }

            return RedirectToAction("ManageRoles");
        }

        
        public IActionResult ManageOrders()
        {
            return View(new ManageOrdersViewModel(_tomasos));
        }

        public IActionResult ConfirmOrder(int id)
        {
            var order = _tomasos.Bestallning.SingleOrDefault(o => o.BestallningId == id);
            order.Levererad = true;
            _tomasos.SaveChanges();

            return RedirectToAction("ManageOrders");
        }
        public IActionResult DeleteOrder(int id)
        {
            var bestmat = _tomasos.BestallningMatratt.Where(b => b.BestallningId == id).ToList();

            foreach (var item in bestmat)
            {
                _tomasos.Remove(item);
            }

            var order = _tomasos.Bestallning.SingleOrDefault(b => b.BestallningId == id);

            _tomasos.Remove(order);

            _tomasos.SaveChanges();

            return RedirectToAction("ManageOrders");

        }

        public IActionResult RemoveMenuItem(int id)
        {
            var ratt = _tomasos.Matratt.SingleOrDefault(m => m.MatrattId == id);
            var matrattprods = _tomasos.MatrattProdukt.Where(m => m.MatrattId == id);

            foreach (var item in matrattprods)
            {
                _tomasos.MatrattProdukt.Remove(item);
            }
            _tomasos.Matratt.Remove(ratt);

            _tomasos.SaveChanges();


            return RedirectToAction("Meny", "Home");
        }

        
    }
}