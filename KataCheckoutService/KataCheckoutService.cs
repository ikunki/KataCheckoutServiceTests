using System;
using System.Linq;
using System.Collections.Generic;
using KataCheckoutService.Models;
using KataCheckoutService.Interfaces;

namespace KataCheckoutService
{
    public class KataCheckoutService : IKataCheckoutService
    {
        private readonly IDictionary<string, decimal> prices;
        private readonly IDictionary<string, IEnumerable<Discount>> discounts;
        private readonly IDictionary<string, int> basket = new Dictionary<string, int>();

        public KataCheckoutService(IDictionary<string, decimal> prices, IDictionary<string, IEnumerable<Discount>> discounts)
        {
            this.prices = prices;
            this.discounts = discounts;
        }

        public void ScanItem(string sku, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than 0!");
            }

            if (this.prices.ContainsKey(sku))
            {
                if (!this.basket.ContainsKey(sku))
                {
                    this.basket.Add(sku, quantity);
                }
                else
                {
                    this.basket[sku] += quantity;
                }
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public decimal TotalCost()
        {
            var totalCost = 0m;
            foreach (var item in this.basket)
            {
                var sku = item.Key;
                var quantity = item.Value;
                if (!this.prices.TryGetValue(sku, out var unitPrice))
                {
                    continue;
                }

                var discounts = this.discounts?.FirstOrDefault(d => d.Key == sku).Value;
                var discount = discounts?.FirstOrDefault();
                if (discount != null && quantity >= discount.Quantity)
                {
                    var noDiscounts = quantity / discount.Quantity;
                    var noRemainingUnits = quantity % discount.Quantity;

                    totalCost += ((discount.TotalCost) * noDiscounts) + (unitPrice * noRemainingUnits);
                }
                else
                {
                    totalCost += unitPrice * quantity;
                }
            }
            return totalCost;
        }
    }
}
