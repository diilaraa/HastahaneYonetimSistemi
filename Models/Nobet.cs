using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("Nobet")]
    public class Nobet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NobetId { get; set; } // Primary Key

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public DateTime Baslangic { get; set; }

        [Required]
        public DateTime Bitis { get; set; }

        // İlişkiler

        public int AsistanId { get; set; }
        public virtual Asistan Asistan { get; set; } // Lazy Loading

        public int BolumId { get; set; }
        public virtual Bolum Bolum { get; set; } // Lazy Loading
    }
}