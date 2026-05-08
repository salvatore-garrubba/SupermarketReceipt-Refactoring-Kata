namespace SupermarketReceipt;

public class ProductQuantity
{
    public ProductQuantity(Product product, decimal weight)
    {
        Product = product;
        Quantity = weight;
    }

    public Product Product { get; }
    public decimal Quantity { get; }
}