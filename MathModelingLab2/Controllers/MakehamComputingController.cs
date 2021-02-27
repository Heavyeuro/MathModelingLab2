// using System.Collections.Generic;
// using System.Threading.Tasks;
// using MathModelingLab2.Models;
// using MathModelingLab2.Services;
// using Microsoft.AspNetCore.Mvc;
//
// namespace MathModelingLab2.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class MakehamComputingController : ControllerBase
//     {
//         private MakehamComputingService _makehamComputingService;
//
//         public MakehamComputingController(MakehamComputingService makehamComputingService)
//         {
//             _makehamComputingService = makehamComputingService;
//         }
//
//         [HttpPost("GetMortalityTable")]
//         public async Task<IActionResult> GetMortalityTable()
//         {
//             var makehamLawParams = new MakehamLawParams {A=0.000354, B = 0.00026544, Alpha = 0.074};
//             var table = await _makehamComputingService.BuildMortalityTable(makehamLawParams);
//             return Ok(table);
//         }
//
//         [HttpPost("GetPlot")]
//         public async Task<IActionResult> GetPlot()
//         {
//             var makehamLawParams = new MakehamLawParams {A=0.000354, B = 0.00026544, Alpha = 0.074};
//             var path = await _makehamComputingService.BuildPlot(makehamLawParams);
//             return PhysicalFile(path, "image/jpeg");
//         }
//
//         [HttpPost("CompareWithRealData")]
//         public async Task<IActionResult> CompareWithRealData()
//         {
//             var makehamLawParams = new MakehamLawParams {A=0.000354, B = 0.00026544, Alpha = 0.074};
//             var path = await _makehamComputingService.CompareWithRealData(makehamLawParams);
//             return PhysicalFile(path, "image/jpeg");
//         }
//
//         [HttpPost("FindAbsoluteError")]
//         public async Task<IActionResult> FindAbsoluteError()
//         {
//             var makehamLawParams = new MakehamLawParams {A=0.000354, B = 0.00026544, Alpha = 0.074};
//             var absoluteError = await _makehamComputingService.FindAbsoluteError(makehamLawParams);
//             return Ok(absoluteError);
//         }
//
//         [HttpPost("FindAbsoluteErrorTable")]
//         public async Task<IActionResult> FitParams()
//         {
//             var absoluteError = await _makehamComputingService.FindAbsoluteError();
//             return Ok(absoluteError);
//         }
//     }
// }