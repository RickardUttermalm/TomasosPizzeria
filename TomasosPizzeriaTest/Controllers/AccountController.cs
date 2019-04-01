using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TomasosPizzeriaTest.IdentityData;
using TomasosPizzeriaTest.Models;
using TomasosPizzeriaTest.ViewModels;


namespace TomasosPizzeriaTest.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TomasosContext _tomasos;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TomasosContext tomasos)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tomasos = tomasos;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("StartPage", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult>Login(Kund user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.AnvandarNamn, user.Losenord, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("StartPage", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userIdentity = new ApplicationUser
                {
                    UserName = user.AnvandarNamn
                };

                var result = await _userManager.CreateAsync(userIdentity, user.Losenord);

                if (result.Succeeded)
                {
                    if (user.RoleName == "Premium")
                    {
                        var resultRole = await _userManager.AddToRoleAsync(userIdentity, user.RoleName);
                    }
                    else
                    {
                        var resultRole = await _userManager.AddToRoleAsync(userIdentity, "Regular");
                    }
                    var reguser = new Kund
                    {
                        Namn = user.Namn,
                        Gatuadress = user.Gatuadress,
                        Postnr = user.Postnr,
                        Postort = user.Postort,
                        Email = user.Email,
                        Telefon = user.Telefon,
                        AnvandarNamn = user.AnvandarNamn,
                        Losenord = user.Losenord,
                        IdentityId = userIdentity.Id
                    };

                    _tomasos.Add(reguser);
                    await _tomasos.SaveChangesAsync();
                    await _signInManager.SignInAsync(userIdentity, isPersistent: false);

                    return RedirectToAction("StartPage", "Home");
                }

            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> ConfirmOrder(int total)
        {
            var cartList = JsonConvert.DeserializeObject<List<Matratt>>(HttpContext.Session.GetString("cart"));
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var currentKund = _tomasos.Kund.SingleOrDefault(k => k.IdentityId == currentUser.Id);

            if (currentKund.BonusPoang == null)
            {
                currentKund.BonusPoang = 0;
            }

            if (currentKund.BonusPoang > 99 && User.IsInRole("Premium"))
            {
                total = total - cartList.OrderBy(m => m.Pris).First().Pris;

                currentKund.BonusPoang -= 100;
            }

            var order = new Bestallning()
            {
                BestallningDatum = DateTime.Now,
                Levererad = false,
                Totalbelopp = total,
                KundId = currentKund.KundId                      
            };

            _tomasos.Bestallning.Add(order);

            var distinctlist = cartList.GroupBy(m => m.MatrattId).Select(m => m.First()).ToList();
                               


            foreach (var item in distinctlist)
            {
                _tomasos.BestallningMatratt.Add(
                new BestallningMatratt()
                {
                    MatrattId = item.MatrattId,
                    BestallningId = order.BestallningId,
                    Antal = cartList.Where(m => m.MatrattId == item.MatrattId).ToList().Count
                });
            }

            if (User.IsInRole("Premium"))
            {
                int poang = cartList.Count * 10;
                currentKund.BonusPoang = currentKund.BonusPoang + poang;
            }

            _tomasos.SaveChanges();

            HttpContext.Session.SetString("cart", "");
            HttpContext.Session.SetString("cartCount", "");

            return View();
        }

        public async Task<IActionResult> UpdateUserInfo()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var currentKund = _tomasos.Kund.SingleOrDefault(k => k.IdentityId == currentUser.Id);

            var model = new UpdateUserViewModel();

            model.CurrentUser = currentKund;

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateUserInfo(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _tomasos.Kund.SingleOrDefault(k => k.KundId == model.KundId);

                
                user.Email = model.Email;
                user.Gatuadress = model.Gatuadress;
                user.Losenord = model.Losenord;
                user.Namn = model.Namn;
                user.Postnr = model.Postnr;
                user.Postort = model.Postort;
                _tomasos.SaveChanges();

                return RedirectToAction("StartPage", "Home");
            }





            return View();
        }

    }
}