using System;
using System.Collections.Generic;

namespace SupermarketReceipt.Catalogs;

public class FakeCatalog : ISupermarketCatalog
{
    private readonly Dictionary<string, decimal> _prices = new();
    private readonly Dictionary<string, Product> _products = new();

    public void AddProduct(Product product, decimal price)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null");
        }
            
        if (!_products.TryAdd(product.Name, product))
        {
            throw new InvalidOperationException($"Product {product.Name} already exists in the catalog for products.");
        }

        if (!_prices.TryAdd(product.Name, price))
        {
            throw new InvalidOperationException($"Product {product.Name} already exists in the catalog for prices.");
        }
    }

    public decimal GetUnitPrice(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null");
        }

        if (!_prices.TryGetValue(product.Name, out var price))
        {
            throw new InvalidOperationException($"Product {product.Name} does not exist in the catalog.");
        }
            
        return price;
    }
}