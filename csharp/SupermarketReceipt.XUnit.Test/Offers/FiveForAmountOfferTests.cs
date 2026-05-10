using SupermarketReceipt.Offers;

namespace SupermarketReceipt.XUnit.Test.Offers;

public class FiveForAmountOfferTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_ShouldThrow_WhenAmountIsInvalid(decimal amount)
    {
        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new FiveForAmountOffer(amount));
    }
    
    [Fact]
    public void Calculate_ShouldThrow_WhenProductIsNull()
    {
        //  Arrange 
        var sut = new FiveForAmountOffer(4);
        const int unitPrice = 1;
        const int quantity = 5;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.Calculate(null!, unitPrice, quantity));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_ShouldThrow_WhenUnitPriceInvalid(decimal unitPrice)
    {
        // Arrange
        var sut = new FiveForAmountOffer(4);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal quantity = 5m;

        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Calculate(product, unitPrice, quantity));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_ShouldThrow_WhenQuantityInvalid(decimal quantity)
    {
        // Arrange
        var sut = new FiveForAmountOffer(4);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 5m;

        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Calculate(product, unitPrice, quantity));
    }
    
    [Fact]
    public void Calculate_ShouldReturnNull_WhenOfferMoreExpensiveThanRegularPrice()
    {
        // Arrange
        var sut = new FiveForAmountOffer(10m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 5m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.False(discountResult.HasDiscount);
    }
    
    [Fact]
    public void Calculate_ShouldReturnDiscount_WhenProductIsEligible()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4);
        var product = new Product("Test Product", ProductUnit.Each);
        const int unitPrice = 1;
        const int quantity = 5;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.True(discountResult.HasDiscount);
        Assert.Equal(product, discountResult.Value.Product);
        Assert.Equal("5 for 4.00", discountResult.Value.Description);
        Assert.Equal(-1, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WhenProductIsNotEligible()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4);
        var product = new Product("Test Product", ProductUnit.Each);
        const int unitPrice = 1;
        const int quantity = 4;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.False(discountResult.HasDiscount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalQuantity()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Apples", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 5.5m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-1.00m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithDecimalQuantityBelowThreshold()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Apples", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 4.99m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.False(discountResult.HasDiscount);
    }

    [Fact]
    public void Calculate_ShouldHandleMultipleSets_WithDecimalQuantity()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Apples", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 10.3m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-2.00m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalUnitPrice()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 0.99m;
        const decimal quantity = 5m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-0.95m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithVerySmallUnitPrice()
    {
        // Arrange
        var sut = new FiveForAmountOffer(0.04m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 0.01m;
        const decimal quantity = 5m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-0.01m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithLargeUnitPrice()
    {
        // Arrange
        var sut = new FiveForAmountOffer(400m);
        var product = new Product("Premium Item", ProductUnit.Each);
        const decimal unitPrice = 99.99m;
        const decimal quantity = 5m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-99.95m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalOfferAmount()
    {
        // Arrange
        var sut = new FiveForAmountOffer(3.49m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 0.99m;
        const decimal quantity = 5m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal("5 for 3.49", discountResult.Value.Description);
        Assert.Equal(-1.46m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithExactlyTenItems()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 10m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-2.00m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithMultipleSetsAndRemainder()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 13m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-2.00m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithExactlyFourItems()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 4m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.False(discountResult.HasDiscount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithExactlyFiveItems()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 5m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-1.00m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithZeroQuantity()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 0m;

        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Calculate(product, unitPrice, quantity));
    }
    
    [Fact]
    public void Calculate_ShouldReturnDiscount_WithRealWorldPricing()
    {
        // Arrange
        var sut = new FiveForAmountOffer(3.99m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 0.99m;
        const decimal quantity = 5m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal("5 for 3.99", discountResult.Value.Description);
        Assert.Equal(-0.96m, discountResult.Value.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithBulkPurchase()
    {
        // Arrange
        var sut = new FiveForAmountOffer(4.00m);
        var product = new Product("Bulk Item", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 26m;

        // Act
        var discountResult = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Equal(-5.00m, discountResult.Value.DiscountAmount);
    }
}