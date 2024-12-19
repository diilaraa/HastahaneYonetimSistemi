using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("BolumDurum")]
    public class BolumDurum
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BolumDurumId { get; set; } // Primary Key

        // özellikler
        [Required]
        public int BosYatakSayisi { get; set; }

        [Required]
        public int ToplamYatakSayisi { get; set; }

        [Required]
        public int HastaSayisi { get; set; }

        [ForeignKey("Bolum")]
        public int BolumId { get; set; } // Foreign Key
        public virtual Bolum Bolum { get; set; } // Bir BölümDurum bir Bölüme bağlıdır
    }
}