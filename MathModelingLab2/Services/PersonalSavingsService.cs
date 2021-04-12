using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathModelingLab2.PlotService;

namespace MathModelingLab2.Services
{
    public class PersonalSavingsService
    {
        // Coeffs in rage -5% to 10%
        private Queue<double> InflationCoefs { get; set; }
        
        // Deposit rates month
        private double Dep { get; set; }
        
        // Monthly income
        private Queue<double> Inc { get; set; }
        
        public PersonalSavingsService()
        {
            Dep = 1.005;
            InflationCoefs = new Queue<double>(Enumerable.Range(0,1000).Select(_ => new Random().NextDouble()*0.2-0.05));
        }

        
        private void InitializeWithSTM(IncomeModel incomeModel)
        {
            // 0.03 - coeff of income growing
            Inc = new Queue<double>(Enumerable.Range(1, 1000).Select( i => incomeModel.MinThreshold*(i*0.03+1)>incomeModel.MaxThreshold ? incomeModel.MaxThreshold : incomeModel.MinThreshold*(i*0.03+1)).ToList());
        }
        
        // Returns Alpha and Beta and mean Error value
        public string DrawPlot(IncomeModel incomeModel)
        {
            InitializeWithSTM(incomeModel);

            var fundT = Enumerable.Range(1, 1000).Select( x => FinancialState(x,incomeModel)).ToList();
            var T = fundT.FindIndex(x => x> incomeModel.FinalSum)+2;
            
            var q = new List<PlotLine>
            {
                new("Current fund", 
                    Enumerable.Range(0, T).Select(x => (double)x).ToArray(),
                    fundT.Take(T).ToArray())
            };

            var path = Directory.GetCurrentDirectory() + $"\\plots\\{Guid.NewGuid()}.png";

            PlotService.PlotService.MakePlot(path, q, "Moments of time(months)", "fund");
    
            return path;
        }

        private double FinancialState(int t, IncomeModel incomeModel)
        {
            var allFundings = incomeModel.U + Inc.Take(t).Sum()*0.8;
            var allSpendings = t / 3 * new Random().NextDouble() * incomeModel.MinThreshold;
            return allFundings - allSpendings;
        }

        private double ApplyDepositProcess(double beforeInflation, int t)
        {
            double inflationCoef = 1;
            
            for (int i = 0; i < t; i++)
                inflationCoef *= (1 + InflationCoefs.ElementAt(i));
            
            return beforeInflation / inflationCoef;
        }
        
        public class IncomeModel
        {
            // Initial capital
            public double U { get; set; }
            
            // Minimal compensation
            public int MinThreshold { get; set; }
            
            // Maximal compensation
            public int MaxThreshold { get; set; }
            
            // Final Sum
            public double FinalSum { get; set; }
        }
    }
}