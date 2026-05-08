using System;

namespace SupermarketReceipt.Offers;

public class FiveForAmountOffer : OfferStrategy, IOfferStrategy
{
    private readonly decimal _amount;

    public FiveForAmountOffer(decimal amount)
    {
        _amount = amount;
    }
    
    public Discount? Calculate(Product product, decimal unitPrice, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity < 5)
        {
            return null;
        }
        
        var timesApplied = (int) quantity / 5;
        var reminder = quantity % 5;
        var discountAmount = quantity * unitPrice - timesApplied * _amount - reminder * unitPrice;
        var discount = new Discount(product, "5 for " + PrintPrice(_amount), Math.Round(-discountAmount, 2));
        
        return discount;
    }
}