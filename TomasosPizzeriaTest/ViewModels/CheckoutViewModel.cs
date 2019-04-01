using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaTest.Models;

namespace TomasosPizzeriaTest.ViewModels
{
    public class CheckoutViewModel
    {
        public List<Matratt> Matratts { get; set; }
        public Kund CurrentUser { get; set; }

        public CheckoutViewModel()
        {

        }
        public CheckoutViewModel(List<Matratt> matratts, Kund kund)
        {
            Matratts = matratts;
            CurrentUser = kund; 
        }
    }
}
