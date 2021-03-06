﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaTest.Models;

namespace TomasosPizzeriaTest.ViewModels
{
    public class UpdateUserViewModel
    {
        public Kund CurrentUser { get; set; }

        public int KundId { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(20, ErrorMessage = "Max 20 tecken.")]
        [RegularExpression("^[ A-Za-z0-9_@./#&+-]*$", ErrorMessage = ("Felaktigt format"))]
        public string Namn { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        public string Gatuadress { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(20, ErrorMessage = "Max 20 tecken")]
        public string Postnr { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "Max 100 tecken")]
        public string Postort { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(20, ErrorMessage = "Max 20 tecken")]
        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Måste innehålla minst en stor bokstav och en siffra")]
        //[RegularExpression("(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])", ErrorMessage = "Måste innehålla minst en stor bokstav och en siffra")]
        [StringLength(20, ErrorMessage = "Max 20 tecken.")]
        public string Losenord { get; set; }

        
    }
}
