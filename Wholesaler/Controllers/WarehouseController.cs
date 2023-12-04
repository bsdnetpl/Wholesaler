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
    {
        private readonly IServiceWar _serviceWar;
        public WarehouseController(IServiceWar serviceWar)
        {
            _serviceWar = serviceWar;
        }
        [HttpPost("ExtractCsv")]
        public async Task<ActionResult<bool>> GetProduct()
        {
            var v1 = Task.Run(() => _serviceWar.ExtractCsvProducts());
            var v2 = Task.Run(() => _serviceWar.ExtractCsvInventory());
            var v3 = Task.Run(() => _serviceWar.ExtractCsvPrices());
            bool[] results = await Task.WhenAll(v1, v2, v3);
            return Ok(true);
        }
        [HttpGet("GetProductSku")]
        public async Task <object> GetProductBySku(string sku)
        {
            return await _serviceWar.GetProductsBySKU(sku);
        }

    }
}
