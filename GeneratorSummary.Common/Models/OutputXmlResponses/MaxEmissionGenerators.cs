using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Common.Models.OutputXmlResponses
{
    public class MaxEmissionGenerators
    {
        public IList<Day> Day { get; set; } = new List<Day>();
    }
}
