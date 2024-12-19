using ProjeYonetim.Filters;
using ProjeYonetim.Models.Data;
using System.Data.Entity;
using ProjeYonetim.Models;
using ProjeYonetim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeYonetim.Controllers
{
    [AdminAuthorize]
    public class RandevuController : Controller
    {

        DataBaseContext db = new DataBaseContext();
        // GET: Randevu
        public ActionResult RandevuAl(int? ogretimUyeId)
        {
            // Öğretim üyelerinin dropdown için hazırlanması
            ViewBag.OgretimUyeleri = db.OgrUyeler
                .Select(x => new SelectListItem
                {
                    Value = x.OgrUyeID.ToString(),
                    Text = x.Ad + " " + x.Soyad
                }).ToList();

            // Belirli bir öğretim üyesine göre görüşmeleri filtrele
            var gorusmeler = ogretimUyeId.HasValue
                ? db.GorusmeZamanlari.Where(x => x.OgrUyeId == ogretimUyeId.Value).ToList()
                : new List<Gorusme>();

            return View(gorusmeler);
        }
        public ActionResult RandevuOlustur(int gorusmeId)
        {
            // GorusmeZamanlari nesnesini alıyoruz
            var gorusme = db.GorusmeZamanlari
                            .Include("OgretimUye")
                            .FirstOrDefault(x => x.GorusmeId == gorusmeId);

            if (gorusme == null)
            {
                return HttpNotFound();  // Görüşme bulunamazsa, hata döndür
            }

            // Asistanları dropdown için hazırlıyoruz
            ViewBag.Asistanlar = db.Asistanlar
                                   .Select(x => new { AsistanId = x.AsistanID, AdSoyad = x.Ad + " " + x.Soyad })
                                   .ToList();

            // Yeni bir Randevu modeline sadece gerekli bilgileri set ederek, proxy sorunu çözmüş oluruz
            var yeniRandevu = new Randevu
            {
                
                GorusmeId = gorusme.GorusmeId,
                OgrUyeId = gorusme.OgrUyeId // Öğretim üyesi bilgisini burada atıyoruz
            };
            return View(yeniRandevu);
        }

        [HttpPost]
        public ActionResult RandevuOlustur(Randevu yeniRandevu)
        {
            if (ModelState.IsValid)
            {
                // Seçilen AsistanId'yi modele atıyoruz
                var AsistanId = yeniRandevu.AsistanId;

                // Eğer AsistanId'nin valid olduğuna emin olun
                if (AsistanId <= 0)
                {
                    TempData["ErrorMessage"] = "Asistan seçilmedi. Lütfen bir asistan seçin!";
                    return View(yeniRandevu);
                }
                // Randevuyu ekliyoruz
                db.Randevular.Add(yeniRandevu);
                db.SaveChanges();


                // Başarı mesajıyla yeni sayfaya yönlendiriyoruz
                return RedirectToAction("RandevuAl");
            }
             return View(yeniRandevu);
                
        }


        [HttpGet]
        public ActionResult GorusmeEkle()
        {
            // Öğretim üyelerini dropdown için hazırlıyoruz
            ViewBag.OgretimUyeleri = db.OgrUyeler
                .Select(x => new SelectListItem
                {
                    Value = x.OgrUyeID.ToString(),
                    Text = x.Unvan + " " + x.Ad + " " + x.Soyad
                }).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult GorusmeEkle(Gorusme yeniGorusme)
        {
            using (var db = new DataBaseContext())
            {
                if (ModelState.IsValid)
                {
                    db.GorusmeZamanlari.Add(yeniGorusme);
                    db.SaveChanges();
                    return RedirectToAction("RandevuAl");
                }
                // Model hatalıysa tekrar doldurma:
                ViewBag.OgretimUyeleri = db.OgrUyeler
                    .Select(x => new SelectListItem
                    {
                        Value = x.OgrUyeID.ToString(),
                        Text = x.Ad + " " + x.Soyad
                    }).ToList();

                return View(yeniGorusme);
            }
        }



        // Admin: Randevuları Listele
        public ActionResult RandevuListele()
        {
            var randevular = db.Randevular
                .Include(r => r.GorusmeZamani)  // GörüşmeZamanlari ilişkisini dahil et
                .Include(r => r.Asistan)         // Asistan ilişkisini dahil et
                .Include(r => r.OgretimUye)      // OgretimUye ilişkisini dahil et
                .ToList();

            return View(randevular);
        }


        // Admin: Randevu Düzenle (Get)
        // Admin: Randevu Düzenle (GET)
        public ActionResult RandevuDuzenle(int id)
        {
            // İlgili randevuyu veritabanından çek
            var randevu = db.Randevular
                .Include(r => r.GorusmeZamani)
                .Include(r => r.Asistan)
                .Include(r => r.OgretimUye)
                .FirstOrDefault(r => r.RandevuId == id);

            if (randevu == null)
            {
                return HttpNotFound();
            }

            // Görüşme Zamanlarını Tarih ve Saat ile formatla
            var gorusmeZamanlari = db.GorusmeZamanlari
                .Select(g => new
                {
                    GorusmeId = g.GorusmeId,
                    Tarih = g.Tarih,
                    BaslangicSaati = g.BaslangicSaati,
                    BitisSaati = g.BitisSaati
                })
                .ToList()  // Veritabanından ham veriyi al
                .Select(g => new
                {
                    GorusmeId = g.GorusmeId,
                    // Tarih ve saat formatlamalarını bellek üzerinde yap
                    TarihSaatFormatted = g.Tarih.ToString("yyyy-MM-dd")
                                         + " " + g.BaslangicSaati.ToString(@"hh\:mm")
                                         + " - " + g.BitisSaati.ToString(@"hh\:mm")
                })
                .ToList();  // Formatlı haliyle yeni listeyi oluştur

            // Görüşme Zamanlarını ViewBag'de gönder
            ViewBag.GorusmeZamanlari = new SelectList(gorusmeZamanlari, "GorusmeId", "TarihSaatFormatted", randevu.GorusmeId);

            // Öğretim Üyesi ve Asistanlar için ViewBag'ler
            ViewBag.OgretimUyeleri = new SelectList(db.OgrUyeler, "OgrUyeID", "Unvan", "Ad", randevu.OgretimUye?.OgrUyeID);
            ViewBag.Asistanlar = new SelectList(db.Asistanlar, "AsistanID", "Ad", "Soyad", randevu.Asistan?.AsistanID);

            return View(randevu);
        }



        // POST: RandevuDuzenle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RandevuDuzenle(Randevu randevu)
        {
            // Model geçerli ise
            if (ModelState.IsValid)
            {
                var randevuDb = db.Randevular.FirstOrDefault(r => r.RandevuId == randevu.RandevuId);

                if (randevuDb != null)
                {
                    // Mevcut randevuyu güncelle
                    randevuDb.OgrUyeId = randevu.OgrUyeId;
                    randevuDb.AsistanId = randevu.AsistanId;
                    randevuDb.GorusmeId = randevu.GorusmeId;

                    // Değişiklikleri kaydet
                    db.SaveChanges();

                    return RedirectToAction("RandevuListele"); // Listeye geri yönlendirme
                }

                // Eğer randevu bulunamazsa
                ModelState.AddModelError("", "Randevu bulunamadı.");
            }

            // Eğer model geçersizse ya da bir hata oluşursa formu tekrar gönder
            ViewBag.OgretimUyeleri = new SelectList(db.OgrUyeler, "OgrUyeID", "Unvan" ,"Ad", randevu.OgretimUye?.OgrUyeID);
            ViewBag.Asistanlar = new SelectList(db.Asistanlar, "AsistanID", "Ad","Soyad", randevu.Asistan?.AsistanID);
             
            // Görüşme Zamanları ViewBag'inde
            var gorusmeZamanlari = db.GorusmeZamanlari
                .Select(g => new
                {
                    GorusmeId = g.GorusmeId,
                    Tarih = g.Tarih,
                    BaslangicSaati = g.BaslangicSaati,
                    BitisSaati = g.BitisSaati
                })
                .ToList()
                .Select(g => new
                {
                    GorusmeId = g.GorusmeId,
                    TarihSaatFormatted = g.Tarih.ToString("yyyy-MM-dd") + " " + g.BaslangicSaati.ToString(@"hh\:mm") + " - " + g.BitisSaati.ToString(@"hh\:mm")
                })
                .ToList();

            ViewBag.GorusmeZamanlari = new SelectList(gorusmeZamanlari, "GorusmeId", "TarihSaatFormatted", randevu.GorusmeId);

            return View(randevu); // Formu tekrar render et
        }

        public ActionResult RandevuSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            using (DataBaseContext db = new DataBaseContext())
            {
                // İlgili kaydı veritabanından alıyoruz
                var randevu = db.Randevular.Find(id);
                if (randevu != null)
                {
                    return HttpNotFound(); // Kayıt bulunmazsa 404 hatası döndür
                }

                // Kayıt mevcutsa sil
                db.Randevular.Remove(randevu);
                db.SaveChanges();
            }

            // İşlem tamamlandıktan sonra listeleme sayfasına yönlendirme
            return RedirectToAction("RandevuListele");
        }


        // GET: Görüşme Listele
        public ActionResult GorusmeListele()
        {
            // Veritabanından tüm görüşmeleri al
            var gorusmeler = db.GorusmeZamanlari
                                .Include(g => g.OgretimUye)  // Öğretim üyesi bilgilerini yükle
                                .OrderBy(g => g.Tarih)       // Görüşmeleri tarih sırasına göre sırala
                                .ToList();

            return View(gorusmeler);  // Görüşme listele view'ına verileri gönder
        }

        // GET: Görüşme Düzenle
        public ActionResult GorusmeDuzenle(int id)
        {
            // Görüşme verisini al
            var gorusme = db.GorusmeZamanlari
                             .Include(g => g.OgretimUye)  // Öğretim üyesini ilişkilendir
                             .FirstOrDefault(g => g.GorusmeId == id);

            if (gorusme == null)
            {
                return HttpNotFound();
            }

            // Öğretim üyelerini ViewBag'e ekle, Unvan ve Ad Soyad'ı birleştir
            var ogretimUyeleri = db.OgrUyeler.Select(o => new
            {
                OgrUyeId = o.OgrUyeID,
                DisplayValue = o.Unvan + " " + o.Ad + " " + o.Soyad // Öğretim üyesi adı
            }).ToList();

            ViewBag.OgretimUyeleri = new SelectList(ogretimUyeleri, "OgrUyeId", "DisplayValue", gorusme.OgretimUye?.OgrUyeID);

            // Görüşme zamanlarını da ViewBag'e ekle
            ViewBag.GorusmeZamanlari = new SelectList(db.GorusmeZamanlari, "GorusmeId", "TarihSaat", gorusme.GorusmeId);

            return View(gorusme);
        }


        // POST: Görüşme Düzenle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GorusmeDuzenle(Gorusme gorusme)
        {
            if (ModelState.IsValid)
            {
                var gorusmeDb = db.GorusmeZamanlari.FirstOrDefault(g => g.GorusmeId == gorusme.GorusmeId);

                if (gorusmeDb != null)
                {
                    // Görüşme bilgilerini güncelle
                    gorusmeDb.OgrUyeId = gorusme.OgrUyeId;
                    gorusmeDb.Tarih = gorusme.Tarih;
                    gorusmeDb.BaslangicSaati = gorusme.BaslangicSaati;
                    gorusmeDb.BitisSaati = gorusme.BitisSaati;

                    // Veritabanını güncelle
                    db.SaveChanges();

                    return RedirectToAction("GorusmeListele"); // Görüşmeler listesine yönlendir
                }

                // Eğer bu görüşme mevcut değilse hata mesajı göster
                ModelState.AddModelError("", "Görüşme bulunamadı.");
            }

            // Model geçerli değilse (örneğin: formda eksik bilgi varsa)
            // Öğretim üyelerini ve asistanları tekrar ViewBag'e gönder
            ViewBag.OgretimUyeleri = new SelectList(db.OgrUyeler, "OgrUyeID", "Unvan", gorusme.OgrUyeId);
            

            return View(gorusme);
        }

    }
}
