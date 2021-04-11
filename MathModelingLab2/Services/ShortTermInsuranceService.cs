using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathModelingLab2.Models;
using MathModelingLab2.PlotService;
using MoreLinq.Extensions;

namespace MathModelingLab2.Services
{
    public class ShortTermInsuranceService
    {
        public class ShortTermModel
        {
            // Initial capital
            public double U { get; set; }
            
            // Number of claims
            public int V { get; set; }
            
            // Minimal compensation
            public int NThreshold { get; set; }
            
            // Maximal compensation
            public int BThreshold { get; set; } 
        }

        private ShortTermModel STM { get; set; }
        
        // Number of policyholders
        private int N { get; set; }
        
        // Interest rate
        private double I { get; set; }
        
        // Time to compute
        private int T { get; set; }
        
        // Coeffs in rage -5% to 10%
        private Queue<double> InflationCoefs { get; set; }
        
        //
        private List<double> Ps { get; set; }
        
        //
        private List<double> Bs { get; set; }
        
        // Insurance case compensation
        private List<double> BVs { get; set; }
        
        public ShortTermInsuranceService()
        {
            STM = new ShortTermModel();
            T = 20;
            I = 0.1;
            N = 3000;
            InflationCoefs = new Queue<double>(Enumerable.Range(0,T).Select(_ => new Random().NextDouble()*0.2-0.05));
        }

        private void InitializeWithSTM(ShortTermModel shortTermModel)
        {
            STM = shortTermModel;
            Bs = Enumerable.Range(0, N).Select( _ => new Random().NextDouble()*(STM.BThreshold - STM.NThreshold)+STM.NThreshold).ToList();
            BVs = Enumerable.Range(0, STM.V).Select(_ => Bs[(int) (new Random().NextDouble()*Bs.Count)]).ToList();
            
            // where 0.003 - probability of insurance event, 0.2 - payload, 1.645 - quantile, M - expectation
            const double M = 1 * 0.003 + (1 - 0.003);
            Ps = Bs.Select(b => (b*(1.645*Math.Sqrt(N*(M-(M*M))) + 0.003*N)/100)/(1-0.2)).ToList();
        }
        
        // Returns Alpha and Beta and mean Error value
        public string DrawPlot(ShortTermModel shortTermModel)
        {
            InitializeWithSTM(shortTermModel);

            // var fundT = Enumerable.Range(0, T).Select(x => ApplyInflationProcess(FinancialState(x), x));
            var fundT = Enumerable.Range(0, T).Select( x =>  ApplyInflationProcess(FinancialState(x),x));
            
            var q = new List<PlotLine>
            {
                new("Current fund", 
                    Enumerable.Range(0, T).Select(x => (double)x).ToArray(),
                    fundT.ToArray())
            };

            var path = Directory.GetCurrentDirectory() + $"\\plots\\{Guid.NewGuid()}.png";

            PlotService.PlotService.MakePlot(path, q, "Moments of time(stochastic)", "fund");
    
            return path;
        }

        private double FinancialState(int t)
        {
            var allFundings = (STM.U + Ps.Sum()) * (Math.Pow(1 + I, t));
            var paymentsSum = Enumerable.Range(0,t+1).Sum( i => BVs.Sum(s => s * Math.Pow(1+I, t-i)));// last just part BVs.Sum()
            return allFundings - paymentsSum;
        }

        private double ApplyInflationProcess(double beforeInflation, int t)
        {
            double inflationCoef = 1;
            
            for (int i = 0; i < t; i++)
                inflationCoef *= (1 + InflationCoefs.ElementAt(i));
            
            return beforeInflation / inflationCoef;
        }
    }
}