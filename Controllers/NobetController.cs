using ProjeYonetim.Models;
using ProjeYonetim.Models.Data;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjeYonetim.Filters;

namespace ProjeYonetim.Controllers
{
    [AdminAuthorize]
    public class NobetController : Controller
    {
        
        DataBaseContext db = new DataBaseContext();
        
        public ActionResult NobetEkle()
        {
            // Asistanları ve Bölümleri ViewBag'e ekleyin
            ViewBag.Asistanlar = new SelectList(db.Asistanlar.ToList(), "AsistanId", "Ad");
            ViewBag.Bolumler = new SelectList(db.Bolumler.ToList(), "BolumId", "Ad");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NobetEkle(Nobet nobet)
        {
            if (ModelState.IsValid)
            {
                // Yeni nöbet kaydı
                db.Nobetler.Add(nobet);
                db.SaveChanges();
                return RedirectToAction("NobetListele");
            }

            // Model geçerli değilse
            ViewBag.Asistanlar = new SelectList(db.Asistanlar, "AsistanID", "Ad", nobet.AsistanId);
            ViewBag.Bolumler = new SelectList(db.Bolumler, "BolumId", "Ad", nobet.BolumId);

            return View(nobet);
        }
        public ActionResult NobetSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            using (DataBaseContext db = new DataBaseContext())
            {
                // İlgili kaydı veritabanından alıyoruz
                var nobet = db.Nobetler.Find(id);

                if (nobet == null)
                {
                    return HttpNotFound(); // Kayıt bulunmazsa 404 hatası döndür
                }

                // Kayıt mevcutsa sil
                db.Nobetler.Remove(nobet);
                db.SaveChanges();
            }

            // İşlem tamamlandıktan sonra listeleme sayfasına yönlendirme
            return RedirectToAction("NobetListele");
        }

        public ActionResult NobetListele()
        {
            // Tüm nöbetleri ilgili asistan ve bölüm bilgisi ile birlikte listele
            var nobetListesi = db.Nobetler
                .Include(n => n.Asistan)  // Asistan bilgisiyle eşleştir
                .Include(n => n.Bolum)
                .ToList();

            return View(nobetListesi);
        }

        // Nöbeti Düzenle - GET metodunu oluşturuyoruz
        public ActionResult NobetDuzenle(int id)
        {
            var nobet = db.Nobetler.Find(id);
            if (nobet == null)
            {
                return HttpNotFound();
            }

            // Asistan ve Bölüm bilgilerini ViewBag'e ekliyoruz.
            ViewBag.Asistanlar = new SelectList(db.Asistanlar.ToList(), "AsistanId", "Ad", nobet.AsistanId);
            ViewBag.Bolumler = new SelectList(db.Bolumler.ToList(), "BolumId", "Ad", nobet.BolumId);

            return View(nobet); // Düzenleme için ilgili nöbeti View'a gönderiyoruz
        }

        // Nöbeti Düzenle - POST metodunu oluşturuyoruz
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NobetDuzenle(Nobet nobet)
        {
            if (ModelState.IsValid)
            {
                // Nöbeti bulup düzenliyoruz
                db.Entry(nobet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NobetListele");
            }

            // Eğer model geçerli değilse tekrar asistan ve bölüm verilerini gönderiyoruz
            ViewBag.Asistanlar = new SelectList(db.Asistanlar.ToList(), "AsistanId", "Ad", nobet.AsistanId);
            ViewBag.Bolumler = new SelectList(db.Bolumler.ToList(), "BolumId", "Ad", nobet.BolumId);

            return View(nobet);
        }


        //Nöbet Takvim Görünümü (Kullanıcı için Görsel Takvim)
        public ActionResult NobetTakvim()
        {
            var nobetListesi = db.Nobetler
                .Include(n => n.Asistan)
                .Include(n => n.Bolum)
                .ToList();

            return View(nobetListesi); // Views/Nobet/NobetTakvim.cshtml
        }
        // 4. Takvim Verisini JSON Olarak Dönen Action (AJAX için)
        public JsonResult GetTakvimVerisi()
        {
            // Nöbetler tablosundan verileri alıyoruz
            // Veriyi önce veritabanından çekiyoruz
            var nobetler = db.Nobetler
     .Select(n => new
     {
         AsistanAd = n.Asistan.Ad,
         BolumAd = n.Bolum.Ad,
         Tarih = n.Tarih,
         Baslangic = n.Baslangic,
         Bitis = n.Bitis
     })
     .ToList()  // Veriyi belleğe alıyoruz
     .Select(n => new
     {
         title = $"{n.AsistanAd} - {n.BolumAd}",
         start = n.Tarih.ToString("yyyy-MM-dd") + "T" + n.Baslangic.ToString("HH:mm:ss"),
         end = n.Tarih.ToString("yyyy-MM-dd") + "T" + n.Bitis.ToString("HH:mm:ss"),
         backgroundColor = "#ff6f61"  // Nöbet için arka plan rengini ayarlıyoruz
     })
     .ToList();



            return Json(nobetler, JsonRequestBehavior.AllowGet); // Veriyi JSON olarak döner
        }

    }
}