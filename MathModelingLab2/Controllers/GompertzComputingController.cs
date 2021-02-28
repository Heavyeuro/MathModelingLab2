using System.Threading.Tasks;
using MathModelingLab2.Models;
using MathModelingLab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace MathModelingLab2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GompertzComputingController : ControllerBase
    {
        private GompertzComputingService _gompertzComputingService;

        public GompertzComputingController(GompertzComputingService gompertzComputingService)
        {
            _gompertzComputingService = gompertzComputingService;
        }

        [HttpPost("GetMortalityTable")]
        public async Task<IActionResult> GetMortalityTableGompertz([FromBody] GompertzLawParams gompertzLawParams)
        {
            var table = await _gompertzComputingService.BuildMortalityTable(gompertzLawParams);
            return Ok(table);
        }

        [HttpPost("GetPlot")]
        public async Task<IActionResult> GetPlotGompertz([FromBody] GompertzLawParams gompertzLawParams)
        {
            var path = await _gompertzComputingService.BuildPlot(gompertzLawParams);
            return PhysicalFile(path, "image/jpeg");
        }

        [HttpPost("CompareWithRealData")]
        public async Task<IActionResult> CompareWithRealData([FromBody] GompertzLawParams gompertzLawParams)
        {
            var path = await _gompertzComputingService.CompareWithRealDataPlot(gompertzLawParams);
            return PhysicalFile(path, "image/jpeg");
        }

        [HttpPost("FindAbsoluteError")]
        public async Task<IActionResult> FindAbsoluteError([FromBody] GompertzLawParams gompertzLawParams)
        {
            var absoluteError = await _gompertzComputingService.CompareWithRealDataAbsoluteError(gompertzLawParams);
            return Ok(absoluteError);
        }

        [HttpPost("FindAbsoluteErrorTable")]
        public async Task<IActionResult> FitParamsTable()
        {
            var absoluteError = await _gompertzComputingService.FitParamsTable();
            return Ok(absoluteError);
        }

        [HttpPost("FitParams")]
        public async Task<IActionResult> FitParams()
        {
            var optimalParameters = await _gompertzComputingService.FitParams();
            return Ok(optimalParameters);
        }
    }
}