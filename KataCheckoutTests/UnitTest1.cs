using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;
using KataCheckoutService.Models;
using KataCheckoutService.Interfaces;

namespace KataCheckoutTests
{
    public class KataCheckoutServiceTests
    {
        private static IKataCheckoutService KataService(IDictionary<string, decimal> prices, IDictionary<string, IEnumerable<Discount>> discounts)
        {
            return new KataCheckoutService(prices, discounts);
        }

        #region ScanItem()

        [Fact]
        public void ScanItem_notFoundException()
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);

            // Act & Assert
            Assert.Throws<SKUNotFoundException>(() => service.ScanItem("E", 1));
        }

        [Theory]
        public void ScanItem_qtyZeroOrLessException(int quantity)
        {
            // Arrange
            var prices = MockedPrices();
            var discounts = MockedDiscounts();
            var service = KataService(prices, discounts);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => service.ScanItem("A", quantity));
        }

        #endregion
    }
}
