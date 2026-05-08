using Assert = Xunit.Assert;

namespace SupermarketReceipt.XUnit.Test
{
    public class SupermarketXUnitTest
    {
        [Fact]
        public void TenPercentDiscount()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99m);
            var apples = new Product("apples", ProductUnit.Kilo);
            catalog.AddProduct(apples, 1.99m);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(apples, 2.5m);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0m);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(4.98m, receipt.GetTotalPrice());
            Assert.Equal(new List<Discount>(), receipt.GetDiscounts());
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(apples, receiptItem.Product);
            Assert.Equal(1.99m, receiptItem.Price);
            Assert.Equal(2.5m * 1.99m, receiptItem.TotalPrice);
            Assert.Equal(2.5m, receiptItem.Quantity);
        }

        [Fact]
        public void TenPercentDiscount_Applied()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99m);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 2.0m);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0m);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(1.78m, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(toothbrush, discount.Product);
            Assert.Equal("10.00% off", discount.Description);
            Assert.Equal(-0.20m, discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(toothbrush, receiptItem.Product);
            Assert.Equal(0.99m, receiptItem.Price);
            Assert.Equal(2.0m * 0.99m, receiptItem.TotalPrice);
            Assert.Equal(2.0m, receiptItem.Quantity);
        }

        [Fact]
        public void ThreeForTwo()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 1.99m);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 3.0m);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, toothbrush, 0.0m);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(2 * 1.99m, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(toothbrush, discount.Product);
            Assert.Equal("3 for 2", discount.Description);
            Assert.Equal(-1.99m, discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(toothbrush, receiptItem.Product);
            Assert.Equal(1.99m, receiptItem.Price);
            Assert.Equal(3.0m * 1.99m, receiptItem.TotalPrice);
            Assert.Equal(3.0m, receiptItem.Quantity);
        }

        [Fact]
        public void TwoForAmount()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.75m);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 2.0m);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, toothbrush, 1.50m);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(1.50m, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(toothbrush, discount.Product);
            Assert.Equal("2 for 1.50", discount.Description);
            Assert.Equal(-(2m * 0.75m - 1.50m), discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(toothbrush, receiptItem.Product);
            Assert.Equal(0.75m, receiptItem.Price);
            Assert.Equal(2.0m * 0.75m, receiptItem.TotalPrice);
            Assert.Equal(2.0m, receiptItem.Quantity);
        }

        [Fact]
        public void FiveForAmount()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 1.00m);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 5.0m);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, toothbrush, 4.00m);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(4.00m, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(toothbrush, discount.Product);
            Assert.Equal("5 for 4.00", discount.Description);
            Assert.Equal(-(5m * 1.00m - 4.00m), discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(toothbrush, receiptItem.Product);
            Assert.Equal(1.00m, receiptItem.Price);
            Assert.Equal(5.0m * 1.00m, receiptItem.TotalPrice);
            Assert.Equal(5.0m, receiptItem.Quantity);
        }

        [Fact]
        public void EmptyShoppingCart()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var cart = new ShoppingCart();
            var teller = new Teller(catalog);
            
            // ACT
            Receipt receipt = teller.ChecksOutArticlesFrom(cart);
            
            // ASSERT
            Assert.Equal(0m, receipt.GetTotalPrice());
        }
    }
}