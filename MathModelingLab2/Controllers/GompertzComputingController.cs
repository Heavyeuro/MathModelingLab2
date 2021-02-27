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
        public async Task<IActionResult> GetMortalityTableGompertz()
        {
            var gompertzLawParams = new GompertzLawParams(0.05615, 0.00273, 0.09);
            var table = await _gompertzComputingService.BuildMortalityTable(gompertzLawParams);
            return Ok(table);
        }

        [HttpPost("GetPlot")]
        public async Task<IActionResult> GetPlotGompertz()
        {
            var gompertzLawParams = new GompertzLawParams(0.05615, 0.00273, 0.09);
            var path = await _gompertzComputingService.BuildPlot(gompertzLawParams);
            return PhysicalFile(path, "image/jpeg");
        }
        
        [HttpPost("CompareWithRealData")]
        public async Task<IActionResult> CompareWithRealData()
        {
            var gompertzLawParams = new GompertzLawParams(0.001, 0.0005, 0.5);
            var path = await _gompertzComputingService.CompareWithRealDataPlot(gompertzLawParams);
            return PhysicalFile(path, "image/jpeg");
        }
        
        [HttpPost("FindAbsoluteError")]
        public async Task<IActionResult> FindAbsoluteError()
        {
            var gompertzLawParams = new GompertzLawParams(0.01715, 0.00156, 0.5);
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
            var optimalParameters  = await _gompertzComputingService.FitParams();
            return Ok(optimalParameters);
        }
    }
}