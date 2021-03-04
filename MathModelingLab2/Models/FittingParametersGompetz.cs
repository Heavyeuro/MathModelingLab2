using System.ComponentModel.DataAnnotations;

namespace MathModelingLab2.Models
{
    public class FittingParametersGompertz
    {
        public GompertzLawParams GompertzLawParams { get; set; }
        public string Params { get; set; }
        public double AbsoluteError { get; set; }

        public FittingParametersGompertz(GompertzLawParams gompertzLawParams, double absoluteError)
        {
            Params = gompertzLawParams.ToString();
            AbsoluteError = absoluteError;
            GompertzLawParams = gompertzLawParams;
        }
    }
}