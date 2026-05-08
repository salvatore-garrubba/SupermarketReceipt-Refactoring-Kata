using System;
using System.Collections.Generic;

namespace SupermarketReceipt;

public class Receipt
{
    private readonly List<Discount> _discounts = new();
    private readonly List<ReceiptItem> _items = new();

    public decimal GetTotalPrice()
    {
        var total = 0.0m;
        foreach (var item in _items) total += item.TotalPrice;
        foreach (var discount in _discounts) total += discount.DiscountAmount;
        return Math.Round(total, 2);
    }

    public void AddProduct(Product p, decimal quantity, decimal price, decimal totalPrice)
    {
        _items.Add(new ReceiptItem(p, quantity, price, totalPrice));
    }

    public List<ReceiptItem> GetItems()
    {
        return new List<ReceiptItem>(_items);
    }

    public void AddDiscount(Discount discount)
    {
        _discounts.Add(discount);
    }

    public List<Discount> GetDiscounts()
    {
        return _discounts;
    }
}