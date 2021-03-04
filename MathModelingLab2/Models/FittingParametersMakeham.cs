using System.ComponentModel.DataAnnotations;

namespace MathModelingLab2.Models
{
    public class FittingParametersMakeham
    {
        public MakehamLawParams MakehamLawParams { get; set; }
        public string Params { get; set; }
        public double AbsoluteError { get; set; }

        public FittingParametersMakeham(MakehamLawParams makehamLawParams, double absoluteError)
        {
            Params = makehamLawParams.ToString();
            AbsoluteError = absoluteError;
            MakehamLawParams = makehamLawParams;
        }
    }
}