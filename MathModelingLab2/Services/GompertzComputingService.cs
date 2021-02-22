using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathModelingLab2.Models;

namespace MathModelingLab2.Services
{
    public class GompertzComputingService
    {
        private const int AgeLimit = 100;
        private const int PeopleNumber = 100000;

        public async Task<MortalityTableModel> BuildMortalityTable(GompertzLawParams gompertzLawParams)
        {
            var table = new MortalityTableModel
            {
                MortalityTableModelRaws = new List<MortalityTableModelRaw>()
            };

            double previusLx = PeopleNumber;
            foreach (var i in Enumerable.Range(0, AgeLimit))
            {
                var tableRaw = new MortalityTableModelRaw();
                
                tableRaw.X = i;
                tableRaw.Lx = previusLx * ComputeGompertzCoef(gompertzLawParams, i);
                previusLx = tableRaw.Lx;
                tableRaw.Dx = tableRaw.Lx - tableRaw.Lx * ComputeGompertzCoef(gompertzLawParams, i+1);
                tableRaw.Qx = tableRaw.Dx / tableRaw.Lx;
                tableRaw.Px = 1 - tableRaw.Qx;
                tableRaw.CDx = tableRaw.Lx * Math.Pow((1 + gompertzLawParams.RatePercents), -i);
                tableRaw.CCx = tableRaw.Dx * Math.Pow((gompertzLawParams.RatePercents + 1), -i + 1);
                
                table.MortalityTableModelRaws.Add(tableRaw);
            }
            
            table.MortalityTableModelRaws.ForEach(raw =>
            {
                for (var i = (int) raw.X; i < AgeLimit; i++)
                {
                    raw.CNx += table.MortalityTableModelRaws.First(x => x.X == i).Dx;
                    raw.CMx += table.MortalityTableModelRaws.First(x => x.X == i).CCx;
                }
            });
            
            return table;
        }

        public async Task<string> BuildPlot(GompertzLawParams gompertzLawParams)
        {
            string path = null;
            return path;
        }

        private double ComputeGompertzCoef(GompertzLawParams gompertzLawParams, int x) =>
            Math.Exp(-gompertzLawParams.Beta * (Math.Exp(gompertzLawParams.Alpha * x)  - 1) / gompertzLawParams.Alpha);
    }
}