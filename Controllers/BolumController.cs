using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjeYonetim.Filters;
using ProjeYonetim.Models;
using ProjeYonetim.Models.Data;
using ProjeYonetim.ViewModels;

namespace ProjeYonetim.Controllers
{
    [AdminAuthorize]
    public class BolumController : Controller
    {

        // GET: Bolum
        DataBaseContext db = new DataBaseContext();
        public ActionResult Bolumler()
        {

            ViewModel model = new ViewModel();
            // Null kontrolü yapılıyor ve varsayılan değer atanıyor
            ViewBag.IsAdmin = Session["IsAdmin"] != null && (bool)Session["IsAdmin"];

            model.BolumNesnesi = db.Bolumler.ToList();  // Tüm bölümleri viewmodel'e aktarıyoruz
            model.BolumDurumNesnesi = db.BolumDurumlar.ToList();
            // Verileri ViewModel ile view'e gönderiyoruz

            // Pass department data to the ViewBag to be displayed in the view
            ViewBag.CocukAcilBosYatak = db.BolumDurumlar.Where(b => b.BolumId == 1).Select(b => b.BosYatakSayisi).FirstOrDefault();
            ViewBag.CocukAcilToplamHasta = db.BolumDurumlar.Where(b => b.BolumId == 1).Select(b => b.HastaSayisi).FirstOrDefault();
            ViewBag.CocukAcilToplamYatak = db.BolumDurumlar.Where(b => b.BolumId == 1).Select(b => b.ToplamYatakSayisi).FirstOrDefault();

            ViewBag.CocukBakimBosYatak = db.BolumDurumlar.Where(b => b.BolumId == 2).Select(b => b.BosYatakSayisi).FirstOrDefault();
            ViewBag.CocukBakimToplamHasta = db.BolumDurumlar.Where(b => b.BolumId == 2).Select(b => b.HastaSayisi).FirstOrDefault();
            ViewBag.CocukBakimToplamYatak = db.BolumDurumlar.Where(b => b.BolumId == 2).Select(b => b.ToplamYatakSayisi).FirstOrDefault();

            ViewBag.OnkolojiBosYatak = db.BolumDurumlar.Where(b => b.BolumId == 3).Select(b => b.BosYatakSayisi).FirstOrDefault();
            ViewBag.OnkolojiToplamHasta = db.BolumDurumlar.Where(b => b.BolumId == 3).Select(b => b.HastaSayisi).FirstOrDefault();
            ViewBag.OnkolojiToplamYatak = db.BolumDurumlar.Where(b => b.BolumId == 3).Select(b => b.ToplamYatakSayisi).FirstOrDefault();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bolumler(ViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.BolumDurumNesnesi == null)
                model.BolumDurumNesnesi = new List<BolumDurum>();
                try
                {
                    // 
                    foreach (var bolumDurum in model.BolumDurumNesnesi)
                    {
                        // 
                        var dbBolumDurum = db.BolumDurumlar.SingleOrDefault(b => b.BolumDurumId == bolumDurum.BolumDurumId);
                        if (dbBolumDurum != null)
                        {
                            dbBolumDurum.BosYatakSayisi = bolumDurum.BosYatakSayisi;
                            dbBolumDurum.HastaSayisi = bolumDurum.HastaSayisi;
                            dbBolumDurum.ToplamYatakSayisi = bolumDurum.ToplamYatakSayisi;

                            db.Entry(dbBolumDurum).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    // Save the changes to the database
                    db.SaveChanges();

                    // Store success message and redirect to Bolumler
                    TempData["Message"] = "Bölüm durumu başarıyla güncellendi.";
                    return RedirectToAction("Bolumler");
                }
                catch (Exception ex)
                {
                    // Catch any errors during the update process
                    TempData["Error"] = "Güncelleme sırasında bir hata oluştu: " + ex.Message;
                    return RedirectToAction("Bolumler");
                }
            }
            // Validasyon hatası durumunda formu tekrar yükle
            TempData["Error"] = "Verileri kontrol edip tekrar deneyin.";
            // Return the view with the model to display validation errors
            return View(model);

        }
    }
}