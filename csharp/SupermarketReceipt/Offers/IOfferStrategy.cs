namespace SupermarketReceipt.Offers;

public interface IOfferStrategy
{
    Discount? Calculate(Product product, decimal unitPrice, decimal quantity);
}