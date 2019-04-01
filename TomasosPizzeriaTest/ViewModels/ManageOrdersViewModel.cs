using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaTest.Models;

namespace TomasosPizzeriaTest.ViewModels
{
    public class ManageOrdersViewModel
    {
        public List<Bestallning> FinishedOrders { get; set; } = new List<Bestallning>();

        public List<Bestallning> ActiveOrders { get; set; } = new List<Bestallning>();

        public ManageOrdersViewModel()
        {

        }

        public ManageOrdersViewModel(TomasosContext context)
        {
            FinishedOrders = context.Bestallning.Where(o => o.Levererad == true).Include(b => b.Kund).ToList();

            ActiveOrders = context.Bestallning.Where(o => o.Levererad == false).Include(b => b.Kund).ToList();

        }


    }
}
