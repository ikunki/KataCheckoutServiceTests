using System;
using System.Collections.Generic;
using Xunit;
using KataCheckoutService.Models;
using KataCheckoutService.Interfaces;

namespace KataCheckoutTests
{
    public class KataCheckoutServiceTests
    {
        private static IKataCheckoutService KataService(IDictionary<string, decimal> prices, IDictionary<string, IEnumerable<Discount>> discounts)
        {
            return new KataCheckoutService.KataCheckoutService(prices, discounts);
        }

        #region Mocking data

        private static Dictionary<string, decimal> MockedPrices()
        {
            var prices = new Dictionary<string, decimal> {
                {"A", 10},
                {"B", 15},
                {"C", 40},
                {"D", 55},
            };
            return prices;
        }

        private static Dictionary<string, IEnumerable<Discount>> MockedDiscounts()
        {
            var costD2 = DiscountPriceD2();
            var discounts = new Dictionary<string, IEnumerable<Discount>> {
                {"B", new List<Discount> { new Discount(3, 40) }},
                {"D", new List<Discount> { new Discount(2, costD2) }},
            };
            return discounts;
        }

        private static decimal DiscountPriceD2()
        {
            var readOnlyPrices = MockedPrices();
            var service = KataService(readOnlyPrices, null);
            service.ScanItem("D", 2);
            var total = service.TotalCost();
            decimal discountPriceD2 = total * 0.75m; // 25% less for 2 D items
            return discountPriceD2;
        }

        #endregion
        #region ScanItem exceptions

        [Fact]
        public void ScanItem_notFoundException()
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);

            // Act & Assert
            Assert.Throws<KataCheckoutService.NotFoundException>(() => service.ScanItem("E", 1));
        }

        [Fact]
        public void ScanItem_qtyZeroOrLessException()
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.ScanItem("A", 0));
        }

        #endregion
        #region A items in basket

        [Fact]
        public void A_in_basket_total()
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);
            service.ScanItem("A", 1);
            service.ScanItem("A", 2);
            service.ScanItem("A", 3);

            // Act
            var total = service.TotalCost();

            // Assert
            Assert.Equal(60, total);
        }

        #endregion
        #region B items in basket

        [Fact]
        public void B_in_basket_total()
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);
            service.ScanItem("B", 1);
            service.ScanItem("B", 2);
            service.ScanItem("B", 1);
            service.ScanItem("B", 3);

            // Act
            var total = service.TotalCost();

            // Assert
            Assert.Equal(95, total);
        }

        #endregion
        #region C items in basket

        [Fact]
        public void C_in_basket_total()
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);
            service.ScanItem("C", 1);
            service.ScanItem("C", 2);
            service.ScanItem("C", 3);

            // Act
            var total = service.TotalCost();

            // Assert
            Assert.Equal(240, total);
        }

        #endregion
        #region D items in basket

        [Fact]
        public void D_in_basket_total()
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);
            service.ScanItem("D", 1);
            service.ScanItem("D", 1);
            service.ScanItem("D", 3);

            // Act
            var total = service.TotalCost();

            // Assert
            Assert.Equal(220.0m, total);
        }

        #endregion
        #region A, B, C & D items in basket

        [Fact]
        public void ABCD_in_basket_total()
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);
            service.ScanItem("A", 1);
            service.ScanItem("B", 3);
            service.ScanItem("C", 1);
            service.ScanItem("D", 2);

            // Act
            var total = service.TotalCost();

            // Assert
            Assert.Equal(172.5m, total);
        }

        #endregion
    }
}
