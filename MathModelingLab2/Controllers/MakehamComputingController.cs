using System.Collections.Generic;
using System.Threading.Tasks;
using MathModelingLab2.Models;
using MathModelingLab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace MathModelingLab2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MakehamComputingController : ControllerBase
    {
        private MakehamComputingService _makehamComputingService;

        public MakehamComputingController(MakehamComputingService makehamComputingService)
        {
            _makehamComputingService = makehamComputingService;
        }

        [HttpPost("GetMortalityTable")]
        public async Task<IActionResult> GetMortalityTable([FromBody] MakehamLawParams makehamLawParams)
        {
            var table = await _makehamComputingService.BuildMortalityTable(makehamLawParams);
            return Ok(table);
        }

        [HttpGet("GetPlot")]
        public async Task<IActionResult> GetPlot(double alpha, double a, double b)
        {
            // var makehamLawParams = new MakehamLawParams(alpha, a, b);
            // var path = await _makehamComputingService.BuildPlot(makehamLawParams);
            var path = await _makehamComputingService.ParamsPlotComparison();
            return PhysicalFile(path, "image/jpeg");
        }
        
        [HttpGet("CompareWithRealData")]
        public async Task<IActionResult> CompareWithRealData(double alpha, double a, double b)
        {
            var makehamLawParams = new MakehamLawParams(alpha, a, b);
            var path = await _makehamComputingService.CompareWithRealDataPlot(makehamLawParams);
            return PhysicalFile(path, "image/jpeg");
        }
        
        [HttpPost("FindAbsoluteError")]
        public async Task<IActionResult> FindAbsoluteError([FromBody] MakehamLawParams makehamLawParams)
        {
            var absoluteError = await _makehamComputingService.CompareWithRealDataAbsoluteError(makehamLawParams);
            return Ok(absoluteError);
        }
        
        [HttpGet("FindAbsoluteErrorTable")]
        public async Task<IActionResult> FitParamsTable()
        {
            var absoluteError = await _makehamComputingService.FitParamsTable();
            return Ok(absoluteError);
        }
        
        [HttpGet("FitParams")]
        public async Task<IActionResult> FitParams()
        {
            var optimalParameters  = await _makehamComputingService.FitParams();
            return Ok(optimalParameters);
        }
    }
}