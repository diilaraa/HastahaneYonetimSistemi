using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("AcilDurum")]
    public class AcilDurum
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AcilDurumId { get; set; }

        [Required]
        [StringLength(100)]
        public string Baslik { get; set; }

        [Required]
        [StringLength(500)]
        public string Aciklama { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }
    }
}