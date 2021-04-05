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
    public class GompertzKramarService
    {
        private const int AgeLimit = 100;
        private const int PeopleNumber = 100000; // Param l(0)

        private double Alpha { get; set; }
        private double Beta { get; set; }
        private int N { get; set; }

        List<ComposedDataGompertz> CDG { get; set; }

        // step 1,2,3
        public GompertzKramarService()
        {
            CDG = new List<ComposedDataGompertz>();
            var xls = XlsService.ReadXls();
            foreach (var xl in xls.Where(x => x.X != 0 && x.X != 109 && x.X != 108))
                CDG.Add(new ComposedDataGompertz {TableViewRaw = xl, mju = xl.Dx / xl.Lx, G = Math.Log(xl.Dx / xl.Lx)});
                
            N = xls.Count;
        }
        
        // Returns Alpha and Beta and mean Error value
        public (GompertzLawParams, double) ComputeAllParams()
        {
            ComputeAlpha();
            ComputeBeta();
            ComputeLMod();
            var meanError = ComputeMeanError();

            return (new GompertzLawParams(Alpha, Beta, 1), meanError);
        }

        // step 4
        private void ComputeAlpha()
        {
            var KTop1 = CDG.Sum(x => x.TableViewRaw.X * x.G);

            var KTop2 = CDG.Sum(x => x.G);
            var KTop3 = CDG.Sum(x => x.TableViewRaw.X);
            var KBot = (CDG.Sum(x => x.TableViewRaw.X * x.TableViewRaw.X * N) -
                     Math.Pow(CDG.Sum(x => x.TableViewRaw.X), 2));
            Alpha = (KTop1*N-KTop2*KTop3)/KBot;
        }

        // step 5
        private void ComputeBeta()
        {
            var TTop1 = CDG.Sum(x => x.TableViewRaw.X * x.TableViewRaw.X) *
                        CDG.Sum(x => x.G);
            
            var TTop2 = CDG.Sum(x => x.TableViewRaw.X) *
                        CDG.Sum(x => x.TableViewRaw.X * x.G);
            
            var TBot = (CDG.Sum(x => x.TableViewRaw.X * x.TableViewRaw.X * N) -
                        Math.Pow(CDG.Sum(x => x.TableViewRaw.X), 2));

            Beta = Math.Exp((TTop1-TTop2)/TBot);
        }
        
        // step 7c
        private void ComputeLMod()
        {
            foreach (var line in CDG)
                line.l_mod = PeopleNumber *
                             ComputeGompertzCoef(new GompertzLawParams(Alpha, Beta, 1), (int) line.TableViewRaw.X);
        }
        
        private static double ComputeGompertzCoef(GompertzLawParams gompertzLawParams, int x) =>
            Math.Exp(-gompertzLawParams.Beta * ((Math.Exp(gompertzLawParams.Alpha * x) - 1) / gompertzLawParams.Alpha));

        // step 8
        private double ComputeMeanError()
        {
            // Max difference between real stat and computed 
            return (CDG.Max(x => Math.Abs(x.TableViewRaw.Lx - x.l_mod)) * 100) / PeopleNumber;
        }

        private class ComposedDataGompertz
        {
            public RealDataTableViewRaw TableViewRaw;

            // 2nd step
            public double mju { get; set; }

            // 3rd step
            public double G { get; set; }

            // 7c step
            public double l_mod { get; set; }
        }
    }
}