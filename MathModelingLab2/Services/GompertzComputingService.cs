using System.Collections.Generic;
using System.Threading.Tasks;
using MathModelingLab2.Models;

namespace MathModelingLab2.Services
{
    public class GompertzComputingService
    {
        public async Task<IEnumerable<MortalityTableModel>> BuildMortalityTable(GompertzLawParams gompertzLawParams)
        {
            
        }
        
        public async Task<string> BuildPlot(GompertzLawParams gompertzLawParams)
        {
            string path = null;
            return path;
        }
    }
}