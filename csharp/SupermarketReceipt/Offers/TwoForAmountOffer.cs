using System;

namespace SupermarketReceipt.Offers;

public class TwoForAmountOffer : OfferStrategy, IOfferStrategy
{
    private readonly decimal _amount;

    public TwoForAmountOffer(decimal amount)
    {
        _amount = amount;
    }
    
    public Discount? Calculate(Product product, decimal unitPrice, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity < 2)
        {
            return null;
        }
        
        var timesApplied = (int) quantity / 2;
        var reminder = quantity % 2;
        var discountAmount = quantity * unitPrice - timesApplied * _amount - reminder * unitPrice;
        var discount = new Discount(product, "2 for " + PrintPrice(_amount), Math.Round(-discountAmount, 2));
        
        return discount;
    }
}