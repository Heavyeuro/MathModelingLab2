using System;
using System.Collections.Generic;
using System.Linq;

namespace MathModelingLab2.Services
{
    public static class FunctionsComputingService
    {
        public static double ComputeAbsoluteError(List<double> firstSelection, List<double> secondSelection)
        {
            var elementCount = firstSelection.Count < secondSelection.Count
                ? firstSelection.Count
                : secondSelection.Count;

            double totalErrorCount = 0;

            var count = elementCount;
            for (var i = 0; i < count; i++)
                totalErrorCount += Math.Abs(firstSelection[i] - secondSelection[i]) /
                                   (firstSelection[i] + secondSelection[i]);

            return totalErrorCount / elementCount;
        }
    }
}