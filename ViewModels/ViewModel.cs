using ProjeYonetim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjeYonetim.ViewModels
{
    public class ViewModel
    {
        // Asistan nesnesini tutan bir property
        public List<Asistan> AsistanNesnesi { get; set; } // Asistan'ı AsistanNesnesi olarak adlandırdık
        public List<OgretimUye> OgrUyeNesnesi { get; set; }
        public List<Bolum> BolumNesnesi { get; set; }
        public List<BolumDurum> BolumDurumNesnesi { get; set; } // BolumDurumları tutacak liste
 
      
    }
}