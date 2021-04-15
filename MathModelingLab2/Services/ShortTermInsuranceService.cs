using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathModelingLab2.PlotService;

namespace MathModelingLab2.Services
{
    public class ShortTermInsuranceService
    {
        private ShortTermModel STM { get; set; }

        public ShortTermInsuranceService()
        {
            STM = new ShortTermModel
            {
                U = 100000,
                T = 20,
                I = 0.1,
                N = 3000,
                InflationCoefs = new Queue<double>(Enumerable.Range(0, 20).Select(_ => new Random().NextDouble() * 0.2 - 0.05))
            };
        }

        private void InitializeWithSTM(InputShortTermModel shortTermModel)
        {
            STM.V = (int) (STM.N * shortTermModel.Q);

            STM.Bs = Enumerable.Range(0, STM.N)
                .Select(_ => new Random().NextDouble() * (shortTermModel.BThreshold - shortTermModel.NThreshold) + shortTermModel.NThreshold).ToList();

            // Randomly insurance case  
            STM.BVs = Enumerable.Range(0, STM.V).Select(_ => STM.Bs[(int) (new Random().NextDouble() * STM.Bs.Count)])
                .ToList();

            // Computing desired premium
            // where 0.2 - payload, 1.645 - quantile, M - expectation
            double M = 1 * shortTermModel.Q + (1 - shortTermModel.Q);
            STM.Ps = STM.Bs
                .Select(b => (b * (1.645 * Math.Sqrt(STM.N * (M - (M * M))) + shortTermModel.Q * STM.N) / 100) / (1 - 0.2))
                .ToList();
        }

        public string DrawPlot(InputShortTermModel shortTermModel)
        {
            InitializeWithSTM(shortTermModel);

            var fundT = Enumerable.Range(0, STM.T).Select(x => ApplyInflationProcess(ComputeFinancialState(x), x));

            var plotLines = new List<PlotLine>
            {
                new("Current fund",
                    Enumerable.Range(0, STM.T).Select(x => (double) x).ToArray(),
                    fundT.ToArray())
            };

            var path = Directory.GetCurrentDirectory() + $"\\plots\\{Guid.NewGuid()}.png";

            PlotService.PlotService.MakePlot(path, plotLines, "Moments of time(stochastic)", "fund");
            DrawSelectionPlot();
            return path;
        }
        
        public void DrawSelectionPlot()
        {
            var plotLines = new List<PlotLine>
            {
                new("Premiums",
                    Enumerable.Range(0, STM.Ps.Count).Select(x => (double) x).ToArray(),
                    STM.Ps.ToArray()),
                new("Compensations",
                    Enumerable.Range(0, STM.Ps.Count).Select(x => (double) x).ToArray(),
                    STM.Bs.ToArray()),
            };

            var path = Directory.GetCurrentDirectory() + $"\\plots\\{Guid.NewGuid()}.png";

            PlotService.PlotService.MakePlot(path, plotLines, "Client number", "Operation value");
        }

        private double ComputeFinancialState(int t)
        {
            var allFundings = (STM.U + STM.Ps.Sum()) * (Math.Pow(1 + STM.I, t));
            var paymentsSum = Enumerable.Range(0, t + 1).Sum(i => STM.BVs.Sum(s => s * Math.Pow(1 + STM.I, t - i)));
            return allFundings - paymentsSum;
        }

        private double ApplyInflationProcess(double beforeInflation, int t)
        {
            double inflationCoef = 1;

            for (int i = 0; i < t; i++)
                inflationCoef *= (1 + STM.InflationCoefs.ElementAt(i));

            return beforeInflation / inflationCoef;
        }

        // Input param.
        public class InputShortTermModel
        {
            // Probability of death within a year
            public double Q { get; set; }

            // Minimal compensation
            public int NThreshold { get; set; }

            // Maximal compensation
            public int BThreshold { get; set; }
        }

        // Input param.
        public class ShortTermModel
        {
            // Initial capital
            public double U { get; set; }
            
            // Number of policyholders
            public int N { get; set; }

            // Interest rate
            public double I { get; set; }

            // Time to compute
            public int T { get; set; }

            // Number of claims
            public int V { get; set; }

            // Coeffs in rage -5% to 10%
            public Queue<double> InflationCoefs { get; set; }

            // Premiums
            public List<double> Ps { get; set; }

            // Insurance compensations
            public List<double> Bs { get; set; }

            // Insurance case compensation
            public List<double> BVs { get; set; }
        }
    }
}