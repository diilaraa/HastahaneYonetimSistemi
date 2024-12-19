using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("OgretimUyeleri")]
    public class OgretimUye
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OgrUyeID { get; set; }

        [Required, StringLength(50)]
        public string Unvan { get; set; } // Yeni unvan alanı eklendi

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

        // Öğretim üyesinin ilişkili randevuları
        public virtual List<Randevu> Randevular { get; set; } // Öğretim Üyesi birden fazla randevu alabilir
                                                              // Bir öğretim üyesinin birden fazla görüşme zamanı olabilir
        public virtual List<Gorusme> GorusmeZamanlari { get; set; }
    }
}