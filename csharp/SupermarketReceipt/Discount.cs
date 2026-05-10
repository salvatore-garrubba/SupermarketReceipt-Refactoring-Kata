using System;

namespace SupermarketReceipt;

public class Discount
{
    public Discount(Product product, string description, decimal discountAmount)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfZero(discountAmount);
        
        Product = product;
        Description = description;
        DiscountAmount = discountAmount;
    }

    public string Description { get; }
    public decimal DiscountAmount { get; }
    public Product Product { get; }
}