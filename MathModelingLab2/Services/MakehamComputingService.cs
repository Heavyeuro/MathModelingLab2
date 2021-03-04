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
    public class MakehamComputingService
    {
        private const int AgeLimit = 100;
        private const int PeopleNumber = 100000;
        private List<RealDataTableViewRaw> RealData { get; set; }

        public MakehamComputingService()
        {
            RealData = XlsService.ReadXls();
        }

        public async Task<List<MortalityTableModelRaw>> BuildMortalityTable(MakehamLawParams makehamLawParams)
        {
            return ComputeMortalityTableModelRaws(makehamLawParams);
        }

        public async Task<string> CompareWithRealDataPlot(MakehamLawParams makehamLawParams)
        {
            var mortalityTableModelRaws = ComputeMortalityTableModelRaws(makehamLawParams);

            return BuildPlot(new List<PlotLine>
            {
                new("Computed Makeham", mortalityTableModelRaws.Select(x => x.X).ToArray(),
                    mortalityTableModelRaws.Select(x => x.Lx).ToArray()),
                new("Real data", RealData.Select(x => x.X).ToArray(),
                    RealData.Select(x => x.Lx).ToArray())
            });
        }

        public async Task<double> CompareWithRealDataAbsoluteError(MakehamLawParams makehamLawParams)
        {
            return CompareDataWithRealDataAbsoluteError(makehamLawParams);
        }

        public async Task<List<FittingParametersMakeham>> FitParamsTable()
        {
            return IterateParamsTable(0.005, 0.1);
        }

        public async Task<FittingParametersMakeham> FitParams()
        {
            return IterateParamsTable(0.00002, 0.1).MinBy(x => x.AbsoluteError).First();
        }

        public async Task<string> BuildPlot(MakehamLawParams makehamLawParams)
        {
            var mortalityTableModelRaws = ComputeMortalityTableModelRaws(makehamLawParams);

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

        private List<FittingParametersMakeham> IterateParamsTable(double step, double range)
        {
            var fittingParams = new List<FittingParametersMakeham>();

            var bestA = 0.001;
            var bestB = 0.001;
            var bestAlpha =  0.001;
            MakehamLawParams temp;
            var error = 1.0;

            for (var j = 0; j < 2; j++)
            {
                
                for (var i = 0.0001; i < range; i = Math.Round(step+i,6))
                {
                    temp = new MakehamLawParams(bestAlpha, bestA, i);
                    var tempError = CompareDataWithRealDataAbsoluteError(temp);
                    
                    if (tempError > error && !double.IsNaN(tempError)) continue;
                    bestB = i;
                    error = tempError;
                    fittingParams.Add(new FittingParametersMakeham(temp, error));
                }
                
                for (var i = 0.0001; i < range; i = Math.Round(step+i,6))
                {
                    temp = new MakehamLawParams(bestAlpha, i, bestB);
                    var tempError = CompareDataWithRealDataAbsoluteError(temp);
                    
                    if (tempError > error && !double.IsNaN(tempError)) continue;
                    bestA = i;
                    error = tempError;
                    fittingParams.Add(new FittingParametersMakeham(temp, error));
                }
                
                for (var i = 0.0001; i < range; i = Math.Round(step+i,6))
                {
                    temp = new MakehamLawParams(i, bestA, bestB);
                    var tempError = CompareDataWithRealDataAbsoluteError(temp);
                    
                    if (tempError > error && !double.IsNaN(tempError)) continue;
                    bestAlpha = i;
                    error = tempError;
                    fittingParams.Add(new FittingParametersMakeham(temp, error));
                }
            }

            return fittingParams;
        }

        private static List<MortalityTableModelRaw> ComputeMortalityTableModelRaws(MakehamLawParams makehamLawParams)
        {
            var raws = new List<MortalityTableModelRaw>();
            double previousLx = PeopleNumber;
            foreach (var i in Enumerable.Range(0, AgeLimit))
            {
                var tableRaw = new MortalityTableModelRaw
                {
                    X = i, Lx = previousLx * ComputeMakehamCoef(makehamLawParams, i)
                };

                tableRaw.Dx = tableRaw.Lx - tableRaw.Lx * ComputeMakehamCoef(makehamLawParams, i + 1);
                tableRaw.Qx = tableRaw.Dx / tableRaw.Lx;
                tableRaw.Px = 1 - tableRaw.Qx;
                previousLx = tableRaw.Lx;

                raws.Add(tableRaw);
            }

            return raws;
        }

        private double CompareDataWithRealDataAbsoluteError(MakehamLawParams makehamLawParams)
        {
            var computedData = ComputeMortalityTableModelRaws(makehamLawParams);

            return Math.Round(FunctionsComputingService.ComputeAbsoluteError(computedData.Select(x => x.Lx).ToList(),
                RealData.Select(x => x.Lx).ToList()),6);
        }

        private static double ComputeMakehamCoef(MakehamLawParams makehamLawParams, int x) =>
            Math.Exp(-makehamLawParams.A*x-((makehamLawParams.B*(Math.Exp(makehamLawParams.Alpha*x)-1))/makehamLawParams.Alpha));
    }
}