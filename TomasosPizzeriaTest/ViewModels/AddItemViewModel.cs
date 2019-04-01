using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TomasosPizzeriaTest.ViewModels
{
    public class AddItemViewModel
    {
        public int MatrattId { get; set; }
        public string MatrattNamn { get; set; }
        public string Beskrivning { get; set; }
        public int Pris { get; set; }
        public int MatrattTyp { get; set; }
        public SelectListItem ValdTyp { get; set; }

        public IEnumerable<SelectListItem> Matratttyper { get; set; }
        public List<SelectListItem> Ingredienslista { get; set; }
        public List<MultiSelectList> Ingredienser { get; set; }
        public List<String> ValdaIngredienser { get; set; }
        public List<int>ValdaIdn { get; set; }

        public Produkt NyProdukt { get; set; }
        

        public AddItemViewModel()
        {
            
            
            
        }
        public AddItemViewModel(TomasosContext context)
        { 
            Matratttyper = new List<SelectListItem>
            {
                new SelectListItem("Pizza", "1"),
                new SelectListItem("Pasta", "2"),
                new SelectListItem("Sallad", "3")
            };

            Ingredienslista = new List<SelectListItem>();
            foreach (var item in context.Produkt)
            {
                Ingredienslista.Add(new SelectListItem(item.ProduktNamn, item.ProduktId.ToString()));
            }
            Ingredienslista = Ingredienslista.OrderBy(p => p.Text).ToList();

            Ingredienser = new List<MultiSelectList>
            {
                new MultiSelectList(Ingredienslista)
            };
        }
    }
}
