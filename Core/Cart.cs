using System.Collections.Generic;
using System.Linq;

namespace ECommerceApp.Core;

public class Cart
{
    public List<Product> Items { get; set; } = new();
    public string Status { get; set; } = "Active";

    public void AddProduct(Product product) => Items.Add(product);
    public void RemoveProduct(Product product) => Items.Remove(product);

    public decimal CalculateTotal()
    {
        decimal total = Items.Sum(x => x.Price);

        // BUG 1: İndirim %10 uygulanması gerekirken bilerek %50 (0.5m) uygulandı.
        if(total > 100)
        {
            total = total * 0.5m;
        }

        // BUG 2: Kargo ücreti (25) fiyata eklenmesi gerekirken çıkarılıyor (Mevcut hatan korundu).
        decimal shippingFee = 25.0m;
        total = total - shippingFee; 

        return total;
    }
}
