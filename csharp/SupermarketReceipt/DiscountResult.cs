using System;

namespace SupermarketReceipt;

public readonly struct DiscountResult
{
    private readonly Discount? _discount;

    private DiscountResult(Discount? discount)
    {
        _discount = discount;
    }
    
    public static DiscountResult None => new(null);
    public static DiscountResult Applied(Discount discount) => new(discount);
    public bool HasDiscount => _discount != null;
    public Discount Value =>  _discount ?? throw new InvalidOperationException("No discount applied");
}