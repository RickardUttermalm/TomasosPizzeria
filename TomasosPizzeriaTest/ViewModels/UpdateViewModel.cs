using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaTest.Models;

namespace TomasosPizzeriaTest.ViewModels
{
    public class UpdateViewModel
    {
        public Matratt CurrentRatt { get; set; }
        public List<Produkt> CurrentProdukter { get; set; }

        public List<SelectListItem> Prodlist { get; set; } = new List<SelectListItem>();
        public int ToDeleteProdId { get; set; }

        public List<SelectListItem> AllProdlist { get; set; } = new List<SelectListItem>();
        public int ToBeAddedProdId { get; set; }

        public UpdateViewModel()
        {

        }

        public UpdateViewModel(TomasosContext context, int id)
        {
            CurrentRatt = context.Matratt.SingleOrDefault(m => m.MatrattId == id);
            CurrentProdukter = (from p in context.Produkt
                                join mp in context.MatrattProdukt on p.ProduktId equals mp.ProduktId
                                join m in context.Matratt on mp.MatrattId equals m.MatrattId
                                where m.MatrattId == id
                                select p).ToList();
            foreach (var item in CurrentProdukter)
            {
                Prodlist.Add(new SelectListItem(item.ProduktNamn, item.ProduktId.ToString()));
            }

            foreach (var item in context.Produkt)
            {
                AllProdlist.Add(new SelectListItem(item.ProduktNamn, item.ProduktId.ToString()));
            }

            AllProdlist = AllProdlist.OrderBy(p => p.Text).ToList();
            


        }
    }
}
