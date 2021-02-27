namespace MathModelingLab2.Models
{
    public class MakehamLawParams
    {
        public double Alpha { get; set; }
        
        public double A { get; set; }
        
        public double B { get; set; }
        
        public MakehamLawParams(double alpha,double a,double b)
        {
            Alpha = alpha;
            A = a;
            B = b;
        }
        
        public override string ToString()
        {
            return $"Alpha = {Alpha}, A = {A}, B = {B}";
        }
    }
}