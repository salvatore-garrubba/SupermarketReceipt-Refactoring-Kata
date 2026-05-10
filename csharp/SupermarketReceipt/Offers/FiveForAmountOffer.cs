using System;

namespace SupermarketReceipt.Offers;

public class FiveForAmountOffer : OfferStrategy, IOfferStrategy
{
    private readonly decimal _amount;

    public FiveForAmountOffer(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        _amount = amount;
    }
    
    public DiscountResult Calculate(Product product, decimal unitPrice, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitPrice);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        if (quantity < 5)
        {
            return DiscountResult.None;
        }
        
        var timesApplied = (int) quantity / 5;
        var reminder = quantity % 5;
        var discountAmount = quantity * unitPrice - timesApplied * _amount - reminder * unitPrice;
        if (discountAmount <= 0)
        {
            return DiscountResult.None;
        }
            
        var discount = new Discount(product, "5 for " + PrintPrice(_amount), Math.Round(-discountAmount, 2));
        
        return DiscountResult.Applied(discount);
    }
}