
namespace WholesalerDapper.Service
{
    public interface IServiceWarhause
    {
        Task<bool> ExtractCsvInventoryD();
        Task<bool> ExtractCsvPricesD();
        Task<bool> ExtractCsvProductsD();
    }
}