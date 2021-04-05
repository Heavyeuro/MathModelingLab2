using System.Threading.Tasks;
using MathModelingLab2.Models;
using MathModelingLab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace MathModelingLab2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiscComputingController : ControllerBase
    {
        private GompertzKramarService _kramarService;

        public MiscComputingController(GompertzKramarService _gompertzComputingService)
        {
            _kramarService = _gompertzComputingService;
        }

        [HttpGet("FitParams")]
        public async Task<IActionResult> FitParams()
        {
            var optimalParameters = _kramarService.ComputeAllParams();
            return Ok(optimalParameters);
        }
    }
}