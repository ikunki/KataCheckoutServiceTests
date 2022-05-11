
namespace KataCheckoutService.Interfaces
{
    public interface IKataCheckoutService
    {
        void ScanItem(string sku, int quantity);
        decimal TotalCost();
    }
}
