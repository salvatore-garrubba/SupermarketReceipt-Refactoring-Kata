using System;

namespace SupermarketReceipt;

public class ProductQuantity
{
    public ProductQuantity(Product product, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        
        Product = product;
        Quantity = quantity;
    }

    public Product Product { get; }
    public decimal Quantity { get; }
}