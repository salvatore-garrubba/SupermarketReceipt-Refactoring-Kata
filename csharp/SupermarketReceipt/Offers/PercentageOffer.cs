using System;

namespace SupermarketReceipt.Offers;

public class PercentageOffer : OfferStrategy, IOfferStrategy
{
    private readonly decimal _percent;

    public PercentageOffer(decimal percent)
    {
        _percent = percent;
    }
    
    public Discount? Calculate(Product product, decimal unitPrice, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        var discountAmount = Math.Round(quantity * unitPrice * _percent / 100.0m, 2);
        var discount = new Discount(product, PrintPrice(_percent) + "% off", discountAmount);

        return discount;
    }
}