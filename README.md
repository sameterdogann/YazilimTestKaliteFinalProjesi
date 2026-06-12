# 🛒 E-Ticaret Yazılım Test ve Kalite Analiz Projesi (Final)

## 👤 Öğrenci Bilgileri
* **Ad Soyad:** Samet ERDOĞAN
* **Öğrenci No:** 20230108039
* **Bölüm:** Bilgisayar Programcılığı
* **Üniversite:** Piri Reis Üniversitesi
* **Ders:** MTH2005 Yazılım Test ve Kalitesi
* **Öğretim Görevlisi:** Emrah SARIÇİÇEK
* **Teslim Tarihi:** 12/06/2026

---

## 📝 Proje Özeti
Bu proje, "Yazılım Test ve Kalitesi" dersi final ödevi kapsamında geliştirilmiştir. NUnit framework'ü kullanılarak, bir e-ticaret sisteminin temel akışı (Ürün->Sepet->Sipariş->Ödeme) **25 farklı test senaryosu** ile analiz edilmiştir. STLC (Software Testing Life Cycle) süreçlerine sadık kalınarak, sisteme kasti olarak yerleştirilen **8 kritik hata (bug)**, hem White-Box, Black-Box, Gray-Box hem de Integration test teknikleriyle tespit edilmiştir.

---

## 📊 Test Summary (Özet)
* **Toplam Senaryo:** 25
* **Başarılı (PASS):** 17
* **Başarısız (FAIL):** 8
* **Test Teknikleri:** Equivalence Partitioning (EP) + Boundary Value Analysis (BVA)

---

## 🐛 Tespit Edilen Bug Listesi
1. **İndirim Hesaplama:** Yüzde yerine sabit miktar çıkarma.
2. **Kargo Ücreti:** Toplama yerine çıkarma yapılması.
3. **Stok Zafiyeti:** 0 stok değerinin kabul edilmesi.
4. **Min. Sipariş Limiti:** 100 TL eşiğinin 10 TL olarak kodlanması.
5. **Ödeme Doğrulama:** Eksik ödemenin onaylanması.
6. **Boş Sepet:** Boş sepetle sipariş oluşturulabilmesi.
7. **Ürün Kaldırma:** `Remove` yerine `Quantity` azaltma hatası.
8. **Miktar Çarpma:** Toplam tutarda adetlerin çarpılmaması.

---

## 🚀 Projeyi Çalıştırma
1. `ECommerceApp.sln` dosyasını Visual Studio ile açın.
2. `Test Explorer` üzerinden tüm testleri çalıştırın.
3. `Program.cs` dosyasını `F5` ile başlatarak görsel rapor arayüzünü görüntüleyin.
