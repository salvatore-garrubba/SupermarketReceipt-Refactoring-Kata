using SupermarketReceipt.Offers;

namespace SupermarketReceipt.XUnit.Test.Offers;

public class PercentageOfferTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100)]
    [InlineData(100.01)]
    public void Calculate_ShouldThrow_WhenPercentIsInvalid(decimal percent)
    {
        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new PercentageOffer(percent));
    }
    
    [Fact]
    public void Calculate_ShouldThrow_WhenProductIsNull()
    {
        //  Arrange 
        var sut = new PercentageOffer(10);
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
        var sut = new PercentageOffer(10);
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
        var sut = new PercentageOffer(10);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 5m;

        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Calculate(product, unitPrice, quantity));
    }
    
    [Fact]
    public void Calculate_ShouldReturnDiscount_WhenProductIsEligible()
    {
        // Arrange
        var sut = new PercentageOffer(10);
        var product = new Product("Test Product", ProductUnit.Each);
        const int unitPrice = 1;
        const int quantity = 5;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(product, discount.Product);
        Assert.Equal("10% off", discount.Description);
        Assert.Equal(-0.5m, discount.DiscountAmount);
    }
    
    [Fact]
    public void Calculate_ShouldReturnCorrectPercentageDescription_WhenPercentageIsDecimal()
    {
        // Arrange
        var sut = new PercentageOffer(10.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const int unitPrice = 1;
        const int quantity = 5;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal("10.50% off", discount.Description);
    }
    
    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalQuantity()
    {
        // Arrange
        var sut = new PercentageOffer(10m);
        var product = new Product("Test Product", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 2.5m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.25m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithVerySmallDecimalQuantity()
    {
        // Arrange
        var sut = new PercentageOffer(5m);
        var product = new Product("Test product", ProductUnit.Kilo);
        const decimal unitPrice = 10.00m;
        const decimal quantity = 0.1m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.05m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithLargeDecimalQuantity()
    {
        // Arrange
        var sut = new PercentageOffer(15m);
        var product = new Product("Bulk Item", ProductUnit.Kilo);
        const decimal unitPrice = 2.00m;
        const decimal quantity = 100.5m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-30.15m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalUnitPrice()
    {
        // Arrange
        var sut = new PercentageOffer(20m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 0.99m;
        const decimal quantity = 5m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.99m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithVerySmallUnitPrice()
    {
        // Arrange
        var sut = new PercentageOffer(10m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 0.01m;
        const decimal quantity = 10m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.01m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithLargeUnitPrice()
    {
        // Arrange
        var sut = new PercentageOffer(5m);
        var product = new Product("Premium Item", ProductUnit.Each);
        const decimal unitPrice = 999.99m;
        const decimal quantity = 1m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-50.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithVerySmallPercentage()
    {
        // Arrange
        var sut = new PercentageOffer(0.01m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 100m;
        const decimal quantity = 1m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal("0.01% off", discount.Description);
        Assert.Equal(-0.01m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithHighPercentage()
    {
        // Arrange
        var sut = new PercentageOffer(99.99m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 10m;
        const decimal quantity = 1m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal("99.99% off", discount.Description);
        Assert.Equal(-10.00m, discount.DiscountAmount);
    }
    
    [Fact]
    public void Calculate_ShouldReturnNoDiscount_WithMinimumValidPercentage()
    {
        // Arrange
        var sut = new PercentageOffer(0.01m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1m;
        const decimal quantity = 1m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithMaximumValidPercentage()
    {
        // Arrange - Just below 100
        var sut = new PercentageOffer(99.99m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1m;
        const decimal quantity = 1m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-1.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithVeryLargeQuantity()
    {
        // Arrange
        var sut = new PercentageOffer(10m);
        var product = new Product("Bulk Item", ProductUnit.Each);
        const decimal unitPrice = 1m;
        const decimal quantity = 10000m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-1000.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithRealWorldPricing()
    {
        // Arrange
        var sut = new PercentageOffer(20m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 9.99m;
        const decimal quantity = 3m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal("20% off", discount.Description);
        Assert.Equal(-5.99m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithBulkPurchase()
    {
        // Arrange
        var sut = new PercentageOffer(5m);
        var product = new Product("Bulk Item", ProductUnit.Kilo);
        const decimal unitPrice = 2.50m;
        const decimal quantity = 100m;

        // Act
        var discount = sut.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-12.50m, discount.DiscountAmount);
    }
}