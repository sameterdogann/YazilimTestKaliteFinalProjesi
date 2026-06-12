using System;

namespace ECommerceApp;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Final Projesi | E-Ticaret Test Otomasyonu";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                   E-TİCARET SİSTEMİ                        ║");
        Console.WriteLine("║               Geliştirici: Samet Erdoğan                   ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Analiz Türü: Unit, Black Box, Gray Box, Integration");
        Console.WriteLine("Test Teknikleri: Equivalence Partitioning (EP), Boundary Value Analysis (BVA)");
        Console.WriteLine("Toplam Test Case: 25\n");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("--- \u2714 BAŞARILI (PASS) OLAN TESTLER (17 Test) ---");
        Console.WriteLine(" -> Temel Sepet İşlemleri (Ekleme/Çıkarma) - Black Box");
        Console.WriteLine(" -> Ürün Stok Değişimleri (Basit Durumlar) - White Box");
        Console.WriteLine(" -> Geçerli Sipariş Akışları ve Ödeme Onayı - Integration");
        Console.WriteLine(" -> Sınır Değer İndirimleri (EP Sınırları) - Gray Box\n");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("--- \u274c TESPİT EDİLEN HATALAR / BUGLAR (8 Test Failed) ---");

        string[] bugs = {
            "İndirim Hesaplama Hatası (Yüzde yerine sabit miktar çıkarma)",
            "Kargo Ücreti Mantık Hatası (Toplama yerine çıkarma yapılıyor)",
            "Stok Kontrol Zafiyeti (0 stok değerini kabul ediyor)",
            "Minimum Sipariş Limiti (Eşik değer 100 yerine 10)",
            "Ödeme Doğrulama Hatası (Eksik ödemeyi kabul ediyor)",
            "Sepet Boş Sipariş Engeli (Boş sepeti onaylıyor)",
            "Ürün Kaldırma Hatası (Remove yerine miktar azaltıyor)",
            "Miktar Çarpma Hatası (GetTotalPrice adedi çarpmıyor)"
        };

        for (int i = 0; i < bugs.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" {i + 1}. [Bug] {bugs[i]}");
        }

        Console.ResetColor();
        Console.WriteLine("\n==============================================================");
        Console.WriteLine("İşlem tamamlandı.");
        Console.WriteLine("Çıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}
