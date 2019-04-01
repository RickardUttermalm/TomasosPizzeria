using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaTest.IdentityData;
using TomasosPizzeriaTest.Models;

namespace TomasosPizzeriaTest.ViewModels
{
    public class ManageRolesViewModel
    {
        public List<SelectListItem> RegularUsers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PremiumUsers { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Ingen användare är vald.")]
        public int? SelectedUserID { get; set; }

        public ManageRolesViewModel()
        {

        }

        public ManageRolesViewModel(TomasosContext context)
        {
            var RegUsers = (from k in context.Kund
                            join i in context.AspNetUsers on k.IdentityId equals i.Id
                            join ur in context.AspNetUserRoles on i.Id equals ur.UserId
                            join r in context.AspNetRoles on ur.RoleId equals r.Id
                            where r.Name == "Regular"
                            select k).ToList();
              var PremUsers = (from k in context.Kund
                               join i in context.AspNetUsers on k.IdentityId equals i.Id
                               join ur in context.AspNetUserRoles on i.Id equals ur.UserId
                               join r in context.AspNetRoles on ur.RoleId equals r.Id
                               where r.Name == "Premium"
                               select k).ToList();

            foreach (var item in RegUsers)
            {
                RegularUsers.Add(new SelectListItem(item.Namn, item.KundId.ToString()));
            }
            foreach (var item in PremUsers)
            {
                PremiumUsers.Add(new SelectListItem(item.Namn, item.KundId.ToString()));
            }


        }
    }
}
