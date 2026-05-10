using SupermarketReceipt.Catalogs;
using Assert = Xunit.Assert;

namespace SupermarketReceipt.XUnit.Test
{
    public class SupermarketXUnitTest
    {
        private readonly ShoppingCart _cart;
        private readonly Teller _teller;
        private readonly Product _toothbrush;
        private const decimal ToothbrushPrice = 0.99m;
        private readonly Product _toothpaste;
        private const decimal ToothpastePrice = 1.79m;
        private readonly Product _apples;
        private const decimal ApplePrice = 1.99m;
        private readonly Product _rice;
        private const decimal RicePrice = 2.49m;
        private readonly Product _cherryTomatoes;
        private const decimal CherryTomatoesPrice = 0.69m;
        
        public SupermarketXUnitTest()
        {
            var catalog = new FakeCatalog();
            _teller = new Teller(catalog);
            _cart = new ShoppingCart();
            _cart = new ShoppingCart();

            _toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(_toothbrush, ToothbrushPrice);
            
            _apples = new Product("apples", ProductUnit.Kilo);
            catalog.AddProduct(_apples, ApplePrice);
            
            _rice = new Product("rice", ProductUnit.Each);
            catalog.AddProduct(_rice, RicePrice);
            
            _toothpaste = new Product("toothpaste", ProductUnit.Each);
            catalog.AddProduct(_toothpaste, ToothpastePrice);
            
            _cherryTomatoes = new Product("cherry tomato box", ProductUnit.Each);
            catalog.AddProduct(_cherryTomatoes, CherryTomatoesPrice);
        }
        
        [Fact]
        public void EmptyShoppingCart()
        {
            // ACT
            Receipt receipt = _teller.ChecksOutArticlesFrom(_cart);
            
            // ASSERT
            Assert.Equal(0m, receipt.GetTotalPrice());
        }

        [Fact]
        public void TenPercentDiscount()
        {
            // ARRANGE
            _cart.AddItemQuantity(_rice, 2);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, _rice, 10.0m);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);

            // ASSERT
            Assert.Equal(4.48m, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(_rice, discount.Product);
            Assert.Equal("10% off", discount.Description);
            Assert.Equal(-0.50m, discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(_rice, receiptItem.Product);
            Assert.Equal(RicePrice, receiptItem.Price);
            Assert.Equal(2 * RicePrice, receiptItem.TotalPrice);
            Assert.Equal(2, receiptItem.Quantity);
        }

        [Fact]
        public void ThreeForTwo()
        {
            // ARRANGE
            _cart.AddItemQuantity(_toothbrush, 3);
            _teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, _toothbrush, 2 * ToothbrushPrice);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);

            // ASSERT
            Assert.Equal(2 * ToothbrushPrice, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(_toothbrush, discount.Product);
            Assert.Equal("3 for 2", discount.Description);
            Assert.Equal(-ToothbrushPrice, discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(_toothbrush, receiptItem.Product);
            Assert.Equal(ToothbrushPrice, receiptItem.Price);
            Assert.Equal(3.0m * ToothbrushPrice, receiptItem.TotalPrice);
            Assert.Equal(3.0m, receiptItem.Quantity);
        }

        [Fact]
        public void TwoForAmount()
        {
            // ARRANGE
            _cart.AddItemQuantity(_toothbrush, 2.0m);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, _toothbrush, ToothbrushPrice);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);

            // ASSERT
            Assert.Equal(ToothbrushPrice, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(_toothbrush, discount.Product);
            Assert.Equal("2 for 0.99", discount.Description);
            Assert.Equal(-ToothbrushPrice, discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(_toothbrush, receiptItem.Product);
            Assert.Equal(ToothbrushPrice, receiptItem.Price);
            Assert.Equal(2 * ToothbrushPrice, receiptItem.TotalPrice);
            Assert.Equal(2, receiptItem.Quantity);
        }

        [Fact]
        public void FiveForAmount()
        {
            // ARRANGE
            _cart.AddItemQuantity(_toothpaste, 5.0m);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, _toothpaste, 7.49m);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);

            // ASSERT
            Assert.Equal(7.49m, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(_toothpaste, discount.Product);
            Assert.Equal("5 for 7.49", discount.Description);
            Assert.Equal(-(5 * ToothpastePrice - 7.49m), discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(_toothpaste, receiptItem.Product);
            Assert.Equal(ToothpastePrice, receiptItem.Price);
            Assert.Equal(5.0m * ToothpastePrice, receiptItem.TotalPrice);
            Assert.Equal(5.0m, receiptItem.Quantity);
        }

        [Fact]
        public void FourBoxesDiscount()
        {
            // ARRANGE
            _cart.AddItemQuantity(_cherryTomatoes, 4);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, _cherryTomatoes, 0.99m);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);

