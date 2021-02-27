using System.ComponentModel.DataAnnotations;

namespace MathModelingLab2.Models
{
    public class FittingParameters
    {
        public string Params { get; set; }
        public double AbsoluteError { get; set; }

        public FittingParameters(string @params, double absoluteError)
        {
            Params = @params;
            AbsoluteError = absoluteError;
        }
    }
}