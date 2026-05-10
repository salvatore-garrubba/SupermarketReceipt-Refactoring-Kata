using SupermarketReceipt.Offers;

namespace SupermarketReceipt.XUnit.Test.Offers;

public class TwoForAmountOfferTests
{
    [Fact]
    public void Calculate_ShouldThrow_WhenProductIsNull()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        const decimal unitPrice = 1m;
        const decimal quantity = 2m;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => offer.Calculate(null!, unitPrice, quantity));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_ShouldThrow_WhenUnitPriceInvalid(decimal unitPrice)
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal quantity = 2m;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => offer.Calculate(product, unitPrice, quantity));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_ShouldThrow_WhenQuantityInvalid(decimal quantity)
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1m;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => offer.Calculate(product, unitPrice, quantity));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_ShouldThrow_WhenAmountInvalid(decimal amount)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new TwoForAmountOffer(amount));
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WhenQuantityLessThanTwo()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1m;
        const decimal quantity = 1m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WhenProductIsEligible()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(product, discount.Product);
        Assert.Equal("2 for 1.50", discount.Description);
        Assert.Equal(-0.50m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalQuantity()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 2.5m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.50m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithDecimalQuantityBelowThreshold()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 1.9m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }

    [Fact]
    public void Calculate_ShouldHandleMultipleSets_WithDecimalQuantity()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 4.3m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-1.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalUnitPrice()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 0.99m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.48m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithVerySmallUnitPrice()
    {
        // Arrange
        var offer = new TwoForAmountOffer(0.05m);
        var product = new Product("Cheap Item", ProductUnit.Each);
        const decimal unitPrice = 0.01m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithLargeUnitPrice()
    {
        // Arrange
        var offer = new TwoForAmountOffer(150m);
        var product = new Product("Premium Item", ProductUnit.Each);
        const decimal unitPrice = 99.99m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-49.98m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalOfferAmount()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.49m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal("2 for 1.49", discount.Description);
        Assert.Equal(-0.51m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithExactlyFourItems()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 4m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-1.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithMultipleSetsAndRemainder()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 5m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-1.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithSixItems()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 6m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-1.50m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithExactlyOneItem()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 1m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithExactlyTwoItems()
    {
        // Arrange
        var offer = new TwoForAmountOffer(1.50m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.50m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithOfferAmountGreaterThanRegularPrice()
    {
        // Arrange
        var offer = new TwoForAmountOffer(3.00m);
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }
}