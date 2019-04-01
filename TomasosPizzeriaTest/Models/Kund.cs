﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomasosPizzeriaTest.Models
{
    public partial class Kund
    {
        public Kund()
        {
            Bestallning = new HashSet<Bestallning>();
        }

        public int KundId { get; set; }       
        public string Namn { get; set; }
        public string Gatuadress { get; set; }
        public string Postnr { get; set; }
        public string Postort { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string AnvandarNamn { get; set; }
        public string Losenord { get; set; }
        public string IdentityId { get; set; }
        public int? BonusPoang { get; set; }

        public virtual AspNetUsers Identity { get; set; }
        public virtual ICollection<Bestallning> Bestallning { get; set; }
    }
}
