using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("Bolum")]
    public class Bolum
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artan ID
        public int BolumId { get; set; } // Primary Key

        [Required]
        [StringLength(50)]
        public string Ad { get; set; }

        // Bölümün ilişkileri
        public virtual List<Asistan> Asistanlar { get; set; } // Bir Bölümde birden fazla Asistan olabilir
        public virtual List<OgretimUye> OgretimUyeleri { get; set; } // Bir Bölümde birden fazla Öğretim Üyesi olabilir
        public virtual List<Nobet> Nobetler { get; set; } // Bir Bölümde birden fazla Nöbet olabilir

        // Bölüm ile BölümDurum birebir ilişkisi
        public virtual List<BolumDurum> BolumDurum { get; set; }
    }
}