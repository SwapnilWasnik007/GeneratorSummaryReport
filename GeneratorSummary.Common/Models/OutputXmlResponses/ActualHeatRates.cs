using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Common.Models.OutputXmlResponses
{
    public class ActualHeatRates
    {
        public IList<ActualHeatRate> ActualHeatRate { get; set; } = new List<ActualHeatRate>();
    }
}
