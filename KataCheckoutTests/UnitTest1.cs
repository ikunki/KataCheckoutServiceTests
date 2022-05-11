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
            return new Dictionary<string, decimal>(prices);
        }

        private static Dictionary<string, IEnumerable<Discount>> MockedDiscounts()
        {
            var discounts = new Dictionary<string, IEnumerable<Discount>> {
                {"B", new List<Discount> {new Discount(3, 40)}},
                {"D", new List<Discount> {new Discount(2, DiscountPriceD2())}},
            };
            return new Dictionary<string, IEnumerable<Discount>>(discounts);
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
    }
}
