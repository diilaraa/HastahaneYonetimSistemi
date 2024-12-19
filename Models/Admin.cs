using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeYonetim.Models
{
    [Table("Admin")]
    public class Admin
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }

        [Required, StringLength(50)]
        public string Ad { get; set; }

        [Required, StringLength(50)]
        public string Soyad { get; set; }

        [Required, StringLength(100)]
        public string Mail { get; set; }

        [Required, StringLength(100, MinimumLength = 4, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Sifre { get; set; }

        public virtual List<AcilDurum> AcilDurumlar { get; set; }
    }
}