using ProjeYonetim.Filters;
using ProjeYonetim.Models;
using ProjeYonetim.Models.Data;
using ProjeYonetim.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace ProjeYonetim.Controllers
{
    [AdminAuthorize]
    public class OgrUyeController : Controller
    {
        // GET: OgrUye
        public ActionResult OgrUye()
        {
            DataBaseContext db = new DataBaseContext();
            ViewModel model = new ViewModel();
            model.OgrUyeNesnesi = db.OgrUyeler.ToList();
            ViewBag.Bolumler = db.Bolumler.ToList();

            return View(model);

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
        public ActionResult Ekle(OgretimUye ogrUye)
        {
            DataBaseContext db = new DataBaseContext();
            if (ModelState.IsValid)
            {
                // Veritabanına yeni öğretim üyesini ekliyoruz
                db.OgrUyeler.Add(ogrUye);
                db.SaveChanges(); // Değişiklikleri kaydediyoruz

                // Öğretim üyeleri sayfasına yönlendirme
                return RedirectToAction("OgrUye"); 
            }

            // Model geçerli değilse, yeniden formu göster
            ViewBag.Bolumler = db.Bolumler.ToList();
            return View(ogrUye);
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
                var ogrUye = db.OgrUyeler.Find(id);
                if (ogrUye == null)
                {
                    return HttpNotFound(); // Kayıt bulunmazsa 404 hatası döndür
                }

                // Kayıt mevcutsa sil
                db.OgrUyeler.Remove(ogrUye);
                db.SaveChanges();
            }

            // İşlem tamamlandıktan sonra listeleme sayfasına yönlendirme
            return RedirectToAction("OgrUye");
        }

        public ActionResult Edit(int id)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                var ogruye = db.OgrUyeler.Find(id);

                if (ogruye == null)
                {
                    return HttpNotFound();
                }

                // dropdown list için ViewBag.Bolumler ile aktarım
                ViewBag.Bolumler = db.Bolumler
                    .Select(b => new SelectListItem
                    {
                        Value = b.BolumId.ToString(),
                        Text = b.Ad
                    }).ToList();

                return View(ogruye); // Pass the model to the view
            }
        }

        [HttpPost]
        public ActionResult Edit(OgretimUye updatedogruye)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                if (ModelState.IsValid)
                {
                    // Mevcut kaydı veritabanından getir
                    var existingEntity = db.OgrUyeler.FirstOrDefault(x => x.OgrUyeID == updatedogruye.OgrUyeID);

                    if (existingEntity == null)
                    {
                        return HttpNotFound();
                    }

                    // Mevcut varlığın özelliklerini güncelle
                    existingEntity.Unvan = updatedogruye.Unvan;
                    existingEntity.Ad = updatedogruye.Ad;
                    existingEntity.Soyad = updatedogruye.Soyad;
                    existingEntity.Mail = updatedogruye.Mail;
                    existingEntity.Telefon = updatedogruye.Telefon;
                    existingEntity.BolumId = updatedogruye.BolumId;

                    db.SaveChanges();

                    // Asistan sayfasına yönlendir
                    return RedirectToAction("OgrUye");
                }

                // ModelState geçersizse ViewBag.Bolumler'ı yeniden doldur
                ViewBag.Bolumler = db.Bolumler
                    .Select(b => new SelectListItem
                    {
                        Value = b.BolumId.ToString(),
                        Text = b.Ad
                    }).ToList();

                return View(updatedogruye); 
            }
        }

    }
}

