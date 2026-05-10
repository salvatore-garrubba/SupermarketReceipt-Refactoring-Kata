using System;

namespace SupermarketReceipt.Offers;

public class TwoForAmountOffer : OfferStrategy, IOfferStrategy
{
    private readonly decimal _amount;

    public TwoForAmountOffer(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        _amount = amount;
    }
    
    public DiscountResult Calculate(Product product, decimal unitPrice, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitPrice);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        if (quantity < 2)
        {
            return DiscountResult.None;
        }
        
        var timesApplied = (int) quantity / 2;
        var reminder = quantity % 2;
        var discountAmount = quantity * unitPrice - timesApplied * _amount - reminder * unitPrice;
        if (discountAmount <= 0)
        {
            return DiscountResult.None;
        }
        
        var discount = new Discount(product, "2 for " + PrintPrice(_amount), Math.Round(-discountAmount, 2));
        
        return DiscountResult.Applied(discount);
    }
}