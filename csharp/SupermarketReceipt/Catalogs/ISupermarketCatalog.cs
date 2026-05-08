namespace SupermarketReceipt.Catalogs;

public interface ISupermarketCatalog
{
    void AddProduct(Product product, decimal price);

    decimal GetUnitPrice(Product product);
}