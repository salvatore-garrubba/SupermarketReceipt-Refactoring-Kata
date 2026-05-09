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
    
    public Discount? Calculate(Product product, decimal unitPrice, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitPrice);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        
        var discountAmount = quantity * unitPrice * _percent / 100.0m;
        
        var discount = new Discount(product, PrintPercentage(_percent) + "% off", Math.Round(-discountAmount, 2));

        return discount;
    }
}