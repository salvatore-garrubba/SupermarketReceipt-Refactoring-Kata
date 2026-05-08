using System;

namespace SupermarketReceipt.Offers;

public class ThreeForTwoOffer : IOfferStrategy
{
    public Discount? Calculate(Product product, decimal unitPrice, decimal quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity < 3)
        {
            return null;
        }

        var timesApplied = (int)(quantity / 3);
        var discountAmount = timesApplied * unitPrice;
        var discount = new Discount(product, "3 for 2", Math.Round(-discountAmount, 2));

        return discount;
    }
}