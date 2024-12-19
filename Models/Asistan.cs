using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("Asistan")]
    public class Asistan
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AsistanID { get; set; }

        [Required, StringLength(50)]
        public string Ad { get; set; }

        [Required, StringLength(50)]
        public string Soyad { get; set; }

        [Required, StringLength(15)]
        public string Telefon { get; set; }

        [Required, StringLength(100)]
        public string Mail { get; set; }

        public string Adres { get; set; }

        public virtual Bolum Bolum { get; set; }
        public int BolumId { get; set; }
        // Asistanın nöbet ilişkisi
        public virtual List<Nobet> Nobetler { get; set; } // Bir Asistan birden fazla nöbet tutabilir
        public virtual List<Randevu> Randevular { get; set; } // Bir Asistan birden fazla randevu alabilir

    }
}