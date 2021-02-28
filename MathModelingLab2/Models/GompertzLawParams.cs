namespace MathModelingLab2.Models
{
    public class GompertzLawParams
    {
        public double RatePercents { get; set; }

        public double Alpha { get; set; }

        public double Beta { get; set; }

        public GompertzLawParams(double alpha, double beta, double ratePercents)
        {
            Alpha = alpha;
            Beta = beta;
            RatePercents = ratePercents;
        }

        public override string ToString()
        {
            return $"Alpha = {Alpha}, Beta = {Beta}, RatePercents = {RatePercents}";
        }
    }
}