using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MathModelingLab2.Models;
using MathModelingLab2.PlotService;

namespace MathModelingLab2.Services
{
    public class GompertzComputingService
    {
        private const int AgeLimit = 100;
        private const int PeopleNumber = 100000;

        public async Task<MortalityTableModel> BuildMortalityTable(GompertzLawParams gompertzLawParams)
        {
            return new()
            {
                MortalityTableModelRaws = ComputeMortalityTableModelRaws(gompertzLawParams)
            };
        }

        public async Task<string> BuildPlot(GompertzLawParams gompertzLawParams)
        {
            var path = Directory.GetCurrentDirectory() + $"\\plots\\{Guid.NewGuid()}.png";

            var mortalityTableModelRaws = ComputeMortalityTableModelRaws(gompertzLawParams);

            PlotService.PlotService.MakePlot(path , new List<PlotLine>
            {
                new("", mortalityTableModelRaws.Select(x => x.X).ToArray(),
                    mortalityTableModelRaws.Select(x => x.Lx).ToArray())
            }, "Age(years)","Alive number");

            return path;
        }


        private static List<MortalityTableModelRaw> ComputeMortalityTableModelRaws(GompertzLawParams gompertzLawParams)
        {
            var raws = new List<MortalityTableModelRaw>();
            double previusLx = PeopleNumber;
            foreach (var i in Enumerable.Range(0, AgeLimit))
            {
                var tableRaw = new MortalityTableModelRaw
                {
                    X = i, Lx = previusLx * ComputeGompertzCoef(gompertzLawParams, i)
                };

                tableRaw.Dx = tableRaw.Lx - tableRaw.Lx * ComputeGompertzCoef(gompertzLawParams, i + 1);
                tableRaw.Qx = tableRaw.Dx / tableRaw.Lx;
                tableRaw.Px = 1 - tableRaw.Qx;
                tableRaw.CDx = tableRaw.Lx * Math.Pow((1 + gompertzLawParams.RatePercents), -i);
                tableRaw.CCx = tableRaw.Dx * Math.Pow((gompertzLawParams.RatePercents + 1), -i + 1);
                previusLx = tableRaw.Lx;

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

        private static double ComputeGompertzCoef(GompertzLawParams gompertzLawParams, int x) =>
            Math.Exp(-gompertzLawParams.Beta * (Math.Exp(gompertzLawParams.Alpha * x) - 1) / gompertzLawParams.Alpha);
    }
}