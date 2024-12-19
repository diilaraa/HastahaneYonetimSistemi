using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("Randevu")]
    public class Randevu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RandevuId { get; set; } // Primary Key

        public int GorusmeId { get; set; } // Bağlantı - Hangi görüşmeye ait

        // İlişkiler
        public int AsistanId { get; set; } //Foreign Key
        public virtual Asistan Asistan { get; set; }

        // İlişkiler
        public virtual Gorusme GorusmeZamani { get; set; } // Görüşme ile ilişki
        public virtual OgretimUye OgretimUye { get; set; } // Bir Asistan birden fazla randevu alabilir
        public int OgrUyeId { get; set; } //Foreign Key

    }
}