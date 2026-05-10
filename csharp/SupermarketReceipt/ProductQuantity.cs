using System;

namespace SupermarketReceipt;

public class ProductQuantity
{
    public ProductQuantity(Product product, decimal weight)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(weight);
        
        Product = product;
        Quantity = weight;
    }

    public Product Product { get; }
    public decimal Quantity { get; }
}