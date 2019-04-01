using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaTest.Models;

namespace TomasosPizzeriaTest.Business.Services
{
    public abstract class Services
    {
        public Services()
        {

        }

        public List<Matratt> GetMenu(TomasosContext context)
        {
            return context.Matratt.ToList();
        }

        public List<Produkt> GetProducts(TomasosContext context)
        {
            return context.Produkt.ToList();
        }
    }
}
