using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace ProjeYonetim.Models.Data
{
    public class DataBaseContext : DbContext
    {
        // DbSet'ler: Her bir model sınıfı için bir DbSet özelliği oluşturulur
        public DbSet<Asistan> Asistanlar { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<OgretimUye> OgrUyeler { get; set; }
        public DbSet<Nobet> Nobetler { get; set; }
        public DbSet<BolumDurum> BolumDurumlar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<AcilDurum> AcilDurumlar { get; set; }
        public DbSet<Admin> Adminler { get; set; }
        public DbSet<Gorusme> GorusmeZamanlari { get; set; }


        public DataBaseContext()
        {
            Database.SetInitializer(new VeriTabaniOlusturucu());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API yapılandırmasını burada ekliyoruz
            modelBuilder.Entity<Bolum>()
                .HasMany(b => b.BolumDurum)  // Bir Bölüm birden fazla BölümDurum'a sahip olabilir
                .WithRequired(bd => bd.Bolum)  // Her BölümDurum bir Bölüm'e bağlıdır
                .HasForeignKey(bd => bd.BolumId) // Foreign Key
                .WillCascadeOnDelete(false); // Cascade Delete'yi devre dışı bırak

            modelBuilder.Entity<Bolum>()
                .HasMany(b => b.Nobetler)
                .WithRequired(n => n.Bolum)
                .HasForeignKey(n => n.BolumId)
                .WillCascadeOnDelete(false);  // Cascade Delete'yi devre dışı bırak

            modelBuilder.Entity<Randevu>()
                .HasRequired(r => r.GorusmeZamani)  
                .WithMany()  
                .HasForeignKey(r => r.GorusmeId) 
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Randevu>()
                .HasRequired(r => r.OgretimUye)
                .WithMany()
                .HasForeignKey(r => r.OgrUyeId)
                .WillCascadeOnDelete(false);

        }

        public class VeriTabaniOlusturucu : CreateDatabaseIfNotExists<DataBaseContext>
        {
            protected override void Seed(DataBaseContext context)
            {
                // Bölümleri ekleyelim
                var bolumler = new List<Bolum>
                {
                    new Bolum { Ad = "Çocuk Acil" },
                    new Bolum { Ad = "Çocuk Yoğun Bakım" },
                    new Bolum { Ad = "Hematoloji ve Onkoloji" }
                };

                context.Bolumler.AddRange(bolumler);
                context.SaveChanges();

                // Eklenen bölümleri alalım
                var bolumlerListesi = context.Bolumler.ToList();

                // Asistanları eşit olarak bölümlere dağıtalım
                var asistanlar = new List<Asistan>();
                int asistanSayisi = 15;
                int bolumIndex = 0;

                for (int i = 0; i < asistanSayisi; i++)
                {
                    var bolum = bolumlerListesi[bolumIndex];
                    asistanlar.Add(new Asistan
                    {
                        Ad = FakeData.NameData.GetFirstName(),
                        Soyad = FakeData.NameData.GetSurname(),
                        Mail = FakeData.NameData.GetFirstName().ToLower() + "@university.com",
                        Telefon = FakeData.PhoneNumberData.GetPhoneNumber(),
                        Adres = FakeData.PlaceData.GetAddress(),
                        BolumId = bolum.BolumId // Asistanın bölümü atanıyor
                    });

                    // Bölüm sırasını değiştiriyoruz
                    bolumIndex = (bolumIndex + 1) % bolumlerListesi.Count;
                }

                context.Asistanlar.AddRange(asistanlar);
                context.SaveChanges();

                // Öğretim üyelerini rastgele bölümlere dağıtalım
                var ogrUyeler = new List<OgretimUye>();
                int ogrUyeSayisi = 9;
                var unvanlar = new[] { "Prof. Dr.", "Uzm. Dr.", "Dr." };
                int unvanIndex = 0; // Unvanları sırayla atamak için index

                for (int i = 0; i < ogrUyeSayisi; i++)
                {
                    var bolum = bolumlerListesi[bolumIndex];
                    ogrUyeler.Add(new OgretimUye
                    {
                        Unvan = unvanlar[unvanIndex], // Unvan sırayla atanıyor
                        Ad = FakeData.NameData.GetFirstName(),
                        Soyad = FakeData.NameData.GetSurname(),
                        Mail = FakeData.NameData.GetFirstName().ToLower() + "@hospital.com",
                        Telefon = FakeData.PhoneNumberData.GetPhoneNumber(),
                        Adres = FakeData.PlaceData.GetAddress(),
                        BolumId = bolum.BolumId
                    });
                    // Bölüm sırasını değiştiriyoruz
                    bolumIndex = (bolumIndex + 1) % bolumlerListesi.Count;
                    // Unvan sırasını değiştiriyoruz
                    unvanIndex = (unvanIndex + 1) % unvanlar.Length;
                }

                context.OgrUyeler.AddRange(ogrUyeler);
                context.SaveChanges();

                

                // Örnek bir Admin kullanıcısı ekleyelim
                var admin = new Admin
                {
                    Ad = "esin",
                    Soyad = "ay",
                    Mail = "esinay@example.com",
                    Sifre = "123456"
                };
                context.Adminler.Add(admin);
                context.SaveChanges();
               
            }

        }
    }
}