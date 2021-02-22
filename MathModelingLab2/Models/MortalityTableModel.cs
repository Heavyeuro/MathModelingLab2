using System.Collections.Generic;

namespace MathModelingLab2.Models
{
    public class MortalityTableModel
    {
        public List<MortalityTableModelRaw> MortalityTableModelRaws { get; set; }
    }
    
    public class MortalityTableModelRaw
    {
        // Age
        public double X { get; set; }
        
        // Number of people at specified age
        public double Lx { get; set; }
        
        // Number of people that have dead during x and x+1 step
        public double Dx { get; set; }
        
        // Prob. to dead at age X before X+1
        public double Qx { get; set; }
        
        // Prob. to dead before age X
        public double Px { get; set; }
        
        public double CDx { get; set; }
        
        public double CMx { get; set; }
        
        public double CNx { get; set; }
        
        // Residual life span
        public double CCx { get; set; }
    }
}