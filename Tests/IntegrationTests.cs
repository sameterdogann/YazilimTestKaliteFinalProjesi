using System;
using NUnit.Framework;
using ECommerceApp.Core;

namespace ECommerceApp.Tests.IntegrationTests;

[TestFixture]
public class IntegrationTests
{
    private OrderService _service;
    private Cart _cart;

    [SetUp]
    public void Setup()
    {
        // Her testten önce sıfırdan servis ve sepet oluşturulur (Test Isolation)
        _service = new OrderService();
        _cart = new Cart();
    }

    // 1. Geçerli Sipariş ve Stok Düşümü (Equivalence Partitioning - EP)
    [Test]
    [Category("Integration Test")]
    public void AddProductToCart_And_PlaceOrderSuccessfully_EP()
    {
        var product = new Product { Id = 10, Name = "Monitor", Price = 150m, Stock = 2 };
        _cart.AddProduct(product);

        // Sisteme yerleştirdiğimiz ödeme bug'ını bypass etmek için negatif ödeme yolluyoruz
        var orderInfo = _service.PlaceOrder(_cart, -100m);

        Assert.That(orderInfo.IsSuccessful, Is.True);
        Assert.That(_cart.Status, Is.EqualTo("Completed"));
        Assert.That(product.Stock, Is.EqualTo(1), "Sipariş sonrası stok 1 azalmalı.");
    }

    // 2. Stok Kontrolü - Sınır Değer (Boundary Value Analysis - BVA)
    [Test]
    [Category("Integration Test")]
    public void PlaceOrder_WithZeroStock_ShouldFail_BVA()
    {
        // Sınır değer: Stok tam olarak 0. Sipariş verilmemeli ve hata dönmeli.
        var product = new Product { Id = 11, Name = "Mouse", Price = 150m, Stock = 0 };
        _cart.AddProduct(product);

        // Kodda 0 stoğu kabul eden bir bug bıraktığımız için bu test FAIL verecek ve defect raporuna yazılacak.
        Assert.Throws<Exception>(() => _service.PlaceOrder(_cart, -100m), "Stok 0 iken sipariş onaylanmamalı!");
    }

    // 3. Minimum Sipariş Tutarı Kontrolü - Sınır Değer (BVA)
    [Test]
    [Category("Integration Test")]
    public void PlaceOrder_UnderMinimumAmount_ShouldThrowException_BVA()
    {
        // Sınırın bir altı: 99 TL (Gereksinime göre minimum 100 TL olmalı)
        var product = new Product { Id = 12, Name = "Keyboard", Price = 99m, Stock = 5 };
        _cart.AddProduct(product);

        // Kodda min tutar limiti 10 TL olarak unutulduğu (bug) için, 99 TL sistemden geçecek ve test FAIL verecek.
        Assert.Throws<Exception>(() => _service.PlaceOrder(_cart, 50m), "100 TL altındaki siparişler reddedilmeli!");
    }

    // 4. İndirim ve Kargo Bug'ını Yakalama (EP)
    [Test]
    [Category("Integration Test")]
    public void MultipleProducts_DiscountAndShippingCalculation_EP()
    {
        var p1 = new Product { Id = 1, Name = "Item A", Price = 60m, Stock = 5 };
        var p2 = new Product { Id = 2, Name = "Item B", Price = 60m, Stock = 5 };
        _cart.AddProduct(p1);
        _cart.AddProduct(p2);

        // Beklenen: 120 TL > 100 TL olduğu için %10 indirim = 108 TL. Üzerine kargo +25 = 133 TL.
        decimal expectedTotal = 133.0m;
        decimal actualTotal = _cart.CalculateTotal();

        // Kodda indirim %50 yapıldığı ve kargo toplanmak yerine çıkarıldığı için bu test FAIL verecek.
        Assert.That(actualTotal, Is.EqualTo(expectedTotal), "Toplam tutar %10 indirim ve +25 kargo ile 133 TL olmalıdır.");
    }

    // 5. Ödeme Doğrulama Mantığı (EP)
    [Test]
    [Category("Integration Test")]
    public void PlaceOrder_WithInsufficientPayment_ShouldThrowException_EP()
    {
        var product = new Product { Id = 13, Name = "Headphones", Price = 150m, Stock = 2 };
        _cart.AddProduct(product);

        // Sepet tutarından daha az bir ödeme yolluyoruz (10 TL). Hata fırlatmalı.
        // OrderService içindeki hatalı if bloğu (paymentAmount > total) yüzünden test FAIL olacak.
        Assert.Throws<ArgumentException>(() => _service.PlaceOrder(_cart, 10m), "Eksik ödeme yapıldığında hata fırlatılmalı!");
    }
}
