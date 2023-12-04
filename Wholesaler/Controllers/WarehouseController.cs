using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Globalization;
using System.Net;
using Wholesaler.DB;
using Wholesaler.Models;
using Wholesaler.Services;

namespace Wholesaler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {   // Initialize the storage controller
        private readonly IServiceWar _serviceWar;
        public WarehouseController(IServiceWar serviceWar)
        {
            _serviceWar = serviceWar;
        }
        // HTTP POST method that calls three asynchronous ExtractCsvProducts methods,
        [HttpPost("ExtractCsv")]
        public async Task<ActionResult<bool>> GetProduct()
        {
            // Running parallel asynchronous tasks to extract CSV data (products, inventory, prices)
            var v1 = Task.Run(() => _serviceWar.ExtractCsvProducts());
            var v2 = Task.Run(() => _serviceWar.ExtractCsvInventory());
            var v3 = Task.Run(() => _serviceWar.ExtractCsvPrices());
            // Wait for all asynchronous tasks to complete and save the results
            bool[] results = await Task.WhenAll(v1, v2, v3);
            return Ok(true);
        }
        // HTTP GET method that returns the product with the given SKU number
        [HttpGet("GetProductBySku")]
        public async Task <ActionResult<object>> GetProductBySku(string sku)
        {
            if(sku is null)
            {
                return StatusCode(400, "Wrong data");
            }
            return Ok(await _serviceWar.GetProductsBySKU(sku));
        }

    }
}
