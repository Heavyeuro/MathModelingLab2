using System.Collections.Generic;
using System.Threading.Tasks;
using MathModelingLab2.Models;
using MathModelingLab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace MathModelingLab2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComputingController : ControllerBase
    {
        private MakehamComputingService _makehamComputingService;
        private GompertzComputingService _gompertzComputingService;

        public ComputingController(GompertzComputingService gompertzComputingService,
            MakehamComputingService makehamComputingService)
        {
            _gompertzComputingService = gompertzComputingService;
            _makehamComputingService = makehamComputingService;
        }

        [HttpPost("GetMortalityTable")]
        public async Task<IActionResult> GetMortalityTableGompertz()
        {
            var gompertzLawParams = new GompertzLawParams {Alpha = 0.05615, Beta = 0.00273, RatePercents = 0.09};
            var table = await _gompertzComputingService.BuildMortalityTable(gompertzLawParams);
            return Ok(table);
        }

        [HttpPost("GetPlotGompertz")]
        public async Task<IActionResult> GetPlotGompertz()
        {
            var gompertzLawParams = new GompertzLawParams {Alpha = 0.05615, Beta = 0.00273, RatePercents = 0.09};
            var path = await _gompertzComputingService.BuildPlot(gompertzLawParams);
            return PhysicalFile(path, "image/jpeg");
        }
    }
}