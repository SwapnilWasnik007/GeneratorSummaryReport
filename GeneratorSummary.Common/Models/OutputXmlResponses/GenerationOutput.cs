using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Common.Models.OutputXmlResponses
{
    public class GenerationOutput
    {
        public Totals Totals { get; set; } = new Totals();
        public MaxEmissionGenerators MaxEmissionGenerators { get; set; } = new MaxEmissionGenerators();
        public ActualHeatRates ActualHeatRates { get; set; } = new ActualHeatRates();
    }
}
