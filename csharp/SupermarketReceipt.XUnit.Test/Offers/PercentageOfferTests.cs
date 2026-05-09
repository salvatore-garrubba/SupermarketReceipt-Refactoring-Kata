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
}