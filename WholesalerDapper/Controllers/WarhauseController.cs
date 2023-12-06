using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholesalerDapper.Service;

namespace WholesalerDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarhauseController : ControllerBase
    {
        private readonly IServiceWarhause _serviceWarhause;

        public WarhauseController(IServiceWarhause serviceWarhause)
        {
            _serviceWarhause = serviceWarhause;
        }
        [HttpPost("ExtractCsvDapper")]
        public async Task<ActionResult<bool>> GetProduct()
        {
         try
        {
                // Running parallel asynchronous tasks to extract CSV data (products, inventory, prices)
            _serviceWarhause.TruncateTable("ProductsDB");
            _serviceWarhause.TruncateTable("InventoriesDB");
            _serviceWarhause.TruncateTable("PricesDB");
            var v1 = Task.Run(() => _serviceWarhause.ExtractCsvProductsD());
            var v2 = Task.Run(() => _serviceWarhause.ExtractCsvInventoryD());
            var v3 = Task.Run(() => _serviceWarhause.ExtractCsvPricesD());
            // Wait for all asynchronous tasks to complete and save the results
            bool[] results = await Task.WhenAll(v1, v2, v3);
            return Ok(results);
        }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetProductBySku")]
        public async Task<ActionResult<object>> GetProductBySku(string sku)
        {
            try
            {
                if (sku is null)
                {
                    return StatusCode(400, "Wrong data");
                }
                return Ok(await _serviceWarhause.GetProductsBySKUD(sku));
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
