namespace SupermarketReceipt.Offers;

public interface IOfferStrategy
{
    DiscountResult Calculate(Product product, decimal unitPrice, decimal quantity);
}