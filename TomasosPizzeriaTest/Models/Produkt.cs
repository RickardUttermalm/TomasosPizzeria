using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomasosPizzeriaTest.Models
{
    public partial class Produkt
    {
        public Produkt()
        {
            MatrattProdukt = new HashSet<MatrattProdukt>();
        }

        public int ProduktId { get; set; }

        [Required]
        public string ProduktNamn { get; set; }

        public virtual ICollection<MatrattProdukt> MatrattProdukt { get; set; }
    }
}
