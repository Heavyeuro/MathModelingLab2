using System.Collections.Generic;
using System.Threading.Tasks;
using MathModelingLab2.Models;
using MathModelingLab2.Services;
using Microsoft.AspNetCore.Mvc;

namespace MathModelingLab2.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet]
        public async Task<IActionResult> GetMortalityTableGompertz(GompertzLawParams gompertzLawParams)
        {
            return await _gompertzComputingService.BuildMortalityTable(gompertzLawParams);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlotGompertz(GompertzLawParams gompertzLawParams)
        {
            string path = await _gompertzComputingService.BuildPlot(gompertzLawParams);
            return PhysicalFile(path, "image/jpeg");
        }
    }
}