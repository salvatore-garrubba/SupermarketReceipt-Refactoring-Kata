using System;

namespace SupermarketReceipt.Offers;

public class PercentageOffer : OfferStrategy, IOfferStrategy
{
    private readonly decimal _percent;

    public PercentageOffer(decimal percent)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(percent);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(percent, 100);

        _percent = percent;
    }
    
    public DiscountResult Calculate(Product product, decimal unitPrice, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitPrice);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        
        var discountAmount = Math.Round(quantity * unitPrice * _percent / 100.0m, 2);
        if (discountAmount == 0)
        {
            return DiscountResult.None;
        }
        
        var discount = new Discount(product, PrintPercentage(_percent) + "% off", - discountAmount);

        return DiscountResult.Applied(discount);
    }
}