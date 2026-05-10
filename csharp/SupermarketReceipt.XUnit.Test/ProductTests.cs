namespace SupermarketReceipt.XUnit.Test;

public class ProductTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmptyString()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Product(string.Empty, ProductUnit.Each));
    }
    
    [Fact]
    public void Constructor_ShouldInitializeNameAndUnit()
    {
        // Arrange
        const string productName = "Apple";
        const ProductUnit unit = ProductUnit.Each;

        // Act
        var product = new Product(productName, unit);

        // Assert
        Assert.Equal(productName, product.Name);
        Assert.Equal(unit, product.Unit);
    }

    [Fact]
    public void Constructor_ShouldInitializeWithKiloUnit()
    {
        // Arrange
        const string productName = "Bananas";
        const ProductUnit unit = ProductUnit.Kilo;

        // Act
        var product = new Product(productName, unit);

        // Assert
        Assert.Equal(productName, product.Name);
        Assert.Equal(unit, product.Unit);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenNamesAndUnitsMatch()
    {
        // Arrange
        var product1 = new Product("Apple", ProductUnit.Each);
        var product2 = new Product("Apple", ProductUnit.Each);

        // Act
        var result = product1.Equals(product2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenNamesAreDifferent()
    {
        // Arrange
        var product1 = new Product("Apple", ProductUnit.Each);
        var product2 = new Product("Orange", ProductUnit.Each);

        // Act
        var result = product1.Equals(product2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenUnitsAreDifferent()
    {
        // Arrange
        var product1 = new Product("Apple", ProductUnit.Each);
        var product2 = new Product("Apple", ProductUnit.Kilo);

        // Act
        var result = product1.Equals(product2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenNamesAreSameButDifferentCase()
    {
        // Arrange
        var product1 = new Product("Apple", ProductUnit.Each);
        var product2 = new Product("apple", ProductUnit.Each);

        // Act
        var result = product1.Equals(product2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenNamesAreDifferentCase_AllUppercase()
    {
        // Arrange
        var product1 = new Product("APPLE", ProductUnit.Each);
        var product2 = new Product("apple", ProductUnit.Each);

        // Act
        var result = product1.Equals(product2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenNamesAreDifferentCase_MixedCase()
    {
        // Arrange
        var product1 = new Product("ApPlE", ProductUnit.Each);
        var product2 = new Product("aPpLe", ProductUnit.Each);

        // Act
        var result = product1.Equals(product2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparingToNull()
    {
        // Arrange
        var product = new Product("Apple", ProductUnit.Each);

        // Act
        var result = product.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparingToNonProductObject()
    {
        // Arrange
        var product = new Product("Apple", ProductUnit.Each);
        var nonProduct = "Apple";

        // Act
        var result = product.Equals(nonProduct);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenProductEqualsItself()
    {
        // Arrange
        var product = new Product("Apple", ProductUnit.Each);

        // Act
        var result = product.Equals(product);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetHashCode_ShouldBeSame_ForProductsWithDifferentCase()
    {
        // Arrange
        var product1 = new Product("Apple", ProductUnit.Each);
        var product2 = new Product("apple", ProductUnit.Each);

        // Act
        var hash1 = product1.GetHashCode();
        var hash2 = product2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_ShouldBeDifferent_ForDifferentProducts()
    {
        // Arrange
        var product1 = new Product("Apple", ProductUnit.Each);
        var product2 = new Product("Orange", ProductUnit.Each);

        // Act
        var hash1 = product1.GetHashCode();
        var hash2 = product2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_ShouldBeDifferent_ForDifferentUnits()
    {
        // Arrange
        var product1 = new Product("Apple", ProductUnit.Each);
        var product2 = new Product("Apple", ProductUnit.Kilo);

        // Act
        var hash1 = product1.GetHashCode();
        var hash2 = product2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }
}