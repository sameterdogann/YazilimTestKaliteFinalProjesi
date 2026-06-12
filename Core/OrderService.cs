using System;

namespace ECommerceApp.Core;

public class OrderService
{
    public class Order
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public Order PlaceOrder(Cart cart, decimal paymentAmount)
    {
        decimal total = cart.CalculateTotal();

        // BUG 3: Minimum sipariş tutarı 100 olması gerekirken 10 olarak kontrol ediliyor.
        if (total < 10) 
        {
            throw new Exception("Minimum sipariş tutarı sağlanamadı!");
        }

        // BUG 4: Normalde eksik ödemede (payment < total) hata vermeli ama fazla ödemede hata veriyor.
        if (paymentAmount > total) 
        {
            throw new ArgumentException("Payment error!");
        }

        foreach (var item in cart.Items)
        {
            // BUG 5: Stok 0 ise sipariş verilmemeli. Ancak burada sadece 0'dan küçükse hata fırlatıyor (0 stoğu kabul ediyor).
            if (item.Stock < 0) 
            {
                throw new Exception("Out of stock!");
            }
            item.DecreaseStock(1);
        }

        cart.Status = "Completed";

        return new Order { IsSuccessful = true, Message = "Success" };
    }
}
