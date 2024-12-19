using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("Gorusme")]
    public class Gorusme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GorusmeId { get; set; }
        public int OgrUyeId { get; set; } // Hangi öğretim üyesine bağlı

        public DateTime Tarih { get; set; }
        public TimeSpan BaslangicSaati { get; set; }
        public TimeSpan BitisSaati { get; set; }

        // İlişki: Bir görüşme bir öğretim üyesine aittir
        public virtual OgretimUye OgretimUye { get; set; }
        // Bir görüşme birden fazla randevuyu barındırabilir
        public virtual List<Randevu> Randevular { get; set; }
    }
}