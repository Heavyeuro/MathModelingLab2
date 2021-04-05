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
        public async Task<IActionResult> GetMortalityTable([FromBody] GompertzLawParams gompertzLawParams)
        {
            var table = await _gompertzComputingService.BuildMortalityTable(gompertzLawParams);
            return Ok(table);
        }

        [HttpGet("GetPlot")]
        public async Task<IActionResult> GetPlot(double alpha, double beta, double ratePercents)
        {
            var gompertzLawParams = new GompertzLawParams(alpha, beta, ratePercents);
            var path = await _gompertzComputingService.BuildPlot(gompertzLawParams);
            // var path = await _gompertzComputingService.ParamsPlotComparison();
            return PhysicalFile(path, "image/jpeg");
        }

        [HttpGet("CompareWithRealData")]
        public async Task<IActionResult> CompareWithRealData(double alpha, double beta, double ratePercents)
        { 
            var gompertzLawParams = new GompertzLawParams(alpha, beta, ratePercents);
            var path = await _gompertzComputingService.CompareWithRealDataPlot(gompertzLawParams);
            return PhysicalFile(path, "image/jpeg");
        }

        [HttpPost("FindAbsoluteError")]
        public async Task<IActionResult> FindAbsoluteError([FromBody] GompertzLawParams gompertzLawParams)
        {
            var absoluteError = await _gompertzComputingService.CompareWithRealDataAbsoluteError(gompertzLawParams);
            return Ok(absoluteError);
        }

        [HttpGet("FindAbsoluteErrorTable")]
        public async Task<IActionResult> FitParamsTable()
        {
            var absoluteError = await _gompertzComputingService.FitParamsTable();
            return Ok(absoluteError);
        }

        [HttpGet("FitParams")]
        public async Task<IActionResult> FitParams()
        {
            var optimalParameters = await _gompertzComputingService.FitParams();
            return Ok(optimalParameters);
        }
    }
}