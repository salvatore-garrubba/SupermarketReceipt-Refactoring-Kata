using System;
using System.Collections.Generic;

namespace SupermarketReceipt;

public class Product
{
    public Product(string name, ProductUnit unit)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        Unit = unit;
    }

    public string Name { get; }
    public ProductUnit Unit { get; }

    public override bool Equals(object? obj)
    {
        var product = obj as Product;
        return product != null &&
               string.Equals(Name, product.Name, StringComparison.OrdinalIgnoreCase) &&
               Unit == product.Unit;
    }

    public override int GetHashCode()
    {
        var hashCode = -1996304355;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name?.ToUpperInvariant() ?? "");
        hashCode = hashCode * -1521134295 + Unit.GetHashCode();
        return hashCode;
    }
}