            // ASSERT
            Assert.Equal(2 * 0.99m, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(_cherryTomatoes, discount.Product);
            Assert.Equal("2 for 0.99", discount.Description);
            Assert.Equal(-(4 * CherryTomatoesPrice - 2 * 0.99m), discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(_cherryTomatoes, receiptItem.Product);
            Assert.Equal(CherryTomatoesPrice, receiptItem.Price);
            Assert.Equal(4 * CherryTomatoesPrice, receiptItem.TotalPrice);
            Assert.Equal(4, receiptItem.Quantity);
        }
        
        [Fact]
        public void FiveBoxesDiscount()
        {
            // ARRANGE
            _cart.AddItemQuantity(_cherryTomatoes, 5);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, _cherryTomatoes, 0.99m);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);

            // ASSERT
            Assert.Equal(2 * 0.99m + CherryTomatoesPrice, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(_cherryTomatoes, discount.Product);
            Assert.Equal("2 for 0.99", discount.Description);
            Assert.Equal(-(4 * CherryTomatoesPrice - 2 * 0.99m), discount.DiscountAmount);
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(_cherryTomatoes, receiptItem.Product);
            Assert.Equal(CherryTomatoesPrice, receiptItem.Price);
            Assert.Equal(5 * CherryTomatoesPrice, receiptItem.TotalPrice);
            Assert.Equal(5, receiptItem.Quantity);
        }
        
        [Fact]
        public void NoDiscount_When_DifferentProductIsOnDiscount()
        {
            // ARRANGE
            _cart.AddItemQuantity(_apples, 2.5m);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, _toothbrush, 10.0m);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);

            // ASSERT
            Assert.Equal(Math.Round(2.5m * ApplePrice, 2), receipt.GetTotalPrice());
            Assert.Equal(new List<Discount>(), receipt.GetDiscounts());
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(_apples, receiptItem.Product);
            Assert.Equal(ApplePrice, receiptItem.Price);
            Assert.Equal(2.5m * ApplePrice, receiptItem.TotalPrice);
            Assert.Equal(2.5m, receiptItem.Quantity);
        }

        [Fact]
        public void DiscountAndNoDiscount()
        {
            // ARRANGE
            _cart.AddItemQuantity(_apples, 2);
            _cart.AddItemQuantity(_rice, 5);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, _rice, 10);
            
            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            
            // ASSERT
            Assert.Equal(2 * ApplePrice + 10, receipt.GetTotalPrice());
            Assert.Single(receipt.GetDiscounts());
            
            Assert.Equal(2, receipt.GetItems().Count);
            var appleItem = receipt.GetItems()[0];
            Assert.Equal(_apples, appleItem.Product);
            Assert.Equal(ApplePrice, appleItem.Price);
            Assert.Equal(2 * ApplePrice, appleItem.TotalPrice);
            Assert.Equal(2, appleItem.Quantity);
            
            var riceItem = receipt.GetItems()[1];
            Assert.Equal(_rice, riceItem.Product);
            Assert.Equal(RicePrice, riceItem.Price);
            Assert.Equal(5 * RicePrice, riceItem.TotalPrice);
            Assert.Equal(5, riceItem.Quantity);

            var discount = receipt.GetDiscounts()[0];
            Assert.Equal(_rice, discount.Product);
            Assert.Equal("5 for 10.00", discount.Description);
            Assert.Equal(-(5 * RicePrice - 10), discount.DiscountAmount);
        }

        [Fact]
        public void MultipleDiscounts()
        {
            // ARRANGE
            _cart.AddItemQuantity(_apples, 2);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, _apples, 10);
            _cart.AddItemQuantity(_rice, 5);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, _rice, 10);
            
            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            
            // ASSERT
            Assert.Equal(3.58m + 10, receipt.GetTotalPrice());
            Assert.Equal(2, receipt.GetItems().Count);
            
            var appleItem = receipt.GetItems()[0];
            Assert.Equal(_apples, appleItem.Product);
            Assert.Equal(ApplePrice, appleItem.Price);
            Assert.Equal(2 * ApplePrice, appleItem.TotalPrice);
            Assert.Equal(2, appleItem.Quantity);
            
            var riceItem = receipt.GetItems()[1];
            Assert.Equal(_rice, riceItem.Product);
            Assert.Equal(RicePrice, riceItem.Price);
            Assert.Equal(5 * RicePrice, riceItem.TotalPrice);
            Assert.Equal(5, riceItem.Quantity);

            Assert.Equal(2, receipt.GetDiscounts().Count);
            var appleDiscount = receipt.GetDiscounts()[0];
            Assert.Equal(_apples, appleDiscount.Product);
            Assert.Equal("10% off", appleDiscount.Description);
            Assert.Equal(-0.4m, appleDiscount.DiscountAmount);
            
            var riceDiscount = receipt.GetDiscounts()[1];
            Assert.Equal(_rice, riceDiscount.Product);
            Assert.Equal("5 for 10.00", riceDiscount.Description);
            Assert.Equal(-(5 * RicePrice - 10), riceDiscount.DiscountAmount);
        }
    }
}