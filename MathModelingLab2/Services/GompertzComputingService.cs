using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MathModelingLab2.Models;
using MathModelingLab2.PlotService;
using MoreLinq.Extensions;

namespace MathModelingLab2.Services
{
    public class GompertzComputingService
    {
        private const int AgeLimit = 100;
        private const int PeopleNumber = 100000;
        private List<RealDataTableViewRaw> RealData { get; set; }

        public GompertzComputingService()
        {
            RealData = XlsService.ReadXls();
        }

        public async Task<List<MortalityTableModelRaw>> BuildMortalityTable(GompertzLawParams gompertzLawParams)
        {
            return ComputeMortalityTableModelRaws(gompertzLawParams);
        }

        public async Task<string> CompareWithRealDataPlot(GompertzLawParams gompertzLawParams)
        {
            var mortalityTableModelRaws = ComputeMortalityTableModelRaws(gompertzLawParams);

            return BuildPlot(new List<PlotLine>
            {
                new("Computed Gompertz", mortalityTableModelRaws.Select(x => x.X).ToArray(),
                    mortalityTableModelRaws.Select(x => x.Lx).ToArray()),
                new("Real data", RealData.Select(x => x.X).ToArray(),
                    RealData.Select(x => x.Lx).ToArray())
            });
        }

        public async Task<double> CompareWithRealDataAbsoluteError(GompertzLawParams gompertzLawParams)
        {
            return CompareDataWithRealDataAbsoluteError(gompertzLawParams);
        }

        public async Task<List<FittingParametersGompertz>> FitParamsTable()
        {
            return IterateParamsTable(0.005, 0.1);
        }

        public async Task<FittingParametersGompertz> FitParams()
        {
            return IterateParamsTable(0.000001, 0.5).MinBy(x => x.AbsoluteError).First();
        }

        public async Task<string> BuildPlot(GompertzLawParams gompertzLawParams)
        {
            var mortalityTableModelRaws = ComputeMortalityTableModelRaws(gompertzLawParams);

            return BuildPlot(new List<PlotLine>
            {
                new("", mortalityTableModelRaws.Select(x => x.X).ToArray(),
                    mortalityTableModelRaws.Select(x => x.Lx).ToArray())
            });
        }

        private static string BuildPlot(List<PlotLine> plotLines)
        {
            var path = Directory.GetCurrentDirectory() + $"\\plots\\{Guid.NewGuid()}.png";

            PlotService.PlotService.MakePlot(path, plotLines, "Age(years)", "Alive number");

            return path;
        }
        
        public List<FittingParametersGompertz> ComputeBParams(double step, double range)
        {
            var fittingParams = new List<FittingParametersGompertz>();

            var bestAlpha = 0.00022;
            // var bestB = 0.00273;

            for (var i = 0.00001; i < range/10; i = Math.Round(step + i, 6))
            {
               var temp = new GompertzLawParams(bestAlpha, i, bestAlpha);
                var tempError = CompareDataWithRealDataAbsoluteError(temp);

                fittingParams.Add(new FittingParametersGompertz(temp, tempError));
            }

            return fittingParams;
        }

        public List<FittingParametersGompertz> ComputeAlphaParams(double step, double range)
        {
            var fittingParams = new List<FittingParametersGompertz>();

            var bestB = 0.0002;

            for (var i = 0.0001; i < range; i = Math.Round( 0.0001+ i, 6))
            {
                var temp = new GompertzLawParams(i, bestB, bestB);
                var tempError = CompareDataWithRealDataAbsoluteError(temp);

                fittingParams.Add(new FittingParametersGompertz(temp, tempError));
            }

            return fittingParams;
        }

