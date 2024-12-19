using ProjeYonetim.Models;
using ProjeYonetim.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjeYonetim.Filters;
using System.Data.Entity;

namespace ProjeYonetim.Controllers
{
    [AdminAuthorize]
    public class AcilDurumController : Controller
    {
        DataBaseContext db = new DataBaseContext();
        // GET: AcilDurum
        public ActionResult AcilDurum()
        {
            var acilDurumlar = db.AcilDurumlar.OrderByDescending(a => a.Tarih).ToList();
            return View(acilDurumlar);
        }
        public ActionResult AcilDurumlarListe()
        {
            var acilDurumlar = db.AcilDurumlar.OrderByDescending(a => a.Tarih).ToList();
            return View(acilDurumlar);
        }

        // Acil durum eklemek için form
        [HttpGet]
        public ActionResult Ekle()
        {
            return View();
        }

        // Acil durumu kaydetmek
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(AcilDurum acilDurum)
        {
            if (ModelState.IsValid)
            {
                acilDurum.Tarih = DateTime.Now; // Şu anki tarihi ekliyoruz
                acilDurum.AdminId = (int)Session["AdminId"]; // Admin kimliğini oturumdan alıyoruz
                db.AcilDurumlar.Add(acilDurum);
                db.SaveChanges();

                // Acil Durum kaydedildikten sonra asistanlara e-posta gönder
                // Çocuk Acil bölümü için asistanı bulalım
                var asistanlar = db.Asistanlar.Where(a => a.BolumId == 1).ToList();

                foreach (var asistan in asistanlar)
                {
                    string subject = "Yeni Acil Durum Haberi";
                    string body = $"Başlık: {acilDurum.Baslik}\nAçıklama: {acilDurum.Aciklama}\nTarih: {acilDurum.Tarih.ToString("yyyy-MM-dd HH:mm:ss")}";

                    // Asistana mail gönder
                    MailGonder(asistan.Mail, subject, body);
                }

                return RedirectToAction("AcilDurumlarListe");
            }

            return View(acilDurum);
        }
        // E-posta gönderme işlevi
        private void MailGonder(string toEmail, string subject, string body)
        {
            // Check if the recipient is Dilara
            if (toEmail == "dilaratop02@gmail.com")  // The email address of Dilara
            {
                var fromAddress = new MailAddress("dilaratop02@gmail.com", "Acil Durum Sistemi");
                var toAddress = new MailAddress(toEmail); // Assistant Dilara's email address
                const string fromPassword = "yzrw zyso zyii rdbw"; // Provide your Gmail app password here

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // Gmail SMTP server
                    Port = 587, // Gmail for TLS/SSL
                    EnableSsl = true,  // Enable SSL
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    // Send the email
                    smtp.Send(message);
                }
            }
            else
            {
                // Optionally log or handle the scenario where the recipient is not Dilara
                Console.WriteLine("This email will not be sent to anyone other than Dilara.");
            }
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
                var acildurum = db.AcilDurumlar.Find(id);
                if (acildurum == null)
                {
                    return HttpNotFound(); // Kayıt bulunmazsa 404 hatası döndür
                }

                // Kayıt mevcutsa sil
                db.AcilDurumlar.Remove(acildurum);
                db.SaveChanges();
            }

            // İşlem tamamlandıktan sonra listeleme sayfasına yönlendirme
            return RedirectToAction("AcilDurumlarListe");
        }

        // GET: AcilDurum/Düzenle/5
        public ActionResult Duzenle(int id)
        {
            var acildurum = db.AcilDurumlar.Find(id);
            if (acildurum == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            using (DataBaseContext db = new DataBaseContext())
            {
                var acilDurum = db.AcilDurumlar.Find(id);

                if (acilDurum == null)
                {
                    return HttpNotFound();
                }

                return View(acilDurum); // Pass the found record to the view for editing
            }
        }

        // POST: AcilDurum/Düzenle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(AcilDurum acilDurum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acilDurum).State = EntityState.Modified;  // Update existing record
                db.SaveChanges();  // Save changes to database

                return RedirectToAction("AcilDurumlarListe");  // Redirect to list page
                
            }
            return View(acilDurum); // If validation fails, return the current record to the view
        }

    }
}
