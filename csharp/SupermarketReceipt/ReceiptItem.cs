using System;

namespace SupermarketReceipt;

public class ReceiptItem
{
    public ReceiptItem(Product p, decimal quantity, decimal price, decimal totalPrice)
    {
        ArgumentNullException.ThrowIfNull(p);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalPrice);
        
        Product = p;
        Quantity = quantity;
        Price = price;
        TotalPrice = totalPrice;
    }

    public Product Product { get; }
    public decimal Price { get; }
    public decimal TotalPrice { get; }
    public decimal Quantity { get; }
}