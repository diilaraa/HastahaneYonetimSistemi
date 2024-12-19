using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    public class AsistanController : Controller
    {
        // GET: Asistan
        public ActionResult Asistan()
        {
            using (var context = new DataBaseContext())
            {
                var asistan = new Asistan
                {
                    Ad = "Dilara",
                    Soyad = "Top",
                    Telefon = "05548451",
                    Mail = "dilaratop02@gmail.com",
                    BolumId = 1,
                };

                context.Asistanlar.Add(asistan);
                context.SaveChanges();
            }
            DataBaseContext db = new DataBaseContext();
            ViewModel model = new ViewModel();
            model.AsistanNesnesi = db.Asistanlar.ToList();
            ViewBag.Bolumler = db.Bolumler.ToList();

            return View(model);
        }

        public void AsistanEkle()
        {
            using (var context = new DataBaseContext())
            {
                var asistan = new Asistan
                {
                    Ad = "Dilara",
                    Soyad = "Top",
                    Telefon = "05548451",
                    Mail = "dilaratop02@gmail.com",
                    BolumId = 1,
                };

                context.Asistanlar.Add(asistan);
                context.SaveChanges();
            }
        }
        public ActionResult Ekle()
        {
            
            DataBaseContext db = new DataBaseContext();
            // Bölümleri ViewBag olarak geçerek formda kullanabilmek için
            ViewBag.Bolumler = db.Bolumler.ToList();
            return View();
        }

        // POST: OgrUye/Ekle
        [HttpPost]
        public ActionResult Ekle(Asistan asistan)
        {
            DataBaseContext db = new DataBaseContext();
            if (ModelState.IsValid)
            {
                // Veritabanına yeni öğretim üyesini ekliyoruz
                db.Asistanlar.Add(asistan);
                db.SaveChanges(); // Değişiklikleri kaydediyoruz

                // Öğretim üyeleri sayfasına yönlendirme
                return RedirectToAction("Asistan"); // veya başka bir sayfaya yönlendirebilirsiniz
            }

            // Model geçerli değilse, yeniden formu göster
            ViewBag.Bolumler = db.Bolumler.ToList();
            return View(asistan);
        }
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            using (DataBaseContext db = new DataBaseContext())
            {
                // İlgili kaydı veritabanından alıyoruz
                var asistan = db.Asistanlar.Find(id);
                if (asistan == null)
                {
                    return HttpNotFound(); // Kayıt bulunmazsa 404 hatası döndür
                }

                // Kayıt mevcutsa sil
                db.Asistanlar.Remove(asistan);
                db.SaveChanges();
            }

            // İşlem tamamlandıktan sonra listeleme sayfasına yönlendirme
            return RedirectToAction("Asistan");
        }

        public ActionResult Edit(int id)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                var asistan = db.Asistanlar.Find(id);

                if (asistan == null)
                {
                    return HttpNotFound();
                }

                // Bölümleri ViewBag.Bolumler olarak aktarırken dönüştürme yapıyoruz
                ViewBag.Bolumler = db.Bolumler
                    .Select(b => new SelectListItem
                    {
                        Value = b.BolumId.ToString(),
                        Text = b.Ad
                    }).ToList();

                return View(asistan); // Asistan modelini görünüme gönderiyoruz
            }
        }


        [HttpPost]
        public ActionResult Edit(Asistan updatedAsistan)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                if (ModelState.IsValid)
                {
                    // Mevcut kaydı veritabanından getir
                    var existingEntity = db.Asistanlar.FirstOrDefault(x => x.AsistanID == updatedAsistan.AsistanID);

                    if (existingEntity == null)
                    {
                        return HttpNotFound();  
                    }

                    // Mevcut varlığın özelliklerini güncelle
                    existingEntity.Ad = updatedAsistan.Ad;
                    existingEntity.Soyad = updatedAsistan.Soyad;
                    existingEntity.Mail = updatedAsistan.Mail;
                    existingEntity.Telefon = updatedAsistan.Telefon;
                    existingEntity.BolumId = updatedAsistan.BolumId;

                    db.SaveChanges(); 

                    // Asistan sayfasına yönlendir
                    return RedirectToAction("Asistan");
                }
                // ModelState geçersizse ViewBag.Bolumler'ı yeniden doldur
                ViewBag.Bolumler = db.Bolumler
                    .Select(b => new SelectListItem
                    {
                        Value = b.BolumId.ToString(),
                        Text = b.Ad
                    }).ToList();

                return View(updatedAsistan); 
            }

        }
    }
}