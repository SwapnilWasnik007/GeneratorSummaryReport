using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Common.Models
{
    public class ReferenceDataView
    {
        public Dictionary<string, double> ValueFactors { get; set; } = new();
        public Dictionary<string, double> EmissionFactors { get; set; } = new();
    }
}
