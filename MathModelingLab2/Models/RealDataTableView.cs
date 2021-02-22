using System.Collections.Generic;

namespace MathModelingLab2.Models
{
    public class RealDataTableView
    {
        public IEnumerable<RealDataTableClusters> RealDataTableClustersEnumerable { get; set; }

        public class RealDataTableClusters
        {
            public string Sex { get; set; }

            public IEnumerable<RealDataTableViewRaw> RealDataTableViewRaws { get; set; }
        }

        public class RealDataTableViewRaw
        {
            // Year #1
            public double Year { get; set; }

            // Age #2
            public double X { get; set; }

            // Number of people at specified age #3
            public double Lx { get; set; }

            // Number of people that have dead during x and x+1 step #4
            public double Dx { get; set; }

            // Prob. to dead at age X before X+1 #5
            public double Qx { get; set; }

            // Residual life span #9
            public double CCx { get; set; }
        }
    }
}