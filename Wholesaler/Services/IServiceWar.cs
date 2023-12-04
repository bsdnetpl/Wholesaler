
namespace Wholesaler.Services
{
    public interface IServiceWar
    {
        Task<bool> ExtractCsvInventory();
        Task<bool> ExtractCsvPrices();
        Task<bool> ExtractCsvProducts();
        Task<object> GetProductsBySKU(string sku);
    }
}