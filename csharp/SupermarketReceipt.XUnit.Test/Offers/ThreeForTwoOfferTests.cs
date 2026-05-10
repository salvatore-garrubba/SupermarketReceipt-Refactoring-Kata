using SupermarketReceipt.Offers;

namespace SupermarketReceipt.XUnit.Test.Offers;

public class ThreeForTwoOfferTests
{
    [Fact]
    public void Calculate_ShouldThrow_WhenProductIsNull()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        const decimal unitPrice = 1m;
        const decimal quantity = 3m;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => offer.Calculate(null!, unitPrice, quantity));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_ShouldThrow_WhenUnitPriceInvalid(decimal unitPrice)
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal quantity = 3m;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => offer.Calculate(product, unitPrice, quantity));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_ShouldThrow_WhenQuantityInvalid(decimal quantity)
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1m;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => offer.Calculate(product, unitPrice, quantity));
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WhenQuantityLessThanThree()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WhenProductIsEligible()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1m;
        const decimal quantity = 3m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(product, discount.Product);
        Assert.Equal("3 for 2", discount.Description);
        Assert.Equal(-1.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalQuantity()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 3.5m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-1.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithDecimalQuantityBelowThreshold()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 2.9m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }

    [Fact]
    public void Calculate_ShouldHandleMultipleSets_WithDecimalQuantity()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Kilo);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 6.7m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-2.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithDecimalUnitPrice()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 0.99m;
        const decimal quantity = 3m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.99m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithVerySmallUnitPrice()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Cheap Item", ProductUnit.Each);
        const decimal unitPrice = 0.01m;
        const decimal quantity = 3m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-0.01m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithLargeUnitPrice()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Premium Item", ProductUnit.Each);
        const decimal unitPrice = 99.99m;
        const decimal quantity = 3m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-99.99m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithExactlySixItems()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 6m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-2.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithMultipleSetsAndRemainder()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 7m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-2.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithNineItems()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 9m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-3.00m, discount.DiscountAmount);
    }

    [Fact]
    public void Calculate_ShouldReturnNull_WithExactlyTwoItems()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 2m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.Null(discount);
    }

    [Fact]
    public void Calculate_ShouldReturnDiscount_WithExactlyThreeItems()
    {
        // Arrange
        var offer = new ThreeForTwoOffer();
        var product = new Product("Test Product", ProductUnit.Each);
        const decimal unitPrice = 1.00m;
        const decimal quantity = 3m;

        // Act
        var discount = offer.Calculate(product, unitPrice, quantity);

        // Assert
        Assert.NotNull(discount);
        Assert.Equal(-1.00m, discount.DiscountAmount);
    }
}