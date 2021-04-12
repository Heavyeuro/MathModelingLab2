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
        private ShortTermInsuranceService _insuranceService;
        private PersonalSavingsService _savingsService;

        public MiscComputingController(GompertzKramarService _gompertzComputingService, ShortTermInsuranceService insuranceService, PersonalSavingsService savingsService)
        {
            _kramarService = _gompertzComputingService;
            _insuranceService = insuranceService;
            _savingsService = savingsService;
        }

        [HttpGet("FitParams")]
        public async Task<IActionResult> FitParams()
        {
            var optimalParameters = _kramarService.ComputeAllParams();
            return Ok(optimalParameters);
        }
        
        [HttpGet("STIPlot")]
        public async Task<IActionResult> STIPlot()
        {
            var STM = new ShortTermInsuranceService.ShortTermModel{U = 100000,V = 20,BThreshold = 45000,NThreshold = 25000};
            var path = _insuranceService.DrawPlot(STM);
            return PhysicalFile(path, "image/jpeg");
        }
        
        [HttpGet("SavingsPlot")]
        public async Task<IActionResult> SavingsPlot()
        {
            var STM = new PersonalSavingsService.IncomeModel
            {
                U = 1000,
                FinalSum = 40000,
                MaxThreshold = 2500,
                MinThreshold = 600
            };
            var path = _savingsService.DrawPlot(STM);
            return PhysicalFile(path, "image/jpeg");
        }
    }
}