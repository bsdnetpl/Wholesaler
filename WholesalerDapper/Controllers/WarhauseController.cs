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
            // Running parallel asynchronous tasks to extract CSV data (products, inventory, prices)
            var v1 = Task.Run(() => _serviceWarhause.ExtractCsvProductsD());
            var v2 = Task.Run(() => _serviceWarhause.ExtractCsvInventoryD());
            var v3 = Task.Run(() => _serviceWarhause.ExtractCsvPricesD());
            // Wait for all asynchronous tasks to complete and save the results
            bool[] results = await Task.WhenAll(v1, v2, v3);
            return Ok(results);
        }
        [HttpGet("GetProductBySku")]
        public async Task<ActionResult<object>> GetProductBySku(string sku)
        {
            if (sku is null)
            {
                return StatusCode(400, "Wrong data");
            }
            return Ok(await _serviceWarhause.GetProductsBySKUD(sku));
        }
    }
}
