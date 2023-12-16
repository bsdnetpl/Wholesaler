
namespace Wholesaler.Services
{
    public interface IServiceWar
    {
        void Cleaning();
        Task<bool> ExtractCsvInventory();
        Task<bool> ExtractCsvPrices();
        Task<bool> ExtractCsvProducts();
        Task<object> GetProductsBySKU(string sku);
    }
}