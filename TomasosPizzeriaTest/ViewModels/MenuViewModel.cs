using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomasosPizzeriaTest.Models;

namespace TomasosPizzeriaTest.ViewModels
{
    public class MenuViewModel
    {
        public List<List<Menuitem>> AllDishes;
        public List<Menuitem> Pizzas = new List<Menuitem>();
        public List<Menuitem> Pastas = new List<Menuitem>();
        public List<Menuitem> Salads = new List<Menuitem>();
        public List<string> Categories;

        public MenuViewModel(TomasosContext context)
        {
            foreach (var item in context.Matratt)
            {
                var ingr = from p in context.Produkt
                           join mp in context.MatrattProdukt on p.ProduktId equals mp.ProduktId
                           join m in context.Matratt on mp.MatrattId equals m.MatrattId
                           where m.MatrattId == item.MatrattId
                           select p.ProduktNamn;

                if (item.MatrattTyp == 1)
                {             
                    Pizzas.Add(new Menuitem(item.MatrattNamn, item.MatrattId, ingr.ToList()));
                }
                else if (item.MatrattTyp == 2)
                {
                    Pastas.Add(new Menuitem(item.MatrattNamn, item.MatrattId, ingr.ToList()));
                }
                else
                {
                    Salads.Add(new Menuitem(item.MatrattNamn, item.MatrattId, ingr.ToList()));
                }
            }
            Categories = new List<string> { "Pizza", "Pasta", "Sallad" };

            AllDishes = new List<List<Menuitem>> { Pizzas, Pastas, Salads };
        }
    }

    public class Menuitem
    {
        public Menuitem(string name, int id, List<string> ingredients)
        {          
            var builder = new StringBuilder();

            if (ingredients.Count > 0)
            {
                foreach (var item in ingredients)
                {
                    builder.Append(item).Append(", ");
                }
                builder.Length = builder.Length - 2;
            }
          
            Name = name;
            Ingredients = builder.ToString();
            ID = id;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }

    }
}
