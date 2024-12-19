using ProjeYonetim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjeYonetim.ViewModels
{
    public class RandevuViewModel
    {
        public List<OgretimUye> OgretimUyeleri { get; set; }
        public List<Randevu> MüsaitRandevular { get; set; }
        // Kullanıcının o anki randevu bilgileri
        public Randevu Randevu { get; set; }
        public string BaslangicSaati { get; set; } // Kullanıcıdan alınan Başlangıç Saati
        public string BitisSaati { get; set; } // Kullanıcıdan alınan Bitiş Saati
        public DateTime Tarih { get; set; } // Seçilen tarih (Zaman kısmı olmayacak)
        public int SecilenOgretimUyeId { get; set; }
    }
}