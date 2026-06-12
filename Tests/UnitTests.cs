using System;
using NUnit.Framework;
using ECommerceApp.Core;

namespace ECommerceApp.Tests.UnitTests;

[TestFixture]
public class UnitTests
{
    private Product _testProduct;
    private Cart _testCart;
    private OrderService _orderService;

    [SetUp]
    public void Setup()
    {
        // Test Isolation: Her test öncesi taze nesneler üretilir.
        _testProduct = new Product { Id = 1, Name = "Laptop", Price = 1000m, Stock = 5 };
        _testCart = new Cart();
        _orderService = new OrderService();
    }

    // --- WHITE BOX TESTS ---

    [Test]
    [Category("White Box")]
    public void Cart_CalculateTotal_ShippingFee_Validation_EP()
    {
        // EP (Equivalence Partitioning): İndirim sınırı altı (100 TL altı).
        // Beklenen: 50 TL ürün + 25 TL Kargo = 75 TL. 
        _testCart.AddProduct(new Product { Id = 2, Name = "Mouse", Price = 50m, Stock = 10 });
        
        decimal total = _testCart.CalculateTotal();
        
        // BUG TESPİTİ: Kargo ücreti eksi (-) olarak kodlandığı için bu test FAIL verecek (25 dönecek).
        Assert.That(total, Is.EqualTo(75.0m), "Kargo ücreti toplam tutara eklenmelidir.");
    }

    [Test]
    [Category("White Box")]
    public void Cart_CalculateTotal_Discount_Validation_BVA()
    {
        // BVA (Boundary Value Analysis): Sınırın hemen üstü (101 TL).
        // Beklenen: %10 indirim (101 * 0.9 = 90.9) + 25 = 115.9 TL.
        _testCart.AddProduct(new Product { Id = 3, Name = "Keyboard", Price = 101m, Stock = 10 });
        
        decimal total = _testCart.CalculateTotal();

        // BUG TESPİTİ: İndirim %50 uygulandığı ve kargo çıkarıldığı için bu test FAIL verecek.
        Assert.That(total, Is.EqualTo(115.9m), "100 TL üzerine %10 indirim uygulanmalıdır.");
    }

    [Test]
    [Category("White Box")]
    public void Cart_RemoveProduct_VerifyCount_EP()
    {
         _testCart.AddProduct(_testProduct);
         _testCart.RemoveProduct(_testProduct);

         Assert.That(_testCart.Items.Count, Is.EqualTo(0));
    }

    // --- BLACK BOX TESTS ---

    [Test]
    [Category("Black Box")]
    public void Cart_AddProduct_VerifyCount_EP()
    {
        _testCart.AddProduct(_testProduct);
        Assert.That(_testCart.Items.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("Black Box")]
    public void Product_DecreaseStock_VerifyValue_EP()
    {
        // Geçerli değerler (EP) ile stok düşme
        _testProduct.DecreaseStock(3);
        Assert.That(_testProduct.Stock, Is.EqualTo(2));
    }

    [Test]
    [Category("Black Box")]
    public void OrderService_Stock_Boundary_Check_BVA()
    {
        // BVA: Sınır değer kontrolü. Stok tam olarak 0.
        var product = new Product { Id = 4, Name = "Monitor", Price = 100m, Stock = 0 };
        _testCart.AddProduct(product);

        // BUG TESPİTİ: Stok 0 olduğunda Exception fırlatması gerekir. Kodda sadece <0 kontrolü olduğu için fırlatmayacak. FAIL verecek.
        Assert.Throws<Exception>(() => _orderService.PlaceOrder(_testCart, -50m), "Stok 0 olan ürün sipariş edilemez.");
    }

    // --- GRAY BOX TESTS ---

    [Test]
    [Category("Gray Box")]
    public void OrderService_ValidOrder_Status_Verify_EP()
    {
         _testCart.AddProduct(new Product { Id = 5, Name = "Cable", Price = 50m, Stock = 10 });
         
         // Buglı hesaplamayı simüle ederek ödeme hatasına düşmeden doğrudan statüyü kontrol ediyoruz.
         var order = _orderService.PlaceOrder(_testCart, 25m); 

         Assert.That(order.IsSuccessful, Is.True);
         Assert.That(_testCart.Status, Is.EqualTo("Completed"));
    }

    [Test]
    [Category("Gray Box")]
    public void OrderService_Payment_Boundary_Check_BVA()
    {
        _testCart.AddProduct(new Product { Id = 6, Name = "Desk", Price = 500m, Stock = 5 });
        decimal buggedTotal = _testCart.CalculateTotal(); // Buglar yüzünden hesaplanan yanlış tutar
        
        // BVA: İstenen tutarın 10 TL eksiğini ödüyoruz. ArgumentException fırlatmalı.
        // BUG TESPİTİ: Kod sadece "fazla" ödemelerde hata fırlattığı için bu test FAIL verecek.
        Assert.Throws<ArgumentException>(() => _orderService.PlaceOrder(_testCart, buggedTotal - 10m), "Eksik ödemede işlem reddedilmelidir.");
    }
}
