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
        public async Task<IActionResult> GetMortalityTableGompertz([FromBody] MakehamLawParams makehamLawParams)
        {
            var table = await _makehamComputingService.BuildMortalityTable(makehamLawParams);
            return Ok(table);
        }

        [HttpPost("GetPlot")]
        public async Task<IActionResult> GetPlotGompertz([FromBody] MakehamLawParams makehamLawParams)
        {
            var path = await _makehamComputingService.BuildPlot(makehamLawParams);
            return PhysicalFile(path, "image/jpeg");
        }
        
        [HttpPost("CompareWithRealData")]
        public async Task<IActionResult> CompareWithRealData([FromBody] MakehamLawParams makehamLawParams)
        {
            var path = await _makehamComputingService.CompareWithRealDataPlot(makehamLawParams);
            return PhysicalFile(path, "image/jpeg");
        }
        
        [HttpPost("FindAbsoluteError")]
        public async Task<IActionResult> FindAbsoluteError([FromBody] MakehamLawParams makehamLawParams)
        {
            var absoluteError = await _makehamComputingService.CompareWithRealDataAbsoluteError(makehamLawParams);
            return Ok(absoluteError);
        }
        
        [HttpPost("FindAbsoluteErrorTable")]
        public async Task<IActionResult> FitParamsTable()
        {
            var absoluteError = await _makehamComputingService.FitParamsTable();
            return Ok(absoluteError);
        }
        
        [HttpPost("FitParams")]
        public async Task<IActionResult> FitParams()
        {
            var optimalParameters  = await _makehamComputingService.FitParams();
            return Ok(optimalParameters);
        }
    }
}