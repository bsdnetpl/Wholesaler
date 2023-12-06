
namespace WholesalerDapper.Service
{
    public interface IServiceWarhause
    {
        Task<bool> ExtractCsvInventoryD();
        Task<bool> ExtractCsvPricesD();
        Task<bool> ExtractCsvProductsD();
        Task<object> GetProductsBySKUD(string sku);
        void TruncateTable(string TableName);
    }
}