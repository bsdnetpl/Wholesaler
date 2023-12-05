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
        [HttpGet("GetData")]
        public async Task<ActionResult<bool>> ExtractCsvProductsD()
        {
            return Ok( await _serviceWarhause.ExtractCsvProductsD());
        }
    }
}
