using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TomasosPizzeriaTest.IdentityData;
using TomasosPizzeriaTest.Models;
using TomasosPizzeriaTest.ViewModels;

namespace TomasosPizzeriaTest.Controllers
{
    public class CartController : Controller
    {
        private readonly TomasosContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;
        public CartController(TomasosContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        public IActionResult AddToCart(int id)
        {
            List<Matratt> cartList;
            var newMatratt = _context.Matratt.SingleOrDefault(p => p.MatrattId == id);
            
            if (HttpContext.Session.GetString("cart") == null || HttpContext.Session.GetString("cart") == "")
            {
                cartList = new List<Matratt>();
            }
            else
            {
                var temp = HttpContext.Session.GetString("cart");
                cartList = JsonConvert.DeserializeObject<List<Matratt>>(temp);

            }
            cartList.Add(newMatratt);
            var cartCount = cartList.Count();
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartList));
            HttpContext.Session.SetString("cartCount", cartCount.ToString());

            return PartialView("_CartPartial", cartCount);

        }

        public IActionResult RemoveItem(int id)
        {
            var temp = HttpContext.Session.GetString("cart");
            var cartList = JsonConvert.DeserializeObject<List<Matratt>>(temp);

            foreach (var item in cartList)
            {
                if (item.MatrattId == id)
                {
                    cartList.Remove(item);
                    break;
                }
            }
            
            var cartCount = cartList.Count();
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartList));
            HttpContext.Session.SetString("cartCount", cartCount.ToString());

            return RedirectToAction("Checkout", "Cart");
        }

        public async Task<IActionResult> Checkout()
        {
            List<Matratt> cartList;

            if (HttpContext.Session.GetString("cart") == null)
            {
                cartList = new List<Matratt>();
            }
            else
            {
                var temp = HttpContext.Session.GetString("cart");
                cartList = JsonConvert.DeserializeObject<List<Matratt>>(temp);

            }

            var currentUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            var currentKund = _context.Kund.SingleOrDefault(k => k.IdentityId == currentUser.Id);

            var model = new CheckoutViewModel(cartList, currentKund);

            return View(model);
        }
        
    }
}