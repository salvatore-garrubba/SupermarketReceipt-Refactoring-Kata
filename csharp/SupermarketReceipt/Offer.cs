using System;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt;

public class Offer
{
    private readonly Product _product;
    private readonly IOfferStrategy _offerStrategy;
    
    public SpecialOfferType OfferType { get; }
    public decimal Amount { get; }

    public Offer(SpecialOfferType offerType, Product product, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        
        OfferType = offerType;
        Amount = amount;
        _product = product;

        _offerStrategy = offerType switch
        {
            SpecialOfferType.ThreeForTwo => new ThreeForTwoOffer(),
            SpecialOfferType.TenPercentDiscount => new PercentageOffer(10),
            SpecialOfferType.TwoForAmount => new TwoForAmountOffer(amount),
            SpecialOfferType.FiveForAmount => new FiveForAmountOffer(amount),
            _ => throw new ArgumentException("Invalid offer type")
        };
    }

    public Discount? CalculateDiscount(decimal unitPrice, decimal quantity)
    {
        return _offerStrategy.Calculate(_product, unitPrice, quantity);
    }
}