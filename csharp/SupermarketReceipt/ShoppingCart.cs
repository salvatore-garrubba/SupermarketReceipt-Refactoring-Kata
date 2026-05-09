using System.Collections.Generic;
using SupermarketReceipt.Catalogs;

namespace SupermarketReceipt;

public class ShoppingCart
{
    private readonly List<ProductQuantity> _items = new();
    private readonly Dictionary<Product, decimal> _productQuantities = new();

    public List<ProductQuantity> GetItems()
    {
        return new List<ProductQuantity>(_items);
    }

    public void AddItem(Product product)
    {
        AddItemQuantity(product, 1.0m);
    }
    
    public void AddItemQuantity(Product product, decimal quantity)
    {
        _items.Add(new ProductQuantity(product, quantity));
        if (_productQuantities.ContainsKey(product))
        {
            var newAmount = _productQuantities[product] + quantity;
            _productQuantities[product] = newAmount;
        }
        else
        {
            _productQuantities.Add(product, quantity);
        }
    }

    public void HandleOffers(Receipt receipt, Dictionary<Product, Offer> offers, ISupermarketCatalog catalog)
    {
        foreach (var p in _productQuantities.Keys)
        {
            var quantity = _productQuantities[p];
            if (!offers.TryGetValue(p, out var offer))
            {
                continue;
            }

            var unitPrice = catalog.GetUnitPrice(p);

            var discount = offer.CalculateDiscount(unitPrice, quantity);
            if (discount == null)
            {
                continue;
            }
            
            receipt.AddDiscount(discount);
        }
    }
}