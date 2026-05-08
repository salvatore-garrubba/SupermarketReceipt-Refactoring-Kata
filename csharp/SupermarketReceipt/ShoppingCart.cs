using System;
using System.Collections.Generic;
using System.Globalization;
using SupermarketReceipt.Catalogs;

namespace SupermarketReceipt;

public class ShoppingCart
{
    private readonly List<ProductQuantity> _items = new();
    private readonly Dictionary<Product, decimal> _productQuantities = new();
    private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");

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
            var quantityAsInt = (int) quantity;
            if (!offers.TryGetValue(p, out var offer))
            {
                continue;
            }

            var unitPrice = catalog.GetUnitPrice(p);
            Discount discount = null;
            var x = 1;
            if (offer.OfferType == SpecialOfferType.ThreeForTwo)
            {
                x = 3;
            }
            else if (offer.OfferType == SpecialOfferType.TwoForAmount)
            {
                x = 2;
                if (quantityAsInt >= 2)
                {
                    var total = offer.Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
                    var discountN = unitPrice * quantity - total;
                    discount = new Discount(p, "2 for " + PrintPrice(offer.Argument), Math.Round(-discountN, 2));
                }
            }

            if (offer.OfferType == SpecialOfferType.FiveForAmount) x = 5;
            var numberOfXs = quantityAsInt / x;
            if (offer.OfferType == SpecialOfferType.ThreeForTwo && quantityAsInt > 2)
            {
                var discountAmount = quantity * unitPrice - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
                discount = new Discount(p, "3 for 2", Math.Round(-discountAmount, 2));
            }

            if (offer.OfferType == SpecialOfferType.TenPercentDiscount) discount = new Discount(p, PrintPrice(offer.Argument) + "% off", Math.Round(-quantity * unitPrice * offer.Argument / 100.0m, 2));
            if (offer.OfferType == SpecialOfferType.FiveForAmount && quantityAsInt >= 5)
            {
                var discountTotal = unitPrice * quantity - (offer.Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
                discount = new Discount(p, x + " for " + PrintPrice(offer.Argument), Math.Round(-discountTotal, 2));
            }

            if (discount != null)
            {
                receipt.AddDiscount(discount);
            }
        }
    }
        
    private static string PrintPrice(decimal price)
    {
        return price.ToString("N2", Culture);
    }
}