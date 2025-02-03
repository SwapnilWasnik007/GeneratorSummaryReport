using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Common.Models.OutputXmlResponses
{
    public class Totals
    {
        public IList<Generator> Generator { get; set; } = new List<Generator>();
    }
}