        public async Task<string> ParamsPlotComparison()
        {
            var step = 0.000001;
            var range = 0.01;
            var B = ComputeBParams(step, range);
            // var Alpha = ComputeAlphaParams(step, 0.2);

            var q = new List<PlotLine>
            {
                 new("B Dependency", B.Select(x => x.AbsoluteError).ToArray(),
                     B.Select(x => x.GompertzLawParams.Beta).ToArray()),
                // new("Alpha Dependency", Alpha.Select(x => x.AbsoluteError).ToArray(),
                //     Alpha.Select(x => x.GompertzLawParams.Alpha).ToArray()),
            };

            var path = Directory.GetCurrentDirectory() + $"\\plots\\{Guid.NewGuid()}.png";

            PlotService.PlotService.MakePlot(path, q, "Absolute error", "Parameter values");

            return path;
        }

        private List<FittingParametersGompertz> IterateParamsTable(double step, double range, int loops = 2)
        {
            var fittingParams = new List<FittingParametersGompertz>();

            var bestA = 0.001;
            var bestB = 0.001;
            var bestRate =  0.001;
            var error = 1.0;

            for (var j = 0; j < loops; j++)
            {
                
                for (var i = 0.000001; i < range/100;i = Math.Round(step+i,6))
                {
                    var temp = new GompertzLawParams(i, bestB, bestRate);
                    var tempError = CompareDataWithRealDataAbsoluteError(temp);
                    
                    fittingParams.Add(new FittingParametersGompertz(temp, tempError));

                    if (tempError > error && !double.IsNaN(tempError)) continue;
                    bestA = i;
                    error = tempError;
                }
                
                error = 1.0;
                for (var i = 0.000001; i < range; i = Math.Round(step+i,6))
                {
                    var temp = new GompertzLawParams(bestA, i, bestRate);
                    var tempError = CompareDataWithRealDataAbsoluteError(temp);
                    fittingParams.Add(new FittingParametersGompertz(temp, tempError));
                    
                    if (tempError > error && !double.IsNaN(tempError)) continue;
                    bestB = i;
                    error = tempError;
                }
            }

            return fittingParams;
        }

        private static List<MortalityTableModelRaw> ComputeMortalityTableModelRaws(GompertzLawParams gompertzLawParams)
        {
            var raws = new List<MortalityTableModelRaw>();
            double previousLx = PeopleNumber;
            foreach (var i in Enumerable.Range(0, AgeLimit))
            {
                var tableRaw = new MortalityTableModelRaw
                {
                    X = i, Lx = previousLx * ComputeGompertzCoef(gompertzLawParams, i)
                };

                tableRaw.Dx = tableRaw.Lx - tableRaw.Lx * ComputeGompertzCoef(gompertzLawParams, i + 1);
                tableRaw.Qx = tableRaw.Dx / tableRaw.Lx;
                tableRaw.Px = 1 - tableRaw.Qx;
                tableRaw.CDx = tableRaw.Lx * Math.Pow((1 + gompertzLawParams.RatePercents), -i);
                tableRaw.CCx = tableRaw.Dx * Math.Pow((gompertzLawParams.RatePercents + 1), -i + 1);
                previousLx = tableRaw.Lx;

                raws.Add(tableRaw);
            }

            raws.ForEach(raw =>
            {
                for (var i = (int) raw.X; i < AgeLimit; i++)
                {
                    raw.CNx += raws.First(x => x.X == i).Dx;
                    raw.CMx += raws.First(x => x.X == i).CCx;
                }
            });

            return raws;
        }

        private double CompareDataWithRealDataAbsoluteError(GompertzLawParams gompertzLawParams)
        {
            var computedData = ComputeMortalityTableModelRaws(gompertzLawParams);

            return Math.Round(FunctionsComputingService.ComputeAbsoluteError(computedData.Select(x => x.Lx).ToList(),
                RealData.Select(x => x.Lx).ToList()),6);
        }

        private static double ComputeGompertzCoef(GompertzLawParams gompertzLawParams, int x) =>
            Math.Exp(-gompertzLawParams.Beta * ((Math.Exp(gompertzLawParams.Alpha * x) - 1) / gompertzLawParams.Alpha));
    }
}