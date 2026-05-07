using System;
using System.Collections.Generic;
using Xunit;
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
            catalog.AddProduct(toothbrush, 0.99);
            var apples = new Product("apples", ProductUnit.Kilo);
            catalog.AddProduct(apples, 1.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(apples, 2.5);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(4.975, receipt.GetTotalPrice());
            Assert.Equal(new List<Discount>(), receipt.GetDiscounts());
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(apples, receiptItem.Product);
            Assert.Equal(1.99, receiptItem.Price);
            Assert.Equal(2.5 * 1.99, receiptItem.TotalPrice);
            Assert.Equal(2.5, receiptItem.Quantity);
        }

        [Fact]
        public void TenPercentDiscount_Applied()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 2.0);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(1.78, Math.Round(receipt.GetTotalPrice(), 2));
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(toothbrush, discount.Product);
            Assert.Equal("10% off", discount.Description);
            Assert.Equal(-0.20, Math.Round(discount.DiscountAmount, 2));
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(toothbrush, receiptItem.Product);
            Assert.Equal(0.99, receiptItem.Price);
            Assert.Equal(2.0 * 0.99, receiptItem.TotalPrice);
            Assert.Equal(2.0, receiptItem.Quantity);
        }

        [Fact]
        public void ThreeForTwo()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 1.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 3.0);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, toothbrush, 0.0); // argument not used

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(3.98, receipt.GetTotalPrice()); // 2 * 1.99
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(toothbrush, discount.Product);
            Assert.Equal("3 for 2", discount.Description);
            Assert.Equal(-1.99, Math.Round(discount.DiscountAmount, 2));
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(toothbrush, receiptItem.Product);
            Assert.Equal(1.99, receiptItem.Price);
            Assert.Equal(3.0 * 1.99, receiptItem.TotalPrice);
            Assert.Equal(3.0, receiptItem.Quantity);
        }

        [Fact]
        public void TwoForAmount()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.75);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 2.0);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, toothbrush, 1.50);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(1.50, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(toothbrush, discount.Product);
            Assert.Equal("2 for 1.50", discount.Description);
            Assert.Equal(- (2*0.75 - 1.50), discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(toothbrush, receiptItem.Product);
            Assert.Equal(0.75, receiptItem.Price);
            Assert.Equal(2.0 * 0.75, receiptItem.TotalPrice);
            Assert.Equal(2.0, receiptItem.Quantity);
        }

        [Fact]
        public void FiveForAmount()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 1.00);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 5.0);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, toothbrush, 4.00);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(4.00, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(toothbrush, discount.Product);
            Assert.Equal("5 for 4.00", discount.Description);
            Assert.Equal(- (5*1.00 - 4.00), discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(toothbrush, receiptItem.Product);
            Assert.Equal(1.00, receiptItem.Price);
            Assert.Equal(5.0 * 1.00, receiptItem.TotalPrice);
            Assert.Equal(5.0, receiptItem.Quantity);
        }
    }
}