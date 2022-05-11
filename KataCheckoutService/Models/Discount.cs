
namespace KataCheckoutService.Models
{
    public class Discount
    {
        public int Quantity { get; }
        public decimal TotalCost { get; }

        public Discount(int quantity, decimal totalCost)
        {
            Quantity = quantity;
            TotalCost = totalCost;
        }
    }
}
